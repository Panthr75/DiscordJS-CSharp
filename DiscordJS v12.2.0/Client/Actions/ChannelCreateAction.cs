using DiscordJS.Data;

namespace DiscordJS.Actions
{
    public class ChannelCreateAction : GenericAction<ChannelCreateAction.ActionResult>
    {
        public class ActionResult
        {
            public Channel Channel { get; }

            internal ActionResult(Channel c)
            {
                Channel = c;
            }
        }

        public ChannelCreateAction(Client client) : base(client)
        { }

        public override ActionResult Handle(dynamic data) => Handle(DiscordUtil.FromDynamic<ChannelData>(data));

        public ActionResult Handle(ChannelData data)
        {
            bool existing = Client.Channels.Cache.Has(data.id);
            Channel channel = Client.Channels.Add(data, null);
            if (!existing && channel != null)
            {
                Client.EmitChannelCreate(channel);
            }
            return new ActionResult(channel);
        }
    }
}