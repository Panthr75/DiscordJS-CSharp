using JavaScript;
using System;

namespace DiscordJS
{
    /// <summary>
    /// The main hub for interacting with the Discord API, and the starting point for any bot.
    /// </summary>
    public class Client : BaseClient
    {
        /// <summary>
        /// All of the Channels that the client is currently handling, mapped by their IDs - as long as sharding isn't being used, this will be every channel in <i>every</i> guild the bot is a member of. Note that DM channels will not be initially cached, and thus not be present in the Manager without their explicit fetching or use.
        /// </summary>
        public ChannelManager Channels { get; internal set; }

        /// <summary>
        /// All custom emojis that the client has access to, mapped by their IDs
        /// </summary>
        public GuildEmojiManager Emojis { get; }

        /// <summary>
        /// All of the guilds the client is currently handling, mapped by their IDs - as long as sharding isn't being used, this will be every guild the bot is a member of
        /// </summary>
        public GuildManager Guilds { get; internal set; }

        /// <summary>
        /// Authorization token for the logged in bot
        /// <br/>
        /// <br/>
        /// <b>This should be kept private at all times.</b>
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// User that the client is logged in as
        /// </summary>
        public ClientUser User { get; internal set; }

        /// <summary>
        /// All of the <see cref="User"/> objects that have been cached at any point, mapped by their IDs
        /// </summary>
        public UserManager Users { get; internal set; }

        /// <summary>
        /// The voice manager of the client
        /// </summary>
        public ClientVoiceManager Voice { get; internal set; }

        /// <summary>
        /// The WebSocket manager of the client
        /// </summary>
        public WebSocketManager WS { get; internal set; }

        /// <summary>
        /// Emitted whenever a channel is created.
        /// </summary>
        public event ChannelCreateEvent ChannelCreate;

        /// <summary>
        /// Emitted whenever a channel is deleted.
        /// </summary>
        public event ChannelDeleteEvent ChannelDelete;

        /// <summary>
        /// Emitted whenever the pins of a channel are updated. Due to the nature of the WebSocket event, not much information can be provided easily here - you need to manually check the pins yourself.
        /// </summary>
        public event ChannelPinsUpdateEvent ChannelPinsUpdate;

        /// <summary>
        /// Emitted whenever a channel is updated - e.g. name change, topic change, channel type change.
        /// </summary>
        public event ChannelUpdateEvent ChannelUpdate;

        /// <summary>
        /// Emitted for general debugging information.
        /// </summary>
        public event DebugEvent Debug;

        /// <summary>
        /// Emitted whenever a custom emoji is created in a guild.
        /// </summary>
        public event EmojiCreateEvent EmojiCreate;

        /// <summary>
        /// Emitted whenever a custom emoji is deleted in a guild.
        /// </summary>
        public event EmojiDeleteEvent EmojiDelete;

        /// <summary>
        /// Emitted whenever a custom emoji is updated in a guild.
        /// </summary>
        public event EmojiUpdateEvent EmojiUpdate;

        /// <summary>
        /// Emitted when the client encounters an exception.
        /// </summary>
        public event ExceptionEvent Exception;

        /// <summary>
        /// Emitted whenever a member is banned from a guild.
        /// </summary>
        public event GuildBanAddEvent GuildBanAdd;

        /// <summary>
        /// Emitted whenever a member is unbanned from a guild.
        /// </summary>
        public event GuildBanRemoveEvent GuildBanRemove;

        /// <summary>
        /// Emitted whenever the client joins a guild.
        /// </summary>
        public event GuildCreateEvent GuildCreate;

        /// <summary>
        /// Emitted whenever a guild kicks the client or the guild is deleted/left.
        /// </summary>
        public event GuildDeleteEvent GuildDelete;

        /// <summary>
        /// Emitted whenever a guild integration is updated
        /// </summary>
        public event GuildIntegrationsUpdateEvent GuildIntegrationsUpdate;

        /// <summary>
        /// Emitted whenever a user joins a guild.
        /// </summary>
        public event GuildMemberAddEvent GuildMemberAdd;

        /// <summary>
        /// Emitted whenever a member leaves a guild, or is kicked.
        /// </summary>
        public event GuildMemberRemoveEvent GuildMemberRemove;

        /// <summary>
        /// Emitted whenever a chunk of guild members is received (all members come from the same guild).
        /// </summary>
        public event GuildMembersChunkEvent GuildMembersChunk;

        /// <summary>
        /// Emitted once a guild member changes speaking state.
        /// </summary>
        public event GuildMemberSpeakingEvent GuildMembersSpeaking;

        /// <summary>
        /// Emitted whenever a guild member changes - i.e. new role, removed role, nickname.
        /// </summary>
        public event GuildMemberUpdateEvent GuildMemberUpdate;

        /// <summary>
        /// Emitted whenever a guild becomes unavailable, likely due to a server outage.
        /// </summary>
        public event GuildUnavailableEvent GuildUnavailable;

        /// <summary>
        /// Emitted whenever a guild is updated - e.g. name change.
        /// </summary>
        public event GuildUpdateEvent GuildUpdate;

        /// <summary>
        /// Emitted when the client's session becomes invalidated. You are expected to handle closing the process gracefully and preventing a boot loop if you are listening to this event.
        /// </summary>
        public event InvalidatedEvent Invalidated;

        /// <summary>
        /// Emitted when an invite is created.
        /// <br/>
        /// <br/>
        /// <info><b>This event only triggers if the client has <c>MANAGE_GUILD</c> permissions for the guild, or <c>MANAGE_CHANNEL</c> permissions for the channel.</b></info>
        /// </summary>
        public event InviteCreateEvent InviteCreate;

        /// <summary>
        /// Emitted when an invite is deleted.
        /// <br/>
        /// <br/>
        /// <info><b>This event only triggers if the client has <c>MANAGE_GUILD</c> permissions for the guild, or <c>MANAGE_CHANNEL</c> permissions for the channel.</b></info>
        /// </summary>
        public event InviteDeleteEvent InviteDelete;

        /// <summary>
        /// Emitted whenever a message is created.
        /// </summary>
        public event MessageEvent Message;

        /// <summary>
        /// Emitted whenever a message is deleted.
        /// </summary>
        public event MessageDeleteEvent MessageDelete;

        /// <summary>
        /// Emitted whenever messages are deleted in bulk.
        /// </summary>
        public event MessageDeleteBulkEvent MessageDeleteBulk;

        /// <summary>
        /// Emitted whenever a reaction is added to a cached message.
        /// </summary>
        public event MessageReactionAddEvent MessageReactionAdd;

        /// <summary>
        /// Emitted whenever a reaction is removed from a cached message.
        /// </summary>
        public event MessageReactionRemoveEvent MessageReactionRemove;

        /// <summary>
        /// Emitted whenever all reactions are removed from a cached message.
        /// </summary>
        public event MessageReactionRemoveAllEvent MessageReactionRemoveAll;

        /// <summary>
        /// Emitted when a bot removes an emoji reaction from a cached message.
        /// </summary>
        public event MessageReactionRemoveEmojiEvent MessageReactionRemoveEmoji;

        /// <summary>
        /// Emitted whenever a message is updated - e.g. embed or content change.
        /// </summary>
        public event MessageUpdateEvent MessageUpdate;

        /// <summary>
        /// Emitted whenever a guild member's presence (e.g. status, activity) is changed.
        /// </summary>
        public event PresenceUpdateEvent PresenceUpdate;

        /// <summary>
        /// Emitted when the client hits a rate limit while making a request
        /// </summary>
        public event RateLimitEvent RateLimit;

        /// <summary>
        /// Emitted when the client becomes ready to start working.
        /// </summary>
        public event ReadyEvent Ready;

        /// <summary>
        /// Emitted whenever a role is created.
        /// </summary>
        public event RoleCreateEvent RoleCreate;

        /// <summary>
        /// Emitted whenever a guild role is deleted.
        /// </summary>
        public event RoleDeleteEvent RoleDelete;

        /// <summary>
        /// Emitted whenever a guild role is updated.
        /// </summary>
        public event RoleUpdateEvent RoleUpdate;

        /// <summary>
        /// Emitted when a shard's WebSocket disconnects and will no longer reconnect.
        /// </summary>
        public event ShardDisconnectEvent ShardDisconnect;

        /// <summary>
        /// Emitted whenever a shard's WebSocket encounters a connection exception.
        /// </summary>
        public event ShardExceptionEvent ShardException;

        /// <summary>
        /// Emitted when a shard turns ready.
        /// </summary>
        public event ShardReadyEvent ShardReady;

        /// <summary>
        /// Emitted when a shard is attempting to reconnect or re-identify.
        /// </summary>
        public event ShardReconnectingEvent ShardReconnecting;

        /// <summary>
        /// Emitted when a shard resumes successfully.
        /// </summary>
        public event ShardResumeEvent ShardResume;

        /// <summary>
        /// Emitted whenever a user starts typing in a channel.
        /// </summary>
        public event TypingStartEvent TypingStart;

        /// <summary>
        /// Emitted whenever a user's details (e.g. username) are changed.
        /// </summary>
        public event UserUpdateEvent UserUpdate;

        /// <summary>
        /// Emitted whenever a member changes voice state - e.g. joins/leaves a channel, mutes/unmutes.
        /// </summary>
        public event VoiceStateUpdateEvent VoiceStateUpdate;

        /// <summary>
        /// Emitted for general warnings.
        /// </summary>
        public event WarnEvent Warn;

        /// <summary>
        /// Emitted whenever a guild text channel has its webhooks changed.
        /// </summary>
        public event WebhookUpdateEvent WebhookUpdate;

        public Client() : this(new ClientOptions())
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options">Options for the client</param>
        public Client(ClientOptions options) : base(options)
        { 
            //
        }

        public async void Login(string token)
        {
            //
        }
    }
}