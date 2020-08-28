using DiscordJS.Data;

namespace DiscordJS
{
    /// <summary>
    /// Represents a Discord voice region for guilds.
    /// </summary>
    public class VoiceRegion
    {
        /// <summary>
        /// Whether the region is custom
        /// </summary>
        public bool Custom { get; internal set; }

        /// <summary>
        /// Whether the region is deprecated
        /// </summary>
        public bool Deprecated { get; internal set; }

        /// <summary>
        /// The ID of the region
        /// </summary>
        public string ID { get; internal set; }

        /// <summary>
        /// Name of the region
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Whether the region is optimal
        /// </summary>
        public bool Optimal { get; internal set; }

        /// <summary>
        /// Whether the region is VIP-only
        /// </summary>
        public bool VIP { get; internal set; }

        public VoiceRegion(VoiceRegionData data)
        {
            ID = data.id;
            Name = data.name;
            VIP = data.vip;
            Deprecated = data.deprecated;
            Optimal = data.optimal;
            Custom = data.custom;
        }
    }
}