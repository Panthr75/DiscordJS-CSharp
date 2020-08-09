using DiscordJS.Data;

namespace DiscordJS
{
    /// <summary>
    /// Represents a guild store channel on Discord.
    /// </summary>
    public class StoreChannel : GuildChannel
    {
        /// <summary>
        /// If the guild considers this channel NSFW
        /// </summary>
        public bool NSFW { get; internal set; }

        public StoreChannel(Guild guild, ChannelData data) : base(guild, data)
        { }

        internal override void _Patch(ChannelData data)
        {
            base._Patch(data);

            NSFW = data.nsfw.HasValue ? data.nsfw.Value : false;
        }

        internal virtual void _Patch(StoreChannel channel)
        {
            base._Patch(channel);

            NSFW = channel.NSFW;
        }

        internal override void _Patch(Channel channel)
        {
            if (channel is StoreChannel storeChannel)
                _Patch(storeChannel);
            else
                base._Patch(channel);
        }

        internal override void _Patch(GuildChannel channel)
        {
            if (channel is StoreChannel storeChannel)
                _Patch(storeChannel);
            else
                base._Patch(channel);
        }

        internal new StoreChannel _Clone()
        {
            return MemberwiseClone() as StoreChannel;
        }
    }
}