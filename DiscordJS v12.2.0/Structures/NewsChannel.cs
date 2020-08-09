using DiscordJS.Data;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Represents a guild news channel on Discord.
    /// </summary>
    public class NewsChannel : TextChannel
    {
        // News channels don't have a rate limit per user, remove it
        private new int RateLimitPerUser => throw new NotSupportedException();

        public NewsChannel(Guild guild, ChannelData data) : base(guild, data)
        { }

        internal override void _Patch(ChannelData data)
        {
            base._Patch(data);

            // News channels don't have a rate limit per user, remove it
            base.RateLimitPerUser = -1;
        }

        internal virtual void _Patch(NewsChannel channel)
        {
            base._Patch(channel);

            base.RateLimitPerUser = -1;
        }

        internal override void _Patch(Channel channel)
        {
            if (channel is NewsChannel newsChannel)
                _Patch(newsChannel);
            else
                base._Patch(channel);
        }

        internal override void _Patch(GuildChannel channel)
        {
            if (channel is NewsChannel newsChannel)
                _Patch(newsChannel);
            else
                base._Patch(channel);
        }

        internal new NewsChannel _Clone()
        {
            return MemberwiseClone() as NewsChannel;
        }
    }
}