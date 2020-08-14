using System.Collections.Generic;

namespace DiscordJS
{
    /// <summary>
    /// The target of an entry. It can be one of:
    /// <list type="bullet">
    /// <item>A guild</item>
    /// <item>A user</item>
    /// <item>A role</item>
    /// <item>An emoji</item>
    /// <item>An invite</item>
    /// <item>A webhook</item>
    /// <item>An integration</item>
    /// <item>An object with an id key if target was deleted</item>
    /// <item>An object where the keys represent either the new value or the old value</item>
    /// </list>
    /// </summary>
    public class AuditLogEntryTarget
    {
        public Snowflake ID { get; internal set; }

        private AuditLogEntryTarget(Guild guild)
        {
            //
        }

        private AuditLogEntryTarget(User user)
        {
            //
        }

        private AuditLogEntryTarget(Role role)
        {
            //
        }

        private AuditLogEntryTarget(GuildEmoji emoji)
        {
            //
        }

        private AuditLogEntryTarget(Invite invite)
        {
            //
        }

        private AuditLogEntryTarget(Webhook hook)
        {
            //
        }

        private AuditLogEntryTarget(IHasID idObject)
        {
            //
        }

        private AuditLogEntryTarget(dynamic obj) // expected to be IHasID object
        {
            ID = obj.ID;
        }

        private AuditLogEntryTarget(Dictionary<string, dynamic> obj)
        {
            //
        }

        public static implicit operator AuditLogEntryTarget(Guild guild) => new AuditLogEntryTarget(guild);
        public static implicit operator AuditLogEntryTarget(User user) => new AuditLogEntryTarget(user);
        public static implicit operator AuditLogEntryTarget(Role role) => new AuditLogEntryTarget(role);
        public static implicit operator AuditLogEntryTarget(GuildEmoji emoji) => new AuditLogEntryTarget(emoji);
        public static implicit operator AuditLogEntryTarget(Invite invite) => new AuditLogEntryTarget(invite);
        public static implicit operator AuditLogEntryTarget(Webhook hook) => new AuditLogEntryTarget(hook);
        public static implicit operator AuditLogEntryTarget(GuildChannel idObject) => new AuditLogEntryTarget(idObject);
        public static implicit operator AuditLogEntryTarget(Channel idObject) => new AuditLogEntryTarget(idObject);
        public static implicit operator AuditLogEntryTarget(TextChannel idObject) => new AuditLogEntryTarget(idObject);
        public static implicit operator AuditLogEntryTarget(VoiceChannel idObject) => new AuditLogEntryTarget(idObject);
        public static implicit operator AuditLogEntryTarget(NewsChannel idObject) => new AuditLogEntryTarget(idObject);
        public static implicit operator AuditLogEntryTarget(StoreChannel idObject) => new AuditLogEntryTarget(idObject);
        public static implicit operator AuditLogEntryTarget(CategoryChannel idObject) => new AuditLogEntryTarget(idObject);
        public static implicit operator AuditLogEntryTarget(Dictionary<string, dynamic> obj) => new AuditLogEntryTarget(obj);

        internal static AuditLogEntryTarget FromDynamic(dynamic dynamic)
        {
            return new AuditLogEntryTarget(dynamic);
        }
    }
}