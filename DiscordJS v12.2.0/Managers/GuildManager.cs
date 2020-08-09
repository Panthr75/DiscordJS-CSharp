using DiscordJS.Data;
using DiscordJS.Resolvables;
using System.Collections.Generic;

namespace DiscordJS
{
    /// <summary>
    /// Manages API methods for Guilds and stores their cache.
    /// </summary>
    public class GuildManager : BaseManager<Collection<Snowflake, Guild>, Guild, GuildData>
    {
        public GuildManager(Client client, IEnumerable<Guild> iterable) : base(client, iterable)
        { }

        protected override Guild AddItem(Guild item)
        {
            Cache.Set(item.ID, item);
            return item;
        }

        internal Guild Add(GuildData data, bool cache = true)
        {
            Snowflake snowflake = data.id;
            Guild existing = Cache.Get(snowflake);
            if (existing != null)
            {
                if (cache) existing._Patch(data);
                return existing;
            }

            var entry = new Guild(Client, data);
            if (cache) Cache.Set(snowflake, entry);
            return entry;
        }

        /// <summary>
        /// Resolves a GuildResolvable to a Guild object.
        /// </summary>
        /// <param name="guild">The guild resolvable to identify</param>
        /// <returns></returns>
        public Guild Resolve(GuildResolvable guild) => guild == null ? null : guild.Resolve(this);

        /// <summary>
        /// Resolves a GuildResolvable to a Guild ID snowflake.
        /// </summary>
        /// <param name="guild">The guild resolvable to identify</param>
        /// <returns></returns>
        public Snowflake ResolveID(GuildResolvable guild) => guild == null ? null : guild.ResolveID();
    }
}