using System;

namespace DiscordJS.Resolvables
{
    /// <summary>
    /// Data that can be resolved to a Message object. This can be:
    /// <list type="bullet">
    /// <item>A Message</item>
    /// <item>A Snowflake</item>
    /// </list>
    /// </summary>
    public class MessageResolvable
    {
        internal enum Type
        {
            Message,
            Snowflake
        }

        internal readonly Type type;
        internal readonly Message message;
        internal readonly Snowflake snowflake;

        internal bool isResolved = false;
        internal Message resolvedValue;

        internal Message Resolve(MessageManager manager)
        {
            if (isResolved)
                return resolvedValue;
            else
            {
                if (type == Type.Message)
                {
                    resolvedValue = message;
                    isResolved = true;
                }
                else if (type == Type.Snowflake)
                {
                    resolvedValue = manager.Cache.Get(snowflake);
                    isResolved = true;
                }
                else
                    throw new ArgumentException("The given type can't be cast to this resolvable");

                return resolvedValue;
            }
        }

        internal Snowflake ResolveID()
        {
            if (type == Type.Snowflake)
                return snowflake;
            else if (type == Type.Message)
                return message.ID;
            else
                throw new ArgumentException("The given type can't be cast to this resolvable");
        }

        public MessageResolvable(Message message)
        {
            type = Type.Message;
            this.message = message;
        }

        public MessageResolvable(Snowflake snowflake)
        {
            type = Type.Snowflake;
            this.snowflake = snowflake;
        }

        public MessageResolvable(JavaScript.String snowflake) : this((Snowflake)snowflake.ToString())
        { }

        public MessageResolvable(string snowflake) : this((Snowflake)snowflake)
        { }

        public static implicit operator MessageResolvable(Message message) => new MessageResolvable(message);
        public static implicit operator MessageResolvable(Snowflake snowflake) => new MessageResolvable(snowflake);
        public static implicit operator MessageResolvable(JavaScript.String snowflake) => new MessageResolvable(snowflake);
        public static implicit operator MessageResolvable(string snowflake) => new MessageResolvable(snowflake);
    }
}