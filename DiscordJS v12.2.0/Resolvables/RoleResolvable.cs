using System;

namespace DiscordJS.Resolvables
{
    /// <summary>
    /// Data that can be resolved to a Role object. This can be:
    /// <list type="bullet">
    /// <item>A Role</item>
    /// <item>A Snowflake</item>
    /// </list>
    /// </summary>
    public class RoleResolvable
    {
        internal enum Type
        {
            Role,
            Snowflake
        }

        internal readonly Type type;

        internal readonly Role role;
        internal readonly Snowflake snowflake;

        internal Role Resolve(RoleManager manager)
        {
            if (type == Type.Role)
                return role;
            else if (type == Type.Snowflake)
                return manager.Cache.Get(snowflake);
            else
                throw new ArgumentException("The given type can't be cast to this resolvable");
        }

        internal Snowflake ResolveID()
        {
            if (type == Type.Role)
                return role == null ? null : role.ID;
            else if (type == Type.Snowflake)
                return snowflake;
            else
                throw new ArgumentException("The given type can't be cast to this resolvable");
        }

        public RoleResolvable(Role role)
        {
            this.role = role;
            type = Type.Role;
        }

        public RoleResolvable(Snowflake snowflake)
        {
            this.snowflake = snowflake;
            type = Type.Snowflake;
        }

        public RoleResolvable(string snowflake) : this((Snowflake)snowflake)
        { }

        public RoleResolvable(JavaScript.String snowflake) : this((Snowflake)snowflake.ToString())
        { }

        public static implicit operator RoleResolvable(Role role) => new RoleResolvable(role);
        public static implicit operator RoleResolvable(Snowflake snowflake) => new RoleResolvable(snowflake);
        public static implicit operator RoleResolvable(string snowflake) => new RoleResolvable(snowflake);
        public static implicit operator RoleResolvable(JavaScript.String snowflake) => new RoleResolvable(snowflake);
    }
}