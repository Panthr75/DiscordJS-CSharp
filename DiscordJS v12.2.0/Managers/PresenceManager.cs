using DiscordJS.Data;
using DiscordJS.Resolvables;
using System.Collections.Generic;

namespace DiscordJS
{
    /// <summary>
    /// Manages API methods for Presences and holds their cache.
    /// </summary>
    public class PresenceManager : BaseManager<Collection<Snowflake, Presence>, Presence, PresenceData>
    {
        public PresenceManager(Client client) : this(client, null)
        { }

        public PresenceManager(Client client, IEnumerable<Presence> iterable) : base(client, iterable)
        { }

        protected override Presence AddItem(Presence item)
        {
            Cache.Set(item.UserID, item);
            return item;
        }

        public Presence Add(PresenceData data, bool cache = false)
        {
            Presence existing = Cache.Get(data.user.id);
            return existing != null ? existing.Patch(data) : Add(data, cache, data.user.id);
        }

        private Presence Add(PresenceData data, bool cache, string userID)
        {
            Presence existing = Cache.Get(userID);
            if (existing != null) return existing;

            Presence entry = new Presence(Client, data);
            if (cache) Cache.Set(userID == null ? entry.UserID : (Snowflake)userID, entry);
            return entry;
        }

        /// <summary>
        /// Resolves a PresenceResolvable to a Presence object.
        /// </summary>
        /// <param name="presence">The presence resolvable to resolve</param>
        /// <returns></returns>
        public Presence Resolve(PresenceResolvable presence) => presence == null ? null : presence.Resolve(this);

        /// <summary>
        /// Resolves a PresenceResolvable to a Presence ID snowflake.
        /// </summary>
        /// <param name="presence">The presence resolvable to resolve</param>
        /// <returns></returns>
        public Snowflake ResolveID(PresenceResolvable presence) => presence == null ? null : presence.ResolveID();
    }
}