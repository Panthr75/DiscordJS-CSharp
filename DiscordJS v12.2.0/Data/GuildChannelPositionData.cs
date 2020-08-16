namespace DiscordJS.Data
{
    [Data]
    public sealed class GuildChannelPositionData
    {
        public ChannelData[] channels;
        public int position;
        public string guild_id;
    }
}