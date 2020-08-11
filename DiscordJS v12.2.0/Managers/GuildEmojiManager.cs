using DiscordJS.Data;
using System.Collections.Generic;

namespace DiscordJS
{
    /// <summary>
    /// Manages API methods for GuildEmojis and stores their cache.
    /// </summary>
    public class GuildEmojiManager : BaseManager<Collection<Snowflake, GuildEmoji>, GuildEmoji, EmojiData>
    {
        /// <summary>
        /// The guild this manager belongs to
        /// </summary>
        public Guild Guild { get; }

        public GuildEmojiManager(Guild guild, IEnumerable<GuildEmoji> iterable) : base(guild.Client, iterable)
        {
            Guild = guild;
        }

        protected override GuildEmoji AddItem(GuildEmoji item)
        {
            Cache.Set(item.ID, item);
            return item;
        }

#warning Implement GuildEmojiManager#Create
#warning Implement GuildEmojiManager#Resolve
#warning Implement GuildEmojiManager#ResolveID
#warning Implement GuildEmojiManager#ResolveIdentifier
    }
}