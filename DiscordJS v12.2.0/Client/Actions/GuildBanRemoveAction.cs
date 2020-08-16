using DiscordJS.Data;

namespace DiscordJS.Actions
{
    public class GuildBanRemoveAction : GenericAction<GuildBanRemoveAction.ActionResult>
    {
        public class ActionResult
        {
            internal ActionResult()
            { }
        }

        public GuildBanRemoveAction(Client client) : base(client)
        { }

        public override ActionResult Handle(dynamic data) => Handle(DiscordUtil.FromDynamic<GuildBanData>(data));

        public ActionResult Handle(GuildBanData data)
        {
            Guild guild = Client.Guilds.Cache.Get(data.guild_id);
            User user = Client.Users.Add(data.user);
            if (guild != null && user != null) Client.EmitGuildBanRemove(guild, user);
            return null;
        }
    }
}