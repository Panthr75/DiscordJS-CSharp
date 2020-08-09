namespace DiscordJS.Resolvables
{
    /// <summary>
    /// Data that can be resolved to give a Channel object. This can be:
    /// <list type="bullet">
    /// <item>A Channel object</item>
    /// <item>A Snowflake</item>
    /// </list>
    /// </summary>
    public class ChannelResolvable : IResolvable<ChannelManager, Channel>
    {
        internal bool isSnowflake;
        internal Snowflake snowflake;
        internal Channel channel;

        internal Channel Resolve(ChannelManager manager)
        {
            if (isSnowflake)
                return manager.Cache.Get(snowflake);
            else
                return channel;
        }

        public ChannelResolvable(Channel channel)
        {
            isSnowflake = false;
            this.channel = channel;
        }

        public ChannelResolvable(Snowflake snowflake)
        {
            isSnowflake = true;
            this.snowflake = snowflake;
        }

        public ChannelResolvable(string snowflake) : this((Snowflake)snowflake)
        { }

        public ChannelResolvable(JavaScript.String snowflake) : this((Snowflake)snowflake.ToString())
        { }

        Channel IResolvable<ChannelManager, Channel>.Resolve(ChannelManager arg1) => Resolve(arg1);

        public static implicit operator ChannelResolvable(Channel channel) => new ChannelResolvable(channel);
        public static implicit operator ChannelResolvable(DMChannel channel) => new ChannelResolvable(channel);
        public static implicit operator ChannelResolvable(GuildChannel channel) => new ChannelResolvable(channel);
        public static implicit operator ChannelResolvable(TextChannel channel) => new ChannelResolvable(channel);
        public static implicit operator ChannelResolvable(VoiceChannel channel) => new ChannelResolvable(channel);
        public static implicit operator ChannelResolvable(CategoryChannel channel) => new ChannelResolvable(channel);
        public static implicit operator ChannelResolvable(NewsChannel channel) => new ChannelResolvable(channel);
        public static implicit operator ChannelResolvable(StoreChannel channel) => new ChannelResolvable(channel);
        public static implicit operator ChannelResolvable(Snowflake snowflake) => new ChannelResolvable(snowflake);
        public static implicit operator ChannelResolvable(string snowflake) => new ChannelResolvable(snowflake);
        public static implicit operator ChannelResolvable(JavaScript.String snowflake) => new ChannelResolvable(snowflake);
    }
}