namespace DiscordJS
{
    /// <summary>
    /// Types of channels, either:
    /// <list type="table">
    /// <item>
    /// <term><see cref="DM"/></term>
    /// <description>A DM Channel</description>
    /// </item>
    /// <item>
    /// <term><see cref="GROUP"/></term>
    /// <description>A Group DM Channel</description>
    /// </item>
    /// <item>
    /// <term><see cref="TEXT"/></term>
    /// <description>A Guild Text Channel</description>
    /// </item>
    /// <item>
    /// <term><see cref="VOICE"/></term>
    /// <description>A Guild Voice Channel</description>
    /// </item>
    /// <item>
    /// <term><see cref="CATEGORY"/></term>
    /// <description>A Guild Category Channel</description>
    /// </item>
    /// <item>
    /// <term><see cref="NEWS"/></term>
    /// <description>A Guild News Channel</description>
    /// </item>
    /// <item>
    /// <term><see cref="STORE"/></term>
    /// <description>A Guild Store Channel</description>
    /// </item>
    /// <item>
    /// <term><see cref="UNKNOWN"/></term>
    /// <description>a Generic Channel of unknown type, could be Channel or GuildChannel</description>
    /// </item>
    /// </list>
    /// </summary>
    public enum ChannelTypes
    {
        /// <summary>
        /// A Guild Text Channel
        /// </summary>
        TEXT = 0,

        /// <summary>
        /// A DM Channel
        /// </summary>
        DM = 1,

        /// <summary>
        /// A Guild Voice Channel
        /// </summary>
        VOICE = 2,

        /// <summary>
        /// A Group DM Channel
        /// </summary>
        GROUP = 3,

        /// <summary>
        /// A Guild Category Channel
        /// </summary>
        CATEGORY = 4,

        /// <summary>
        /// A Guild Category Channel
        /// </summary>
        NEWS = 5,

        /// <summary>
        /// A Guild Store Channel
        /// </summary>
        STORE = 6,

        /// <summary>
        /// a Generic Channel of unknown type, could be Channel or GuildChannel
        /// </summary>
        UNKNOWN
    }
}