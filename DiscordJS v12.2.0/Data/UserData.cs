namespace DiscordJS.Data
{
    [Data]
    public class UserData
    {
        public string id;
        public string username;
        public string discriminator;
        public bool? bot;
        public bool? system;
        public string locale;
        public string avatar;
        public long? public_flags;
    }
}