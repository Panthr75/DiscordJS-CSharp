namespace DiscordJS.Data
{
    /// <summary>
    /// Represents raw data for an activity
    /// </summary>
    public class ActivityData
    {
        public string name;
        public int type;
        public string url;
        public string details;
        public string state;
        public RichPresenceAssetsData assets;
        public ActivityTimestampsData timestamps;
        public ActivityPartyData party;
        public long flags;
        public EmojiData emoji;
        public long created_at;
    }

    public class ActivityTimestampsData
    {
        public long? start;
        public long? end;
    }

    public class ActivityPartyData
    {
        public string id;
        public int[] size;
    }

    public class RichPresenceAssetsData
    {
        public string large_text;
        public string large_image;
        public string small_text;
        public string small_image;
    }
}