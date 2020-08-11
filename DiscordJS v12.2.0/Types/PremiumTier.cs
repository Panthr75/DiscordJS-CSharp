namespace DiscordJS
{
    /// <summary>
    /// The type of premium tier:
    /// <list type="table">
    /// <item>
    /// <term>0</term>
    /// <description>NONE</description>
    /// </item>
    /// <item>
    /// <term>1</term>
    /// <description>TIER_1</description>
    /// </item>
    /// <item>
    /// <term>2</term>
    /// <description>TIER_2</description>
    /// </item>
    /// <item>
    /// <term>3</term>
    /// <description>TIER_3</description>
    /// </item>
    /// </list>
    /// </summary>
    public enum PremiumTier
    {
        /// <summary>
        /// The guild is not boosted
        /// </summary>
        NONE = 0,

        /// <summary>
        /// The guild is at a level 1 boost. The perks given:
        /// <list type="bullet">
        /// <item><b>100 guild emoji slots</b></item>
        /// <item><b>128 Kbps audio quality</b></item>
        /// <item><b>Animated guild icon</b></item>
        /// <item><b>Custom guild invite background</b></item>
        /// <item><b>Stream to your friends in high quality.</b></item>
        /// </list>
        /// </summary>
        TIER_1 = 1,

        /// <summary>
        /// The guild is at a level 2 boost. The perks given:
        /// <list type="bullet">
        /// <item><b>150 total guild emoji slots</b></item>
        /// <item><b>256 Kbps audio quality</b></item>
        /// <item><b>Guild Banner</b></item>
        /// <item><b>50 MB upload limit for all guild members (Parity with Discord Nitro Classic's upload limit)</b></item>
        /// <item><b>1080p 60fps Go Live streams</b></item>
        /// <item><i>Animated guild icon</i></item>
        /// <item><i>Custom guild invite background</i></item>
        /// </list>
        /// </summary>
        TIER_2 = 2,

        /// <summary>
        /// The guild is at the maximum boost Discord offers for a guild: level 3. The perks given:
        /// <list type="bullet">
        /// <item><b>250 total guild emoji slots</b></item>
        /// <item><b>384 Kbps audio quality</b></item>
        /// <item><b>Vanity URL for the guild</b></item>
        /// <item><b>100 MB upload limit for all guild members (Parity with Discord Nitro's upload limit)</b></item>
        /// <item><i>Guild Banner</i></item>
        /// <item><i>1080p 60fps Go Live streams</i></item>
        /// <item><i>Animated guild icon</i></item>
        /// <item><i>Custom guild invite background</i></item>
        /// </list>
        /// </summary>
        TIER_3 = 3
    }
}