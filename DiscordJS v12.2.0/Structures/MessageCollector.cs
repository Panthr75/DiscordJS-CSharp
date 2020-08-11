using JavaScript;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Collects messages on a channel. Will automatically stop if the channel ('channelDelete') or guild ('guildDelete') are deleted.
    /// </summary>
    public class MessageCollector : Collector<Message, MessageCollectorOptions>
    {
        /// <summary>
        /// The channel
        /// </summary>
        public ITextBasedChannel Channel { get; internal set; }

        /// <summary>
        /// Total number of messages that were received in the channel during message collection
        /// </summary>
        public int Received { get; internal set; }

        /// <summary>
        /// Instantiates a new message collector
        /// </summary>
        /// <param name="channel">The channel</param>
        /// <param name="filter">The filter to be applied to this collector</param>
        /// <param name="options">The options to be applied to this collector</param>
        public MessageCollector(ITextBasedChannel channel, Func<Message, Collection<Snowflake, Message>, bool> filter, MessageCollectorOptions options) : base(channel.Client, filter, options)
        {
            Channel = channel;
            Received = 0;
            MessageDeleteBulkEvent bulkDeleteListener = (messages) =>
            {
                foreach (Message message in messages.Values()) HandleDispose(message);
            };
            Client.Message += HandleCollect;
            Client.MessageDelete += HandleDispose;
            Client.MessageDeleteBulk += bulkDeleteListener;
            Client.ChannelDelete += _HandleChannelDeletion;
            Client.GuildDelete += _HandleGuildDeletion;

            OnceEnd += (collected, reason) =>
            {
                Client.Message -= HandleCollect;
                Client.MessageDelete -= HandleDispose;
                Client.MessageDeleteBulk -= bulkDeleteListener;
                Client.ChannelDelete -= _HandleChannelDeletion;
                Client.GuildDelete -= _HandleGuildDeletion;
            };
        }

        private void _HandleGuildDeletion(Guild guild)
        {
            if (Channel.Guild != null && guild.ID == Channel.Guild.ID)
                Stop("guildDelete");
        }

        /// <summary>
        /// Handles checking if the channel has been deleted, and if so, stops the collector with the reason 'channelDelete'.
        /// </summary>
        /// <param name="channel">The channel that was deleted</param>
        private void _HandleChannelDeletion(Channel channel)
        {
            if (channel.ID == Channel.ID)
                Stop("channelDelete");
        }

        /// <summary>
        /// Handles a message for possible collection.
        /// </summary>
        /// <param name="message">The message that could be collected</param>
        /// <returns></returns>
        public override Snowflake Collect(Message message)
        {
            if (message.Channel.ID != Channel.ID) return null;
            Received++;
            return message.ID;
        }

        /// <summary>
        /// Handles a message for possible disposal.
        /// </summary>
        /// <param name="message">The message that could be disposed of</param>
        /// <returns></returns>
        public override Snowflake Dispose(Message message)
        {
            return message.Channel.ID == Channel.ID ? message.ID : null;
        }

        /// <summary>
        /// Checks after un/collection to see if the collector is done.
        /// </summary>
        /// <returns></returns>
        public override string EndReason()
        {
            if (Options.Max.HasValue && Collected.Size >= Options.Max.Value) return "limit";
            if (Options.MaxProcessed.HasValue && Received >= Options.MaxProcessed.Value) return "processedLimit";
            return null;
        }
    }

    /// <summary>
    /// The options for a message collector
    /// </summary>
    public class MessageCollectorOptions : CollectorOptions
    {
        /// <summary>
        /// The maximum amount of messages to collect
        /// </summary>
        public int? Max { get; set; } = null;

        /// <summary>
        /// The maximum amount of messages to process
        /// </summary>
        public int? MaxProcessed { get; set; } = null;
    }

    public class MessageCollectorCollectionException : CollectorCollectionException<Message>
    {
        public MessageCollectorCollectionException(Collection<Snowflake, Message> collection, string reason) : base(collection, reason)
        { }
    }
}