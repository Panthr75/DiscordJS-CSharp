using JavaScript;
using WebSocketSharp;
using System;
using System.Net;

namespace DiscordJS.WebSockets
{
    /// <summary>
    /// The WebSocket manager for this client.
    /// <br/>
    /// <br/>
    /// <info><b>This class forwards raw dispatch events, read more about it here:<br/>
    /// </b><see href="https://discordapp.com/developers/docs/topics/gateway"/></info>
    /// </summary>
    public class WebSocketManager
    {
        private static Array<WSEventType> BeforeReadyWhitelist;

        static WebSocketManager()
        {
            BeforeReadyWhitelist = new Array<WSEventType>();
            BeforeReadyWhitelist.Push(WSEventType.READY, WSEventType.RESUMED, WSEventType.GUILD_DELETE, WSEventType.GUILD_CREATE, WSEventType.GUILD_MEMBERS_CHUNK, WSEventType.GUILD_MEMBER_ADD, WSEventType.GUILD_MEMBER_REMOVE);
        }

        /// <summary>
        /// The client that instantiated this WebSocketManager
        /// </summary>
        public Client Client { get; }

        /// <summary>
        /// The gateway this manager uses
        /// </summary>
        public string Gateway { get; internal set; }

        /// <summary>
        /// The average ping of all WebSocketShards
        /// </summary>
        public int Ping { get; }

        /// <summary>
        /// A collection of all shards this manager handles
        /// </summary>
        public Collection<int, WebSocketShard> Shards { get; internal set; }

        /// <summary>
        /// The current status of this WebSocketManager
        /// </summary>
        public Status Status { get; internal set; }


        /// <summary>
        /// The amount of shards this manager handles
        /// </summary>
        internal int TotalShards { get; set; }

        /// <summary>
        /// An array of shards to be connected or that need to reconnect
        /// </summary>
        internal Set<WebSocketShard> ShardQueue { get; set; }

        /// <summary>
        /// An array of queued events before this WebSocketManager became ready
        /// </summary>
        internal Array<PacketQueueItem> PacketQueue { get; set; }

        /// <summary>
        /// If this manager was destroyed. It will prevent shards from reconnecting
        /// </summary>
        internal bool Destroyed { get; set; }

        /// <summary>
        /// If this manager is currently reconnecting one or multiple shards
        /// </summary>
        internal bool Reconnecting { get; set; }

        /// <summary>
        /// The current session limit of the client
        /// </summary>
        internal SessionLimit SessionStartLimit { get; set; }

        /// <summary>
        /// Instantiates a new WebSocketManager
        /// </summary>
        /// <param name="client">The instantiating client</param>
        public WebSocketManager(Client client)
        {
            Client = client;
            Gateway = null;
            TotalShards = Client.Options.shards.Length;
            Shards = new Collection<int, WebSocketShard>();
            ShardQueue = new Set<WebSocketShard>();
            PacketQueue = new Array<PacketQueueItem>();
            Status = Status.IDLE;
            SessionStartLimit = null;
        }

        /// <summary>
        /// Emits a debug message.
        /// </summary>
        /// <param name="message">The debug message</param>
        /// <param name="shard">The shard that emitted this message, if any</param>
        internal void Debug(string message, WebSocketShard shard = null)
        {
            Client.EmitDebug($"[WS => {(shard == null ? "Manager" : shard.ID.ToString())}] {message}");
        }

        /// <summary>
        /// Connects this manager to the gateway.
        /// </summary>
        /// <returns></returns>
        internal IPromise<bool> Connect()
        {
            var invalidToken = new DJSError.Error(Constants.WSCodes[4004]);
            IPromise<dynamic> promise = Client.API.Gateway.Bot.Get();
            return promise.Then((data) =>
            {
                string gatewayURL = data.url;
                int recommendedShards = data.shards;
                SessionLimit sessionLimitStart = new SessionLimit()
                {
                    remaining = data.session_start_limit.remaining,
                    reset_after = data.session_start_limit.reset_after,
                    total = data.session_start_limit.total
                };

                Debug($@"Fetched Gateway Information
    URL: {gatewayURL}
    Recommended Shards: {recommendedShards}");

                Debug($@"Session Limit Information
    Total: {sessionLimitStart.total}
    Remaining: {sessionLimitStart.remaining}");

                Gateway = $"{gatewayURL}/";
                if (Client.Options.shards == null)
                {
                    Debug($"Using the recommended shard count provided by Discord: {recommendedShards}");
                    Client.Options.shardCount = recommendedShards;
                    TotalShards = (int)Client.Options.shardCount;
                    Client.Options.shards = new Array<int>(recommendedShards).Map((_, i) => i);
                }

                TotalShards = Client.Options.shards.Length;
                Debug($"Spawning shards: {Client.Options.shards.Join(", ")}");
                ShardQueue = new Set<WebSocketShard>(Client.Options.shards.Map((id) => new WebSocketShard(this, id)));

                return _HandleSessionLimit(sessionLimitStart.remaining, sessionLimitStart.reset_after).Then(() => CreateShards());
            }, (error) =>
            {
                return Promise<bool>.Resolved(false);
#warning throw invalidToken if error.httpStatus is 401
            });
        }

        /// <summary>
        /// Handles the creation of a shard.
        /// </summary>
        /// <returns></returns>
        internal IPromise<bool> CreateShards()
        {
            // If we don't have any shards to handle, return
            if (ShardQueue.Size == 0) return Promise<bool>.Resolved(false);

            var shard = ShardQueue[0];

            ShardQueue.Delete(shard);

            if (!shard.EventsAttached)
            {
                shard.OnAllReady += (unavailableGuilds) =>
                {
                    Client.EmitShardReady(shard.ID, unavailableGuilds);

                    if (ShardQueue.Size == 0) Reconnecting = false;
                    CheckShardsReady();
                };

                shard.OnClose += (ev) =>
                {
                    if (ev.Code == 1000 ? Destroyed : Constants.WSCodes.IsUnrecoverable(ev.Code))
                    {
                        Client.EmitShardDisconnect(ev, shard.ID);
                        Debug(Constants.WSCodes[ev.Code], shard);
                        return;
                    }

                    if (Constants.WSCodes.IsUnresumable(ev.Code))
                    {
                        shard.SessionID = null;
                    }

                    Client.EmitShardReconnecting(shard.ID);

                    ShardQueue.Add(shard);

                    if (shard.SessionID == null)
                    {
                        shard.Destroy(new WebSocketShardDestroyInfo()
                        {
                            Reset = true,
                            Emit = false,
                            Log = false
                        });
                        Reconnect(true);
                    }
                    else
                    {
                        shard.Destroy(new WebSocketShardDestroyInfo()
                        {
                            Reset = true,
                            Emit = false,
                            Log = false
                        });
                        Reconnect();
                    }
                };

                shard.OnInvalidSession += () =>
                {
                    Client.EmitShardReconnecting(shard.ID);
                };

                shard.OnDestroyed += () =>
                {
                    Debug("Shard was destroyed but no WebSocket connection was present! Reconnecting...", shard);

                    Client.EmitShardReconnecting(shard.ID);

                    ShardQueue.Add(shard);
                    Reconnect();
                };

                shard.EventsAttached = true;
            }

            return shard.Connect().Then<bool>(() =>
            {
                // If we have more shards, add a 5s delay
                if (ShardQueue.Size > 0)
                {
                    Debug($"Shard Queue Size: {ShardQueue.Size}; continuing in 5 seconds...");
                    return new Promise((resolve, reject) =>
                    {
                        new Timeout(5000, resolve, false);
                    }).Then(() => _HandleSessionLimit()).Then(() => CreateShards());
                }
                return Promise<bool>.Resolved(true);
            }, (error) =>
            {
                if (error != null && error is WebSocketException webSocketException && Constants.WSCodes.IsUnrecoverable((int)webSocketException.Code))
                {
                    throw new DJSError.Error(Constants.WSCodes[(int)webSocketException.Code], new object[0] { });
                    // Undefined if session is invalid, error event for regular closes
                }
                else if (error == null || error is WebSocketException)
                {
                    Debug("Failed to connect to the gateway, requeueing...", shard);
                    ShardQueue.Add(shard);
                    return Promise<bool>.Resolved(false);
                }
                else
                    throw error;
            });
        }

        /// <summary>
        /// Handles reconnects for this manager.
        /// </summary>
        /// <param name="skipLimit">If this reconnect should skip checking the session limit</param>
        /// <returns></returns>
        internal IPromise<bool> Reconnect(bool skipLimit = false)
        {
            if (Reconnecting || Status != Status.READY) return Promise<bool>.Resolved(false);
            Reconnecting = true;

            IPromise<bool> OnPromiseError(Exception error)
            {
                Debug($"Couldn't reconnect or fetch information about the gateway. {error}");
                if (error is WebException webException && (int)webException.Status != 401)
                {
                    Debug("Possible network error occurred. Retrying in 5s...");
                    return new Promise((resolve, reject) =>
                    {
                        new Timeout(5000, resolve, false);
                    }).Then(() =>
                    {
                        Reconnecting = false;
                        return Reconnect();
                    });
                }
                // If we get an error at this point, it means we cannot reconnect anymore
                if (Client.EmitInvalidated())
                {
                    // Destroy just the shards. This means you have to handle the cleanup yourself
                    Destroy();
                }
                else
                    Client.Destroy();

                Reconnecting = false;
                return Promise<bool>.Resolved(true);
            }

            IPromise promise;
            if (!skipLimit) promise = _HandleSessionLimit();
            else promise = Promise.Resolved();
            return promise.Then(() =>
            {
                return CreateShards().Then((_) =>
                {
                    Reconnecting = false;
                    return true;
                }).Catch((err) =>
                {
                    OnPromiseError(err);
                    return false;
                });
            }).Catch((err) =>
            {
                OnPromiseError(err);
                return false;
            });
        }

        /// <summary>
        /// Broadcasts a packet to every shard this manager handles.
        /// </summary>
        /// <param name="packet">The packet to send</param>
        internal void Broadcast(dynamic packet)
        {
            foreach (WebSocketShard shard in Shards.Values()) shard.Send(packet);
        }

        /// <summary>
        /// Broadcasts a pre JSON stringified packet to every shard this manager handles.
        /// </summary>
        /// <param name="packet">The packet to send</param>
        internal void Broadcast(string packet)
        {
            foreach (WebSocketShard shard in Shards.Values()) shard.Send(packet);
        }

        /// <summary>
        /// Destroys this manager and all its shards.
        /// </summary>
        internal void Destroy()
        {
            if (Destroyed) return;
            Debug("Manager was destroyed");
            Destroyed = true;
            ShardQueue.Clear();
            foreach (WebSocketShard shard in Shards.Values()) shard.Destroy(new WebSocketShardDestroyInfo()
            {
                CloseCode = 1000,
                Reset = true,
                Emit = false,
                Log = false
            });
        }

        /// <summary>
        /// Handles the timeout required if we cannot identify anymore.
        /// </summary>
        /// <param name="remaining">The amount of remaining identify sessions that can be done today</param>
        /// <param name="resetAfter">The amount of time in which the identify counter resets</param>
        /// <returns></returns>
        internal IPromise _HandleSessionLimit(int? remaining = null, long? resetAfter = null)
        {
            if (!remaining.HasValue && !resetAfter.HasValue)
            {
                return Client.API.Gateway.Bot.Get().Then((data) =>
                {
                    var session_start_limit = data.session_start_limit;
                    remaining = session_start_limit.remaining;
                    resetAfter = session_start_limit.reset_after;
                    Debug($@"Session Limit Information
    Total: {session_start_limit.total}
    Remaining: {remaining.Value}");

                    if (remaining.Value == 0)
                    {
                        Debug($"Exceeded identify threshold. Will attempt a connection in {resetAfter.Value}ms");
                        return new Promise((resolve, reject) =>
                        {
                            new Timeout(resetAfter.Value, resolve, false);
                        });
                    }
                });
            }
            else if (remaining.Value == 0)
            {
                Debug($"Exceeded identify threshold. Will attempt a connection in {resetAfter.Value}ms");
                return new Promise((resolve, reject) =>
                {
                    new Timeout(resetAfter.Value, resolve, false);
                });
            }
            return Promise.Resolved();
        }

        /// <summary>
        /// Processes a packet and queues it if this WebSocketManager is not ready.
        /// </summary>
        /// <param name="packet">The packet to be handled</param>
        /// <param name="shard">The shard that will handle this packet</param>
        /// <returns></returns>
        public bool HandlePacket(dynamic packet = null, WebSocketShard shard = null)
        {
            WSEventType? type = null;
            if (packet != null && Status != Status.READY && Enum.TryParse(packet.t, out type))
            {
                if (!BeforeReadyWhitelist.Includes(type.Value))
                {
                    PacketQueue.Push(new PacketQueueItem()
                    {
                        packet = packet,
                        shard = shard
                    });
                    return false;
                }
            }

            if (PacketQueue.Length > 0)
            {
                var item = PacketQueue.Shift();
                new Timeout(0, () => HandlePacket(item.packet, item.shard), false);
            }

            if (packet != null)
            {
                var handler = PacketHandler.Get(packet.t);
                if (handler != null)
                    handler.Handle(Client, packet, shard);
            }

            return true;
        }

        /// <summary>
        /// Checks whether the client is ready to be marked as ready.
        /// </summary>
        /// <returns></returns>
        internal IPromise CheckShardsReady()
        {
            if (Status == Status.READY) return Promise.Rejected(null);
            if (Shards.Size != TotalShards || Shards.Some((s) => s.Status != Status.READY)) return Promise.Rejected(null);

            Status = Status.NEARLY;

            if (Client.Options.fetchAllMembers)
            {
                var promises = Client.Guilds.Cache.Map((guild) =>
                {
                    if (guild.Available) return (IPromise)guild.Members.Fetch();
                    return Promise.Resolved();
                });
                return Promise.All(promises.ToArray()).Then(() =>
                {
                    TriggerClientReady();
                }).Catch((err) =>
                {
                    Debug($"Failed to fetch all members before ready! {err}");
                });
            }
            TriggerClientReady();
            return Promise.Resolved();
        }

        /// <summary>
        /// Causes the client to be marked as ready and emits the ready event.
        /// </summary>
        internal void TriggerClientReady()
        {
            Status = Status.READY;

            Client.ReadyAt = new Date();

            Client.EmitReady();

            HandlePacket();
        }
    }

    internal class PacketQueueItem
    {
        /// <summary>
        /// The packet
        /// </summary>
        public dynamic packet;

        /// <summary>
        /// The shard
        /// </summary>
        public WebSocketShard shard;
    }

    internal class SessionLimit
    {
        /// <summary>
        /// Total number of identifies available
        /// </summary>
        public int total;

        /// <summary>
        /// Number of identifies remaining
        /// </summary>
        public int remaining;

        /// <summary>
        /// Number of milliseconds after which the limit resets
        /// </summary>
        public long reset_after;
    }
}