namespace DiscordJS
{
    /// <summary>
    /// An object containing information about a guild member's ban.
    /// </summary>
    public class BanInfo
    {
        /// <summary>
        /// User that was banned
        /// </summary>
        public User User { get; }

        /// <summary>
        /// Reason the user was banned
        /// </summary>
        public string Reason { get; }

        internal BanInfo(User user, string reason)
        {
            User = user;
            Reason = reason;
        }
    }
}