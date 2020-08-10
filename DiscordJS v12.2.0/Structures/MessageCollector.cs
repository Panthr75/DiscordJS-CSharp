using JavaScript;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Collects messages on a channel. Will automatically stop if the channel ('channelDelete') or guild ('guildDelete') are deleted.
    /// </summary>
    public class MessageCollector : Collector<Message>
    {
        /// <summary>
        /// The channel
        /// </summary>
        public ITextBasedChannel Channel { get; internal set; }

        /// <summary>
        /// The filter applied to this collector
        /// </summary>
        public Func<Message, Collection<Snowflake, Message>, bool> Filter { get; internal set; }

        /// <summary>
        /// Returns a promise that resolves with the next collected element;<br/>
        /// rejects with collected elements if the collector finishes without receiving a next element
        /// </summary>
        public IPromise<Message> Next
        {
            get
            {
                return new Promise<Message>((resolve, reject) =>
                {
                    if (Ended)
                    {
                        reject(new MessageCollectorCollectionException(Collected, "ended"));
                        return;
                    }

                    
                });
            }
        }

        /// <summary>
        /// The options of this collector
        /// </summary>
        public MessageCollectorOptions Options { get; internal set; }

        /// <summary>
        /// Emitted whenever a message is collected.
        /// </summary>
        /// <param name="message">The message that was collected</param>
        public delegate void CollectEvent(Message message);

        /// <summary>
        /// Emitted whenever a message is disposed of.
        /// </summary>
        /// <param name="message">The message that was disposed of</param>
        public delegate void DisposeEvent(Message message);


        /// <summary>
        /// Emitted whenever a message is collected.
        /// </summary>
        public event CollectEvent OnCollect;

        /// <summary>
        /// Emitted whenever a message is collected.
        /// </summary>
        public event CollectEvent OnceCollect;

        /// <summary>
        /// Emitted whenever a message is disposed of.
        /// </summary>
        public event DisposeEvent OnDispose;

        /// <summary>
        /// Emitted whenever a message is disposed of.
        /// </summary>
        public event DisposeEvent OnceDispose;

        /// <summary>
        /// Instantiates a new message collector
        /// </summary>
        /// <param name="channel">The channel</param>
        /// <param name="filter">The filter to be applied to this collector</param>
        /// <param name="options">The options to be applied to this collector</param>
        public MessageCollector(ITextBasedChannel channel, Func<Message, Collection<Snowflake, Message>, bool> filter, MessageCollectorOptions options) : base(channel.Client, options)
        {
            //
        }

        public void HandleCollect(Message message)
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
        {
            //
        }
    }
}