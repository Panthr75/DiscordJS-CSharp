using JavaScript;
using System;
using DiscordJS.Data;

namespace DiscordJS
{
    /// <summary>
    /// Represents an emoji, see <seealso cref="GuildEmoji"/> and <seealso cref="ReactionEmoji"/>.
    /// </summary>
    public class Emoji : Base, IHasID
    {
        /// <summary>
        /// Whether this emoji is animated
        /// </summary>
        public bool Animated { get; internal set; }

        /// <summary>
        /// The time the emoji was created at, or null if unicode
        /// </summary>
        public Date CreatedAt
        {
            get
            {
                if (ID == null) return null;
                return new Date(CreatedTimestamp.Value);
            }
        }

        /// <summary>
        /// The timestamp the emoji was created at, or null if unicode
        /// </summary>
        public long? CreatedTimestamp
        {
            get
            {
                if (ID == null) return null;
                return Snowflake.Deconstruct(ID).Timestamp;
            }
        }

        /// <summary>
        /// Whether this emoji has been deleted
        /// </summary>
        public bool Deleted { get; internal set; }

        /// <summary>
        /// The ID of this emoji
        /// </summary>
        public Snowflake ID { get; internal set; }

        /// <summary>
        /// The identifier of this emoji, used for message reactions
        /// </summary>
        public string Identifier
        {
            get
            {
                if (ID != null) return $"{(Animated ? "a:" : "")}{Name}:{ID}";
                return Uri.EscapeUriString(Name);
            }
        }

        /// <summary>
        /// The name of this emoji
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The URL to the emoji file if its a custom emoji
        /// </summary>
        public string URL
        {
            get
            {
                if (ID == null) return null;
                return Client.rest.CDN.Emoji(ID, Animated ? "gif" : "png");
            }
        }

        public Emoji(Client client, EmojiData emoji) : base(client)
        {
            Animated = emoji.animated;
            Name = emoji.name;
            ID = emoji.id;
            Deleted = false;
        }

        /// <summary>
        /// When concatenated with a string, this automatically returns the text required to form a graphical emoji on Discord instead of the Emoji object.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => ID != null ? $"<{(Animated ? "a" : "")}:{ID}>" : Name;

        Snowflake IHasID.ID => ID == null ? (Snowflake)Name : ID;
    }
}