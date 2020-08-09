using DiscordJS.Resolvables;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Data structure that makes it easy to interact with a User#Flags bitfield.
    /// </summary>
    public class UserFlags : BitField
    {
        /// <summary>
        /// Creates a new instance of the user flags bitfield using the given bits
        /// </summary>
        /// <param name="bits">Bit(s) to read from</param>
        public UserFlags(BitFieldResolvable bits) : base(bits)
        { }

        /// <summary>
        /// Numeric user flags. All available properties:
        /// <list type="table">
        /// <item>
        /// <term>DISCORD_EMPLOYEE</term>
        /// <description>The flags for a Discord Employee. It's like a special little thanks for them :D</description>
        /// </item>
        /// <item>
        /// <term>DISCORD_PARTNER</term>
        /// <description>The flags for a discord partner. This person is either partnered through a guild, or is a direct partner of Discord. (to be verified)</description>
        /// </item>
        /// <item>
        /// <term>HYPESQUAD_EVENTS</term>
        /// </item>
        /// <item>
        /// <term>BUGHUNTER_LEVEL_1</term>
        /// </item>
        /// <item>
        /// <term>HOUSE_BRAVERY</term>
        /// </item>
        /// <item>
        /// <term>HOUSE_BRILLIANCE</term>
        /// </item>
        /// <item>
        /// <term>HOUSE_BALANCE</term>
        /// </item>
        /// <item>
        /// <term>EARLY_SUPPORTER</term>
        /// <description>The flags for a user that has had Discord Nitro before January 1st 2019. (To be verified).</description>
        /// </item>
        /// <item>
        /// <term>TEAM_USER</term>
        /// </item>
        /// <item>
        /// <term>SYSTEM</term>
        /// <description>The flags for the "System Account". As such, it's important that it's identified uniquely from other users</description>
        /// </item>
        /// <item>
        /// <term>BUGHUNTER_LEVEL_2</term>
        /// </item>
        /// <item>
        /// <term>VERIFIED_BOT</term>
        /// <description>The flags for a verified bot. This means the bots is in 75+ guilds, and has gone through the bot verification process.
        /// <br/>
        /// <b>See <see href="https://support.discord.com/hc/en-us/articles/360040720412-Bot-Verification-and-Data-Whitelisting">this article</see> for more details.</b></description>
        /// </item>
        /// <item>
        /// <term>VERIFIED_DEVELOPER</term>
        /// <description>The flags for a verified bot developer. This flag is given if one of their bot oauth applications are verified.</description>
        /// </item>
        /// </list>
        /// <b>See: <see href="https://discordapp.com/developers/docs/resources/user#user-object-user-flags"/></b>
        /// </summary>
        public enum FLAGS : long
        {
            /// <summary>
            /// The flags for a Discord Employee. It's like a special little thanks for them :D
            /// </summary>
            DISCORD_EMPLOYEE = 1 << 0,

            /// <summary>
            /// The flags for a discord partner. This person is either partnered through a guild, or is a direct partner of Discord. (to be verified)
            /// </summary>
            DISCORD_PARTNER = 1 << 1,

            /// <summary>
            /// 
            /// </summary>
            HYPESQUAD_EVENTS = 1 << 2,

            /// <summary>
            /// 
            /// </summary>
            BUGHUNTER_LEVEL_1 = 1 << 3,

            /// <summary>
            /// 
            /// </summary>
            HOUSE_BRAVERY = 1 << 6,

            /// <summary>
            /// 
            /// </summary>
            HOUSE_BRILLIANCE = 1 << 7,

            /// <summary>
            /// 
            /// </summary>
            HOUSE_BALANCE = 1 << 8,

            /// <summary>
            /// The flags for a user that has had Discord Nitro before January 1st 2019. (To be verified).
            /// </summary>
            EARLY_SUPPORTER = 1 << 9,

            /// <summary>
            /// 
            /// </summary>
            TEAM_USER = 1 << 10,

            /// <summary>
            /// The flags for the "System Account". As such, it's important that it's identified uniquely from other users
            /// </summary>
            SYSTEM = 1 << 12,

            /// <summary>
            /// 
            /// </summary>
            BUGHUNTER_LEVEL_2 = 1 << 14,

            /// <summary>
            /// The flags for a verified bot. This means the bots is in 75+ guilds, and has gone through the bot verification process.
            /// <br/>
            /// <b>See <see href="https://support.discord.com/hc/en-us/articles/360040720412-Bot-Verification-and-Data-Whitelisting">this article</see> for more details.</b>
            /// </summary>
            VERIFIED_BOT = 1 << 16,

            /// <summary>
            /// The flags for a verified bot developer. This flag is given if one of their bot oauth applications are verified.
            /// </summary>
            VERIFIED_DEVELOPER = 1 << 17,
        }

        /// <inheritdoc/>
        protected override Type FlagsType => base.FlagsType;
    }
}