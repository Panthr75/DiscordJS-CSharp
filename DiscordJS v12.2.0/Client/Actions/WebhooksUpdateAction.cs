using DiscordJS.Data;

namespace DiscordJS.Actions
{
    public class WebhooksUpdateAction : GenericAction<WebhooksUpdateAction.ActionResult>
    {
        public class ActionResult
        {
            //
        }

        public WebhooksUpdateAction(Client client) : base(client)
        { }

        public override ActionResult Handle(dynamic data) => Handle(DiscordUtil.FromDynamic<ChannelData>(data));

        public ActionResult Handle(ChannelData data)
        {
            Channel channel = Client.Channels.Cache.Get(data.id);
            if (channel != null) Client.EmitWebhookUpdate((TextChannel)channel);
            return null;
        }
    }
}