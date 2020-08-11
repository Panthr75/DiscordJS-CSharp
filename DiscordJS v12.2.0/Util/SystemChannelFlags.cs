using DiscordJS.Resolvables;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Data structure that makes it easy to interact with a <see cref="Guild.SystemChannelFlags"/> bitfield.
    /// <br/>
    /// <br/>
    /// <info><b>Note that all event message types are enabled by default, and by setting their corresponding flags you are disabling them</b></info>
    /// </summary>
    public class SystemChannelFlags : BitField
    {
        /// <summary>
        /// Numeric system channel flags. All available properties:
        /// </summary>
        public enum FLAGS : long
        {
            /// <summary>
            /// The welcome messages of users joining is disabled
            /// </summary>
            WELCOME_MESSAGE_DISABLED = 1 << 0,

            /// <summary>
            /// The boost message for when users boost the guild is disabled
            /// </summary>
            BOOST_MESSAGE_DISABLED = 1 << 1
        }

        /// <inheritdoc/>
        protected override Type FlagsType => typeof(FLAGS);

        /// <summary>
        /// Instantiates a new Empty SystemChannelFlags bitfield
        /// </summary>
        public SystemChannelFlags() : base(0)
        { }

        /// <summary>
        /// Instantiates a new SystemChannelFlags with the given bits
        /// </summary>
        /// <param name="bits">Bit(s) to read from</param>
        public SystemChannelFlags(SystemChannelFlagsResolvable bits) : base(bits)
        { }

        ///<inheritdoc/>
        public new SystemChannelFlags Freeze()
        {
            base.Freeze();
            return this;
        }
    }
}