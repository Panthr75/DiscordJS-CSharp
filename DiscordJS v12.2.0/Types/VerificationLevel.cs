namespace DiscordJS
{
    /// <summary>
    /// The value set for the verification levels for a guild:
    /// </summary>
    public enum VerificationLevel
    {
        /// <summary>
        /// Unrestricted
        /// </summary>
        NONE,

        /// <summary>
        /// Guild members must have a verified email on their Discord account
        /// </summary>
        LOW,

        /// <summary>
        /// Guild members must have a verified email on their Discord account, and must be registered on Discord for longer than 5 minutes.
        /// </summary>
        MEDIUM,

        /// <summary>
        /// Guild members must have a verified email on their Discord account, must be registered on Discord for longer than 5 minutes, and must also be a member of this guild for more than 10 minutes.
        /// </summary>
        HIGH,

        /// <summary>
        /// Guild members must have a verified email on their Discord account, must be registered on Discord for longer than 5 minutes, must also be a member of this guild for more than 10 minutes, and must have a verified phone on their discord account.
        /// </summary>
        VERY_HIGH
    }
}