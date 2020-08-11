namespace DiscordJS
{
    /// <summary>
    /// An array of enabled guild features, here are the possible values:
    /// <list type="bullet">
    /// <item>ANIMATED_ICON</item>
    /// <item>BANNER</item>
    /// <item>COMMERCE</item>
    /// <item>DISCOVERABLE</item>
    /// <item>FEATURABLE</item>
    /// <item>INVITE_SPLASH</item>
    /// <item>NEWS</item>
    /// <item>PARTNERED</item>
    /// <item>PUBLIC</item>
    /// <item>PUBLIC_DISABLED</item>
    /// <item>VANITY_URL</item>
    /// <item>VERIFIED</item>
    /// <item>VIP_REGIONS</item>
    /// <item>WELCOME_SCREEN_ENABLED</item>
    /// </list>
    /// </summary>
    public enum Features
    {
        /// <summary>
        /// The guild has an animated icon
        /// </summary>
        ANIMATED_ICON,

        /// <summary>
        /// The guild has a banner
        /// </summary>
        BANNER,

        /// <summary>
        /// ?
        /// </summary>
        COMMERCE,

        /// <summary>
        /// The guild is discoverable via the guild discovery
        /// </summary>
        DISCOVERABLE,

        /// <summary>
        /// The guild can be featured by Discord
        /// </summary>
        FEATURABLE,

        /// <summary>
        /// The guild has a splash for their banner
        /// </summary>
        INVITE_SPLASH,

        /// <summary>
        /// The guild channel supports news
        /// </summary>
        NEWS,

        /// <summary>
        /// The guild is partnered with discord
        /// </summary>
        PARTNERED,

        /// <summary>
        /// The guild is public
        /// </summary>
        PUBLIC,

        /// <summary>
        /// The guild has public disabled
        /// </summary>
        PUBLIC_DISABLED,

        /// <summary>
        /// The guild has a vanity url (e.g. discord.gg/discordjs)
        /// </summary>
        VANITY_URL,

        /// <summary>
        /// The guild is verified. This is a requirement for guilds that want to sell their game
        /// </summary>
        VERIFIED,

        /// <summary>
        /// The regions for this guild are exclusive
        /// </summary>
        VIP_REGIONS,

        /// <summary>
        /// This guild has a custom welcome screen
        /// </summary>
        WELCOME_SCREEN_ENABLED
    }
}