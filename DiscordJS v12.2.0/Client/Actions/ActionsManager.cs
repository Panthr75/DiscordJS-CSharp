namespace DiscordJS.Actions
{
    /// <summary>
    /// The manager for the client that handles actions
    /// </summary>
    public class ActionsManager
    {
        public ChannelCreateAction ChannelCreate { get; }
        public ChannelDeleteAction ChannelDelete { get; }
        public ChannelUpdateAction ChannelUpdate { get; }
        public GuildBanRemoveAction GuildBanRemove { get; }
        public GuildChannelsPositionUpdateAction GuildChannelsPositionUpdate { get; }
        public GuildDeleteAction GuildDelete { get; }
        public GuildEmojiCreateAction GuildEmojiCreate { get; }
        public GuildEmojiDeleteAction GuildEmojiDelete { get; }
        public GuildEmojiUpdateAction GuildEmojiUpdate { get; }
        public GuildEmojisUpdateAction GuildEmojisUpdate { get; }
        public GuildIntegrationsUpdateAction GuildIntegrationsUpdate { get; }
        public GuildMemberRemoveAction GuildMemberRemove { get; }
        public GuildRoleCreateAction GuildRoleCreate { get; }
        public GuildRoleDeleteAction GuildRoleDelete { get; }
        public GuildRoleUpdateAction GuildRoleUpdate { get; }
        public GuildRolesPositionUpdateAction GuildRolesPositionUpdate { get; }
        public GuildUpdateAction GuildUpdate { get; }
        public InviteCreateAction InviteCreate { get; }
        public InviteDeleteAction InviteDelete { get; }
        public MessageCreateAction MessageCreate { get; }
        public MessageDeleteAction MessageDelete { get; }
        public MessageDeleteBulkAction MessageDeleteBulk { get; }
        public MessageReactionAddAction MessageReactionAdd { get; }
        public MessageReactionRemoveAction MessageReactionRemove { get; }
        public MessageReactionRemoveAllAction MessageReactionRemoveAll { get; }
        public MessageReactionRemoveEmojiAction MessageReactionRemoveEmoji { get; }
        public MessageUpdateAction MessageUpdate { get; }
        public PresenceUpdateAction PresenceUpdate { get; }
        public UserUpdateAction UserUpdate { get; }
        public VoiceStateUpdateAction VoiceStateUpdate { get; }
        public WebhooksUpdateAction WebhooksUpdate { get; }

        public ActionsManager(Client client)
        {
            ChannelCreate = new ChannelCreateAction(client);
            ChannelDelete = new ChannelDeleteAction(client);
            ChannelUpdate = new ChannelUpdateAction(client);
            GuildBanRemove = new GuildBanRemoveAction(client);
            GuildChannelsPositionUpdate = new GuildChannelsPositionUpdateAction(client);
            GuildDelete = new GuildDeleteAction(client);
            GuildEmojiCreate = new GuildEmojiCreateAction(client);
            GuildEmojiDelete = new GuildEmojiDeleteAction(client);
            GuildEmojiUpdate = new GuildEmojiUpdateAction(client);
            GuildEmojisUpdate = new GuildEmojisUpdateAction(client);
            GuildIntegrationsUpdate = new GuildIntegrationsUpdateAction(client);
            GuildMemberRemove = new GuildMemberRemoveAction(client);
            GuildRoleCreate = new GuildRoleCreateAction(client);
            GuildRoleDelete = new GuildRoleDeleteAction(client);
            GuildRoleUpdate = new GuildRoleUpdateAction(client);
            GuildRolesPositionUpdate = new GuildRolesPositionUpdateAction(client);
            GuildUpdate = new GuildUpdateAction(client);
            InviteCreate = new InviteCreateAction(client);
            InviteDelete = new InviteDeleteAction(client);
            MessageCreate = new MessageCreateAction(client);
            MessageDelete = new MessageDeleteAction(client);
            MessageDeleteBulk = new MessageDeleteBulkAction(client);
            MessageReactionAdd = new MessageReactionAddAction(client);
            MessageReactionRemove = new MessageReactionRemoveAction(client);
            MessageReactionRemoveAll = new MessageReactionRemoveAllAction(client);
            MessageReactionRemoveEmoji = new MessageReactionRemoveEmojiAction(client);
            MessageUpdate = new MessageUpdateAction(client);
            PresenceUpdate = new PresenceUpdateAction(client);
            UserUpdate = new UserUpdateAction(client);
            VoiceStateUpdate = new VoiceStateUpdateAction(client);
            WebhooksUpdate = new WebhooksUpdateAction(client);
        }
    }
}