using DiscordJS.Data;
using DiscordJS.Resolvables;
using JavaScript;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Represents a role on Discord.
    /// </summary>
    public class Role : Base, IHasID
    {
        /// <summary>
        /// The base 10 color of the role
        /// </summary>
        public int Color { get; internal set; }

        /// <summary>
        /// The time the role was created at
        /// </summary>
        public Date CreatedAt => Snowflake.Deconstruct(ID).Date;

        /// <summary>
        /// The timestamp the role was created at
        /// </summary>
        public long CreatedTimestamp => Snowflake.Deconstruct(ID).Timestamp;

        /// <summary>
        /// Whether the role has been deleted
        /// </summary>
        public bool Deleted { get; internal set; }

        /// <summary>
        /// Whether the role is editable by the client user
        /// </summary>
        public bool Editable
        {
            get
            {
                if (Managed) return false;
                GuildMember clientMember = Guild.Member(Client.User);
                if (!clientMember.Permissions.Has(Permissions.FLAGS.MANAGE_ROLES)) return false;
                return clientMember.Roles.Highest.ComparePositionTo(this) > 0;
            }
        }

        /// <summary>
        /// The guild that the role belongs to
        /// </summary>
        public Guild Guild { get; internal set; }

        /// <summary>
        /// The hexadecimal version of the role color, with a leading hashtag
        /// </summary>
        public string HexColor => $"#{new JavaScript.String(Convert.ToString(Color, 16)).PadStart(6, "0")}";

        /// <summary>
        /// If true, users that are part of this role will appear in a separate category in the users list
        /// </summary>
        public bool Hoist { get; internal set; }

        /// <summary>
        /// The ID of the role (unique to the guild it is part of)
        /// </summary>
        public Snowflake ID { get; internal set; }

        /// <summary>
        /// Whether or not the role is managed by an external service
        /// </summary>
        public bool Managed { get; internal set; }

        /// <summary>
        /// The cached guild members that have this role
        /// </summary>
        public Collection<Snowflake, GuildMember> Members => Guild.Members.Cache.Filter((m) => m.Roles.Cache.Has(ID));

        /// <summary>
        /// Whether or not the role can be mentioned by anyone
        /// </summary>
        public bool Mentionable { get; internal set; }

        /// <summary>
        /// The name of the role
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The permissions of the role
        /// </summary>
        public Permissions Permissions { get; internal set; }

        /// <summary>
        /// The position of the role in the role manager
        /// </summary>
        public int Position
        {
            get
            {
                Collection<Snowflake, Role> sorted = Guild._SortedRoles();
                return sorted.Array().IndexOf(sorted.Get(ID));
            }
        }

        /// <summary>
        /// The raw position of the role from the API
        /// </summary>
        public int RawPosition { get; internal set; }

        /// <summary>
        /// Instantiates a new role
        /// </summary>
        /// <param name="client">The instantiating client</param>
        /// <param name="data">The data for the role</param>
        /// <param name="guild">The guild the role is part of</param>
        public Role(Client client, RoleData data, Guild guild) : base(client)
        {
            Guild = guild;
        }

        internal void _Patch(RoleData data)
        {
            ID = data.id;
            Name = data.name;
            Color = data.color;
            Hoist = data.hoist;
            RawPosition = data.position;
            Permissions = new Permissions(data.permissions).Freeze();
            Managed = data.managed;
            Mentionable = data.mentionable;
            Deleted = false;
        }

        internal new Role _Clone() => MemberwiseClone() as Role;

        /// <summary>
        /// Compares this role's position to another role's.
        /// </summary>
        /// <param name="role">Role to compare to this one</param>
        /// <returns>Negative number if this role's position is lower (other role's is higher), positive number if this one is higher (other's is lower), 0 if equal</returns>
        public int ComparePositionTo(RoleResolvable role)
        {
            Role r = Guild.Roles.Resolve(role);
            if (r == null) throw new DJSError.Error("INVALID_TYPE", "role", "Role nor a Snowflake");
            return ComparePositions(this, r);
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="reason">Reason for deleting this role</param>
        /// <returns></returns>
        public IPromise<Role> Delete(string reason = null)
        {
            return Client.API.Guilds(Guild.ID).Roles(ID).Delete(new { reason }).Then(() =>
            {
                Client.Actions.GuildRoleDelete.Handle(Guild.ID, ID);
                return this;
            });
        }

        /// <summary>
        /// Edits the role.
        /// </summary>
        /// <param name="data">The new data for the role</param>
        /// <param name="reason">Reason for editing this role</param>
        /// <returns></returns>
        public IPromise<Role> Edit(RoleEditData data, string reason = null)
        {
            if (data == null) data = new RoleEditData();
            long permissions = (data.permissions == null ? Permissions : new Permissions(data.permissions)).Bit;
            IPromise<object> promise;
            if (data.position.HasValue)
                promise = DiscordUtil.SetPosition(this, data.position.Value, Guild._SortedRoles(), Client.API.Guilds(Guild.ID).Roles(), reason).Then((updatedRoles) =>
                {
                    Client.Actions.GuildRolesPositionUpdate.Handle(Guild.ID, updatedRoles);
                    return null;
                });
            else promise = Promise<object>.Resolved(null);
            return promise.Then((_) =>
            {
                return Client.API.Guilds(Guild.ID).Roles(ID).Patch(new
                {
                    data = new
                    {
                        name = data.name == null ? Name : data.name,
                        color = DiscordUtil.ResolveColor(data.color == null ? (ColorResolvable)Color : data.color),
                        hoist = data.hoist.HasValue ? data.hoist.Value : Hoist,
                        permissions,
                        mentionable = data.mentionable.HasValue ? data.mentionable.Value : Mentionable
                    },
                    reason
                }).Then((RoleData role) =>
                {
                    var clone = _Clone();
                    clone._Patch(role);
                    return clone;
                });
            });
        }

        /// <summary>
        /// Whether this role equals another role. It compares all properties, so for most operations it is advisable to just compare <c>role.ID == role2.ID</c> as it is much faster and is often what most users need.
        /// </summary>
        /// <param name="role">Role to compare with</param>
        /// <returns></returns>
        public bool Equals(Role role)
        {
            return (role != null && ID == role.ID && Name == role.Name && Color == role.Color && Hoist == role.Hoist && Position == role.Position && Permissions.Bit == role.Permissions.Bit && Managed == role.Managed);
        }

        /// <summary>
        /// Returns <c>channel.PermissionsFor(role)</c>. Returns permissions for a role in a guild channel, taking into account permission overwrites.
        /// </summary>
        /// <param name="channel">The guild channel to use as context</param>
        /// <returns></returns>
        public Permissions PermissionsIn(GuildChannelResolvable channel)
        {
            GuildChannel c = Guild.Channels.Resolve(channel);
            if (c == null) throw new DJSError.Error("GUILD_CHANNEL_RESOLVE");
            return c.RolePermissions(this);
        }

        /// <summary>
        /// Sets a new color for the role.
        /// </summary>
        /// <param name="color">The color of the role</param>
        /// <param name="reason">Reason for changing the role's color</param>
        /// <returns></returns>
        public IPromise<Role> SetColor(ColorResolvable color, string reason = null)
        {
            return Edit(new RoleEditData() { color = color }, reason);
        }

        /// <summary>
        /// Sets whether or not the role should be hoisted.
        /// </summary>
        /// <param name="hoist">Whether or not to hoist the role</param>
        /// <param name="reason">Reason for setting whether or not the role should be hoisted</param>
        /// <returns></returns>
        public IPromise<Role> SetHoist(bool hoist, string reason = null)
        {
            return Edit(new RoleEditData() { hoist = hoist }, reason);
        }

        /// <summary>
        /// Sets whether this role is mentionable.
        /// </summary>
        /// <param name="mentionable">Whether this role should be mentionable</param>
        /// <param name="reason">Reason for setting whether or not this role should be mentionable</param>
        /// <returns></returns>
        public IPromise<Role> SetMentionable(bool mentionable, string reason = null)
        {
            return Edit(new RoleEditData() { mentionable = mentionable }, reason);
        }

        /// <summary>
        /// Sets a new name for the role.
        /// </summary>
        /// <param name="name">The new name of the role</param>
        /// <param name="reason">Reason for changing the role's name</param>
        /// <returns></returns>
        public IPromise<Role> SetName(string name, string reason = null)
        {
            return Edit(new RoleEditData() { name = name }, reason);
        }

        /// <summary>
        /// Sets the permissions of the role.
        /// </summary>
        /// <param name="permissions">The permissions of the role</param>
        /// <param name="reason">Reason for changing the role's permissions</param>
        /// <returns></returns>
        public IPromise<Role> SetPermissions(PermissionResolvable permissions, string reason = null)
        {
            return Edit(new RoleEditData() { permissions = permissions }, reason);
        }

        /// <summary>
        /// Sets the position of the role.
        /// </summary>
        /// <param name="position">The position of the role</param>
        /// <param name="relative">Change the position relative to its current value</param>
        /// <param name="reason">Reason for changing the position</param>
        /// <returns></returns>
        public IPromise<Role> SetPosition(int position, bool relative = false, string reason = null)
        {
            return DiscordUtil.SetPosition(this, position, relative, Guild._SortedRoles(), Client.API.Guilds(Guild.ID).Roles(), reason).Then((updatedRoles) =>
            {
                Client.Actions.GuildRolesPositionUpdate.Handle(Guild.ID, updatedRoles);
                return this;
            });
        }

        /// <summary>
        /// When concatenated with a string, this automatically returns the role's mention instead of the Role object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (ID == Guild.ID) return "@everyone";
            return $"<@&{ID}>";
        }

        /// <summary>
        /// Compares the positions of two roles.
        /// </summary>
        /// <param name="role1">First role to compare</param>
        /// <param name="role2">Second role to compare</param>
        /// <returns>Negative number if the first role's position is lower (second role's is higher), positive number if the first's is higher (second's is lower), 0 if equal</returns>
        public static int ComparePositions(Role role1, Role role2)
        {
            int role1Pos = role1.Position, role2Pos = role2.Position;
            if (role1Pos == role2Pos) return int.Parse(role2.ID) - int.Parse(role1.ID);
            return role1Pos - role2Pos;
        }
    }

    /// <summary>
    /// The data for editing a role.
    /// </summary>
    public class RoleEditData
    {
        /// <summary>
        /// The name of the role
        /// </summary>
        public string name;

        /// <summary>
        /// The color of the role, either a hex string or a base 10 number
        /// </summary>
        public ColorResolvable color;

        /// <summary>
        /// Whether or not the role should be hoisted
        /// </summary>
        public bool? hoist;

        /// <summary>
        /// The position of the role
        /// </summary>
        public int? position;

        /// <summary>
        /// The permissions of the role
        /// </summary>
        public PermissionResolvable permissions;

        /// <summary>
        /// Whether or not the role should be mentionable
        /// </summary>
        public bool? mentionable;
    }
}