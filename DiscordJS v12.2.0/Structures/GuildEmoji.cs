using DiscordJS.Data;
using DiscordJS.Resolvables;
using JavaScript;
using System;

namespace DiscordJS
{
    public class GuildEmojiEditData
    {
        /// <summary>
        /// The name of the emoji
        /// </summary>
        public string name;

        /// <summary>
        /// Roles to restrict emoji to
        /// </summary>
        public RoleResolvable[] roles;
    }

    /// <summary>
    /// Represents a custom emoji.
    /// </summary>
    public class GuildEmoji : BaseGuildEmoji
    {
        /// <summary>
        /// Whether the emoji is deletable by the client user
        /// </summary>
        public bool Deletable
        {
            get
            {
                if (Guild.Me == null) throw new DJSError.Error("GUILD_UNCACHED_ME");
                return !Managed && Guild.Me.HasPermission(Permissions.FLAGS.MANAGE_EMOJIS);
            }
        }

        /// <summary>
        /// Whether this emoji is managed by an external service
        /// </summary>
        public bool Managed { get; internal set; }

        /// <summary>
        /// Whether this emoji is available
        /// </summary>
        public bool Available { get; internal set; }

        /// <summary>
        /// A manager for roles this emoji is active for.
        /// </summary>
        public GuildEmojiRoleManager Roles => new GuildEmojiRoleManager(this);

        /// <summary>
        /// Whether or not this emoji requires colons surrounding it
        /// </summary>
        public bool RequiresColons { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client">The instantiating client</param>
        /// <param name="data">The data for the guild emoji</param>
        /// <param name="guild">The guild the guild emoji is part of</param>
        public GuildEmoji(Client client, EmojiData data, Guild guild) : base(client, data, guild)
        {

        }

        internal new GuildEmoji _Clone()
        {
            GuildEmoji clone = MemberwiseClone() as GuildEmoji;
            return clone;
        }

        internal new void _Patch(EmojiData data)
        {
            base._Patch(data);

            if (data.require_colons.HasValue) RequiresColons = data.require_colons.Value;
            if (data.managed.HasValue) Managed = data.managed.Value;
            if (data.available.HasValue) Available = data.available.Value;
        }

        /// <summary>
        /// Fetches the author for this emoji
        /// </summary>
        /// <returns></returns>
        public IPromise<User> FetchAuthor()
        {
            if (Managed)
            {
                return Promise<User>.Rejected(new DJSError.Error("EMOJI_MANAGED"));
            }
            else
            {
                if (Guild.Me == null) throw new DJSError.Error("GUILD_UNCACHED_ME");
                if (!Guild.Me.Permissions.Has(Permissions.FLAGS.MANAGE_EMOJIS))
                {
                    return Promise<User>.Rejected(new DJSError.Error("MISSING_MANAGE_EMOJIS_PERMISSION", Guild));
                }
            }
            return Client.API
                .Guilds(Guild.ID)
                .Emojis(ID)
                .Get()
                .Then((emoji) => Client.Users.Add(emoji.user));
        }

        /// <summary>
        /// Edits the emoji.
        /// </summary>
        /// <param name="data">The new data for the emoji</param>
        /// <param name="reason">Reason for editing this emoji</param>
        /// <returns></returns>
        public IPromise<GuildEmoji> Edit(GuildEmojiEditData data, string reason = null)
        {
            Func<RoleResolvable, Snowflake> mapFn = (r) => r.Resolve(Guild);
            var roles = data.roles != null ? 
                new Array<RoleResolvable>(data.roles).Map<Snowflake>(mapFn).ToArray() : 
                null;
            return Client.API
                .Guilds(Guild.ID)
                .Emojis(ID)
                .Patch(new
                {
                    data = new
                    {
                        data.name,
                        roles
                    },
                    reason
                }).Then((newData) =>
                {
                    var clone = _Clone();
                    clone._Patch(newData);
                    return clone;
                });
        }

        /// <summary>
        /// Sets the name of the emoji.
        /// </summary>
        /// <param name="name">The new name for the emoji</param>
        /// <param name="reason">Reason for changing the emoji's name</param>
        /// <returns></returns>
        public IPromise<GuildEmoji> SetName(string name, string reason = null) => Edit(new GuildEmojiEditData() { name = name }, reason);

        /// <summary>
        /// Deletes the emoji.
        /// </summary>
        /// <param name="reason">Reason for deleting the emoji</param>
        /// <returns></returns>
        public IPromise<GuildEmoji> Delete(string reason = null)
        {
            return Client.API
                .Guilds(Guild.ID)
                .Emojis(ID)
                .Delete(new { reason })
                .Then((_) => this);
        }

        /// <summary>
        /// Whether this emoji is the same as another one.
        /// </summary>
        /// <param name="other">The emoji to compare it to</param>
        /// <returns>Whether the emoji is equal to the given emoji or not</returns>
        public bool Equals(dynamic other)
        {
            if (other is GuildEmoji)
            {
                Func<Role, bool> everyRoleCB = (role) => Roles.Cache.Has(role.ID);
                return other.ID == ID &&
                    other.Name == Name &&
                    other.Managed == Managed &&
                    other.RequiresColons == RequiresColons &&
                    other.Roles.Cache.Size == Roles.Cache.Size &&
                    other.Roles.Cache.Every(everyRoleCB);
            }
            else
            {
                Func<dynamic, bool> everyCB = (role) => Roles.Cache.Has(role);
                try
                {
                    return other.ID == ID &&
                        other.Name == Name &&
                        other.Roles.Length == Roles.Cache.Size &&
                        other.Roles.Every(everyCB);
                }
                catch(Exception)
                {
                    return false;
                }
            }
        }
    }
}