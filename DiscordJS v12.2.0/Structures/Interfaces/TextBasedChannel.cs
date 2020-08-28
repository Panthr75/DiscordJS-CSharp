using DiscordJS.Resolvables;
using JavaScript;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Interface for classes that have text-channel-like features.
    /// </summary>
    public static class TextBasedChannel
    {
        /// <summary>
        /// The Message object of the last message in the channel, if one was sent
        /// </summary>
        internal static object LastMessageDoc = null;

        /// <summary>
        /// The Message object of the last message in the channel, if one was sent
        /// </summary>
        internal static object LastMessageIDDoc = null;

        /// <summary>
        /// The timestamp when the last pinned message was pinned, if there was one
        /// </summary>
        internal static object LastPinTimestampDoc = null;

        /// <summary>
        /// The date when the last pinned message was pinned, if there was one
        /// </summary>
        internal static object LastPinAtDoc = null;

        /// <summary>
        /// A manager of the messages belonging to this channel
        /// </summary>
        internal static object MessagesDoc = null;

        /// <summary>
        /// Whether or not the typing indicator is being shown in the channel
        /// </summary>
        internal static object TypingDoc = null;

        /// <summary>
        /// Number of times <c>StartTyping</c> has been called
        /// </summary>
        internal static object TypingCountDoc = null;

        public static Message LastMessage(MessageManager messages, Snowflake lastMessageID) => messages.Cache.Get(lastMessageID);

        public static Date LastPinAt(long? lastPinTimestamp) => lastPinTimestamp.HasValue ? new Date(lastPinTimestamp.Value) : null;

        public static bool Typing(Client client, Snowflake id) => client.User._typing.Has(id);

        public static int TypingCount(Client client, Snowflake id) => client.User._typing.Has(id) ? client.User._typing.Get(id).count : 0;

        public static IPromise<Collection<Snowflake, Message>> AwaitMessages(DMChannel channel, Func<Message, Collection<Snowflake, Message>, bool> filter, AwaitMessagesOptions options = null)
        {
            return new Promise<Collection<Snowflake, Message>>((resolve, reject) =>
            {
                MessageCollector collector = CreateMessageCollector(channel, filter, options);
                collector.OnceEnd += (Collection<Snowflake, Message> collection, string reason) =>
                {
                    if (options.errors != null && options.errors.Includes(reason))
                        reject(new MessageCollectorCollectionException(collection, reason));
                    else
                        resolve(collection);
                };
            });
        }

        public static IPromise<Collection<Snowflake, Message>> AwaitMessages(TextChannel channel, Func<Message, Collection<Snowflake, Message>, bool> filter, AwaitMessagesOptions options = null)
        {
            return new Promise<Collection<Snowflake, Message>>((resolve, reject) =>
            {
                MessageCollector collector = CreateMessageCollector(channel, filter, options);
                collector.OnceEnd += (Collection<Snowflake, Message> collection, string reason) =>
                {
                    if (options.errors != null && options.errors.Includes(reason))
                        reject(new MessageCollectorCollectionException(collection, reason));
                    else
                        resolve(collection);
                };
            });
        }

        public static IPromise<Collection<Snowflake, Message>> BulkDelete(Client client, Snowflake id, TextChannel channel, Collection<Snowflake, Message> messages, bool filterOld = false)
        {
            Array<Snowflake> messageIds = messages.KeyArray();
            if (filterOld)
            {
                messageIds = messageIds.Filter((msgID) => Date.Now() - Snowflake.Deconstruct(msgID).Date.GetTime() < 1209600000);
            }
            if (messageIds.Length == 0) return Promise<Collection<Snowflake, Message>>.Resolved(new Collection<Snowflake, Message>());
            if (messageIds.Length == 1)
            {
                return client.API.Channels(id).Messages(messageIds[0]).Delete().Then((_) =>
                {
                    var message = client.Actions.MessageDelete.GetMessage(messageIds[0], channel);
                    var result = message == null ? new Collection<Snowflake, Message>() : new Collection<Snowflake, Message>();
                    if (message != null) result.Set(message.ID, message);
                    return result;
                });
            }
            else
            {
                return client.API.Channels(id).Messages().BulkDelete().Post(new { data = new { messages = messageIds.ToArray() }}).Then((_) =>
                {
                    return messageIds.Reduce((col, msgId) => col.Set(msgId, client.Actions.MessageDeleteBulk.GetMessage(msgId, channel)), new Collection<Snowflake, Message>());
                });
            }
        }

        public static IPromise<Collection<Snowflake, Message>> BulkDelete(Client client, Snowflake id, TextChannel channel, Array<Message> messages, bool filterOld = false)
        {
            Array<Snowflake> messageIds = messages.Map((m) => m.ID);
            if (filterOld)
            {
                messageIds = messageIds.Filter((msgID) => Date.Now() - Snowflake.Deconstruct(msgID).Date.GetTime() < 1209600000);
            }
            if (messageIds.Length == 0) return Promise<Collection<Snowflake, Message>>.Resolved(new Collection<Snowflake, Message>());
            if (messageIds.Length == 1)
            {
                return client.API.Channels(id).Messages(messageIds[0]).Delete().Then((_) =>
                {
                    var message = client.Actions.MessageDelete.GetMessage(messageIds[0], channel);
                    var result = message == null ? new Collection<Snowflake, Message>() : new Collection<Snowflake, Message>();
                    if (message != null) result.Set(message.ID, message);
                    return result;
                });
            }
            else
            {
                return client.API.Channels(id).Messages().BulkDelete().Post(new { data = new { messages = messageIds.ToArray() } }).Then((_) =>
                {
                    return messageIds.Reduce((col, msgId) => col.Set(msgId, (Message)client.Actions.MessageDeleteBulk.GetMessage(msgId, channel)), new Collection<Snowflake, Message>());
                });
            }
        }

        public static IPromise<Collection<Snowflake, Message>> BulkDelete(Client client, Snowflake id, TextChannel channel, Array<Snowflake> messages, bool filterOld = false)
        {
            Array<Snowflake> messageIds = messages;
            if (filterOld)
            {
                messageIds = messageIds.Filter((msgID) => Date.Now() - Snowflake.Deconstruct(msgID).Date.GetTime() < 1209600000);
            }
            if (messageIds.Length == 0) return Promise<Collection<Snowflake, Message>>.Resolved(new Collection<Snowflake, Message>());
            if (messageIds.Length == 1)
            {
                return client.API.Channels(id).Messages(messageIds[0]).Delete().Then((_) =>
                {
                    var message = client.Actions.MessageDelete.GetMessage(messageIds[0], channel);
                    var result = message == null ? new Collection<Snowflake, Message>() : new Collection<Snowflake, Message>();
                    if (message != null) result.Set(message.ID, message);
                    return result;
                });
            }
            else
            {
                return client.API.Channels(id).Messages().BulkDelete().Post(new { data = new { messages = messageIds.ToArray() } }).Then((_) =>
                {
                    return messageIds.Reduce((col, msgId) => col.Set(msgId, client.Actions.MessageDeleteBulk.GetMessage(msgId, channel)), new Collection<Snowflake, Message>());
                });
            }
        }

        public static IPromise<Collection<Snowflake, Message>> BulkDelete(Client client, Snowflake id, TextChannel channel, MessageManager messageManager, int messages, bool filterOld = false)
        {
            return messageManager.Fetch(new ChannelLogsQueryOptions() { limit = messages }).Then((msgs) => BulkDelete(client, id, channel, msgs, filterOld));
        }

        public static MessageCollector CreateMessageCollector(TextChannel channel, Func<Message, Collection<Snowflake, Message>, bool> filter, MessageCollectorOptions options)
        {
            return new MessageCollector(channel, filter, options);
        }

        public static MessageCollector CreateMessageCollector(DMChannel channel, Func<Message, Collection<Snowflake, Message>, bool> filter, MessageCollectorOptions options)
        {
            return new MessageCollector(channel, filter, options);
        }

        // Send

        public static IPromise StartTyping(Client client, Snowflake id, int? count = 1)
        {
            if (count < 1) throw new DJSError.Error("TYPING_COUNT");
            TypingInfo entry;
            if (client.User._typing.Has(id))
            {
                entry = client.User._typing.Get(id);
                entry.count = count.HasValue ? count.Value : entry.count + 1;
                return entry.promise;
            }
            entry = new TypingInfo();
            entry.promise = new Promise((resolve, reject) =>
            {
                var endpoint = client.API.Channels(id).Typing;
                entry.count = count.HasValue ? count.Value : 1;
                entry.interval = client.SetInterval(() =>
                {
                    endpoint.Post().Catch((err) =>
                    {
                        client.ClearInterval(entry.interval);
                        client.User._typing.Delete(id);
                        reject(err);
                    });
                }, 9000);
                entry.resolve = resolve;
                client.User._typing.Set(id, entry);
            });
            return entry.promise;
        }

        public static void StopTyping(Client client, Snowflake id, bool force = false)
        {
            if (client.User._typing.Has(id))
            {
                TypingInfo entry = client.User._typing.Get(id);
                entry.count--;
                if (entry.count <= 0 || force)
                {
                    client.ClearInterval(entry.interval);
                    client.User._typing.Delete(id);
                    entry.resolve();
                }
            }
        }
    }

    /// <summary>
    /// Options provided when sending or editing a message.
    /// </summary>
    public class MessageOptions
    {
        /// <summary>
        /// Whether or not the message should be spoken aloud
        /// </summary>
        public bool tts = false;

        /// <summary>
        /// The nonce for the message
        /// </summary>
        public string nonce = "";

        /// <summary>
        /// The content for the message
        /// </summary>
        public string content = "";

        /// <summary>
        /// An embed for the message.
        /// (see <see href="https://discord.com/developers/docs/resources/channel#embed-object">here</see> for more details)
        /// </summary>
        public object embed = null;

        /// <summary>
        /// Which mentions should be parsed from the message content
        /// </summary>
        public MessageMentionOptions allowedMentions;

        /// <summary>
        /// Whether or not all mentions or everyone/here mentions should be sanitized to prevent unexpected mentions
        /// </summary>
        public DisableMentionType disableMentions;

        /// <summary>
        /// Files to send with the message
        /// </summary>
        public BufferResolvable[] files = null;

        /// <summary>
        /// Language for optional codeblock formatting to apply
        /// </summary>
        public string code = null;

        /// <summary>
        /// Whether or not the message should be split into multiple messages if it exceeds the character limit
        /// </summary>
        public SplitOptions split = false;

        /// <summary>
        /// User to reply to (prefixes the message with a mention, except in DMs)
        /// </summary>
        public UserResolvable reply = null;
    }

    public class AwaitMessagesOptions : MessageCollectorOptions
    {
        /// <summary>
        /// Stop/end reasons that cause the promise to reject
        /// </summary>
        public Array<string> errors;
    }
}