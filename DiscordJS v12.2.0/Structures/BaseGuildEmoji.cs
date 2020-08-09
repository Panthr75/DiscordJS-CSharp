using JavaScript;
using DiscordJS.Data;

namespace DiscordJS
{
    /// <summary>
    /// Parent class for <seealso cref="GuildEmoji"/> and <seealso cref="GuildPreviewEmoji"/>.
    /// </summary>
    public class BaseGuildEmoji : Emoji
    {
        /// <summary>
        /// The guild this emoji is a part of
        /// </summary>
        public Guild Guild { get; }

        /// <summary>
        /// Array of role ids this emoji is active for
        /// </summary>
        internal Array<Snowflake> _roles;

        public BaseGuildEmoji(Client client, EmojiData data, Guild guild) : base(client, data)
        {
            Guild = guild;
            _roles = new Array<Snowflake>();

            _Patch(data);
        }

        internal void _Patch(EmojiData data)
        {
            //
        }
    }
}