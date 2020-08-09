using DiscordJS.Resolvables;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Data structure that makes it easy to interact with an Activity#Flags bitfield.
    /// </summary>
    public class ActivityFlags : BitField
    {
        protected override Type FlagsType => typeof(FLAGS);

        /// <summary>
        /// Creates a new instance of the activity flags
        /// </summary>
        /// <param name="bit">Bit(s) to read from</param>
        public ActivityFlags(BitFieldResolvable bit) : base(bit)
        { }

        /// <summary>
        /// Numeric activity flags. All available properties:
        /// <list type="bullet">
        /// <item><c>INSTANCE</c></item>
        /// <item><c>JOIN</c></item>
        /// <item><c>SPECTATE</c></item>
        /// <item><c>JOIN_REQUEST</c></item>
        /// <item><c>SYNC</c></item>
        /// <item><c>PLAY</c></item>
        /// </list>
        /// <b>See also:</b> <seealso href="https://discordapp.com/developers/docs/topics/gateway#activity-object-activity-flags"/>
        /// </summary>
        public enum FLAGS
        {
            INSTANCE = 1 << 0,
            JOIN = 1 << 1,
            SPECTATE = 1 << 2,
            JOIN_REQUEST = 1 << 3,
            SYNC = 1 << 4,
            PLAY = 1 << 5
        }
    }
}