using DiscordJS.Data;
using System;

namespace DiscordJS.Actions
{
    public class MessageUpdateAction : GenericAction<MessageUpdateAction.ActionResult>
    {
        public class ActionResult
        {
            public Message Old { get; }
            public Message Updated { get; }

            internal ActionResult(Message old, Message updated)
            {
                Old = old;
                Updated = updated;
            }
        }

        public MessageUpdateAction(Client client) : base(client)
        { }

        public override ActionResult Handle(dynamic data) => Handle(DiscordUtil.FromDynamic<MessageData>(data));

        public ActionResult Handle(MessageData data)
        {
            dynamic channel = GetChannel(data);
            bool isChannel = channel is Channel;
            if (channel != null)
            {
                dynamic message = GetMessage(new
                {
                    id = isChannel ? (string)channel.ID : null,
                    channel_id = isChannel ? null : channel.channel_id,
                    guild_id = isChannel ? null : channel.guild_id,
                    author = isChannel ? channel.Author : channel.author,
                    timestamp = isChannel ? null : channel.timestamp,
                    type = isChannel ? Enum.TryParse<ChannelTypes>(channel.Type.ToUpper(), out ChannelTypes types) ? (int?)types : null : channel.type
                }, channel);
                if (message != null && message is Message)
                {
                    Message msg = message as Message;
                    msg._Patch(data);
                    return new ActionResult(msg._edits[0], msg);
                }
            }

            return new ActionResult(null, null);
        }
    }
}