using JavaScript;

namespace DiscordJS
{
    /// <summary>
    /// Options provided to control parsing of mentions by Discord
    /// </summary>
    public class MessageMentionOptions
    {
        /// <summary>
        /// Types of mentions to be parsed
        /// </summary>
        public Array<MessageMentionTypes> parse;

        /// <summary>
        /// Snowflakes of Users to be parsed as mentions
        /// </summary>
        public Array<string> users; // Array<Snowflake>

        /// <summary>
        /// Snowflakes of Roles to be parsed as mentions
        /// </summary>
        public Array<string> roles; // Array<Snowflake>
    }
}