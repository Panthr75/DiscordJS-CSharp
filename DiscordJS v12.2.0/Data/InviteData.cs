namespace DiscordJS.Data
{
    public class InviteData
    {
        public string code;
        public GuildData guild;
        public ChannelData channel;
        public UserData user;
        public UserData target_user;
        public int? target_user_type;
        public int? approximate_presence_count;
        public int? approximate_member_count;
    }
}