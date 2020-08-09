using DiscordJS.Data;
using DiscordJS.Resolvables;
using System.Collections.Generic;

namespace DiscordJS
{
    /// <summary>
    /// Manages API methods for roles and stores their cache.
    /// </summary>
    public class RoleManager : BaseManager<Collection<Snowflake, Role>, Role, RoleData>
    {
        /// <summary>
        /// The <c>@everyone</c> role of the guild
        /// </summary>
        public Role Everyone => Cache.Get(Guild.ID);

        /// <summary>
        /// The guild belonging to this manager
        /// </summary>
        public Guild Guild { get; internal set; }

        /// <summary>
        /// The role with the highest position in the cache
        /// </summary>
        public Role Highest => Cache.Reduce((prev, role) => role.ComparePositionTo(prev) > 0 ? role : prev, Cache.First());

        public RoleManager(Guild guild, IEnumerable<Role> iterable) : base(guild.Client, iterable)
        {
            Guild = guild;
        }

        /// <summary>
        /// Resolves a RoleResolvable to a Role object.
        /// </summary>
        /// <param name="role">The role resolvable to resolve</param>
        /// <returns></returns>
        public Role Resolve(RoleResolvable role) => role == null ? null : role.Resolve(this);

        /// <summary>
        /// Resolves a RoleResolvable to a role ID snowflake.
        /// </summary>
        /// <param name="role">The role resolvable to resolve</param>
        /// <returns></returns>
        public Snowflake ResolveID(RoleResolvable role) => role == null ? null : role.ResolveID();

        protected override Role AddItem(Role item)
        {
            Cache.Set(item.ID, item);
            return item;
        }
    }
}