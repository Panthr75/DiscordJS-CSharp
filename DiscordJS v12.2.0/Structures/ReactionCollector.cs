using System;

namespace DiscordJS
{
    /// <summary>
    /// Collects reactions on messages. Will automatically stop if the message ('messageDelete'), channel ('channelDelete'), or guild ('guildDelete') are deleted.
    /// </summary>
    public class ReactionCollector : Collector<MessageReaction, User, ReactionCollectorOptions>
    {
        /// <summary>
        /// The message upon which to collect reactions
        /// </summary>
        public Message Message { get; internal set; }

        /// <summary>
        /// The total number of reactions collected
        /// </summary>
        public int Total { get; internal set; }

        /// <summary>
        /// The users which have reacted to this message
        /// </summary>
        public Collection<Snowflake, User> Users { get; internal set; }


        /// <summary>
        /// Emitted whenever a reaction is removed from a message. Will emit on all reaction removals, as opposed to Collector#Dispose which will only be emitted when the entire reaction is removed.
        /// </summary>
        /// <param name="reaction">The reaction that was removed</param>
        /// <param name="user">The user that removed the reaction</param>
        public delegate void RemoveEvent(MessageReaction reaction, User user);


        /// <summary>
        /// Emitted whenever a reaction is removed from a message. Will emit on all reaction removals, as opposed to Collector#Dispose which will only be emitted when the entire reaction is removed.
        /// </summary>
        public event RemoveEvent OnRemove;

        /// <summary>
        /// Emitted whenever a reaction is removed from a message. Will emit on all reaction removals, as opposed to Collector#Dispose which will only be emitted when the entire reaction is removed.
        /// </summary>
        public event RemoveEvent OnceRemove;


        /// <summary>
        /// Instantiates a new ReactionCollector
        /// </summary>
        /// <param name="message">The message upon which to collect reactions</param>
        /// <param name="filter">The filter to apply to this collector</param>
        /// <param name="options">The options to apply to this collector</param>
        public ReactionCollector(Message message, Func<MessageReaction, User, Collection<Snowflake, MessageReaction>, bool> filter, ReactionCollectorOptions options) : base(message.Client, filter, options)
        {
            Message = message;
            Users = new Collection<Snowflake, User>();
            Total = 0;
            Client.MessageReactionAdd += HandleCollect;
            Client.MessageReactionRemove += HandleDispose;
            Client.MessageReactionRemoveAll += Empty;
            Client.MessageDelete += _HandleMessageDeletion;
            Client.ChannelDelete += _HandleChannelDeletion;
            Client.GuildDelete += _HandleGuildDeletion;

            OnceEnd += (collected, reason) =>
            {
                Client.MessageReactionAdd -= HandleCollect;
                Client.MessageReactionRemove -= HandleDispose;
                Client.MessageReactionRemoveAll -= Empty;
                Client.MessageDelete -= _HandleMessageDeletion;
                Client.ChannelDelete -= _HandleChannelDeletion;
                Client.GuildDelete -= _HandleGuildDeletion;
            };

            OnCollect += (reaction, user) =>
            {
                Total++;
                Users.Set(user.ID, user);
            };

            OnRemove += (reaction, user) =>
            {
                Total--;
                if (!Collected.Some((r) => r.Users.Cache.Has(user.ID))) Users.Delete(user.ID);
            };
        }

        /// <summary>
        /// Handles an incoming reaction for possible collection.
        /// </summary>
        /// <param name="reaction">The reaction to possibly collect</param>
        /// <param name="user">The user. Is not used.</param>
        /// <returns></returns>
        public override Snowflake Collect(MessageReaction reaction, User user)
        {
            if (reaction.Message.ID != Message.ID) return null;
            return ((IHasID)reaction).ID;
        }

        /// <summary>
        /// Handles a reaction deletion for possible disposal.
        /// </summary>
        /// <param name="reaction">The reaction to possibly dispose of</param>
        /// <param name="user">The user that removed the reaction</param>
        /// <returns></returns>
        public override Snowflake Dispose(MessageReaction reaction, User user)
        {
            if (reaction.Message.ID != Message.ID) return null;

            if (Collected.Has(((IHasID)reaction).ID) && Users.Has(user.ID))
            {
                OnRemove?.Invoke(reaction, user);
                OnceRemove?.Invoke(reaction, user);
                OnceRemove = null;
            }

            return reaction.Count.HasValue ? null : ((IHasID)reaction).ID;
        }

        /// <inheritdoc/>
        public override string EndReason()
        {
            if (Options.Max.HasValue && Total >= Options.Max.Value) return "limit";
            if (Options.MaxEmojis.HasValue && Collected.Size >= Options.MaxEmojis.Value) return "emojiLimit";
            if (Options.MaxUsers.HasValue && Users.Size >= Options.MaxUsers.Value) return "userLimit";
            return null;
        }

        /// <summary>
        /// Handles checking if the message has been deleted, and if so, stops the collector with the reason 'messageDelete'.
        /// </summary>
        /// <param name="message">The message that was deleted</param>
        private void _HandleMessageDeletion(Message message)
        {
            if (message.ID == Message.ID)
                Stop("messageDelete");
        }

        /// <summary>
        /// Handles checking if the channel has been deleted, and if so, stops the collector with the reason 'channelDelete'.
        /// </summary>
        /// <param name="channel">The channel that was deleted</param>
        private void _HandleChannelDeletion(Channel channel)
        {
            if (channel.ID == Message.Channel.ID)
                Stop("channelDelete");
        }

        /// <summary>
        /// Handles checking if the guild has been deleted, and if so, stops the collector with the reason 'guildDelete'.
        /// </summary>
        /// <param name="guild">The guild that was deleted</param>
        private void _HandleGuildDeletion(Guild guild)
        {
            if (Message.Guild != null && guild.ID == Message.Guild.ID)
                Stop("guildDelete");
        }

        /// <summary>
        /// Empties this reaction collector.
        /// </summary>
        /// <param name="message">The message to compare to for deciding if to empty</param>
        private void Empty(Message message)
        {
            if (message.ID == Message.ID)
            {
                Total = 0;
                Collected.Clear();
                Users.Clear();
                CheckEnd();
            }
        }
    }

    /// <summary>
    /// The options for a Reaction Collector
    /// </summary>
    public class ReactionCollectorOptions : CollectorOptions
    {
        /// <summary>
        /// The maximum total amount of reactions to collect
        /// </summary>
        public int? Max { get; set; }

        /// <summary>
        /// The maximum number of emojis to collect
        /// </summary>
        public int? MaxEmojis { get; set; }

        /// <summary>
        /// The maximum number of users to react
        /// </summary>
        public int? MaxUsers { get; set; }
    }

    public class ReactionCollectorCollectionException : CollectorCollectionException<MessageReaction>
    {
        public ReactionCollectorCollectionException(Collection<Snowflake, MessageReaction> collection, string reason) : base(collection, reason)
        { }
    }
}