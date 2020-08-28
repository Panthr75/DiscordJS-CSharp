namespace DiscordJS
{
    /// <summary>
    /// An interface class for volume transformation.
    /// </summary>
    public interface IVolume
    {
        /// <summary>
        /// Whether or not the volume of this stream is editable
        /// </summary>
        bool VolumeEditable { get; }

        /// <summary>
        /// The current volume of the stream
        /// </summary>
        double Volume { get; }

        /// <summary>
        /// The current volume of the stream in decibels
        /// </summary>
        double VolumeDecibels { get; }

        /// <summary>
        /// The current volume of the stream from a logarithmic scale
        /// </summary>
        double VolumeLogarithmic { get; }

        /// <summary>
        /// Sets the volume relative to the input stream - i.e. 1 is normal, 0.5 is half, 2 is double.
        /// </summary>
        /// <param name="volume">The volume that you want to set</param>
        void SetVolume(double volume);

        /// <summary>
        /// Sets the volume in decibels.
        /// </summary>
        /// <param name="db">The decibels</param>
        void SetVolumeDecibels(double db);

        /// <summary>
        /// Sets the volume so that a perceived value of 0.5 is half the perceived volume etc.
        /// </summary>
        /// <param name="value">The value for the volume</param>
        void SetVolumeLogarithmic(double value);
    }
}