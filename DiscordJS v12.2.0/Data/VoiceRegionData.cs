namespace DiscordJS.Data
{
    /// <summary>
    /// The data for a voice region
    /// </summary>
    [Data]
    public class VoiceRegionData
    {
        /// <summary>
        /// unique ID for the region
        /// </summary>
        public string id;

        /// <summary>
        /// name of the region
        /// </summary>
        public string name;

        /// <summary>
        /// true if this is a vip-only server
        /// </summary>
        public bool vip;

        /// <summary>
        /// true for a single server that is closest to the current user's client
        /// </summary>
        public bool optimal;

        /// <summary>
        /// whether this is a deprecated voice region (avoid switching to these)
        /// </summary>
        public bool deprecated;

        /// <summary>
        /// whether this is a custom voice region (used for events/etc)
        /// </summary>
        public bool custom;
    }
}