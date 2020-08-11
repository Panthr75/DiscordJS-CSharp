namespace DiscordJS.WebSockets
{
    /// <summary>
    /// The current status of the client. Here are the available statuses:
    /// <list type="bullet">
    /// <item>READY</item>
    /// <item>CONNECTING</item>
    /// <item>RECONNECTING</item>
    /// <item>IDLE</item>
    /// <item>NEARLY</item>
    /// <item>DISCONNECTED</item>
    /// <item>WAITING_FOR_GUILDS</item>
    /// <item>IDENTIFYING</item>
    /// <item>RESUMING</item>
    /// </list>
    /// </summary>
    public enum Status
    {
        READY = 0,
        CONNECTING = 1,
        RECONNECTING = 2,
        IDLE = 3,
        NEARLY = 4,
        DISCONNECTED = 5,
        WAITING_FOR_GUILDS = 6,
        IDENTIFYING = 7,
        RESUMING = 8
    }
}