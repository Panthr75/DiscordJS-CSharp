using DiscordJS.Data;
using JavaScript;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Represents a reaction to a message.
    /// </summary>
    public class MessageReaction : IHasID
    {
        /// <summary>
        /// The client that instantiated this message reaction
        /// </summary>
        public Client Client { get; }

        /// <summary>
        /// The number of people that have given the same reaction
        /// </summary>
        public int? Count { get; internal set; }

        /// <summary>
        /// The emoji of this reaction, either an GuildEmoji object for known custom emojis, or a ReactionEmoji object which has fewer properties. Whatever the prototype of the emoji, it will still have Name, ID, Identifier and ToString()
        /// </summary>
        public MessageReactionEmoji Emoji
        {
            get
            {
                if (_emoji.IsGuildEmoji()) return (GuildEmoji)_emoji;
                // Check to see if the emoji has become known to the client
                if (_emoji.ID != null)
                {
                    var emojis = Message.Client.Emojis.Cache;
                    if (emojis.Has(_emoji.ID))
                    {
                        var emoji = emojis.Get(_emoji.ID);
                        _emoji = emoji;
                        return emoji;
                    }
                }
                return _emoji;
            }
        }

        /// <summary>
        /// Whether the client has given this reaction
        /// </summary>
        public bool Me { get; internal set; }

        /// <summary>
        /// The message that this reaction refers to
        /// </summary>
        public Message Message { get; internal set; }

        /// <summary>
        /// Whether or not this reaction is a partial
        /// </summary>
        public bool Partial => Count.HasValue;

        /// <summary>
        /// A manager of the users that have given this reaction
        /// </summary>
        public ReactionUserManager Users { get; internal set; }

        Snowflake IHasID.ID => Emoji == null ? null : Emoji.ID;

        internal MessageReactionEmoji _emoji;

        /// <summary>
        /// Instantiates a new Message Reaction
        /// </summary>
        /// <param name="client">The instantiating client</param>
        /// <param name="data">The data for the message reaction</param>
        /// <param name="message">The message the reaction refers to</param>
        public MessageReaction(Client client, ReactionData data, Message message)
        {
            Client = client;
            Message = message;
            Me = data.me;
            Users = new ReactionUserManager(Client, null, this);
            _emoji = new ReactionEmoji(this, data.emoji);
            _Patch(data);
        }

        internal MessageReaction(Client client, MessageReaction reaction, Message message)
        {
            Client = client;
            Message = message;
            Me = reaction.Me;
            Users = new ReactionUserManager(Client, reaction.Users.Cache.Values(), this);
            _emoji = reaction._emoji;
            Count = reaction.Count;
        }

        internal void _Patch(ReactionData data)
        {
            if (!Count.HasValue) Count = data.count;
        }

        internal void _Patch(MessageReaction reaction)
        {
            if (!Count.HasValue) Count = reaction.Count.Value;
        }

        internal void _Add(User user)
        {
            if (Partial) return;
            Users.Cache.Set(user.ID, user);
            if (!Me || user.ID != Message.Client.User.ID || (Count.HasValue && Count.Value == 0)) Count = Count.HasValue ? (int?)(Count.Value + 1) : null;
            if (!Me) Me = user.ID == Message.Client.User.ID;
        }

        internal void _Remove(User user)
        {
            if (Partial) return;
            Users.Cache.Delete(user.ID);
            if (!Me || user.ID != Message.Client.User.ID || (Count.HasValue && Count.Value == 0)) Count = Count.HasValue ? (int?)(Count.Value - 1) : null;
            if (user.ID == Message.Client.User.ID) Me = false;
            if ((Count.HasValue && Count <= 0) || Users.Cache.Size == 0)
            {
                Message.Reactions.Cache.Delete(Emoji.ID, Emoji.Name);
            }
        }

        /// <summary>
        /// Fetch this reaction.
        /// </summary>
        /// <returns></returns>
        public IPromise<MessageReaction> Fetch()
        {
            return Message.Fetch().Then((message) =>
            {
                var existing = message.Reactions.Cache.Get(Emoji.ID == null ? Emoji.Name : (string)Emoji.ID);
                if (existing == null) _Patch(new ReactionData() { count = 0 });
                else _Patch(existing);
                return this;
            });
        }

        /// <summary>
        /// Removes all users from this reaction.
        /// </summary>
        /// <returns></returns>
        public IPromise<MessageReaction> Remove()
        {
            return Client.API.Channels(Message.Channel.ID).Messages(Message.ID).Reactions(_emoji.Identifier, null).Delete().Then((_) => this);
        }
    }

    /// <summary>
    /// Represents the emoji for a <see cref="MessageReaction"/> object.
    /// </summary>
    public class MessageReactionEmoji
    {
        internal enum Type
        {
            GuildEmoji,
            ReactionEmoji
        }

        internal readonly Type type;

        internal readonly GuildEmoji guildEmoji;
        internal readonly ReactionEmoji reactionEmoji;

        /// <inheritdoc cref="Emoji.Name"/>
        public string Name
        {
            get
            {
                if (type == Type.GuildEmoji)
                    return guildEmoji == null ? null : guildEmoji.Name;
                else if (type == Type.ReactionEmoji)
                    return reactionEmoji == null ? null : reactionEmoji.Name;
                else
                    throw new ArgumentException("The given emoji cannot be cast to a 'MessageReactionEmoji'");
            }
        }

        /// <inheritdoc cref="Emoji.ID"/>
        public Snowflake ID
        {
            get
            {
                if (type == Type.GuildEmoji)
                    return guildEmoji == null ? null : guildEmoji.ID;
                else if (type == Type.ReactionEmoji)
                    return reactionEmoji == null ? null : reactionEmoji.ID;
                else
                    throw new ArgumentException("The given emoji cannot be cast to a 'MessageReactionEmoji'");
            }
        }

        /// <inheritdoc cref="Emoji.Identifier"/>
        public string Identifier
        {
            get
            {
                if (type == Type.GuildEmoji)
                    return guildEmoji == null ? null : guildEmoji.Identifier;
                else if (type == Type.ReactionEmoji)
                    return reactionEmoji == null ? null : reactionEmoji.Identifier;
                else
                    throw new ArgumentException("The given emoji cannot be cast to a 'MessageReactionEmoji'");
            }
        }

        private MessageReactionEmoji(GuildEmoji emoji)
        {
            guildEmoji = emoji;
            type = Type.GuildEmoji;
        }

        private MessageReactionEmoji(ReactionEmoji emoji)
        {
            reactionEmoji = emoji;
            type = Type.ReactionEmoji;
        }

        /// <inheritdoc cref="Emoji.ToString"/>
        public override string ToString()
        {
            if (type == Type.GuildEmoji)
                return guildEmoji == null ? null : guildEmoji.ToString();
            else if (type == Type.ReactionEmoji)
                return reactionEmoji == null ? null : reactionEmoji.ToString();
            else
                throw new ArgumentException("The given emoji cannot be cast to a 'MessageReactionEmoji'");
        }

        /// <summary>
        /// Returns whether this reaction emoji is a guild emoji
        /// </summary>
        /// <returns></returns>
        public bool IsGuildEmoji() => type == Type.GuildEmoji;

        /// <summary>
        /// Returns whether this reaction emoji is a reaction emoji
        /// </summary>
        /// <returns></returns>
        public bool IsReactionEmoji() => type == Type.ReactionEmoji;

        /// <inheritdoc/>
        public static implicit operator MessageReactionEmoji(GuildEmoji emoji) => new MessageReactionEmoji(emoji);
        /// <inheritdoc/>
        public static implicit operator MessageReactionEmoji(ReactionEmoji emoji) => new MessageReactionEmoji(emoji);
        /// <inheritdoc/>
        public static explicit operator GuildEmoji(MessageReactionEmoji emoji)
        {
            if (emoji == null) throw new InvalidCastException("'null' cannot be cast to GuildEmoji");
            else if (emoji.type != Type.GuildEmoji) throw new InvalidCastException("The given 'MessageReactionEmoji' is not a 'GuildEmoji'. You can check if it is with MessageReactionEmoji.#IsGuildEmoji()");
            else return emoji.guildEmoji;
        }
        /// <inheritdoc/>
        public static explicit operator ReactionEmoji(MessageReactionEmoji emoji)
        {
            if (emoji == null) throw new InvalidCastException("'null' cannot be cast to ReactionEmoji");
            else if (emoji.type != Type.ReactionEmoji) throw new InvalidCastException("The given 'MessageReactionEmoji' is not a 'ReactionEmoji'. You can check if it is with MessageReactionEmoji.#IsReactionEmoji()");
            else return emoji.reactionEmoji;
        }
    }
}