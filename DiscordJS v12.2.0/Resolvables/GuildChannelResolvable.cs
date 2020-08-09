namespace DiscordJS.Resolvables
{
    /// <summary>
    /// Data that can be resolved to give a Guild Channel object. This can be:
    /// <list type="bullet">
    /// <item>A GuildChannel object</item>
    /// <item>A Snowflake</item>
    /// </list>
    /// </summary>
    public class GuildChannelResolvable : IResolvable<GuildChannelManager, GuildChannel>
    {
        internal bool isSnowflake;
        internal GuildChannel channel;
        internal Snowflake snowflake;

        public GuildChannelResolvable(GuildChannel channel)
        {
            isSnowflake = false;
            this.channel = channel;
        }

        public GuildChannelResolvable(string snowflake) : this((Snowflake)snowflake)
        { }

        public GuildChannelResolvable(JavaScript.String snowflake) : this((Snowflake)snowflake.ToString())
        { }

        public GuildChannelResolvable(Snowflake snowflake)
        {
            isSnowflake = true;
            this.snowflake = snowflake;
        }

        internal GuildChannel Resolve(GuildChannelManager manager)
        {
            if (isSnowflake)
                return manager.Cache.Get(snowflake);
            else
                return channel;
        }

        GuildChannel IResolvable<GuildChannelManager, GuildChannel>.Resolve(GuildChannelManager arg1) => Resolve(arg1);

        public static implicit operator GuildChannelResolvable(Snowflake snowflake) => new GuildChannelResolvable(snowflake);
        public static implicit operator GuildChannelResolvable(string snowflake) => new GuildChannelResolvable(snowflake);
        public static implicit operator GuildChannelResolvable(JavaScript.String snowflake) => new GuildChannelResolvable(snowflake);
        public static implicit operator GuildChannelResolvable(GuildChannel channel) => new GuildChannelResolvable(channel);
        public static implicit operator GuildChannelResolvable(TextChannel channel) => new GuildChannelResolvable(channel);
        public static implicit operator GuildChannelResolvable(VoiceChannel channel) => new GuildChannelResolvable(channel);
        public static implicit operator GuildChannelResolvable(CategoryChannel channel) => new GuildChannelResolvable(channel);
        public static implicit operator GuildChannelResolvable(NewsChannel channel) => new GuildChannelResolvable(channel);
        public static implicit operator GuildChannelResolvable(StoreChannel channel) => new GuildChannelResolvable(channel);
    }
}