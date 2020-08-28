using System.IO;

namespace DiscordJS
{
    /// <summary>
    /// An interface class to allow you to play audio over VoiceConnections and VoiceBroadcasts.
    /// </summary>
    public interface IPlay
    {
        /// <summary>
        /// The player of this play interface
        /// </summary>
        BasePlayer Player { get; }

        /// <summary>
        /// Play an audio resource.
        /// </summary>
        /// <param name="resource">The resource to play.</param>
        /// <param name="options">The options to play.</param>
        /// <returns></returns>
        /// <example><code>
        /// // Play a local audio file
        /// connection.Play("/home/hydrabolt/audio.mp3", new StreamOptions() { volume: 0.5D });
        /// </code></example>
        StreamDispatcher Play(VoiceBroadcast resource, StreamOptions options = null);

        /// <summary>
        /// Play an audio resource.
        /// </summary>
        /// <param name="resource">The resource to play.</param>
        /// <param name="options">The options to play.</param>
        /// <returns></returns>
        StreamDispatcher Play(Stream resource, StreamOptions options = null);

        /// <summary>
        /// Play an audio resource.
        /// </summary>
        /// <param name="resource">The resource to play.</param>
        /// <param name="options">The options to play.</param>
        /// <returns></returns>
        StreamDispatcher Play(string resource, StreamOptions options = null);
    }

    /// <summary>
    /// Options that can be passed to stream-playing methods:
    /// </summary>
    public sealed class StreamOptions
    {
        /// <summary>
        /// The type of stream.
        /// </summary>
        public StreamType type = StreamType.Unknown;

        /// <summary>
        /// The time to seek to, will be ignored when playing <see cref="StreamType.OggOpus"/> or <see cref="StreamType.WebmOpus"/> streams
        /// </summary>
        public double seek = 0;

        /// <summary>
        /// The volume to play at. Set this to null to disable volume transforms for
        /// this stream to improve performance.
        /// </summary>
        public double? volume = 1;

        /// <summary>
        /// Expected packet loss percentage
        /// </summary>
        public double? plp;

        /// <summary>
        /// Enabled forward error correction
        /// </summary>
        public bool? fec;

        /// <summary>
        /// The bitrate (quality) of the audio in kbps.
        /// If set to null, the voice channel's bitrate will be used
        /// </summary>
        public int? bitrate = 96;

        /// <summary>
        /// The maximum number of opus packets to make and store before they are
        /// actually needed. See <see href="https://nodejs.org/en/docs/guides/backpressuring-in-streams/"/>. Setting this value to
        /// 1 means that changes in volume will be more instant.
        /// </summary>
        public int highWaterMark = 12;
    }
}
