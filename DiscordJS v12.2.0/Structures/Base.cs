namespace DiscordJS
{
    /// <summary>
    /// Represents a data model that is identifiable by a Snowflake (i.e. Discord API data models).
    /// </summary>
    public class Base
    {
        /// <summary>
        /// The client that instantiated this
        /// </summary>
        public Client Client { get; }

        public Base(Client client)
        {
            Client = client;
        }

        internal object _Clone()
        {
            return null;
        }
    }
}