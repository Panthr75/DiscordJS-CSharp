using JavaScript;
using NodeJS;
using System;
using System.IO;

namespace DiscordJS
{
    /// <summary>
    /// An Audio Player for a Voice Connection.
    /// </summary>
    public class BasePlayer : EventEmitter
    {
        internal readonly string[] FFMPEG_ARGUMENTS = new string[10] { "-analyzeduration", "0", "-loglevel", "0", "-f", "s16le", "-ar", "48000", "-ac", "2" };

        public StreamDispatcher Dispatcher { get; internal set; }

        public virtual VoiceConnection VoiceConnection => throw new NotImplementedException("VoiceConnection is not implemented for a BasePlayer");

        public override StreamDispatcher PlayBroadcast(VoiceBroadcast broadcast, StreamOptions options) => throw new NotImplementedException("PlayBroadcast is not implemented for a base player")

        internal class StreamingDataInfo
        {
            public int channels = 2;
            public int sequence = 0;
            public long timestamp = 0;
        }

        internal StreamingDataInfo streamingData;

        public BasePlayer()
        {
            Dispatcher = null;
            streamingData = new StreamingDataInfo();
        }

        public void Destroy()
        {
            DestroyDispatcher();
        }

        public void DestroyDispatcher()
        {
            if (Dispatcher != null)
            {
                Dispatcher.Destroy();
                Dispatcher = null;
            }
        }

        public StreamDispatcher PlayUnknown(Stream input, StreamOptions options)
        {
            DestroyDispatcher();

            Array<string> args = new Array<string>(FFMPEG_ARGUMENTS).Slice();
            if (options.seek != 0) args.Unshift("-ss", options.seek.ToString());

            dynamic ffmpeg = null;//new Prism.FFmpeg(args);

            StreamDispatcher.Streams streams = new StreamDispatcher.Streams()
            {
                FFMPEG = ffmpeg
            };
            streams.Input = input;
            // TODO: Find a Buffer.Pipe equivilent for C#
            return PlayPCMStream(ffmpeg, options, streams);
        }

        public StreamDispatcher PlayPCMStream(Stream stream, StreamOptions options, StreamDispatcher.Streams streams = null)
        {
            if (streams == null) streams = new StreamDispatcher.Streams();
            DestroyDispatcher();
            dynamic opus = null;//new Prism.Opus.Encoder(new OpusEncoderOptions() { channels: 2, rate: 48000, frameSize: 960 });

            if (options != null && options.volume == null)
            {
                // stream.Pipe(opus);
                return PlayOpusStream(opus, options, streams);
            }
            streams.Volume = null;//new Prism.VolumeTransformer(new VolumeTransformerOptions() { type: "s16le", volume: options == null ? 1 : options.volume.GetValueOrDefault(1) });
            // stream.Pipe(streams.Volume).Pipe(opus);
            return PlayOpusStream(stream, options, streams);
        }

        public StreamDispatcher PlayOpusStream(dynamic stream, StreamOptions options, StreamDispatcher.Streams streams = null)
        {
            if (streams == null) streams = new StreamDispatcher.Streams();
            DestroyDispatcher();
            streams.Opus = stream;
            if (options.volume.HasValue && streams.Input == null)
            {
                streams.Input = stream;
                dynamic decoder = null;//new Prism.Opus.Decoder(new OpusDecoderOptions() { channels: 2, rate: 48000, frameSize: 960 });
                streams.volume = null;//new Prism.VolumeTransformer(new VolumeTransformerOptions() { type: "s16le", volume: options == null ? 1 : options.volume.GetValueOrDefault(1) });
                //streams.Opus = stream.Pipe(decoder)
                //    .Pipe(streams.Volume)
                //    .Pipe(new Prism.Opus.Encoder(new OpusEncoderOptions() { channels: 2, rate: 48000, frameSize: 960 }));
            }
            StreamDispatcher dispatcher = CreateDispatcher(options, streams);
            // streams.Opus.Pipe(dispatcher);
            return dispatcher;
        }

        internal StreamDispatcher CreateDispatcher(StreamOptions options, StreamDispatcher.Streams streams)
        {
            DestroyDispatcher();
            Dispatcher = new StreamDispatcher(this, options, streams);
            return Dispatcher;
        }
    }
}