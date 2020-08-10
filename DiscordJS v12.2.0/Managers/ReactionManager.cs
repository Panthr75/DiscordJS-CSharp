using DiscordJS.Data;
using DiscordJS.Resolvables;
using JavaScript;
using System;
using System.Collections.Generic;

namespace DiscordJS
{
    /// <summary>
    /// Manages API methods for reactions and holds their cache.
    /// </summary>
    public class ReactionManager : BaseManager<Collection<Snowflake, MessageReaction>, MessageReaction, ReactionData>
    {
        /// <summary>
        /// The message that this manager belongs to
        /// </summary>
        public Message Message { get; }

        /// <inheritdoc/>
        public ReactionManager(Message message, IEnumerable<MessageReaction> iterable) : base(message.Client, iterable)
        {
            Message = message;
        }

        /// <inheritdoc/>
        protected override MessageReaction AddItem(MessageReaction item)
        {
            Cache.Set(((IHasID)item).ID, item);
            return item;
        }

        internal MessageReaction Add(MessageReaction data, bool cache = true)
        {
            Snowflake snowflake = data.Emoji.ID == null ? (Snowflake)data.Emoji.Identifier : data.Emoji.ID;
            MessageReaction existing = Cache.Get(snowflake);
            if (existing != null)
            {
                if (cache) existing._Patch(data);
                return existing;
            }

            var entry = new MessageReaction(Client, data, Message);
            if (cache) Cache.Set(snowflake, entry);
            return entry;
        }

        internal MessageReaction Add(ReactionData data, bool cache = true)
        {
            Snowflake snowflake = data.emoji.id == null ? Uri.EscapeUriString(data.emoji.name) : data.emoji.id;
            MessageReaction existing = Cache.Get(snowflake);
            if (existing != null)
            {
                if (cache) existing._Patch(data);
                return existing;
            }

            var entry = new MessageReaction(Client, data, Message);
            if (cache) Cache.Set(snowflake, entry);
            return entry;
        }

        /// <summary>
        /// Resolves a MessageReactionResolvable to a MessageReaction object.
        /// </summary>
        /// <param name="reaction">The MessageReaction to resolve</param>
        /// <returns></returns>
        public MessageReaction Resolve(MessageReactionResolvable reaction) => reaction.Resolve(this);

        /// <summary>
        /// Resolves a MessageReactionResolvable to a user ID snowflake.
        /// </summary>
        /// <param name="reaction">The MessageReactionResolvable to resolve</param>
        /// <returns></returns>
        public Snowflake ResolveID(MessageReactionResolvable reaction) => reaction.ResolveID();

        /// <summary>
        /// Removes all reactions from a message.
        /// </summary>
        /// <returns></returns>
        public IPromise<Message> RemoveAll()
        {
            return Client.API.Channels(Message.Channel.ID).Messages(Message.ID).Reactions(null, null).Delete().Then((_) => Message);
        }
    }
}