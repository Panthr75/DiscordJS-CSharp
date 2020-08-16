using DiscordJS.Data;

namespace DiscordJS.Actions
{
    public class PresenceUpdateAction : GenericAction<PresenceUpdateAction.ActionResult>
    {
        public class ActionResult
        {
            //
        }

        public PresenceUpdateAction(Client client) : base(client)
        { }

        public override ActionResult Handle(dynamic data) => Handle(DiscordUtil.FromDynamic<PresenceUpdateData>(data));

        public ActionResult Handle(PresenceUpdateData data)
        {
            User user = data.user == null ? Client.Users.Cache.Get(data.user.id) : null;
            if (user == null && data.user != null && !string.IsNullOrEmpty(data.user.username)) user = Client.Users.Add(data.user);
            if (user == null) return null;

            if (data.user != null && !string.IsNullOrWhiteSpace(data.user.username))
            {
                if (!user.Equals(new User(Client, data.user)))
                {
                    Client.Actions.UserUpdate.Handle(data.user);
                }
            }

            Guild guild = Client.Guilds.Cache.Get(data.guild_id);
            if (guild == null) return null;

            Presence oldPresence = guild.Presences.Cache.Get(user.ID);
            if (oldPresence != null) oldPresence = oldPresence._Clone();
            GuildMember member = guild.Members.Cache.Get(user.ID);
            if (member == null && data.status != "offline")
            {
                member = guild.Members.Add(new GuildMemberData()
                {
                    user = data.user,
                    roles = data.roles,
                    deaf = false,
                    mute = false
                });
                Client.EmitGuildMemberAvailable(member);
            }
            guild.Presences.Add(new PresenceData()
            {
                user = data.user,
                status = data.status,
                activities = data.activities,
                client_status = data.client_status,
                premium_since = data.premium_since,
                nick = data.nick,
                guild = guild
            });
            if (member != null)
            {
                Client.EmitPresenceUpdate(oldPresence, member.Presence);
            }

            return null;
        }
    }
}