using DiscordJS.Data;

namespace DiscordJS
{
    /// <summary>
    /// Represents the logged in client's Discord user.
    /// </summary>
    public class ClientUser : User
    {
        internal Collection<Snowflake, TypingInfo> _typing = new Collection<Snowflake, TypingInfo>();

        public ClientUser(Client client, UserData data) : base(client, data)
        {
            //
        }
    }
}