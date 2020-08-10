using System;

namespace DiscordJS.Resolvables
{
    /// <summary>
    /// Data that can be resolved to a MessageReaction object. This can be:
    /// <list type="bullet">
    /// <item>A MessageReaction</item>
    /// <item>A Snowflake</item>
    /// </list>
    /// </summary>
    public class MessageReactionResolvable
    {
        internal enum Type
        {
            MessageReaction,
            Snowflake
        }

        internal readonly Type type;

        internal readonly MessageReaction reaction;
        internal readonly Snowflake snowflake;

        /// <summary>
        /// Resolves this resolvable using the given reaction manager.
        /// </summary>
        /// <param name="manager">The reaction manager to use</param>
        /// <returns></returns>
        internal MessageReaction Resolve(ReactionManager manager)
        {
            if (type == Type.MessageReaction)
                return reaction;
            else if (type == Type.Snowflake)
                return manager.Cache.Get(snowflake);
            else
                throw new ArgumentException("The given type can't be cast to this resolvable");
        }

        internal Snowflake ResolveID()
        {
            if (type == Type.MessageReaction)
                return ((IHasID)reaction).ID;
            else if (type == Type.Snowflake)
                return snowflake;
            else
                throw new ArgumentException("The given type can't be cast to this resolvable");
        }

        public MessageReactionResolvable(MessageReaction reaction)
        {
            this.reaction = reaction;
            type = Type.MessageReaction;
        }

        public MessageReactionResolvable(Snowflake snowflake)
        {
            this.snowflake = snowflake;
            type = Type.Snowflake;
        }

        public MessageReactionResolvable(JavaScript.String snowflake) : this((Snowflake)snowflake.ToString())
        { }

        public MessageReactionResolvable(string snowflake) : this((Snowflake)snowflake)
        { }


        public static implicit operator MessageReactionResolvable(MessageReaction reaction) => new MessageReactionResolvable(reaction);
        public static implicit operator MessageReactionResolvable(Snowflake snowflake) => new MessageReactionResolvable(snowflake);
        public static implicit operator MessageReactionResolvable(JavaScript.String snowflake) => new MessageReactionResolvable(snowflake);
        public static implicit operator MessageReactionResolvable(string snowflake) => new MessageReactionResolvable(snowflake);
    }
}