using DiscordJS.Data;

namespace DiscordJS
{
    /// <summary>
    /// Represents a guild category channel on Discord.
    /// </summary>
    public class CategoryChannel : GuildChannel
    {
        /// <summary>
        /// Channels that are a part of this category
        /// </summary>
        public Collection<Snowflake, GuildChannel> Children => Guild.Channels.Cache.Filter((c) => c.ParentID == ID);

        /// <summary>
        /// Creates a new Category Channel Instance
        /// </summary>
        /// <param name="guild">The guild the category channel is a part of</param>
        /// <param name="data">The data for the category channel</param>
        public CategoryChannel(Guild guild, ChannelData data) : base(guild, data)
        { }
    }
}