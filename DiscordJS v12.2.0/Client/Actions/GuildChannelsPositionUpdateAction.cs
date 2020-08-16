using DiscordJS.Data;

namespace DiscordJS.Actions
{
    public class GuildChannelsPositionUpdateAction : GenericAction<GuildChannelsPositionUpdateAction.ActionResult>
    {
        public class ActionResult
        {
            public Guild Guild { get; }

            internal ActionResult(Guild g)
            {
                Guild = g;
            }
        }

        public GuildChannelsPositionUpdateAction(Client client) : base(client)
        { }

        public override ActionResult Handle(dynamic data) => Handle(DiscordUtil.FromDynamic<GuildChannelPositionData>(data));

        public ActionResult Handle(GuildChannelPositionData data)
        {
            Guild guild = Client.Guilds.Cache.Get(data.guild_id);
            if (guild != null)
            {
                for (int index = 0, length = data.channels.Length; index < length; index++)
                {
                    var partialChannel = data.channels[index];
                    GuildChannel channel = guild.Channels.Cache.Get(partialChannel.id);
                    if (channel != null) channel.RawPosition = partialChannel.position.GetValueOrDefault(0);
                }
            }

            return new ActionResult(guild);
        }
    }
}