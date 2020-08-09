namespace DiscordJS.Data
{
    public sealed class GuildMemberData
    {
        public UserData user;
        public string nick;
        public string[] roles;
        public long? joined_at;
        public long? premium_since;
        public bool? deaf;
        public bool? mute;
    }
}