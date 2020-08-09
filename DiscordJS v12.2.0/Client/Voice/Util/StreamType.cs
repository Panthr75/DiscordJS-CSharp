namespace DiscordJS
{
    /// <summary>
    /// An option passed as part of <see cref="StreamOptions"/> specifying the type of the stream.
    /// </summary>
    public enum StreamType
    {
        /// <summary>
        /// The default type, streams/input will be passed through to ffmpeg before encoding. Will play most streams.
        /// </summary>
        Unknown,

        /// <summary>
        /// Play a stream of 16bit signed stereo PCM data, skipping ffmpeg.
        /// </summary>
        Converted,

        /// <summary>
        /// Play a stream of opus packets, skipping ffmpeg. You lose the ability to alter volume.
        /// </summary>
        Opus,

        /// <summary>
        /// Play an ogg file with the opus encoding, skipping ffmpeg. You lose the ability to alter volume.
        /// </summary>
        OggOpus,

        /// <summary>
        /// Play a webm file with opus audio, skipping ffmpeg. You lose the ability to alter volume.
        /// </summary>
        WebmOpus
    }
}