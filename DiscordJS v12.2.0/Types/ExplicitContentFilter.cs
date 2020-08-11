namespace DiscordJS
{
    /// <summary>
    /// The value set for the explicit content filter levels for a guild:
    /// <list type="bullet">
    /// <item>DISABLED</item>
    /// <item>MEMBERS_WITHOUT_ROLES</item>
    /// <item>ALL_MEMBERS</item>
    /// </list>
    /// </summary>
    public enum ExplicitContentFilterLevel
    {
        /// <summary>
        /// No explicit content filter on the guild
        /// </summary>
        DISABLED,

        /// <summary>
        /// The filter should apply to members without any roles
        /// </summary>
        MEMBERS_WITHOUT_ROLES,

        /// <summary>
        /// The filter should apply to all members
        /// </summary>
        ALL_MEMBERS
    }
}