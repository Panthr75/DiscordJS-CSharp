namespace DiscordJS
{
    /// <summary>
    /// The action type of an entry, e.g. <c>CREATE</c>. Here are the available types:
    /// </summary>
    public enum AuditLogActionType
    {
        /// <summary>
        /// A thing was created
        /// </summary>
        CREATE,

        /// <summary>
        /// A thing was deleted
        /// </summary>
        DELETE,

        /// <summary>
        /// A thing was updated
        /// </summary>
        UPDATE,

        /// <summary>
        /// All at once (woah)
        /// </summary>
        ALL
    }
}