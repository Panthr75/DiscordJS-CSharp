using DiscordJS.Resolvables;

namespace DiscordJS
{
    /// <summary>
    /// Data structure that makes it easy to interact with a <see cref="VoiceConnection.Speaking"/>
    /// and <see cref="GuildMemberSpeakingEvent"/> event bitfields.
    /// </summary>
    public class Speaking : BitField
    {
        /// <summary>
        /// Instantiates a new Speaking BitField with the bitfield value of '0'.
        /// </summary>
        public Speaking() : this(0)
        { }
        /// <summary>
        /// Instantiates a new Speaking BitField with the given bits
        /// </summary>
        /// <param name="bits">Bit(s) to read from</param>
        public Speaking(BitFieldResolvable bits) : base(bits)
        { }

        /// <summary>
        /// Numeric speaking flags. All available properties:
        /// <list type="bullet">
        /// <item>SPEAKING</item>
        /// <item>SOUNDSHARE</item>
        /// <item>PRIORITY_SPEAKING</item>
        /// </list>
        /// <b>See also: </b> <see href="https://discordapp.com/developers/docs/topics/voice-connections#speaking"/>
        /// </summary>
        public enum FLAGS : long
        {
            /// <summary>
            /// Speaking normally
            /// </summary>
            SPEAKING = 1L << 0,

            /// <summary>
            /// Speaking via soundshare
            /// </summary>
            SOUNDSHARE = 1L << 1,

            /// <summary>
            /// Speaking as a priority speaker
            /// </summary>
            PRIORITY_SPEAKING = 1L << 2
        }
    }
}