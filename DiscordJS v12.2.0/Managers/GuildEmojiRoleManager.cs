using DiscordJS.Resolvables;
using JavaScript;
using System.Collections.Generic;

namespace DiscordJS
{
    /// <summary>
    /// Manages API methods for roles belonging to emojis and stores their cache.
    /// </summary>
    public class GuildEmojiRoleManager
    {
        /// <summary>
        /// The cache of roles belonging to this emoji
        /// </summary>
        public Collection<Snowflake, Role> Cache => _Roles;

        /// <summary>
        /// The client belonging to this manager
        /// </summary>
        public Client Client { get; }

        /// <summary>
        /// The emoji belonging to this manager
        /// </summary>
        public GuildEmoji Emoji { get; internal set; }

        /// <summary>
        /// The guild belonging to this manager
        /// </summary>
        public Guild Guild { get; internal set; }

        /// <summary>
        /// The filtered collection of roles of the guild emoji
        /// </summary>
        /// <returns></returns>
        internal Collection<Snowflake, Role> _Roles => Guild.Roles.Cache.Filter(role => Emoji._roles.Includes(role.ID));


        /// <summary>
        /// Instantiates a new GuildEmojiRoleManager
        /// </summary>
        /// <param name="emoji">The emoji</param>
        public GuildEmojiRoleManager(GuildEmoji emoji)
        {
            Emoji = emoji;
            Guild = emoji.Guild;
            Client = emoji.Client;
        }

        /// <summary>
        /// Adds a role (or multiple roles) to the list of roles that can use this emoji.
        /// </summary>
        /// <param name="role">The role or roles to add</param>
        /// <returns></returns>
        public IPromise<GuildEmoji> Add(RoleResolvable role) => Add(new RoleResolvable[1] { role });

        /// <summary>
        /// Adds a role (or multiple roles) to the list of roles that can use this emoji.
        /// </summary>
        /// <param name="roles">The role or roles to add</param>
        /// <returns></returns>
        public IPromise<GuildEmoji> Add(IEnumerable<RoleResolvable> roles)
        {
            Array<Snowflake> r = new Array<RoleResolvable>(roles).Map((role) => Guild.Roles.ResolveID(role));
            if (r.Includes(null))
            {
                return Promise<GuildEmoji>.Rejected(new DJSError.Error("INVALID_TYPE", "roles", "Array or Collection of Roles or Snowflakes", true));
            }
            var newRoles = r.Map((role) => (RoleResolvable)role).Concat(new Array<Role>(_Roles.Values()).Map((role) => (RoleResolvable)role));
            return Set(newRoles);
        }

        /// <summary>
        /// Adds a role (or multiple roles) to the list of roles that can use this emoji.
        /// </summary>
        /// <param name="roles">The role or roles to add</param>
        /// <returns></returns>
        public IPromise<GuildEmoji> Add(Collection<Snowflake, Role> roles) => Add(roles.KeyArray().Map((role) => (RoleResolvable)role));

        /// <summary>
        /// Removes a role (or multiple roles) from the list of roles that can use this emoji.
        /// </summary>
        /// <param name="role">The role or roles to remove</param>
        /// <returns></returns>
        public IPromise<GuildEmoji> Remove(RoleResolvable role) => Remove(new RoleResolvable[1] { role });

        /// <summary>
        /// Removes a role (or multiple roles) from the list of roles that can use this emoji.
        /// </summary>
        /// <param name="roles">The role or roles to remove</param>
        /// <returns></returns>
        public IPromise<GuildEmoji> Remove(IEnumerable<RoleResolvable> roles)
        {
            Array<Snowflake> r = new Array<RoleResolvable>(roles).Map((role) => Guild.Roles.ResolveID(role));
            if (r.Includes(null))
            {
                return Promise<GuildEmoji>.Rejected(new DJSError.Error("INVALID_TYPE", "roles", "Array or Collection of Roles or Snowflakes", true));
            }
            var newRoles = _Roles.KeyArray().Filter((role) => !r.Includes(role)).Map((role) => (RoleResolvable)role);
            return Set(newRoles);
        }

        /// <summary>
        /// Removes a role (or multiple roles) from the list of roles that can use this emoji.
        /// </summary>
        /// <param name="roles">The role or roles to remove</param>
        /// <returns></returns>
        public IPromise<GuildEmoji> Remove(Collection<Snowflake, Role> roles) => Remove(roles.KeyArray().Map((role) => (RoleResolvable)role));

        /// <summary>
        /// Sets the role(s) that can use this emoji.
        /// </summary>
        /// <param name="roles">The roles or role IDs to apply</param>
        /// <returns></returns>
        public IPromise<GuildEmoji> Set(Collection<Snowflake, Role> roles)
        {
            Array<RoleResolvable> r = new Array<RoleResolvable>();
            foreach (Snowflake key in roles.KeyArray())
                r.Push(key);
            return Emoji.Edit(new GuildEmojiEditData() { roles = r.ToArray() });
        }

        /// <summary>
        /// Sets the role(s) that can use this emoji.
        /// </summary>
        /// <param name="roles">The roles or role IDs to apply</param>
        /// <returns></returns>
        public IPromise<GuildEmoji> Set(IEnumerable<RoleResolvable> roles)
        {
            return Emoji.Edit(new GuildEmojiEditData() { roles = new Array<RoleResolvable>(roles).ToArray() });
        }

        /// <inheritdoc/>
        public GuildEmojiRoleManager Clone()
        {
            var clone = new GuildEmojiRoleManager(Emoji);
            clone._Patch(_Roles.KeyArray().Slice());
            return clone;
        }

        /// <summary>
        /// Patches the roles for this manager's cache
        /// </summary>
        /// <param name="roles">The new roles</param>
        internal void _Patch(Array<Snowflake> roles)
        {
            Emoji._roles = roles;
        }
    }
}