using System.IO;

namespace DiscordJS
{
    /// <summary>
    /// An interface class to allow you to play audio over VoiceConnections and VoiceBroadcasts.
    /// </summary>
    static class PlayInterface
    {
        /// <summary>
        /// Play an audio resource.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="resource">The resource to play.</param>
        /// <param name="options">The options to play.</param>
        /// <returns></returns>
        public static StreamDispatcher Play<T>(T t, VoiceBroadcast resource, StreamOptions options = null) where T : IPlay
        {
            if (options == null) options = new StreamOptions();
            if (!(resource.Player is AudioPlayer)) throw new DJSError.Error("VOICE_PLAY_INTERFACE_NO_BROADCAST");
            var player = t.Player;
            return player is AudioPlayer audioPlayer ? audioPlayer.PlayBroadcast(resource, options) : null;
        }

        /// <summary>
        /// Play an audio resource.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="resource">The resource to play.</param>
        /// <param name="options">The options to play.</param>
        /// <returns></returns>
        public static StreamDispatcher Play<T>(T t, Stream resource, StreamOptions options = null) where T : IPlay
        {
            if (options == null) options = new StreamOptions();
            StreamType type = options.type;

            switch (type)
            {
                case StreamType.Unknown:
                    return t.Player.PlayUnknown(resource, options);

                case StreamType.Converted:
                    return t.Player.PlayPCMStream(resource, options);

                case StreamType.Opus:
                    return t.Player.PlayOpusStream(resource, options);

                // TODO: Implement a `Stream.pipe` method helper
                //       What Discord Does Here:
                //           this.player.playOpusStream(resource.pipe(new prism.opus.OggDemuxer()), options);
                case StreamType.OggOpus:
                    throw new System.NotImplementedException();

                // TODO: Implement a `Stream.pipe` method helper
                //       What Discord Does Here:
                //           this.player.playOpusStream(resource.pipe(new prism.opus.WebmDemuxer()), options);
                case StreamType.WebmOpus:
                    throw new System.NotImplementedException();

                default:
                    throw new System.Exception("This exception should not be thrown");
            }
        }

        /// <summary>
        /// Play an audio resource.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="resource">The resource to play.</param>
        /// <param name="options">The options to play.</param>
        /// <returns></returns>
        public static StreamDispatcher Play<T>(T t, string resource, StreamOptions options = null) where T : IPlay
        {
            if (options == null) options = new StreamOptions();
            StreamType type = options.type;


            switch (type)
            {
                case StreamType.Unknown:
                    return t.Player.PlayUnknown(resource, options);

                case StreamType.Converted:
                    return t.Player.PlayPCMStream(resource, options);

                case StreamType.Opus:
                    return t.Player.PlayOpusStream(resource, options);

                case StreamType.OggOpus:
                    throw new DJSError.Error("VOICE_PRISM_DEMUXERS_NEED_STREAM");

                case StreamType.WebmOpus:
                    throw new DJSError.Error("VOICE_PRISM_DEMUXERS_NEED_STREAM");

                default:
                    throw new System.Exception("This exception should not be thrown");
            }
        }
    }
}