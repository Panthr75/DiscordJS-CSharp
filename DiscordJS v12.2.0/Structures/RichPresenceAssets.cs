using DiscordJS.Data;

namespace DiscordJS
{
    /// <summary>
    /// Assets for a rich presence
    /// </summary>
    public class RichPresenceAssets
    {
        /// <summary>
        /// Hover text for the large image
        /// </summary>
        public string LargeText { get; internal set; }

        /// <summary>
        /// ID of the large image asset
        /// </summary>
        public Snowflake LargeImage { get; internal set; }

        /// <summary>
        /// Hover text for the small image
        /// </summary>
        public string SmallText { get; internal set; }

        /// <summary>
        /// ID of the small image asset
        /// </summary>
        public Snowflake SmallImage { get; internal set; }

        public readonly Activity activity;

        public RichPresenceAssets(Activity activity, RichPresenceAssetsData assets)
        {
            this.activity = activity;

            LargeText = assets.large_text;
            SmallText = assets.small_text;

            if (assets.large_image != null) LargeImage = new Snowflake(assets.large_image);
            if (assets.small_image != null) SmallImage = new Snowflake(assets.small_image);
        }
    }
}