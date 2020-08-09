using DiscordJS.Data;
using JavaScript;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Represents a direct message channel between two users.
    /// </summary>
    public class DMChannel : Channel
    {
        /// <inheritdoc cref="TextBasedChannel.LastMessageDoc"/>
        public Message LastMessage { get; }

        /// <inheritdoc cref="TextBasedChannel.LastMessageIDDoc"/>
        public Snowflake LastMessageID { get; internal set; }

        /// <inheritdoc cref="TextBasedChannel.LastPinAtDoc"/>
        public Date LastPinAt { get; }

        /// <inheritdoc cref="TextBasedChannel.LastPinTimestampDoc"/>
        public long? LastPinTimestamp { get; internal set; }

        /// <inheritdoc cref="TextBasedChannel.MessagesDoc"/>
        public MessageManager Messages { get; internal set; }

        /// <summary>
        /// Whether this DMChannel is a partial
        /// </summary>
        public override bool Partial => LastMessageID == null;

        /// <summary>
        /// The recipient on the other end of the DM
        /// </summary>
        public User Recipient { get; internal set; }

        /// <inheritdoc cref="TextBasedChannel.TypingDoc"/>
        public bool Typing => TextBasedChannel.Typing(Client, ID);

        /// <inheritdoc cref="TextBasedChannel.TypingCountDoc"/>
        public int TypingCount => TextBasedChannel.TypingCount(Client, ID);

        /// <summary>
        /// Instantiates a new DMChannel
        /// </summary>
        /// <param name="client">The instantiating client</param>
        /// <param name="data">The data for the DM channel</param>
        public DMChannel(Client client, ChannelData data) : base(client, data)
        {
            // Override the channel type so partials have a known type
            Type = "dm";
            Messages = new MessageManager(this);
            _typing = new Map<Snowflake, TypingInfo>();
        }

        internal override void _Patch(ChannelData data)
        {
            base._Patch(data);

            if (data.recipients != null)
            {
                Recipient = Client.Users.Add(data.recipients[0]);
            }

            LastMessageID = data.last_message_id;
            LastPinTimestamp = data.last_pin_timestamp.HasValue ? (long?)new Date(data.last_pin_timestamp.Value).GetTime() : null;
        }

        /// <summary>
        /// Similar to CreateMessageCollector but in promise form. Resolves with a collection of messages that pass the specified filter.
        /// </summary>
        /// <param name="filter">The filter function to use</param>
        /// <param name="options">Optional options to pass to the internal collector</param>
        /// <returns></returns>
        public IPromise<Collection<Snowflake, Message>> AwaitMessages(Func<Message, bool> filter, AwaitMessagesOptions options = null)
        {
            //
        }

        /// <summary>
        /// Creates a Message Collector.
        /// </summary>
        /// <param name="filter">The filter to create the collector with</param>
        /// <param name="options">The options to pass to the collector</param>
        /// <returns></returns>
        public MessageCollector CreateMessageCollector(Func<Message, bool> filter, MessageCollectorOptions options = null)
        {
            //
        }

        /// <summary>
        /// Fetch this DMChannel.
        /// </summary>
        /// <returns></returns>
        public new IPromise<DMChannel> Fetch() => Recipient.CreateDM();

        // Send

        /// <summary>
        /// Starts a typing indicator in the channel.
        /// </summary>
        /// <param name="count">The number of times StartTyping should be considered to have been called</param>
        /// <returns>Resolves once the bot stops typing gracefully, or rejects when an error occurs</returns>
        public IPromise StartTyping(int count = 1)
        {
            //
        }

        /// <summary>
        /// Stops the typing indicator in the channel. The indicator will only stop if this is called as many times as StartTyping().
        /// <br/>
        /// <br/>
        /// <info><b>It can take a few seconds for the client user to stop typing.</b></info>
        /// </summary>
        /// <param name="force">Whether or not to reset the call count and force the indicator to stop</param>
        public void StopTyping(bool force = false)
        {
            //
        }

        /// <summary>
        /// When concatenated with a string, this automatically returns the recipient's mention instead of the DMChannel object.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Recipient.ToString();
    }
}