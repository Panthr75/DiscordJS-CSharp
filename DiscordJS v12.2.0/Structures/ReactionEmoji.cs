using DiscordJS.Data;

namespace DiscordJS
{
    /// <summary>
    /// Represents a limited emoji set used for both custom and unicode emojis. Custom emojis will use this class opposed to the Emoji class when the client doesn't know enough information about them.
    /// </summary>
    public class ReactionEmoji : Emoji
    {
        /// <summary>
        /// The message reaction this emoji refers to
        /// </summary>
        public MessageReaction Reaction { get; }

        /// <inheritdoc/>
        public ReactionEmoji(MessageReaction reaction, EmojiData emoji) : base(reaction.Message.Client, emoji)
        {
            Reaction = reaction;
        }

        /// <inheritdoc/>
        public string ValueOf() => ID;
    }
}