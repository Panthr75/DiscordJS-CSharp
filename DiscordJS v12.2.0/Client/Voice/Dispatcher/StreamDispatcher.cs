using JavaScript;
using NodeJS;
using System;
using System.Collections.Generic;

namespace DiscordJS
{
    /// <summary>
    /// The class that sends voice packet data to the voice connection.
    /// </summary>
    public class StreamDispatcher : WritableStream, IVolume
    {
        internal const int FRAME_LENGTH = 20;
        internal const int CHANNELS = 2;
        internal const int TIMESTAMP_INC = (48000 / 100) * CHANNELS;
        internal const uint MAX_NONCE_SIZE = uint.MaxValue;

        /// <summary>
        /// Emitted whenever the dispatcher has debug information.
        /// </summary>
        /// <param name="info">The debug info</param>
        public delegate void DebugEvent(string info);

        /// <summary>
        /// Emitted when the dispatcher encounters an exception.
        /// </summary>
        public delegate void ExceptionEvent();

        /// <summary>
        /// Emitted when the dispatcher starts/stops speaking.
        /// </summary>
        /// <param name="speaking">Whether or not the dispatcher is speaking</param>
        public delegate void SpeakingEvent(bool speaking);

        /// <summary>
        /// Emitted once the stream has started to play.
        /// </summary>
        public delegate void StartEvent();

        /// <summary>
        /// Emitted when the volume of this dispatcher changes.
        /// </summary>
        /// <param name="oldVolume">The old volume of this dispatcher</param>
        /// <param name="newVolume">The new volume of this dispatcher</param>
        public delegate void VolumeChangeEvent(double oldVolume, double newVolume);


        /// <summary>
        /// Emitted whenever the dispatcher has debug information.
        /// </summary>
        [Listener(EventName = "debug", InvokesOnce = false)]
        public event DebugEvent OnDebug;

        /// <summary>
        /// Emitted whenever the dispatcher has debug information.
        /// </summary>
        [Listener(EventName = "debug", InvokesOnce = true)]
        public event DebugEvent OnceDebug;

        internal bool EmitDebug(string info)
        {
            bool result = OnDebug == null && OnceDebug == null;
            OnDebug?.Invoke(info);
            OnceDebug?.Invoke(info);
            OnceDebug = null;
            return result;
        }

        /// <summary>
        /// Emitted when the dispatcher encounters an exception.
        /// </summary>
        [Listener(EventName = "exception", InvokesOnce = false)]
        public event ExceptionEvent OnException;

        /// <summary>
        /// Emitted when the dispatcher encounters an exception.
        /// </summary>
        [Listener(EventName = "exception", InvokesOnce = true)]
        public event ExceptionEvent OnceException;

        internal bool EmitException()
        {
            bool result = OnException == null && OnceException == null;
            OnException?.Invoke();
            OnceException?.Invoke();
            OnceException = null;
            return result;
        }

        /// <summary>
        /// Emitted when the dispatcher starts/stops speaking.
        /// </summary>
        [Listener(EventName = "speaking", InvokesOnce = false)]
        public event SpeakingEvent OnSpeaking;

        /// <summary>
        /// Emitted when the dispatcher starts/stops speaking.
        /// </summary>
        [Listener(EventName = "speaking", InvokesOnce = true)]
        public event SpeakingEvent OnceSpeaking;

        internal bool EmitSpeaking(bool speaking)
        {
            bool result = OnSpeaking == null && OnceSpeaking == null;
            OnSpeaking?.Invoke(speaking);
            OnceSpeaking?.Invoke(speaking);
            OnceSpeaking = null;
            return result;
        }

        /// <summary>
        /// Emitted once the stream has started to play.
        /// </summary>
        [Listener(EventName = "start", InvokesOnce = false)]
        public event StartEvent OnStart;

        /// <summary>
        /// Emitted once the stream has started to play.
        /// </summary>
        [Listener(EventName = "start", InvokesOnce = true)]
        public event StartEvent OnceStart;

        internal bool EmitStart()
        {
            bool result = OnStart == null && OnceStart == null;
            OnStart?.Invoke();
            OnceStart?.Invoke();
            OnceStart = null;
            return result;
        }

        /// <summary>
        /// Emitted when the volume of this dispatcher changes.
        /// </summary>
        [Listener(EventName = "volumeChange", InvokesOnce = false)]
        public event VolumeChangeEvent OnVolumeChange;

        /// <summary>
        /// Emitted when the volume of this dispatcher changes.
        /// </summary>
        [Listener(EventName = "volumeChange", InvokesOnce = true)]
        public event VolumeChangeEvent OnceVolumeChange;

        internal bool EmitVolumeChange(double oldVolume, double newVolume)
        {
            bool result = OnVolumeChange == null && OnceVolumeChange == null;
            OnVolumeChange?.Invoke(oldVolume, newVolume);
            OnceVolumeChange?.Invoke(oldVolume, newVolume);
            OnceVolumeChange = null;
            return result;
        }


        /// <summary>
        /// Whether or not the Opus bitrate of this stream is editable
        /// </summary>
        public bool BitrateEditable => streams.Opus != null; // TODO: Add check for SetBitrate method

        /// <summary>
        /// The broadcast controlling this dispatcher, if any
        /// </summary>
        public VoiceBroadcast Broadcast { get; }

        /// <summary>
        /// Whether or not playback is paused
        /// </summary>
        public bool Paused => PausedSince.HasValue;

        /// <summary>
        /// The time that the stream was paused at (null if not paused)
        /// </summary>
        public long? PausedSince { get; internal set; }

        /// <summary>
        /// Total time that this dispatcher has been paused in milliseconds
        /// </summary>
        public long PauseTime => _silentPausedTime + _pausedTime + (Paused ? Date.Now() - PausedSince.Value : 0);

        /// <summary>
        /// The Audio Player that controls this dispatcher
        /// </summary>
        public BasePlayer Player { get; }

        /// <summary>
        /// The time (in milliseconds) that the dispatcher has actually been playing audio for
        /// </summary>
        public long StreamTime => count * FRAME_LENGTH;

        /// <summary>
        /// The time (in milliseconds) that the dispatcher has been playing audio for, taking into account skips and pauses
        /// </summary>
        public long TotalStreamTime => Date.Now() - startTime.GetValueOrDefault(0);

        /// <inheritdoc/>
        public double Volume => streams.Volume == null ? 1 : streams.Volume.Volume;
        /// <inheritdoc/>
        public double VolumeDecibels => VolumeInterface.VolumeDecibels(this);
        /// <inheritdoc/>
        public bool VolumeEditable => VolumeInterface.VolumeEditable(this);
        /// <inheritdoc/>
        public double VolumeLogarithmic => VolumeInterface.VolumeLogarithmic(this);

        internal Options streamOptions;
        internal Streams streams;

        internal uint _nonce;
        internal byte[] _nonceBuffer;

        internal Action _writeCallback;

        internal long _pausedTime;
        internal long _silentPausedTime;
        internal long count;

        internal long? startTime;

        internal bool _silence;

        public class Streams
        {
            private bool locked;

            private Silence _silence;
            private VoiceBroadcast _broadcast;

            public Silence Silence
            {
                get => _silence;
                set
                {
                    if (!locked)
                        _silence = value;
                }
            }

            public VoiceBroadcast Broadcast
            {
                get => _broadcast;
                set
                {
                    if (!locked)
                        _broadcast = value;
                }
            }
        }

        private StreamDispatcher(BasePlayer player, StreamOptions streamOptions, Options options, Streams streams) : base(options)
        {
            Player = player;
            this.streamOptions = options;
            this.streams = streams;
            this.streams.Silence = new Silence();

            _nonce = 0;
            _nonceBuffer = new byte[24];

            PausedSince = null;
            _writeCallback = null;

            Broadcast = this.streams.Broadcast;

            _pausedTime = 0;
            _silentPausedTime = 0;
            count = 0;

            On("finish", () =>
            {
                _Cleanup();
                _SetSpeaking(0);
            });

            SetVolume(streamOptions.volume.GetValueOrDefault(1));
            SetBitrate(streamOptions.bitrate);
            if (streamOptions.fec.HasValue) SetFEC(streamOptions.fec.Value);
            if (streamOptions.plp.HasValue) SetPLP(streamOptions.plp.Value);
            
            void streamException(string type, Exception ex)
            {
                if (type != null && ex != null)
                {
                    Exception newEx = new Exception($"{type} stream: {ex.Message}", ex);
                    Emit(Player.Dispatcher == this ? "error" : "debug", newEx);
                }
                Destroy();
            }

            On("exception", () => streamException(null, null));
            if (this.streams.Input != null) this.streams.Input.On("exception", (Exception ex) => streamException("input", ex));
            if (this.streams.FFMPEG != null) this.streams.FFMPEG.On("exception", (Exception ex) => streamException("ffmpeg", ex));
            if (this.streams.Opus != null) this.streams.Opus.On("exception", (Exception ex) => streamException("opus", ex));
            if (this.streams.Volume != null) this.streams.Volume.On("exception", (Exception ex) => streamException("volume", ex));
        }

        public StreamDispatcher(BasePlayer player, StreamOptions options, Streams streams) : this(player, options, new Options()
        {
            Bitrate = options.bitrate.Value,
            FEC = options.fec,
            HighWaterMark = options.highWaterMark,
            PLP = options.plp,
            Seek = options.seek,
            Volume = options.volume
        }, streams)
        { }

        public void On(string eventName, Action fn) => On(eventName, (Delegate)fn);
        public void On<P1>(string eventName, Action<P1> fn) => On(eventName, (Delegate)fn);
        public void On<P1, P2>(string eventName, Action<P1, P2> fn) => On(eventName, (Delegate)fn);
        internal virtual void Emit(string eventName)
        {
            switch (eventName)
            {
                case "debug":
                    EmitDebug(default);
                    break;
                case "exception":
                    EmitException();
                    break;
                case "speaking":
                    EmitSpeaking(default);
                    break;
                case "start":
                    EmitStart();
                    break;
                case "volumeChange":
                    EmitVolumeChange(default, default);
                    break;
            }
        }
        internal virtual void Emit<P1>(string eventName, P1 arg1)
        {
            switch (eventName)
            {
                case "debug":
                    EmitDebug(arg1.ToString());
                    break;
                case "exception":
                    EmitException();
                    break;
                case "speaking":
                    EmitSpeaking(arg1 is bool bArg1 ? bArg1 : bool.Parse(arg1.ToString()));
                    break;
                case "start":
                    EmitStart();
                    break;
                case "volumeChange":
                    EmitVolumeChange(arg1 is double dArg1 ? dArg1 : double.Parse(arg1.ToString()), default);
                    break;
            }
        }
        internal virtual void Emit<P1, P2>(string eventName, P1 arg1, P2 arg2)
        {
            switch (eventName)
            {
                case "debug":
                    EmitDebug(arg1.ToString());
                    break;
                case "exception":
                    EmitException();
                    break;
                case "speaking":
                    EmitSpeaking(arg1 is bool bArg1 ? bArg1 : bool.Parse(arg1.ToString()));
                    break;
                case "start":
                    EmitStart();
                    break;
                case "volumeChange":
                    EmitVolumeChange(arg1 is double dArg1 ? dArg1 : double.Parse(arg1.ToString()), 
                        arg2 is double dArg2 ? dArg2 : double.Parse(arg2.ToString()));
                    break;
            }
        }

        internal BasePlayer.StreamingDataInfo _SData => Player.streamingData;

        internal virtual void _Write(byte[] chunk, string enc, Action done)
        {
            if (!startTime.HasValue)
            {
                Emit("start");
                startTime = Date.Now();
            }
            _PlayChunk(chunk);
            _Step(done);
        }

        internal virtual void _Destroy(Exception ex, Action cb)
        {
            _Cleanup();
            base._Destroy(ex, cb);
        }

        internal virtual void _Destroy(Exception ex, Action<Exception> cb)
        {
            _Cleanup();
            base._Destroy(ex, cb);
        }

        internal virtual void _Cleanup()
        {
            if (Player.Dispatcher == this) Player.Dispatcher = null;
            if (streams.Broadcast != null) streams.Broadcast.Delete(this);
            if (streams.Opus != null) streams.Opus.Destroy();
            if (streams.FFMPEG != null) streams.Opus.Destroy();
        }

        /// <summary>
        /// Pauses playback
        /// </summary>
        /// <param name="silence">Whether to play silence while paused to prevent audio glitches</param>
        public void Pause(bool silence = false)
        {
            if (Paused) return;
            if (streams.Opus != null) streams.Opus.Unpipe(this);
            if (silence)
            {
                streams.Silence.Pipe(this);
                _silence = true;
            }
        }

        /// <summary>
        /// Resumes playback
        /// </summary>
        public void Resume()
        {
            if (!PausedSince.HasValue) return;
            streams.Silence.Unpipe(this);
            if (_silence)
            {
                _silentPausedTime += Date.Now() - PausedSince.Value;
                _silence = false;
            }
            else
            {
                _pausedTime = Date.Now() - PausedSince.Value;
            }
            PausedSince = null;
            _writeCallback?.Invoke();
        }

        /// <summary>
        /// Set the bitrate of the current Opus encoder if using a compatible Opus stream.
        /// </summary>
        /// <param name="value">New bitrate, in kbps If set to null, the voice channel's bitrate will be used</param>
        /// <returns><see langword="true"/> if the bitrate has been successfully changed.</returns>
        public bool SetBitrate(int? value)
        {
            if (BitrateEditable)
            {
                int bitrate = value.GetValueOrDefault(Player.VoiceConnection.Channel.Bitrate);
                streams.Opus.SetBitrate(bitrate * 1000);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Enables or disables forward error correction if using a compatible Opus stream.
        /// </summary>
        /// <param name="enabled">true to enable</param>
        /// <returns>Returns <see langword="true"/> if it was successfully set.</returns>
        public bool SetFEC(bool enabled)
        {
            if (!BitrateEditable) return false;
            streams.Opus.SetFEC(enabled);
            return true;
        }

        /// <summary>
        /// Sets the expected packet loss percentage if using a compatible Opus stream.
        /// </summary>
        /// <param name="value">between 0 and 1</param>
        /// <returns>Returns <see langword="true"/> if it was successfully set.</returns>
        public bool SetPLP(double value)
        {
            if (!BitrateEditable) return false;
            streams.Opus.SetPLP(value);
            return true;
        }

        /// <inheritdoc cref="IVolume.SetVolume(double)"/>
        public bool SetVolume(double volume)
        {
            if (streams.Volume == null) return false;
            Emit("volumeChange", Volume, volume);
            streams.Volume.SetVolume(volume);
            return true;
        }

        void IVolume.SetVolume(double volume) => SetVolume(volume);

        /// <inheritdoc/>
        public void SetVolumeDecibels(double db) => VolumeInterface.SetVolumeDecibels(this, db);

        /// <inheritdoc/>
        public void SetVolumeLogarithmic(double value) => VolumeInterface.SetVolumeDecibels(this, value);

        internal virtual void _Step(Action done)
        {
            _writeCallback = () =>
            {
                _writeCallback = null;
                done?.Invoke();
            };
            if (streams.Broadcast == null)
            {
                var next = FRAME_LENGTH + count * FRAME_LENGTH - (Date.Now() - startTime.GetValueOrDefault(0) - _pausedTime);
                new Timeout(next, () =>
                {
                    if ((!PausedSince.HasValue || _silence) && _writeCallback != null) _writeCallback();
                }, false);
            }
            _SData.sequence++;
            _SData.timestamp += TIMESTAMP_INC;
            if (_SData.sequence >= Math.Pow(2, 16)) _SData.sequence = 0;
            if (_SData.timestamp >= Math.Pow(2, 32)) _SData.timestamp = 0;
            count++;
        }

        internal virtual void _Final(Action callback)
        {
            _writeCallback = null;
            callback();
        }

        internal virtual void _PlayChunk(byte[] chunk)
        {
            if (Player.Dispatcher != this || Player.VoiceConnection.Authentication.secret_key == null) return;
            _SendPacket(_CreatePacket(_SData.sequence, _SData.timestamp, chunk));
        }

        internal virtual byte[][] _Encrypt(byte[] buffer)
        {
            var auth = Player.VoiceConnection.Authentication;
            string secret_key = auth.secret_key,
                mode = auth.mode;
            if (mode == "xsalsa20_poly1305_lite")
            {
                _nonce++;
                if (_nonce > MAX_NONCE_SIZE) _nonce = 0;
                List<byte> temp = new List<byte>(_nonceBuffer);
                temp.InsertRange(0, BitConverter.GetBytes(_nonce));
                _nonceBuffer = temp.ToArray();
                temp.Clear();
                return new byte[][] { 
                    SecretBox.Methods.Close(buffer, _nonceBuffer, secret_key), 
                    new Array<byte>(_nonceBuffer).Slice(0, 4).ToArray()
                };
            }
            else if (mode == "xsalsa20_poly1305_suffix")
            {
                var random = SecretBox.Methods.Random(24);
                return new byte[][] {
                    SecretBox.Methods.Close(buffer, _nonceBuffer, secret_key),
                    random
                };
            }
            else
            {
                return new byte[][]
                {
                    SecretBox.Methods.Close(buffer, _nonceBuffer, secret_key)
                };
            }
        }

        internal virtual byte[] _CreatePacket(int sequence, long timestamp, byte[] buffer)
        {
            byte[] packetBuffer = new byte[12];
            packetBuffer[0] = 0x80;
            packetBuffer[1] = 0x78;

            byte[] sequenceBytes = BitConverter.GetBytes((ushort)sequence);
            byte[] timestampBytes = BitConverter.GetBytes((uint)timestamp);
            byte[] authBytes = BitConverter.GetBytes((uint)Player.VoiceConnection.Authentication.ssrc);

            packetBuffer[2] = sequenceBytes[0];
            packetBuffer[3] = sequenceBytes[1];

            packetBuffer[4] = timestampBytes[0];
            packetBuffer[5] = timestampBytes[1];
            packetBuffer[6] = timestampBytes[2];
            packetBuffer[7] = timestampBytes[3];

            packetBuffer[8] = authBytes[0];
            packetBuffer[9] = authBytes[1];
            packetBuffer[10] = authBytes[2];
            packetBuffer[11] = authBytes[3];

            // Potential Misspell: Discord.JS: nonce (no this)
            // Buffer.BlockCopy(packetBuffer, 0, nonce, 0, 12);
            var encrypted = _Encrypt(buffer);

            List<byte> bytes = new List<byte>();
            bytes.AddRange(packetBuffer);
            for (int index = 0, length = encrypted.Length; index < length; index++)
                bytes.AddRange(encrypted[index]);
            return bytes.ToArray();
        }

        internal virtual void _SendPacket(byte[] packet)
        {
            _SetSpeaking(1);
            if (!Player.VoiceConnection.Sockets.UDP)
            {
                Emit("debug", "Failed to send a packet - no UDP socket");
                return;
            }
            Player.VoiceConnection.Sockets.UDP.Send(packet).Catch((e) =>
            {
                _SetSpeaking(0);
                Emit("debug", $"Failed to send a packet - {e}");
            });
        }

        internal virtual void _SetSpeaking(int value)
        {
            if (Player.VoiceConnection != null)
            {
                Player.VoiceConnection.SetSpeaking(value);
            }

            Emit("speaking", value);
        }
    }
}