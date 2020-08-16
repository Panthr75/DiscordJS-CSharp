namespace DiscordJS.Data
{
    [Data]
    public class PresenceUpdateData
    {
        /// <summary>
        /// the user presence is being updated for
        /// </summary>
        public UserData user;

        /// <summary>
        /// roles this user is in
        /// </summary>
        public string[] roles;

        /// <summary>
        /// null, or the user's current activity
        /// </summary>
        public ActivityData game;

        /// <summary>
        /// id of the guild
        /// </summary>
        public string guild_id;

        /// <summary>
        /// either "idle", "dnd", "online", or "offline"
        /// </summary>
        public string status;

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
}