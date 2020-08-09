using DiscordJS.Resolvables;
using JavaScript;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Data structure that makes it easy to calculate intents.
    /// </summary>
    public class Intents : BitField
    {
        protected override Type FlagsType => typeof(FLAGS);

        /// <summary>
        /// Creates a new Intents object
        /// </summary>
        /// <param name="intents">The intents</param>
        public Intents(IntentsResolvable intents) : base(intents)
        { }

        /// <summary>
        /// Numeric websocket intents. All available properties:
        /// <list type="bullet">
        /// <item><c>GUILDS</c></item>
        /// <item><c>GUILD_MEMBERS</c></item>
        /// <item><c>GUILD_BANS</c></item>
        /// <item><c>GUILD_EMOJIS</c></item>
        /// <item><c>GUILD_INTEGRATIONS</c></item>
        /// <item><c>GUILD_WEBHOOKS</c></item>
        /// <item><c>GUILD_INVITES</c></item>
        /// <item><c>GUILD_VOICE_STATES</c></item>
        /// <item><c>GUILD_PRESENCES</c></item>
        /// <item><c>GUILD_MESSAGES</c></item>
        /// <item><c>GUILD_MESSAGE_REACTIONS</c></item>
        /// <item><c>GUILD_MESSAGE_TYPING</c></item>
        /// <item><c>DIRECT_MESSAGES</c></item>
        /// <item><c>DIRECT_MESSAGE_REACTIONS</c></item>
        /// <item><c>DIRECT_MESSAGE_TYPING</c></item>
        /// </list>
        /// <b>See also:</b>
        /// <seealso href="https://discordapp.com/developers/docs/topics/gateway#list-of-intents"/>
        /// </summary>
        public enum FLAGS : long
        {
            GUILDS = 1 << 0,
            GUILD_MEMBERS = 1 << 1,
            GUILD_BANS = 1 << 2,
            GUILD_EMOJIS = 1 << 3,
            GUILD_INTEGRATIONS = 1 << 4,
            GUILD_WEBHOOKS = 1 << 5,
            GUILD_INVITES = 1 << 6,
            GUILD_VOICE_STATES = 1 << 7,
            GUILD_PRESENCES = 1 << 8,
            GUILD_MESSAGES = 1 << 9,
            GUILD_MESSAGE_REACTIONS = 1 << 10,
            GUILD_MESSAGE_TYPING = 1 << 11,
            DIRECT_MESSAGES = 1 << 12,
            DIRECT_MESSAGE_REACTIONS = 1 << 13,
            DIRECT_MESSAGE_TYPING = 1 << 14
        }

        /// <summary>
        /// Bitfield representing all privileged intents
        /// <br/>
        /// <b>See also:</b>
        /// <seealso href="https://discordapp.com/developers/docs/topics/gateway#privileged-intents"/>
        /// </summary>
        public static long PRIVILEGED { get; }

        /// <summary>
        /// Bitfield representing all intents combined
        /// </summary>
        public static long ALL { get; }

        /// <summary>
        /// Bitfield representing all non-privileged intents
        /// </summary>
        public static long NON_PRIVILEGED { get; }

        static Intents()
        {
            PRIVILEGED = (long)FLAGS.GUILD_MEMBERS | (long)FLAGS.GUILD_PRESENCES;

            ALL = new Array<long>(Enum.GetValues(typeof(FLAGS)) as long[]).Reduce((acc, p) => acc | p, 0L);

            NON_PRIVILEGED = ALL & ~PRIVILEGED;
        }
    }
}