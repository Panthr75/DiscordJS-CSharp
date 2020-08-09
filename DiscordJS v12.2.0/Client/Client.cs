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
        /// <param name="channel">The channel that was created</param>
        public delegate void ChannelCreateEvent(Channel channel);

        /// <summary>
        /// Emitted whenever a channel is deleted.
        /// </summary>
        /// <param name="channel">The channel that was deleted</param>
        public delegate void ChannelDeleteEvent(Channel channel);

        /// <summary>
        /// Emitted whenever the pins of a channel are updated. Due to the nature of the WebSocket event, not much information can be provided easily here - you need to manually check the pins yourself.
        /// </summary>
        /// <param name="channel">The channel that the pins update occurred in</param>
        /// <param name="time">The time of the pins update</param>
        public delegate void ChannelPinsUpdateEvent(Channel channel, Date time);

        /// <summary>
        /// Emitted whenever a channel is updated - e.g. name change, topic change, channel type change.
        /// </summary>
        /// <param name="oldChannel">The channel before the update</param>
        /// <param name="newChannel">The channel after the update</param>
        public delegate void ChannelUpdateEvent(Channel oldChannel, Channel newChannel);

        /// <summary>
        /// Emitted for general debugging information.
        /// </summary>
        /// <param name="info">The debug information</param>
        public delegate void DebugEvent(string info);

        /// <summary>
        /// Emitted whenever a custom emoji is created in a guild.
        /// </summary>
        /// <param name="emoji">The emoji that was created</param>
        public delegate void EmojiCreateEvent(GuildEmoji emoji);

        /// <summary>
        /// Emitted whenever a custom emoji is deleted in a guild.
        /// </summary>
        /// <param name="emoji">The emoji that was deleted</param>
        public delegate void EmojiDeleteEvent(GuildEmoji emoji);

        /// <summary>
        /// Emitted whenever a custom emoji is updated in a guild.
        /// </summary>
        /// <param name="oldEmoji">The old emoji</param>
        /// <param name="newEmoji">The new emoji</param>
        public delegate void EmojiUpdateEvent(GuildEmoji oldEmoji, GuildEmoji newEmoji);

        /// <summary>
        /// Emitted when the client encounters an exception.
        /// </summary>
        /// <param name="exception">The exception encountered</param>
        public delegate void ExceptionEvent(Exception exception);

        /// <summary>
        /// Emitted whenever a member is banned from a guild.
        /// </summary>
        /// <param name="guild">The guild that the ban occurred in</param>
        /// <param name="user">The user that was banned</param>
        public delegate void GuildBanAddEvent(Guild guild, User user);

        /// <summary>
        /// Emitted whenever a member is unbanned from a guild.
        /// </summary>
        /// <param name="guild">The guild that the unban occurred in</param>
        /// <param name="user">The user that was unbanned</param>
        public delegate void GuildBanRemoveEvent(Guild guild, User user);

        /// <summary>
        /// Emitted whenever the client joins a guild.
        /// </summary>
        /// <param name="guild">The created guild</param>
        public delegate void GuildCreateEvent(Guild guild);

        /// <summary>
        /// Emitted whenever a guild kicks the client or the guild is deleted/left.
        /// </summary>
        /// <param name="guild">The guild that was deleted</param>
        public delegate void GuildDeleteEvent(Guild guild);

        /// <summary>
        /// Emitted whenever a guild integration is updated
        /// </summary>
        /// <param name="guild">The guild whose integrations were updated</param>
        public delegate void GuildIntegrationsUpdateEvent(Guild guild);

        /// <summary>
        /// Emitted whenever a member leaves a guild, or is kicked.
        /// </summary>
        /// <param name="member">The member that has joined a guild</param>
        public delegate void GuildMemberRemoveEvent(GuildMember member);

        /// <summary>
        /// Emitted whenever a chunk of guild members is received (all members come from the same guild).
        /// </summary>
        /// <param name="members">The members in the chunk</param>
        /// <param name="guild">The guild related to the member chunk</param>
        public delegate void GuildMembersChunkEvent(Collection<Snowflake, GuildMember> members, Guild guild);

        /// <summary>
        /// Emitted whenever a message is created.
        /// </summary>
        /// <param name="message">The created message</param>
        public delegate void MessageEvent(Message message);

        /// <summary>
        /// Emitted whenever a user starts typing in a channel.
        /// </summary>
        /// <param name="channel">The channel the user started typing in</param>
        /// <param name="user">The user that started typing</param>
        public delegate void TypingStartEvent(Channel channel, User user);

        /// <summary>
        /// Emitted whenever a user's details (e.g. username) are changed.
        /// </summary>
        /// <param name="oldUser">The user before the update</param>
        /// <param name="newUser">The user after the update</param>
        public delegate void UserUpdateEvent(User oldUser, User newUser);

        /// <summary>
        /// Emitted whenever a member changes voice state - e.g. joins/leaves a channel, mutes/unmutes.
        /// </summary>
        /// <param name="oldState">The voice state before the update</param>
        /// <param name="newState">The voice state after the update</param>
        public delegate void VoiceStateUpdateEvent(VoiceState oldState, VoiceState newState);

        /// <summary>
        /// Emitted for general warnings.
        /// </summary>
        /// <param name="info">The warning</param>
        public delegate void WarnEvent(string info);

        /// <summary>
        /// Emitted whenever a guild text channel has its webhooks changed.
        /// </summary>
        /// <param name="channel">The channel that had a webhook update</param>
        public delegate void WebhookUpdateEvent(TextChannel channel);

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
        /// Emitted whenever a member leaves a guild, or is kicked.
        /// </summary>
        public event GuildMemberRemoveEvent GuildMemberRemove;

        /// <summary>
        /// Emitted whenever a chunk of guild members is received (all members come from the same guild).
        /// </summary>
        public event GuildMembersChunkEvent GuildMembersChunk;

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