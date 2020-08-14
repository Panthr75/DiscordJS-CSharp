using JavaScript;
using DiscordJS.Data;

namespace DiscordJS
{
    /// <summary>
    /// Audit logs entries are held in this class.
    /// </summary>
    public class GuildAuditLogs
    {
        /// <summary>
        /// The entries for this guild's audit logs
        /// </summary>
        public Collection<Snowflake, GuildAuditLogsEntry> Entries { get; internal set; }

        /// <summary>
        /// Cached webhooks
        /// </summary>
        internal Collection<Snowflake, Webhook> Webhooks { get; }

        /// <summary>
        /// Cached integrations
        /// </summary>
        internal Collection<Snowflake, Integration> Integrations { get; }

        /// <summary>
        /// Instantiates a new GuildAuditLogs instance
        /// </summary>
        /// <param name="guild">The instantiating guild</param>
        /// <param name="data">The data</param>
        public GuildAuditLogs(Guild guild, AuditLogData data)
        {
            if (data.users != null)
            {
                for (int index = 0, length = data.users.Length; index < length; index++)
                    guild.Client.Users.Add(data.users[index]);
            }
            Webhooks = new Collection<Snowflake, Webhook>();
            if (data.webhooks != null)
            {
                for (int index = 0, length = data.webhooks.Length; index < length; index++)
                {
                    var hook = data.webhooks[index];
                    Webhooks.Set(hook.id, new Webhook(guild.Client, hook));
                }
            }
            Integrations = new Collection<Snowflake, Integration>();
            if (data.integrations != null)
            {
                for (int index = 0, length = data.integrations.Length; index < length; index++)
                {
                    var integration = data.integrations[index];
                    Integrations.Set(integration.id, new Integration(guild.Client, integration, guild));
                }
            }
            Entries = new Collection<Snowflake, GuildAuditLogsEntry>();
            if (data.audit_log_entries != null)
            {
                for (int index = 0, length = data.audit_log_entries.Length; index < length; index++)
                {
                    var item = data.audit_log_entries[index];
                    GuildAuditLogsEntry entry = new GuildAuditLogsEntry(this, guild, item);
                    Entries.Set(entry.ID, entry);
                }
            }
        }

        private static Array<int> CreateAuditLogActions { get; }
        private static Array<int> DeleteAuditLogActions { get; }
        private static Array<int> UpdateAuditLogActions { get; }

        /// <summary>
        /// Finds the target type from the entry action.
        /// </summary>
        /// <param name="target">The action target</param>
        /// <returns></returns>
        public static AuditLogTargetType TargetType(AuditLogAction target) => (AuditLogTargetType)(((int)target + 1) / 10);

        /// <summary>
        /// Finds the target type from the entry action.
        /// </summary>
        /// <param name="target">The action target</param>
        /// <returns></returns>
        public static AuditLogTargetType TargetType(int target) => TargetType((AuditLogAction)target);

        /// <summary>
        /// Finds the action type from the entry action.
        /// </summary>
        /// <param name="action">The action target</param>
        /// <returns></returns>
        public static AuditLogActionType ActionType(AuditLogAction action)
        {
            int a = (int)action;
            if (CreateAuditLogActions.Includes(a)) return AuditLogActionType.CREATE;
            if (DeleteAuditLogActions.Includes(a)) return AuditLogActionType.DELETE;
            if (UpdateAuditLogActions.Includes(a)) return AuditLogActionType.UPDATE;
            return AuditLogActionType.ALL;
        }

        /// <summary>
        /// Finds the action type from the entry action.
        /// </summary>
        /// <param name="action">The action target</param>
        /// <returns></returns>
        public static AuditLogActionType ActionType(int action) => ActionType((AuditLogAction)action);

        static GuildAuditLogs()
        {
            CreateAuditLogActions = new Array<AuditLogAction>(
                AuditLogAction.CHANNEL_CREATE,
                AuditLogAction.CHANNEL_OVERWRITE_CREATE,
                AuditLogAction.MEMBER_BAN_REMOVE,
                AuditLogAction.BOT_ADD,
                AuditLogAction.ROLE_CREATE,
                AuditLogAction.INVITE_CREATE,
                AuditLogAction.WEBHOOK_CREATE,
                AuditLogAction.EMOJI_CREATE,
                AuditLogAction.EMOJI_CREATE,
                AuditLogAction.MESSAGE_PIN,
                AuditLogAction.INTEGRATION_CREATE).Map((a) => (int)a);

            DeleteAuditLogActions = new Array<AuditLogAction>(
                AuditLogAction.CHANNEL_DELETE,
                AuditLogAction.CHANNEL_OVERWRITE_DELETE,
                AuditLogAction.MEMBER_KICK,
                AuditLogAction.MEMBER_PRUNE,
                AuditLogAction.MEMBER_BAN_ADD,
                AuditLogAction.MEMBER_DISCONNECT,
                AuditLogAction.ROLE_DELETE,
                AuditLogAction.INVITE_DELETE,
                AuditLogAction.WEBHOOK_DELETE,
                AuditLogAction.EMOJI_DELETE,
                AuditLogAction.MESSAGE_DELETE,
                AuditLogAction.MESSAGE_BULK_DELETE,
                AuditLogAction.MESSAGE_UNPIN,
                AuditLogAction.INTEGRATION_UPDATE).Map((a) => (int)a);

            UpdateAuditLogActions = new Array<AuditLogAction>(
                AuditLogAction.GUILD_UPDATE,
                AuditLogAction.CHANNEL_UPDATE,
                AuditLogAction.CHANNEL_OVERWRITE_UPDATE,
                AuditLogAction.MEMBER_UPDATE,
                AuditLogAction.MEMBER_ROLE_UPDATE,
                AuditLogAction.MEMBER_MOVE,
                AuditLogAction.ROLE_UPDATE,
                AuditLogAction.INVITE_UPDATE,
                AuditLogAction.WEBHOOK_UPDATE,
                AuditLogAction.EMOJI_UPDATE,
                AuditLogAction.INTEGRATION_UPDATE).Map((a) => (int)a);
        }
    }
}