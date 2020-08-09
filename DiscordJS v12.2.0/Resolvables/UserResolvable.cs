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
    public class UserResolvable : IResolvable<UserManager, User>, IHasID
    {
        /// <summary>
        /// The ID of this user. Is not set until it is resolved
        /// </summary>
        public Snowflake ID => User == null ? null : User.ID;

        /// <summary>
        /// The user this user resolvable references. Is not set until it is resolved
        /// </summary>
        public User User { get; internal set; }

        internal bool isUser = false;
        internal bool isSnowflake = false;
        internal bool isMessage = false;
        internal bool isGuildMember = false;
        internal User user;
        internal Snowflake snowflake;
        internal Message message;
        internal GuildMember member;

        /// <summary>
        /// Resolves this resolvable using the given user manager.
        /// <br/>
        /// <br/>
        /// <info><b>This will update <see cref="DiscordJS.User"/> if the user is resolved</b></info>
        /// </summary>
        /// <param name="manager">The user manager to use</param>
        /// <returns>A <see cref="DiscordJS.User"/>, or <see langword="null"/> if no user was found.</returns>
        public User Resolve(UserManager manager)
        {
            User result;
            if (isUser)
                result = user;
            else if (isSnowflake)
                result = manager.Cache.Get(snowflake);
            else if (isMessage)
                result = message.Author;
            else if (isGuildMember)
                result = member.User;
            else
                result = null;

            if (result != null) User = result;
            return result;
        }

        public UserResolvable(Snowflake snowflake)
        {
            isSnowflake = true;
            this.snowflake = snowflake;
        }

        public UserResolvable(string str) : this((Snowflake)str)
        { }

        public UserResolvable(JavaScript.String str) : this((Snowflake)str)
        { }

        public UserResolvable(Message message)
        {
            isMessage = true;
            this.message = message;
        }

        public UserResolvable(GuildMember member)
        {
            isGuildMember = true;
            this.member = member;
        }

        public UserResolvable(User user)
        {
            isUser = true;
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