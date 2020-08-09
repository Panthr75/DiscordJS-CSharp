using System;

namespace DiscordJS
{
    public class MessageCollector : Collector<Message>
    {
        public MessageCollector(TextChannel channel, Func<Message, Collection<Snowflake, Message>, bool> filter, MessageCollectorOptions options) : base(channel.Client, options)
        {
            //
        }
        public MessageCollector(DMChannel channel, Func<Message, Collection<Snowflake, Message>, bool> filter, MessageCollectorOptions options) : base(channel.Client, options)
        {
            //
        }
    }

    public class MessageCollectorOptions : CollectorOptions
    {
        //
    }

    public class MessageCollectorCollectionException : CollectorCollectionException<Message>
    {
        public MessageCollectorCollectionException(Collection<Snowflake, Message> collection, string reason)
        {
            //
        }
    }
}