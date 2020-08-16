using DiscordJS.Data;
using JavaScript;

namespace DiscordJS.Actions
{
    public class ChannelDeleteAction : GenericAction<ChannelDeleteAction.ActionResult>
    {
        public class ActionResult
        {
            public Channel Channel { get; }
            internal ActionResult(Channel c)
            {
                Channel = c;
            }
        }

        internal Map<object, object> deleted;

        public ChannelDeleteAction(Client client) : base(client)
        {
            deleted = new Map<object, object>();
        }

        public override ActionResult Handle(dynamic data) => Handle(DiscordUtil.FromDynamic<ChannelData>(data));

        public ActionResult Handle(ChannelData data)
        {
            Channel channel = Client.Channels.Cache.Get(data.id);

            if (channel != null)
            {
                Client.Channels.Remove(channel.ID);
                channel.Deleted = true;
                if (channel is ITextBasedChannel textChannel && !(textChannel is DMChannel))
                {
                    foreach (Message message in textChannel.Messages.Cache.Values())
                    {
                        message.Deleted = true;
                    }
                }

                Client.EmitChannelDelete(channel);
            }

            return new ActionResult(channel);
        }
    }
}