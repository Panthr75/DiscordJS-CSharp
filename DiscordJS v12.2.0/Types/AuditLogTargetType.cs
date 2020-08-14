namespace DiscordJS
{
    /// <summary>
    /// The target type of an entry, e.g. <c>GUILD</c>. Here are the available types:
    /// <list type="bullet">
    /// <item>GUILD</item>
    /// <item>CHANNEL</item>
    /// <item>USER</item>
    /// <item>ROLE</item>
    /// <item>INVITE</item>
    /// <item>WEBHOOK</item>
    /// <item>EMOJI</item>
    /// <item>MESSAGE</item>
    /// <item>INTEGRATION</item>
    /// </list>
    /// </summary>
    public enum AuditLogTargetType
    {
        UNKNOWN = 0,
        GUILD = 1,
        CHANNEL = 2,
        USER = 3,
        ROLE = 4,
        INVITE = 5,
        WEBHOOK = 6,
        EMOJI = 7,
        MESSAGE = 8,
        INTEGRATION = 9
    }
}