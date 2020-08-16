namespace DiscordJS.Data
{
    /// <summary>
    /// Sent when a guild channel's webhook is created, updated, or deleted.
    /// </summary>
    [Data]
    public sealed class WebhookUpdateData
    {
        /// <summary>
        /// id of the guild
        /// </summary>
        public string guild_id;

        /// <summary>
        /// id of the channel
        /// </summary>
        public string channel_id;
    }
}