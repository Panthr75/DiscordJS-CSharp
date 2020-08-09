namespace DiscordJS.Resolvables
{
    /// <summary>
    /// Data that resolves to give a GuildMember object. This can be:
    /// <list type="bullet">
    /// <item>A GuildMember object</item>
    /// <item>A User resolvable</item>
    /// <item>A User object</item>
    /// <item>A Snowflake</item>
    /// <item>A Message object (resolves to the message member)</item>
    /// </list>
    /// </summary>
    public class GuildMemberResolvable : IResolvable<GuildMemberManager, GuildMember>, IHasID
    {
        /// <summary>
        /// The ID of this guild member. Is not set until it is resolved
        /// </summary>
        public Snowflake ID => GuildMember == null ? null : GuildMember.ID;

        /// <summary>
        /// The user this guild member resolvable references. Is not set until it is resolved
        /// </summary>
        public GuildMember GuildMember { get; internal set; }

        internal bool isGuildMember = false;
        internal bool isUser = false;
        internal bool isSnowflake = false;
        internal bool isMessage = false;
        internal GuildMember member;
        internal User user;
        internal Snowflake snowflake;
        internal Message message;

        /// <summary>
        /// Resolves this resolvable using the given guild member manager.
        /// <br/>
        /// <br/>
        /// <info><b>This will update <see cref="DiscordJS.GuildMember"/> if the guild member is resolved</b></info>
        /// </summary>
        /// <param name="manager">The guild member manager to use</param>
        /// <returns>A <see cref="DiscordJS.GuildMember"/>, or <see langword="null"/> if no guild member was found.</returns>
        public GuildMember Resolve(GuildMemberManager manager)
        {
            GuildMember result;
            if (isGuildMember)
                result = member;
            else if (isSnowflake)
                result = manager.Cache.Get(snowflake);
            else if (isMessage)
                result = message.Member;
            else if (isUser)
                result = manager.Cache.Get(user.ID);
            else
                result = null;

            if (result != null) GuildMember = result;
            return result;
        }

        public GuildMemberResolvable(UserResolvable userResolvable)
        {
            isSnowflake = userResolvable.isSnowflake;
            isUser = userResolvable.isUser;
            isGuildMember = userResolvable.isGuildMember;
            isMessage = userResolvable.isMessage;
            snowflake = userResolvable.snowflake;
            user = userResolvable.user;
            member = userResolvable.member;
            message = userResolvable.message;
        }

        public GuildMemberResolvable(Snowflake snowflake)
        {
            isSnowflake = true;
            this.snowflake = snowflake;
        }

        public GuildMemberResolvable(string str) : this((Snowflake)str)
        { }

        public GuildMemberResolvable(JavaScript.String str) : this((Snowflake)str)
        { }

        public GuildMemberResolvable(Message message)
        {
            isMessage = true;
            this.message = message;
        }

        public GuildMemberResolvable(GuildMember member)
        {
            isGuildMember = true;
            this.member = member;
        }

        public GuildMemberResolvable(User user)
        {
            isUser = true;
            this.user = user;
        }

        public static implicit operator GuildMemberResolvable(Snowflake snowflake) => new GuildMemberResolvable(snowflake);
        public static implicit operator GuildMemberResolvable(string str) => new GuildMemberResolvable(str);
        public static implicit operator GuildMemberResolvable(JavaScript.String str) => new GuildMemberResolvable(str);
        public static implicit operator GuildMemberResolvable(Message message) => new GuildMemberResolvable(message);
        public static implicit operator GuildMemberResolvable(GuildMember member) => new GuildMemberResolvable(member);
        public static implicit operator GuildMemberResolvable(User user) => new GuildMemberResolvable(user);
        public static implicit operator GuildMemberResolvable(UserResolvable userResolvable) => new GuildMemberResolvable(userResolvable);
    }
}