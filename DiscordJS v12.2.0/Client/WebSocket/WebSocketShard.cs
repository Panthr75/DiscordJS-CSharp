using DiscordJS.Packets;
using JavaScript;
using System;
using WebSocketSharp;

namespace DiscordJS.WebSockets
{
    /// <summary>
    /// Represents a Shard's WebSocket connection
    /// </summary>
    public class WebSocketShard
    {
        /// <summary>
        /// The ID of the shard
        /// </summary>
        public int ID { get; internal set; }

        /// <summary>
        /// The WebSocketManager of the shard
        /// </summary>
        public WebSocketManager Manager { get; internal set; }

        /// <summary>
        /// The previous heartbeat ping of the shard
        /// </summary>
        public int Ping { get; internal set; }

        /// <summary>
        /// The current status of the shard
        /// </summary>
        public Status Status { get; internal set; }



        /// <summary>
        /// The current sequence of the shard
        /// </summary>
        internal int Sequence { get; set; }

        /// <summary>
        /// The sequence of the shard after close
        /// </summary>
        internal int CloseSequence { get; set; }

        /// <summary>
        /// The current session ID of the shard
        /// </summary>
        internal string SessionID { get; set; }

        /// <summary>
        /// The last time a ping was sent (a timestamp)
        /// </summary>
        internal long LastPingTimestamp { get; set; }

        /// <summary>
        /// If we received a heartbeat ack back. Used to identify zombie connections
        /// </summary>
        internal bool LastHeartbeatAcked { get; set; }

        /// <summary>
        /// Contains the rate limit queue and metadata
        /// </summary>
        internal RateLimitStuff RateLimit { get; set; }

        internal long ConnectedAt { get; set; }

        internal bool EventsAttached { get; set; }

        internal WebSocket Connection { get; set; }


        /// <summary>
        /// Instantiates a new WebSocketShard.
        /// </summary>
        /// <param name="manager">The instantiating manager</param>
        /// <param name="id">The ID of this shard</param>
        public WebSocketShard(WebSocketManager manager, int id)
        {
            Manager = manager;
            ID = id;
            Status = Status.IDLE;
            Sequence = -1;
            CloseSequence = 0;
            SessionID = null;
            Ping = -1;
            LastPingTimestamp = -1;
            LastHeartbeatAcked = true;
            RateLimit = new RateLimitStuff();
        }


        /// <summary>
        /// Emits a debug event.
        /// </summary>
        /// <param name="info">The debug message</param>
        internal void Debug(string info)
        {
            Manager.Debug(info, this);
        }

        /// <summary>
        /// Connects the shard to the gateway.
        /// </summary>
        /// <returns>A promise that will resolve if the shard turns ready successfully,
        /// or reject if we couldn't connect</returns>
        internal IPromise Connect()
        {
            if (Connection != null && Connection.ReadyState == WebSocketState.Open && Status == Status.READY) return Promise.Resolved();

            return new Promise((resolve, reject) =>
            {
                WSShardCloseEvent onClose = null;
                WSShardReadyEvent onReady = null;
                WSShardResumedEvent onResumed = null;
                Action onInvalidOrDestroyed = null;
                WSShardInvalidSessionEvent onInvalid = null;
                WSShardDestroyedEvent onDestroyed = null;

                void cleanup()
                {
                    OnceClose -= onClose;
                    OnceReady -= onReady;
                    OnceResumed -= onResumed;
                    OnceInvalidSession -= onInvalid;
                    OnceDestroyed -= onDestroyed;
                }

                onReady = () =>
                {
                    cleanup();
                    resolve();
                };

                onResumed = () =>
                {
                    cleanup();
                    resolve();
                };

                onClose = (ev) =>
                {
                    cleanup();
                    reject(null); //TODO: Make an WSClosedEventException class
                };

                onInvalidOrDestroyed = () =>
                {
                    cleanup();
                    reject(null);
                };

                onInvalid = new WSShardInvalidSessionEvent(onInvalidOrDestroyed);
                onDestroyed = new WSShardDestroyedEvent(onInvalidOrDestroyed);

                OnceReady += onReady;
                OnceResumed += onResumed;
                OnceClose += onClose;
                OnceInvalidSession += onInvalid;
                OnceDestroyed += onDestroyed;

                if (Connection != null && Connection.ReadyState == WebSocketState.Open)
                {
                    Debug("An open connection was found, attempting an immediate identify.");
                    Identify();
                    return;
                }

                if (Connection != null)
                {
                    Debug("A connection object was found. Cleaning up before continuing.");
                    Destroy(new WebSocketShardDestroyInfo()
                    {
                        Emit = false
                    });
                }

                dynamic wsQuery = new { v = Manager.Client.Options.ws.version };
                Debug($@"[CONNECT]
    Gateway    : {Manager.Gateway}
    Version    : {Manager.Client.Options.ws.version}
    Encoding   : Default
    Compression: None");

                Status = Status == Status.DISCONNECTED ? Status.RECONNECTING : Status.CONNECTING;

                SetHelloTimeout();

                ConnectedAt = Date.Now();
                Connection = new WebSocket(Manager.Gateway + "?v=" + wsQuery.v);
                Connection.OnOpen += OnWSOpen;
                Connection.OnMessage += OnWSMessage;
                Connection.OnError += OnWSError;
                Connection.OnClose += OnWSClose;
            });
        }

        /// <summary>
        /// Called whenever a connection is opened to the gateway.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWSOpen(object sender, EventArgs e)
        {
            Debug($"[CONNECTED] {Connection.Url} in {Date.Now() - ConnectedAt}ms");
            Status = Status.NEARLY;
        }

        /// <summary>
        /// Called whenever a message is received.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event received</param>
        private void OnWSMessage(object sender, MessageEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called whenever an error occurs with the WebSocket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">The error that occurred</param>
        private void OnWSError(object sender, ErrorEventArgs e)
        {
            Manager.Client.EmitShardException(e.Exception, ID);
        }

        /// <summary>
        /// Called whenever a connection to the gateway is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Close event that was received</param>
        private void OnWSClose(object sender, CloseEventArgs e)
        {
            if (Sequence != -1) CloseSequence = Sequence;
            Sequence = -1;

            Debug($@"[CLOSE]
    Event Code: {(int)e.Code}
    Clean     : {e.WasClean}
    Reason    : {(e.Reason == null ? "No Reason Received" : e.Reason)}");

            SetHeartbeatTimer(-1);
            SetHelloTimeout(-1);
            // If we still have a connection object, clean up its listeners
            if (Connection != null) _CleanupConnection();

            Status = Status.DISCONNECTED;

            EmitClose(new WSCloseEvent((ushort)(int)e.Code, e.Reason, e.WasClean));
        }

        /// <summary>
        /// Called whenever a packet is received.
        /// </summary>
        /// <param name="packet">The received packet</param>
        private void OnPacket(dynamic packet)
        {
            if (packet == null)
            {
                Debug($"Received broken packet: '{packet}'");
                return;
            }

            WSEventType? type = null;
            if (Enum.TryParse(packet.t, out type))
            {
                switch(type.Value)
                {
                    case WSEventType.READY:
                        EmitReady();
                        SessionID = packet.d.session_id;
                        ExpectedGuilds = new Set<string>(new Array<GuildData>(packet.d.guilds).Map((d) => d.id));
                        Status = Status.WAITING_FOR_GUILDS;
                        Debug($"[READY] Session {SessionID}");
                        LastHeartbeatAcked = true;
                        SendHeartbeat("ReadyHeartbeat");
                        break;
                    case WSEventType.RESUMED:
                        EmitResumed();

                        Status = Status.READY;
                        int replayed = packet.s - CloseSequence;
                        Debug($"[RESUMED] Session {SessionID} | Replayed {replayed} events");
                        LastHeartbeatAcked = true;
                        SendHeartbeat("ResumeHeartbeat");

                        break;
                    default:
                        break;
                }
            }

            if (packet.s > Sequence) Sequence = packet.s;

            switch((OPCode)packet.op)
            {
                case OPCode.HELLO:
                    SetHelloTimeout(-1);
                    SetHeartbeatTimer(packet.d.heartbeat_interval);
                    Identify();
                    break;
                case OPCode.RECONNECT:
                    Debug("[RECONNECT] Discord asked us to reconnect");
                    Destroy(new WebSocketShardDestroyInfo()
                    {
                        CloseCode = 4000
                    });
                    break;
                case OPCode.INVALID_SESSION:
                    Debug($"[INVALID SESSION] Resumable: {packet.d}.");
                    // If we can resume the session, do so immediately
                    if (packet.d.HasValue)
                    {
                        IdentifyResume();
                        return;
                    }
                    // Reset the sequence
                    Sequence = -1;
                    // Reset the session ID as it's invalid
                    SessionID = null;
                    // Set the status to reconnecting
                    Status = Status.RECONNECTING;
                    // Finally, emit the INVALID_SESSION event
                    EmitInvalidSession();
                    break;
                case OPCode.HEARTBEAT_ACK:
                    AckHeartbeat();
                    break;
                case OPCode.HEARTBEAT:
                    SendHeartbeat("HeartbeatRequest", true);
                    break;
                default:
                    Manager.HandlePacket(packet, this);
                    if (Status == Status.WAITING_FOR_GUILDS && (WSEventType)Enum.Parse(typeof(WSEventType), packet.t) == WSEventType.GUILD_CREATE)
                    {
                        ExpectedGuilds.Delete(packet.d.id);
                        CheckReady();
                    }
                    break;

#warning Implement WebSocketShard starting at CheckReady
            }
        }

        /// <summary>
        /// Adds a packet to the queue to be sent to the gateway.
        /// <br/>
        /// <br/>
        /// <warn><b>If you use this method, make sure you understand that you need to provide a full <see href="https://discordapp.com/developers/docs/topics/gateway#commands-and-events-gateway-commands">Payload</see>.<br/>
        /// Do not use this method if you don't know what you're doing.</b></warn>
        /// </summary>
        /// <param name="data">The full packet to send</param>
        /// <param name="important">If this packet should be added first in queue</param>
        public void Send(dynamic data, bool important = false)
        {
            //
        }

        /// <summary>
        /// Adds an already JSON Stringified packet to the queue to be sent to the gateway.
        /// <br/>
        /// <br/>
        /// <warn><b>If you use this method, make sure you understand that you need to provide a full <see href="https://discordapp.com/developers/docs/topics/gateway#commands-and-events-gateway-commands">Payload</see>.<br/>
        /// Do not use this method if you don't know what you're doing.</b></warn>
        /// </summary>
        /// <param name="data">The full packet to send</param>
        /// <param name="important">If this packet should be added first in queue</param>
        public void Send(string data, bool important = false)
        {
            //
        }

        /// <summary>
        /// Emitted when the shard is fully ready. This event is emitted if:
        /// <list type="bullet">
        /// <item>all guilds were received by this shard</item>
        /// <item>the ready timeout expired, and some guilds are unavailable</item>
        /// </list>
        /// </summary>
        public event WSShardAllReadyEvent OnAllReady;

        /// <summary>
        /// Emitted when the shard is fully ready. This event is emitted if:
        /// <list type="bullet">
        /// <item>all guilds were received by this shard</item>
        /// <item>the ready timeout expired, and some guilds are unavailable</item>
        /// </list>
        /// </summary>
        public event WSShardAllReadyEvent OnceAllReady;

        /// <summary>
        /// Emitted when a shard's WebSocket closes.
        /// </summary>
        public event WSShardCloseEvent OnClose;

        /// <summary>
        /// Emitted when a shard's WebSocket closes.
        /// </summary>
        public event WSShardCloseEvent OnceClose;

        internal bool EmitClose(WSCloseEvent ev)
        {
            var result = OnClose == null && OnceClose == null;
            OnClose?.Invoke(ev);
            OnceClose?.Invoke(ev);
            OnceClose = null;
            return result;
        }

        /// <summary>
        /// Emitted when a shard is destroyed, but no WebSocket connection was present.
        /// </summary>
        public event WSShardDestroyedEvent OnDestroyed;

        /// <summary>
        /// Emitted when a shard is destroyed, but no WebSocket connection was present.
        /// </summary>
        public event WSShardDestroyedEvent OnceDestroyed;

        /// <summary>
        /// Emitted when the shard receives the READY payload and is now waiting for guilds
        /// </summary>
        public event WSShardReadyEvent OnReady;

        /// <summary>
        /// Emitted when the shard receives the READY payload and is now waiting for guilds
        /// </summary>
        public event WSShardReadyEvent OnceReady;

        public bool EmitReady()
        {
            var result = OnReady == null && OnceReady == null;
            OnReady?.Invoke();
            OnceReady?.Invoke();
            OnceReady = null;
            return result;
        }

        /// <summary>
        /// Emitted when the shard resumes successfully
        /// </summary>
        public event WSShardResumedEvent OnResumed;

        /// <summary>
        /// Emitted when the shard resumes successfully
        /// </summary>
        public event WSShardResumedEvent OnceResumed;

        internal bool EmitResumed()
        {
            var result = OnResumed == null && OnceResumed == null;
            OnResumed?.Invoke();
            OnceResumed?.Invoke();
            OnceResumed = null;
            return result;
        }

        /// <summary>
        /// Emitted when the shard has an invalid session
        /// </summary>
        internal event WSShardInvalidSessionEvent OnInvalidSession;

        /// <summary>
        /// Emitted when the shard has an invalid session
        /// </summary>
        internal event WSShardInvalidSessionEvent OnceInvalidSession;

        public bool EmitInvalidSession()
        {
            var result = OnInvalidSession == null && OnceInvalidSession == null;
            OnInvalidSession?.Invoke();
            OnceInvalidSession?.Invoke();
            OnceInvalidSession = null;
            return result;
        }
    }

    public class WebSocketShardDestroyInfo
    {
        public bool Reset { get; set; }
        public bool Emit { get; set; }
        public bool Log { get; set; }
        public ushort CloseCode { get; set; } = 0;
    }

    internal class RateLimitStuff
    {
        public Array<string> Queue { get; internal set; }
        public int Total { get; internal set; }
        public int Remaining { get; internal set; }
        public int Time { get; internal set; }
        public int? Timer { get; internal set; }

        public RateLimitStuff() : this(120, 120, 60000, null)
        { }

        public RateLimitStuff(int total, int remaining, int time, int? timer)
        {
            Queue = new Array<string>();
            Remaining = remaining;
            Time = time;
            Timer = timer;
        }
    }

    /// <summary>
    /// Emitted when the shard is fully ready. This event is emitted if:
    /// <list type="bullet">
    /// <item>all guilds were received by this shard</item>
    /// <item>the ready timeout expired, and some guilds are unavailable</item>
    /// </list>
    /// </summary>
    /// <param name="unavailableGuilds">Set of unavailable guilds, if any</param>
    public delegate void WSShardAllReadyEvent(Set<string> unavailableGuilds);

    /// <summary>
    /// Emitted when a shard's WebSocket closes.
    /// </summary>
    /// <param name="ev">The received event</param>
    public delegate void WSShardCloseEvent(WSCloseEvent ev);

    /// <summary>
    /// Emitted when a shard is destroyed, but no WebSocket connection was present.
    /// </summary>
    public delegate void WSShardDestroyedEvent();

    /// <summary>
    /// Emitted when the shard receives the READY payload and is now waiting for guilds
    /// </summary>
    public delegate void WSShardReadyEvent();

    /// <summary>
    /// Emitted when the shard resumes successfully
    /// </summary>
    public delegate void WSShardResumedEvent();

    /// <summary>
    /// Emitted when the shard has an invalid session
    /// </summary>
    internal delegate void WSShardInvalidSessionEvent();
}