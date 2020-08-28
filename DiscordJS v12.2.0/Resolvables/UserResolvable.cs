using System;

namespace DiscordJS.Resolvables
{
    /// <summary>
    /// Data that resolves to give a User object. This can be:
    /// <list type="bullet">
    /// <item>A User object</item>
    /// <item>A Snowflake</item>
    /// <item>A Message object (resolves to the message author)</item>
    /// <item>A GuildMember object</item>
    /// </list>
    /// </summary>
    public class UserResolvable
    {
        internal enum Type
        {
            User,
            Message,
            GuildMember,
            Snowflake
        }

        internal readonly Type type;

        internal readonly User user;
        internal readonly Snowflake snowflake;
        internal readonly Message message;
        internal readonly GuildMember member;

        internal User Resolve(UserManager manager)
        {
            if (type == Type.User)
                return user;
            else if (type == Type.Message)
                return message.Author;
            else if (type == Type.GuildMember)
                return member.User;
            else if (type == Type.Snowflake)
                return manager.Cache.Get(snowflake);
            else
                throw new ArgumentException("The given type can't be cast to this resolvable");
        }

        internal Snowflake ResolveID()
        {
            if (type == Type.User)
                return user.ID;
            else if (type == Type.Message)
                return message.Author.ID;
            else if (type == Type.GuildMember)
                return member.User.ID;
            else if (type == Type.Snowflake)
                return snowflake;
            else
                throw new ArgumentException("The given type can't be cast to this resolvable");
        }

        private UserResolvable(Snowflake snowflake)
        {
            type = Type.Snowflake;
            this.snowflake = snowflake;
        }

        private UserResolvable(string str) : this((Snowflake)str)
        { }

        private UserResolvable(JavaScript.String str) : this((Snowflake)str)
        { }

        private UserResolvable(Message message)
        {
            type = Type.Message;
            this.message = message;
        }

        private UserResolvable(GuildMember member)
        {
            type = Type.GuildMember;
            this.member = member;
        }

        private UserResolvable(User user)
        {
            type = Type.User;
            this.user = user;
        }

        public static implicit operator UserResolvable(Snowflake snowflake) => new UserResolvable(snowflake);
        public static implicit operator UserResolvable(string str) => new UserResolvable(str);
        public static implicit operator UserResolvable(JavaScript.String str) => new UserResolvable(str);
        public static implicit operator UserResolvable(Message message) => new UserResolvable(message);
        public static implicit operator UserResolvable(GuildMember member) => new UserResolvable(member);
        public static implicit operator UserResolvable(User user) => new UserResolvable(user);
    }
}