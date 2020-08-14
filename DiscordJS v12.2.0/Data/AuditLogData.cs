namespace DiscordJS.Data
{
    /// <summary>
    /// Data for a GuildAuditLog
    /// </summary>
    public class AuditLogData
    {
        /// <summary>
        /// list of webhooks found in the audit log
        /// </summary>
        public WebhookData[] webhooks;

        /// <summary>
        /// list of users found in the audit log
        /// </summary>
        public UserData[] users;

        /// <summary>
        /// list of audit log entries
        /// </summary>
        public AuditLogEntryData[] audit_log_entries;

        /// <summary>
        /// list of partial integration objects
        /// </summary>
        public IntegrationData[] integrations;
    }

    /// <summary>
    /// The data for an audit log entry
    /// </summary>
    public class AuditLogEntryData
    {
        /// <summary>
        /// id of the affected entity (webhook, user, role, etc.)
        /// </summary>
        public string target_id;

        /// <summary>
        /// changes made to the target_id
        /// </summary>
        public AuditLogChangeData[] changes;

        /// <summary>
        /// the user who made the changes
        /// </summary>
        public string user_id;

        /// <summary>
        /// id of the entry
        /// </summary>
        public string id;

        /// <summary>
        /// type of action that occurred
        /// </summary>
        public int action_type;

        /// <summary>
        /// additional info for certain action types
        /// </summary>
        public AuditLogEntryInfoData options;

        /// <summary>
        /// the reason for the change (0-512 characters)
        /// </summary>
        public string reason;
    }

    /// <summary>
    /// The optional audit log entry info that contains additional info for certain action types
    /// </summary>
    public class AuditLogEntryInfoData
    {
        /// <summary>
        /// number of days after which inactive members were kicked
        /// </summary>
        public string delete_member_days;

        /// <summary>
        /// number of members removed by the prune
        /// </summary>
        public string members_removed;

        /// <summary>
        /// channel in which the entities were targeted
        /// </summary>
        public string channel_id;

        /// <summary>
        /// id of the message that was targeted
        /// </summary>
        public string message_id;

        /// <summary>
        /// number of entities that were targeted
        /// </summary>
        public string count;

        /// <summary>
        /// id of the overwritten entity
        /// </summary>
        public string id;

        /// <summary>
        /// type of overwritten entity ("member" or "role")
        /// </summary>
        public string type;

        /// <summary>
        /// name of the role if type is "role"
        /// </summary>
        public string role_name;
    }

    /// <summary>
    /// Represents the changes for an audit log entry
    /// </summary>
    public class AuditLogChangeData
    {
        /// <summary>
        /// new value of the key
        /// </summary>
        public dynamic new_value;

        /// <summary>
        /// old value of the key
        /// </summary>
        public dynamic old_value;

        /// <summary>
        /// name of audit log change key
        /// </summary>
        public string key;
    }
}