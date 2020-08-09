using DiscordJS.Data;
using DiscordJS.Resolvables;
using JavaScript;
using System;
using System.Collections.Generic;

namespace DiscordJS
{
    public class MessageManager : BaseManager<Collection<Snowflake, Message>, Message, MessageData>
    {
        /// <summary>
        /// The channel that the messages belong to
        /// </summary>
        public ITextBasedChannel Channel { get; internal set; }

        public MessageManager(ITextBasedChannel channel, IEnumerable<Message> iterable) : base(channel.Client, iterable)
        {
            Channel = channel;
        }

        protected override Message AddItem(Message item) => Add(item);

        public Message Add(MessageData data, bool cache = false)
        {
            Snowflake snowflake = data.id;
            Message existing = Cache.Get(snowflake);
            if (existing != null)
            {
                if (cache) existing._Patch(data);
                return existing;
            }

            var entry = new Message(Client, data, Channel);
            if (cache) Cache.Set(snowflake, entry);
            return entry;
        }

        public Message Add(Message data, bool cache = false)
        {
            Snowflake snowflake = data.ID;
            Message existing = Cache.Get(snowflake);
            if (existing != null)
            {
                if (cache) existing._Patch(data);
                return existing;
            }

            var entry = new Message(Client, data, Channel);
            if (cache) Cache.Set(snowflake, entry);
            return entry;
        }

        /// <summary>
        /// Deletes a message, even if it's not cached.
        /// </summary>
        /// <param name="message">The message to delete</param>
        /// <param name="reason">Reason for deleting this message, if it does not belong to the client user</param>
        /// <returns><see cref="IPromise"/>&lt;<see langword="null"/>&gt;</returns>
        public IPromise<object> Delete(MessageResolvable message, string reason = null)
        {
            Snowflake msg = ResolveID(message);
            if (msg != null)
                return Client.API.Channels(Channel.ID).Messages(msg).Delete(new { reason });
            else return Promise<object>.Rejected(new Exception("Could not resolve ID"));
        }

        /// <summary>
        /// Gets a message, or messages, from this channel.
        /// <br/>
        /// <br/>
        /// <info><b>The returned Collection does not contain reaction users of the messages if they<br/>
        /// were not cached. Those need to be fetched separately in such a case.</b></info>
        /// </summary>
        /// <param name="options">query parameters</param>
        /// <param name="cache">Whether to cache the message(s)</param>
        /// <returns></returns>
        public IPromise<Collection<Snowflake, Message>> Fetch(ChannelLogsQueryOptions options, bool cache = true)
        {
            return Client.API.Channels(Channel.ID).Messages().Get(new { query = options }).Then((data) =>
            {
                var messages = new Collection<Snowflake, Message>();
                for (int index = 0, length = data.Length; index < length; index++)
                {
                    var message = data[index];
                    messages.Set(message.id, Add(message, cache));
                }
                return messages;
            });
        }

        /// <summary>
        /// Gets a message, or messages, from this channel.
        /// </summary>
        /// <param name="messageID">The ID of the message to fetch</param>
        /// <param name="cache">Whether to cache the message(s)</param>
        /// <returns></returns>
        public IPromise<Message> Fetch(Snowflake messageID, bool cache = true)
        {
            var existing = Cache.Get(messageID);
            if (existing != null && !existing.Partial) Promise<Message>.Resolved(existing);
            return Client.API.Channels(Channel.ID).Messages(messageID).Get().Then((data) =>
            {
                return Add(data[0], cache);
            });
        }

        /// <summary>
        /// Fetches the pinned messages of this channel and returns a collection of them.
        /// <br/>
        /// <br/>
        /// <info><b>The returned Collection does not contain any reaction data of the messages.
        /// <br/>Those need to be fetched separately.</b></info>
        /// </summary>
        /// <param name="cache">Whether to cache the message(s)</param>
        /// <returns></returns>
        public IPromise<Collection<Snowflake, Message>> FetchPinned(bool cache = true)
        {
            return Client.API.Channels(Channel.ID).Pins().Get().Then((data) =>
            {
                var messages = new Collection<Snowflake, Message>();
                for (int index = 0, length = data.Length; index < length; index++)
                {
                    var message = data[index];
                    messages.Set(message.id, Add(message, cache));
                }
                return messages;
            });
        }

        /// <summary>
        /// Resolves a MessageResolvable to a Message object.
        /// </summary>
        /// <param name="message">The MessageResolvable to identify</param>
        /// <returns></returns>
        public Message Resolve(MessageResolvable message) => message.Resolve(this);

        /// <summary>
        /// Resolves a MessageResolvable to a message ID string.
        /// </summary>
        /// <param name="message">The MessageResolvable to identify</param>
        /// <returns></returns>
        public Snowflake ResolveID(MessageResolvable message) => message.ResolveID();
    }

    
    public class ChannelLogsQueryOptions
    {
        /// <summary>
        /// Number of messages to acquire
        /// </summary>
        public int limit = 50;

        /// <summary>
        /// ID of a message to get the messages that were posted before it
        /// </summary>
        public Snowflake before;

        /// <summary>
        /// ID of a message to get the messages that were posted after it
        /// </summary>
        public Snowflake after;

        /// <summary>
        /// ID of a message to get the messages that were posted around it
        /// </summary>
        public Snowflake around;
    }
}