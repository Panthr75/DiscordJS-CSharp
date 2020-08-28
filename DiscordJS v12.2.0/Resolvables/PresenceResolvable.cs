using System;

namespace DiscordJS.Resolvables
{
    /// <summary>
    /// Data that can be resolved to a Presence object. This can be:
    /// <list type="bullet">
    /// <item>A Presence</item>
    /// <item>A UserResolvable</item>
    /// <item>A Snowflake</item>
    /// </list>
    /// </summary>
    public class PresenceResolvable
    {
        internal enum Type
        {
            Presence,
            UserResolvable,
            User,
            Message,
            GuildMember,
            Snowflake
        }

        internal Type type;

        internal readonly Presence presence;
        internal readonly UserResolvable userResolvable;
        internal readonly User user;
        internal readonly Message message;
        internal readonly GuildMember member;
        internal readonly Snowflake snowflake;

        internal Presence Resolve(PresenceManager manager)
        {
            if (type == Type.Presence)
                return presence;
            else if (type == Type.UserResolvable)
                return manager.Cache.Get(manager.Client.Users.Resolve(userResolvable).ID);
            else if (type == Type.User)
                return manager.Cache.Get(user.ID);
            else if (type == Type.Message)
                return manager.Cache.Get(message.Author.ID);
            else if (type == Type.GuildMember)
                return manager.Cache.Get(member.ID);
            else if (type == Type.Snowflake)
                return manager.Cache.Get(snowflake);
            else
                throw new ArgumentException("The given type can't be cast to this resolvable");
        }

        internal Snowflake ResolveID()
        {
            if (type == Type.Presence)
                return presence.UserID;
            else if (type == Type.UserResolvable)
                return userResolvable.ResolveID();
            else if (type == Type.User)
                return user.ID;
            else if (type == Type.Message)
                return message.Author.ID;
            else if (type == Type.GuildMember)
                return member.ID;
            else if (type == Type.Snowflake)
                return snowflake;
            else
                throw new ArgumentException("The given type can't be cast to this resolvable");
        }

        private PresenceResolvable(Presence presence)
        {
            this.presence = presence;
            type = Type.Presence;
        }

        private PresenceResolvable(UserResolvable userResolvable)
        {
            this.userResolvable = userResolvable;
            type = Type.UserResolvable;
        }

        private PresenceResolvable(User user)
        {
            this.user = user;
            type = Type.User;
        }

        private PresenceResolvable(Message message)
        {
            this.message = message;
            type = Type.Message;
        }

        private PresenceResolvable(GuildMember member)
        {
            this.member = member;
            type = Type.GuildMember;
        }

        private PresenceResolvable(Snowflake snowflake)
        {
            this.snowflake = snowflake;
            type = Type.Snowflake;
        }

        private PresenceResolvable(JavaScript.String snowflake) : this((Snowflake)snowflake.ToString())
        { }

        private PresenceResolvable(string snowflake) : this((Snowflake)snowflake)
        { }

        public static implicit operator PresenceResolvable(Presence presence) => new PresenceResolvable(presence);
        public static implicit operator PresenceResolvable(UserResolvable userResolvable) => new PresenceResolvable(userResolvable);
        public static implicit operator PresenceResolvable(User user) => new PresenceResolvable(user);
        public static implicit operator PresenceResolvable(Message message) => new PresenceResolvable(message);
        public static implicit operator PresenceResolvable(GuildMember member) => new PresenceResolvable(member);
        public static implicit operator PresenceResolvable(Snowflake snowflake) => new PresenceResolvable(snowflake);
        public static implicit operator PresenceResolvable(JavaScript.String snowflake) => new PresenceResolvable(snowflake);
        public static implicit operator PresenceResolvable(string snowflake) => new PresenceResolvable(snowflake);
    }
}