using JavaScript;
using System;

namespace DiscordJS
{
    public interface ITextBasedChannel
    {
        /// <summary>
        /// The client this text based channel belongs to
        /// </summary>
        Client Client { get; }

        /// <summary>
        /// The time the channel was created at
        /// </summary>
        Date CreatedAt { get; }

        /// <summary>
        /// The timestamp the channel was created at
        /// </summary>
        long CreatedTimestamp { get; }

        /// <summary>
        /// Whether the channel has been deleted
        /// </summary>
        bool Deleted { get; }

        /// <summary>
        /// The ID of this text channel
        /// </summary>
        Snowflake ID { get; }

        /// <summary>
        /// A manager of the messages belonging to this channel
        /// </summary>
        MessageManager Messages { get; }

        /// <summary>
        /// The type of the channel, either:
        /// <list type="table">
        /// <item>
        /// <term>dm</term>
        /// <description>A DM Channel</description>
        /// </item>
        /// <item>
        /// <term>group</term>
        /// <description>A Group DM Channel</description>
        /// </item>
        /// <item>
        /// <term>text</term>
        /// <description>A Guild Text Channel</description>
        /// </item>
        /// <item>
        /// <term>voice</term>
        /// <description>A Guild Voice Channel</description>
        /// </item>
        /// <item>
        /// <term>category</term>
        /// <description>A Guild Category Channel</description>
        /// </item>
        /// <item>
        /// <term>news</term>
        /// <description>A Guild News Channel</description>
        /// </item>
        /// <item>
        /// <term>store</term>
        /// <description>A Guild Store Channel</description>
        /// </item>
        /// <item>
        /// <term>unknown</term>
        /// <description>a Generic Channel of unknown type, could be Channel or GuildChannel</description>
        /// </item>
        /// </list>
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Similar to CreateMessageCollector but in promise form. Resolves with a collection of messages that pass the specified filter.
        /// </summary>
        /// <param name="filter">The filter function to use</param>
        /// <param name="options">Optional options to pass to the internal collector</param>
        /// <returns></returns>
        IPromise<Collection<Snowflake, Message>> AwaitMessages(Func<Message, Collection<Snowflake, Message>, bool> filter, AwaitMessagesOptions options = null);

        /// <summary>
        /// Bulk deletes given messages that are newer than two weeks.
        /// </summary>
        /// <param name="messages">Messages or number of messages to delete</param>
        /// <param name="filterOld">Filter messages to remove those which are older than two weeks automatically</param>
        /// <returns>Deleted messages</returns>
        IPromise<Collection<Snowflake, Message>> BulkDelete(Collection<Snowflake, Message> messages, bool filterOld = true);

        /// <summary>
        /// Bulk deletes given messages that are newer than two weeks.
        /// </summary>
        /// <param name="messages">Messages or number of messages to delete</param>
        /// <param name="filterOld">Filter messages to remove those which are older than two weeks automatically</param>
        /// <returns>Deleted messages</returns>
        IPromise<Collection<Snowflake, Message>> BulkDelete(Array<Message> messages, bool filterOld = true);

        /// <summary>
        /// Bulk deletes given messages that are newer than two weeks.
        /// </summary>
        /// <param name="messages">Messages or number of messages to delete</param>
        /// <param name="filterOld">Filter messages to remove those which are older than two weeks automatically</param>
        /// <returns>Deleted messages</returns>
        IPromise<Collection<Snowflake, Message>> BulkDelete(Array<Snowflake> messages, bool filterOld = true);

        /// <summary>
        /// Bulk deletes given messages that are newer than two weeks.
        /// </summary>
        /// <param name="messages">Messages or number of messages to delete</param>
        /// <param name="filterOld">Filter messages to remove those which are older than two weeks automatically</param>
        /// <returns>Deleted messages</returns>
        IPromise<Collection<Snowflake, Message>> BulkDelete(int messages, bool filterOld = true);

        /// <summary>
        /// Creates a Message Collector.
        /// </summary>
        /// <param name="filter">The filter to create the collector with</param>
        /// <param name="options">The options to pass to the collector</param>
        /// <returns></returns>
        MessageCollector CreateMessageCollector(Func<Message, Collection<Snowflake, Message>, bool> filter, MessageCollectorOptions options = null);

        /// <summary>
        /// Starts a typing indicator in the channel.
        /// </summary>
        /// <param name="count">The number of times startTyping should be considered to have been called</param>
        /// <returns>Resolves once the bot stops typing gracefully, or rejects when an error occurs</returns>
        IPromise StartTyping(int? count = 1);

        /// <summary>
        /// Stops the typing indicator in the channel. The indicator will only stop if this is called as many times as StartTyping().
        /// <br/>
        /// <br/>
        /// <info><b>It can take a few seconds for the client user to stop typing.</b></info>
        /// </summary>
        /// <param name="force">Whether or not to reset the call count and force the indicator to stop</param>
        void StopTyping(bool force = false);
    }
}