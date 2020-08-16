using DiscordJS.Data;
using Newtonsoft.Json;

namespace DiscordJS.Actions
{
    public class VoiceStateUpdateAction : GenericAction<VoiceStateUpdateAction.ActionResult>
    {
        public class ActionResult
        {
            //
        }

        public VoiceStateUpdateAction(Client client) : base(client)
        { }

        public override ActionResult Handle(dynamic data) => Handle(DiscordUtil.FromDynamic<VoiceStateData>(data));

        public ActionResult Handle(VoiceStateData data)
        {
            Guild guild = Client.Guilds.Cache.Get(data.guild_id);
            if (guild != null)
            {
                // Update the state
                VoiceState oldState = guild.VoiceStates.Cache.Has(data.user_id) ?
                    guild.VoiceStates.Cache.Get(data.user_id)._Clone() :
                    new VoiceState(guild, new VoiceStateData() { user_id = data.user_id });
                VoiceState newState = guild.VoiceStates.Add(data);

                // Get the member
                GuildMember member = guild.Members.Cache.Get(data.user_id);
                if (member != null && data.member != null)
                {
                    member._Patch(data.member);
                }
                else if (data.member != null && data.member.user != null && data.member.joined_at.HasValue)
                {
                    member = guild.Members.Add(data.member);
                }

                // Emit event
                if (member != null && member.User.ID == Client.User.ID)
                {
                    Client.EmitDebug($"[VOICE] received voice state update: {JsonConvert.SerializeObject(data)}");
                    Client.Voice.OnVoiceStateUpdate(data);
                }

                Client.EmitVoiceStateUpdate(oldState, newState);
            }
            return null;
        }
    }
}