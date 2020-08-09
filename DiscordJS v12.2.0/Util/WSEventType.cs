namespace DiscordJS
{
    /// <summary>
    /// The type of a websocket message event, e.g. <c>MESSAGE_CREATE</c>
    /// </summary>
    public enum WSEventType
    {
        /// <summary>
        /// The Client becomes ready
        /// </summary>
        READY,

        /// <summary>
        /// The Client has resumed connection to the gateway
        /// </summary>
        RESUMED,

        /// <summary>
        /// A <see cref="Guild"/> was created
        /// </summary>
        GUILD_CREATE,

        /// <summary>
        /// A <see cref="Guild"/> was deleted
        /// </summary>
        GUILD_DELETE,

        /// <summary>
        /// A <see cref="Guild"/> was updated
        /// </summary>
        GUILD_UPDATE,

        /// <summary>
        /// An <see cref="Invite"/> was created
        /// </summary>
        INVITE_CREATE,

        /// <summary>
        /// An <see cref="Invite"/> was deleted
        /// </summary>
        INVITE_DELETE,

        /// <summary>
        /// A <see cref="GuildMember"/> joined the guild
        /// </summary>
        GUILD_MEMBER_ADD,

        /// <summary>
        /// A <see cref="GuildMember"/> left/was removed from the guild
        /// </summary>
        GUILD_MEMBER_REMOVE,

        /// <summary>
        /// A <see cref="GuildMember"/> was updated (e.g. nickname change)
        /// </summary>
        GUILD_MEMBER_UPDATE,

        /// <summary>
        /// A chunk <see cref="GuildMember"/>s was fetched
        /// </summary>
        GUILD_MEMBERS_CHUNK,

        /// <summary>
        /// A <see cref="Role"/> was created
        /// </summary>
        GUILD_ROLE_CREATE,

        /// <summary>
        /// A <see cref="Role"/> was deleted
        /// </summary>
        GUILD_ROLE_DELETE,

        /// <summary>
        /// A <see cref="Role"/> was updated (e.g. color change)
        /// </summary>
        GUILD_ROLE_UPDATE,

        /// <summary>
        /// A <see cref="GuildMember"/> was banned
        /// </summary>
        GUILD_BAN_ADD,

        /// <summary>
        /// A <see cref="GuildMember"/> was unbanned
        /// </summary>
        GUILD_BAN_REMOVE,

        /// <summary>
        /// The Emojis in a guild were updated (created or deleted)
        /// </summary>
        GUILD_EMOJIS_UPDATE,

        /// <summary>
        /// A <see cref="Channel"/> was created. (Either a <see cref="DMChannel"/> was opened, or a <see cref="GuildChannel"/> was created)
        /// </summary>
        CHANNEL_CREATE,

        /// <summary>
        /// A <see cref="Channel"/> was deleted. (Either a <see cref="DMChannel"/> was closed, or a <see cref="GuildChannel"/> was deleted)
        /// </summary>
        CHANNEL_DELETE,

        /// <summary>
        /// A <see cref="Channel"/> was updated. (e.g. Guild Channel permissions changing)
        /// </summary>
        CHANNEL_UPDATE,

        /// <summary>
        /// A <see cref="Channel"/>'s pins were updated (Either a pin was added or removed)
        /// </summary>
        CHANNEL_PINS_UPDATE,

        /// <summary>
        /// A <see cref="Message"/> was received by the Client.
        /// </summary>
        MESSAGE_CREATE,

        /// <summary>
        /// A <see cref="Message"/> was deleted.
        /// </summary>
        MESSAGE_DELETE,

        /// <summary>
        /// A <see cref="Message"/> was updated. (<b>This does not always mean it was edited</b>. Embeds could've been added)
        /// </summary>
        MESSAGE_UPDATE,

        /// <summary>
        /// A group of <see cref="Message"/>s were deleted.
        /// </summary>
        MESSAGE_DELETE_BULK,

        /// <summary>
        /// A reaction was added to a <see cref="Message"/>
        /// </summary>
        MESSAGE_REACTION_ADD,

        /// <summary>
        /// A reaction was removed from a <see cref="Message"/>
        /// </summary>
        MESSAGE_REACTION_REMOVE,

        /// <summary>
        /// All reactions were removed from a <see cref="Message"/>
        /// </summary>
        MESSAGE_REACTION_REMOVE_ALL,

        /// <summary>
        /// An emoji was removed from a reaction <see cref="Message"/>
        /// </summary>
        MESSAGE_REACTION_REMOVE_EMOJI,

        /// <summary>
        /// A <see cref="User"/> was updated (e.g. username change)
        /// </summary>
        USER_UPDATE,

        /// <summary>
        /// A <see cref="User"/>'s presence was updated. (e.g. their online status changed, or they game they were playing changed)
        /// </summary>
        PRESENCE_UPDATE,

        /// <summary>
        /// A <see cref="User"/> started typing in a <see cref="Channel"/>
        /// </summary>
        TYPING_START,

        /// <summary>
        /// A <see cref="User"/>'s voice state changed. (e.g. they muted themselves)
        /// </summary>
        VOICE_STATE_UPDATE,

        /// <summary>
        /// A voice server was updated
        /// </summary>
        VOICE_SERVER_UPDATE,

        /// <summary>
        /// A <see cref="Guild"/>'s <see cref="Webhook"/>'s were updated
        /// </summary>
        WEBHOOKS_UPDATE
    }
}