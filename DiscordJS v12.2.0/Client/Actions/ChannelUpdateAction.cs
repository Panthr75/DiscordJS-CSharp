using DiscordJS.Data;
using JavaScript;
using System;

namespace DiscordJS.Actions
{
    public class ChannelUpdateAction : GenericAction<ChannelUpdateAction.ActionResult>
    {
        public class ActionResult
        {
            public Channel Old { get; }
            public Channel Updated { get; }

            internal ActionResult(Channel old, Channel updated)
            {
                Old = old;
                Updated = updated;
            }
        }

        public ChannelUpdateAction(Client client) : base(client)
        { }

        public override ActionResult Handle(dynamic data) => Handle(DiscordUtil.FromDynamic<ChannelData>(data));

        public ActionResult Handle(ChannelData data)
        {
            Channel channel = Client.Channels.Cache.Get(data.id);
            if (channel != null)
            {
                Channel old = channel._Update(data);

                if (Enum.TryParse(channel.Type.ToUpper(), out ChannelTypes type) && (int)type != data.type)
                {
                    Guild guild = channel is GuildChannel guildChannel ? guildChannel.Guild : null;

                    GuildChannel newChannel;
                    switch((ChannelTypes)data.type)
                    {
                        case ChannelTypes.CATEGORY:
                            newChannel = new CategoryChannel(guild, data);
                            break;
                        case ChannelTypes.NEWS:
                            newChannel = new NewsChannel(guild, data);
                            break;
                        case ChannelTypes.STORE:
                            newChannel = new StoreChannel(guild, data);
                            break;
                        case ChannelTypes.TEXT:
                            newChannel = new TextChannel(guild, data);
                            break;
                        case ChannelTypes.VOICE:
                            newChannel = new VoiceChannel(guild, data);
                            break;
                        default:
                            newChannel = null;
                            break;
                    }

                    if (newChannel is ITextBasedChannel textChannel && channel is ITextBasedChannel originalText)
                    {
                        foreach (Map<Snowflake, Message>.Item item in originalText.Messages.Cache)
                            textChannel.Messages.Cache.Set(item.Key, item.Value);

                        newChannel._typing = new Map<Snowflake, TypingInfo>();

                        foreach (Map<Snowflake, TypingInfo>.Item item in channel._typing)
                            newChannel._typing.Set(item.Key, item.Value);

                        channel = newChannel;
                        Client.Channels.Cache.Set(channel.ID, channel);
                    }
                }

                return new ActionResult(old, channel);
            }

            return null;
        }
    }
}