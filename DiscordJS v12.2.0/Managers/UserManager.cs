using System.Collections.Generic;
using DiscordJS.Data;
using DiscordJS.Resolvables;
using JavaScript;

namespace DiscordJS
{
    /// <summary>
    /// Manages API methods for users and stores their cache.
    /// </summary>
    public class UserManager : BaseManager<Collection<Snowflake, User>, User, UserData>
    {
        public UserManager(Client client, IEnumerable<User> iterable) : base(client, iterable)
        {
            //
        }

        protected override User AddItem(User item) => Add(item, true);

        internal User Add(User data, bool cache = true, Snowflake id = null)
        {
            Snowflake snowflake = id == null ? data.ID : id;
            User existing = Cache.Get(snowflake);
            if (existing != null)
            {
                if (cache) existing._Patch(data);
                return existing;
            }

            var entry = new User(Client, data);
            if (cache) Cache.Set(snowflake, entry);
            return entry;
        }

        internal User Add(UserData data, bool cache = true, Snowflake id = null)
        {
            Snowflake snowflake = id == null ? (Snowflake)data.id : id;
            User existing = Cache.Get(snowflake);
            if (existing != null)
            {
                if (cache) existing._Patch(data);
                return existing;
            }

            var entry = new User(Client, data);
            if (cache) Cache.Set(snowflake, entry);
            return entry;
        }

        /// <summary>
        /// Resolves a UserResolvable to a User object.
        /// </summary>
        /// <param name="user">The UserResolvable to identify</param>
        /// <returns></returns>
        public User Resolve(UserResolvable user) => user == null ? null : user.Resolve(this);

        /// <summary>
        /// Resolves a UserResolvable to a user ID string.
        /// </summary>
        /// <param name="user">The UserResolvable to identify</param>
        /// <returns></returns>
        public Snowflake ResolveID(UserResolvable user) => user == null ? null : user.ResolveID();

        /// <summary>
        /// Obtains a user from Discord, or the user cache if it's already available.
        /// </summary>
        /// <param name="ID">ID of the user</param>
        /// <param name="cache">Whether to cache the new user object if it isn't already</param>
        /// <returns></returns>
        public IPromise<User> Fetch(Snowflake ID, bool cache = true)
        {
            User existing = Cache.Get(ID);
            if (existing != null && !existing.Partial) return Promise<User>.Resolved(existing);
            return Client.API.Users(ID).Get().Then((data) => Add(data, cache));
        }
    }
}