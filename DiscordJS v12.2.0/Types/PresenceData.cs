namespace DiscordJS.Data
{
    /// <summary>
    /// Data resembling a raw Discord presence.
    /// </summary>
    public class PresenceData
    {
        /// <summary>
        /// Whether the user is AFK
        /// </summary>
        public bool? afk;

        /// <summary>
        /// the user presence is being updated for
        /// </summary>
        public UserData user;

        /// <summary>
        /// roles this user is in
        /// </summary>
        public string[] roles;

        /// <summary>
        /// Status of the user: either "idle", "dnd", "online", or "offline"
        /// </summary>
        public string status;

        /// <summary>
        /// Activity the user is playing: 	null, or the user's current activity
        /// </summary>
        public ActivityData activity;

        /// <summary>
        /// user's current activities
        /// </summary>
        public ActivityData[] activities;

        /// <summary>
        /// user's platform-dependent status
        /// </summary>
        public ClientStatusData client_status;

        /// <summary>
        /// when the user started boosting the guild
        /// </summary>
        public long? premium_since;

        /// <summary>
        /// this users guild nickname (if one is set)
        /// </summary>
        public string nick;
    }

    /// <summary>
    /// Active sessions are indicated with an "online", "idle", or "dnd" string per platform.
    /// </summary>
    public class ClientStatusData
    {
        /// <summary>
        /// the user's status set for an active desktop (Windows, Linux, Mac) application session
        /// </summary>
        public string desktop;

        /// <summary>
        /// the user's status set for an active mobile (iOS, Android) application session
        /// </summary>
        public string mobile;

        /// <summary>
        /// the user's status set for an active web (browser, bot account) application session
        /// </summary>
        public string web;
    }
}