using JavaScript;
using System;
using DiscordJS.Rest;
using DiscordJS.WebSockets;
using DiscordJS.Actions;

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
        /// The options the client was instantiated with
        /// </summary>
        public ClientOptions Options { get; internal set; }

        /// <summary>
        /// Time at which the client was last regarded as being in the READY state (each time the client disconnects and successfully reconnects, this will be overwritten)
        /// </summary>
        public Date ReadyAt { get; internal set; }

        /// <summary>
        /// Timestamp of the time the client was last READY at
        /// </summary>
        public long? ReadyTimestamp { get; }

        /// <summary>
        /// Shard helpers for the client (only if the process was spawned from a ShardingManager)
        /// </summary>
        public ShardClientUtil Shard { get; internal set; }

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

        internal ActionsManager Actions { get; }

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

        /// <summary>
        /// Emitted whenever a channel is created.
        /// </summary>
        public event ChannelCreateEvent ChannelCreate;

        /// <summary>
        /// Emitted whenever a channel is created.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event ChannelCreateEvent OnceChannelCreate;

        internal bool EmitChannelCreate(Channel channel)
        {
            bool result = ChannelCreate == null && OnceChannelCreate == null;
            ChannelCreate?.Invoke(channel);
            OnceChannelCreate?.Invoke(channel);
            OnceChannelCreate = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a channel is deleted.
        /// </summary>
        public event ChannelDeleteEvent ChannelDelete;

        /// <summary>
        /// Emitted whenever a channel is deleted.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event ChannelDeleteEvent OnceChannelDelete;

        internal bool EmitChannelDelete(Channel channel)
        {
            bool result = ChannelDelete == null && OnceChannelDelete == null;
            ChannelDelete?.Invoke(channel);
            OnceChannelDelete?.Invoke(channel);
            OnceChannelDelete = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever the pins of a channel are updated. Due to the nature of the WebSocket event, not much information can be provided easily here - you need to manually check the pins yourself.
        /// </summary>
        public event ChannelPinsUpdateEvent ChannelPinsUpdate;

        /// <summary>
        /// Emitted whenever the pins of a channel are updated. Due to the nature of the WebSocket event, not much information can be provided easily here - you need to manually check the pins yourself.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event ChannelPinsUpdateEvent OnceChannelPinsUpdate;

        internal bool EmitChannelPinsUpdate(Channel channel, Date time)
        {
            bool result = ChannelPinsUpdate == null && OnceChannelPinsUpdate == null;
            ChannelPinsUpdate?.Invoke(channel, time);
            OnceChannelPinsUpdate?.Invoke(channel, time);
            OnceChannelPinsUpdate = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a channel is updated - e.g. name change, topic change, channel type change.
        /// </summary>
        public event ChannelUpdateEvent ChannelUpdate;

        /// <summary>
        /// Emitted whenever a channel is updated - e.g. name change, topic change, channel type change.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event ChannelUpdateEvent OnceChannelUpdate;

        internal bool EmitChannelUpdate(Channel oldChannel, Channel newChannel)
        {
            bool result = ChannelUpdate == null && OnceChannelUpdate == null;
            ChannelUpdate?.Invoke(oldChannel, newChannel);
            OnceChannelUpdate?.Invoke(oldChannel, newChannel);
            OnceChannelUpdate = null;
            return result;
        }

        /// <summary>
        /// Emitted for general debugging information.
        /// </summary>
        public event DebugEvent Debug;

        /// <summary>
        /// Emitted for general debugging information.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event DebugEvent OnceDebug;

        internal bool EmitDebug(string info)
        {
            bool result = Debug == null && OnceDebug == null;
            Debug?.Invoke(info);
            OnceDebug?.Invoke(info);
            OnceDebug = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a custom emoji is created in a guild.
        /// </summary>
        public event EmojiCreateEvent EmojiCreate;

        /// <summary>
        /// Emitted whenever a custom emoji is created in a guild.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event EmojiCreateEvent OnceEmojiCreate;

        internal bool EmitEmojiCreate(GuildEmoji emoji)
        {
            bool result = EmojiCreate == null && OnceEmojiCreate == null;
            EmojiCreate?.Invoke(emoji);
            OnceEmojiCreate?.Invoke(emoji);
            OnceEmojiCreate = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a custom emoji is deleted in a guild.
        /// </summary>
        public event EmojiDeleteEvent EmojiDelete;

        /// <summary>
        /// Emitted whenever a custom emoji is deleted in a guild.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event EmojiDeleteEvent OnceEmojiDelete;

        internal bool EmitEmojiDelete(GuildEmoji emoji)
        {
            bool result = EmojiDelete == null && OnceEmojiDelete == null;
            EmojiDelete?.Invoke(emoji);
            OnceEmojiDelete?.Invoke(emoji);
            OnceEmojiDelete = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a custom emoji is updated in a guild.
        /// </summary>
        public event EmojiUpdateEvent EmojiUpdate;

        /// <summary>
        /// Emitted whenever a custom emoji is updated in a guild.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event EmojiUpdateEvent OnceEmojiUpdate;

        internal bool EmitEmojiUpdate(GuildEmoji oldEmoji, GuildEmoji newEmoji)
        {
            bool result = EmojiUpdate == null && OnceEmojiUpdate == null;
            EmojiUpdate?.Invoke(oldEmoji, newEmoji);
            OnceEmojiUpdate?.Invoke(oldEmoji, newEmoji);
            OnceEmojiUpdate = null;
            return result;
        }

        /// <summary>
        /// Emitted when the client encounters an exception.
        /// </summary>
        public event ExceptionEvent Exception;

        /// <summary>
        /// Emitted when the client encounters an exception.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event ExceptionEvent OnceException;

        internal bool EmitException(Exception exception)
        {
            bool result = Exception == null && OnceException == null;
            Exception?.Invoke(exception);
            OnceException?.Invoke(exception);
            OnceException = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a member is banned from a guild.
        /// </summary>
        public event GuildBanAddEvent GuildBanAdd;

        /// <summary>
        /// Emitted whenever a member is banned from a guild.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event GuildBanAddEvent OnceGuildBanAdd;

        internal bool EmitGuildBanAdd(Guild guild, User user)
        {
            bool result = GuildBanAdd == null && OnceGuildBanAdd == null;
            GuildBanAdd?.Invoke(guild, user);
            OnceGuildBanAdd?.Invoke(guild, user);
            OnceGuildBanAdd = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a member is unbanned from a guild.
        /// </summary>
        public event GuildBanRemoveEvent GuildBanRemove;

        /// <summary>
        /// Emitted whenever a member is unbanned from a guild.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event GuildBanRemoveEvent OnceGuildBanRemove;

        internal bool EmitGuildBanRemove(Guild guild, User user)
        {
            bool result = GuildBanRemove == null && OnceGuildBanRemove == null;
            GuildBanRemove?.Invoke(guild, user);
            OnceGuildBanRemove?.Invoke(guild, user);
            OnceGuildBanRemove = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever the client joins a guild.
        /// </summary>
        public event GuildCreateEvent GuildCreate;

        /// <summary>
        /// Emitted whenever the client joins a guild.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event GuildCreateEvent OnceGuildCreate;

        internal bool EmitGuildCreate(Guild guild)
        {
            bool result = GuildCreate == null && OnceGuildCreate == null;
            GuildCreate?.Invoke(guild);
            OnceGuildCreate?.Invoke(guild);
            OnceGuildCreate = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a guild kicks the client or the guild is deleted/left.
        /// </summary>
        public event GuildDeleteEvent GuildDelete;

        /// <summary>
        /// Emitted whenever a guild kicks the client or the guild is deleted/left.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event GuildDeleteEvent OnceGuildDelete;

        internal bool EmitGuildDelete(Guild guild)
        {
            bool result = GuildDelete == null && OnceGuildDelete == null;
            GuildDelete?.Invoke(guild);
            OnceGuildDelete?.Invoke(guild);
            OnceGuildDelete = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a guild integration is updated
        /// </summary>
        public event GuildIntegrationsUpdateEvent GuildIntegrationsUpdate;

        /// <summary>
        /// Emitted whenever a guild integration is updated
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event GuildIntegrationsUpdateEvent OnceGuildIntegrationsUpdate;

        internal bool EmitGuildIntegrationsUpdate(Guild guild)
        {
            bool result = GuildIntegrationsUpdate == null && OnceGuildIntegrationsUpdate == null;
            GuildIntegrationsUpdate?.Invoke(guild);
            OnceGuildIntegrationsUpdate?.Invoke(guild);
            OnceGuildIntegrationsUpdate = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a user joins a guild.
        /// </summary>
        public event GuildMemberAddEvent GuildMemberAdd;

        /// <summary>
        /// Emitted whenever a user joins a guild.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event GuildMemberAddEvent OnceGuildMemberAdd;

        internal bool EmitGuildMemberAdd(GuildMember member)
        {
            bool result = GuildMemberAdd == null && OnceGuildMemberAdd == null;
            GuildMemberAdd?.Invoke(member);
            OnceGuildMemberAdd?.Invoke(member);
            OnceGuildMemberAdd = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a member leaves a guild, or is kicked.
        /// </summary>
        public event GuildMemberRemoveEvent GuildMemberRemove;

        /// <summary>
        /// Emitted whenever a member leaves a guild, or is kicked.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event GuildMemberRemoveEvent OnceGuildMemberRemove;

        internal bool EmitGuildMemberRemove(GuildMember member)
        {
            bool result = GuildMemberRemove == null && OnceGuildMemberRemove == null;
            GuildMemberRemove?.Invoke(member);
            OnceGuildMemberRemove?.Invoke(member);
            OnceGuildMemberRemove = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a chunk of guild members is received (all members come from the same guild).
        /// </summary>
        public event GuildMembersChunkEvent GuildMembersChunk;

        /// <summary>
        /// Emitted whenever a chunk of guild members is received (all members come from the same guild).
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event GuildMembersChunkEvent OnceGuildMembersChunk;

        internal bool EmitGuildMembersChunk(Collection<Snowflake, GuildMember> members, Guild guild)
        {
            bool result = GuildMembersChunk == null && OnceGuildMembersChunk == null;
            GuildMembersChunk?.Invoke(members, guild);
            OnceGuildMembersChunk?.Invoke(members, guild);
            OnceGuildMembersChunk = null;
            return result;
        }

        /// <summary>
        /// Emitted once a guild member changes speaking state.
        /// </summary>
        public event GuildMemberSpeakingEvent GuildMembersSpeaking;

        /// <summary>
        /// Emitted once a guild member changes speaking state.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event GuildMemberSpeakingEvent OnceGuildMembersSpeaking;

        internal bool EmitGuildMemberSpeaking(GuildMember member, Speaking speaking)
        {
            bool result = GuildMembersSpeaking == null && OnceGuildMembersSpeaking == null;
            GuildMembersSpeaking?.Invoke(member, speaking);
            OnceGuildMembersSpeaking?.Invoke(member, speaking);
            OnceGuildMembersSpeaking = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a guild member changes - i.e. new role, removed role, nickname.
        /// </summary>
        public event GuildMemberUpdateEvent GuildMemberUpdate;

        /// <summary>
        /// Emitted whenever a guild member changes - i.e. new role, removed role, nickname.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event GuildMemberUpdateEvent OnceGuildMemberUpdate;

        internal bool EmitGuildMemberUpdate(GuildMember oldMember, GuildMember newMember)
        {
            bool result = GuildMemberUpdate == null && OnceGuildMemberUpdate == null;
            GuildMemberUpdate?.Invoke(oldMember, newMember);
            OnceGuildMemberUpdate?.Invoke(oldMember, newMember);
            OnceGuildMemberUpdate = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a guild becomes unavailable, likely due to a server outage.
        /// </summary>
        public event GuildUnavailableEvent GuildUnavailable;

        /// <summary>
        /// Emitted whenever a guild becomes unavailable, likely due to a server outage.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event GuildUnavailableEvent OnceGuildUnavailable;

        internal bool EmitGuildUnavailable(Guild guild)
        {
            bool result = GuildUnavailable == null && OnceGuildUnavailable == null;
            GuildUnavailable?.Invoke(guild);
            OnceGuildUnavailable?.Invoke(guild);
            OnceGuildUnavailable = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a guild is updated - e.g. name change.
        /// </summary>
        public event GuildUpdateEvent GuildUpdate;

        /// <summary>
        /// Emitted whenever a guild is updated - e.g. name change.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event GuildUpdateEvent OnceGuildUpdate;

        internal bool EmitGuildUpdate(Guild oldGuild, Guild newGuild)
        {
            bool result = GuildUpdate == null && OnceGuildUpdate == null;
            GuildUpdate?.Invoke(oldGuild, newGuild);
            OnceGuildUpdate?.Invoke(oldGuild, newGuild);
            OnceGuildUpdate = null;
            return result;
        }

        /// <summary>
        /// Emitted when the client's session becomes invalidated. You are expected to handle closing the process gracefully and preventing a boot loop if you are listening to this event.
        /// </summary>
        public event InvalidatedEvent Invalidated;

        /// <summary>
        /// Emitted when the client's session becomes invalidated. You are expected to handle closing the process gracefully and preventing a boot loop if you are listening to this event.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event InvalidatedEvent OnceInvalidated;

        internal bool EmitInvalidated()
        {
            bool result = Invalidated == null && OnceInvalidated == null;
            Invalidated?.Invoke();
            OnceInvalidated?.Invoke();
            OnceInvalidated = null;
            return result;
        }

        /// <summary>
        /// Emitted when an invite is created.
        /// <br/>
        /// <br/>
        /// <info><b>This event only triggers if the client has <c>MANAGE_GUILD</c> permissions for the guild, or <c>MANAGE_CHANNEL</c> permissions for the channel.</b></info>
        /// </summary>
        public event InviteCreateEvent InviteCreate;

        /// <summary>
        /// Emitted when an invite is created.
        /// <br/>
        /// <br/>
        /// <info><b>This event only triggers if the client has <c>MANAGE_GUILD</c> permissions for the guild, or <c>MANAGE_CHANNEL</c> permissions for the channel.</b></info>
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event InviteCreateEvent OnceInviteCreate;

        internal bool EmitInviteCreate(Invite invite)
        {
            bool result = InviteCreate == null && OnceInviteCreate == null;
            InviteCreate?.Invoke(invite);
            OnceInviteCreate?.Invoke(invite);
            OnceInviteCreate = null;
            return result;
        }

        /// <summary>
        /// Emitted when an invite is deleted.
        /// <br/>
        /// <br/>
        /// <info><b>This event only triggers if the client has <c>MANAGE_GUILD</c> permissions for the guild, or <c>MANAGE_CHANNEL</c> permissions for the channel.</b></info>
        /// </summary>
        public event InviteDeleteEvent InviteDelete;

        /// <summary>
        /// Emitted when an invite is deleted.
        /// <br/>
        /// <br/>
        /// <info><b>This event only triggers if the client has <c>MANAGE_GUILD</c> permissions for the guild, or <c>MANAGE_CHANNEL</c> permissions for the channel.</b></info>
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event InviteDeleteEvent OnceInviteDelete;

        internal bool EmitInviteDelete(Invite invite)
        {
            bool result = InviteDelete == null && OnceInviteDelete == null;
            InviteDelete?.Invoke(invite);
            OnceInviteDelete?.Invoke(invite);
            OnceInviteDelete = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a message is created.
        /// </summary>
        public event MessageEvent Message;

        /// <summary>
        /// Emitted whenever a message is created.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event MessageEvent OnceMessage;

        internal bool EmitMessage(Message message)
        {
            bool result = Message == null && OnceMessage == null;
            Message?.Invoke(message);
            OnceMessage?.Invoke(message);
            OnceMessage = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a message is deleted.
        /// </summary>
        public event MessageDeleteEvent MessageDelete;

        /// <summary>
        /// Emitted whenever a message is deleted.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event MessageDeleteEvent OnceMessageDelete;

        internal bool EmitMessageDelete(Message message)
        {
            bool result = MessageDelete == null && OnceMessageDelete == null;
            MessageDelete?.Invoke(message);
            OnceMessageDelete?.Invoke(message);
            OnceMessageDelete = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever messages are deleted in bulk.
        /// </summary>
        public event MessageDeleteBulkEvent MessageDeleteBulk;

        /// <summary>
        /// Emitted whenever messages are deleted in bulk.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event MessageDeleteBulkEvent OnceMessageDeleteBulk;

        internal bool EmitMessageDeleteBulk(Collection<Snowflake, Message> messages)
        {
            bool result = MessageDeleteBulk == null && OnceMessageDeleteBulk == null;
            MessageDeleteBulk?.Invoke(messages);
            OnceMessageDeleteBulk?.Invoke(messages);
            OnceMessageDeleteBulk = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a reaction is added to a cached message.
        /// </summary>
        public event MessageReactionAddEvent MessageReactionAdd;

        /// <summary>
        /// Emitted whenever a reaction is added to a cached message.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event MessageReactionAddEvent OnceMessageReactionAdd;

        internal bool EmitMessageReactionAdd(MessageReaction messageReaction, User user)
        {
            bool result = MessageReactionAdd == null && OnceMessageReactionAdd == null;
            MessageReactionAdd?.Invoke(messageReaction, user);
            OnceMessageReactionAdd?.Invoke(messageReaction, user);
            OnceMessageReactionAdd = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a reaction is removed from a cached message.
        /// </summary>
        public event MessageReactionRemoveEvent MessageReactionRemove;

        /// <summary>
        /// Emitted whenever a reaction is removed from a cached message.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event MessageReactionRemoveEvent OnceMessageReactionRemove;

        internal bool EmitMessageReactionRemove(MessageReaction messageReaction, User user)
        {
            bool result = MessageReactionRemove == null && OnceMessageReactionRemove == null;
            MessageReactionRemove?.Invoke(messageReaction, user);
            OnceMessageReactionRemove?.Invoke(messageReaction, user);
            OnceMessageReactionRemove = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever all reactions are removed from a cached message.
        /// </summary>
        public event MessageReactionRemoveAllEvent MessageReactionRemoveAll;

        /// <summary>
        /// Emitted whenever all reactions are removed from a cached message.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event MessageReactionRemoveAllEvent OnceMessageReactionRemoveAll;

        internal bool EmitMessageReactionRemoveAll(Message message)
        {
            bool result = MessageReactionRemoveAll == null && OnceMessageReactionRemoveAll == null;
            MessageReactionRemoveAll?.Invoke(message);
            OnceMessageReactionRemoveAll?.Invoke(message);
            OnceMessageReactionRemoveAll = null;
            return result;
        }

        /// <summary>
        /// Emitted when a bot removes an emoji reaction from a cached message.
        /// </summary>
        public event MessageReactionRemoveEmojiEvent MessageReactionRemoveEmoji;

        /// <summary>
        /// Emitted when a bot removes an emoji reaction from a cached message.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event MessageReactionRemoveEmojiEvent OnceMessageReactionRemoveEmoji;

        internal bool EmitMessageReactionRemoveEmoji(MessageReaction reaction)
        {
            bool result = MessageReactionRemoveEmoji == null && OnceMessageReactionRemoveEmoji == null;
            MessageReactionRemoveEmoji?.Invoke(reaction);
            OnceMessageReactionRemoveEmoji?.Invoke(reaction);
            OnceMessageReactionRemoveEmoji = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a message is updated - e.g. embed or content change.
        /// </summary>
        public event MessageUpdateEvent MessageUpdate;

        /// <summary>
        /// Emitted whenever a message is updated - e.g. embed or content change.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event MessageUpdateEvent OnceMessageUpdate;

        internal bool EmitMessageUpdate(Message oldMessage, Message newMessage)
        {
            bool result = MessageUpdate == null && OnceMessageUpdate == null;
            MessageUpdate?.Invoke(oldMessage, newMessage);
            OnceMessageUpdate?.Invoke(oldMessage, newMessage);
            OnceMessageUpdate = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a guild member's presence (e.g. status, activity) is changed.
        /// </summary>
        public event PresenceUpdateEvent PresenceUpdate;

        /// <summary>
        /// Emitted whenever a guild member's presence (e.g. status, activity) is changed.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event PresenceUpdateEvent OncePresenceUpdate;

        internal bool EmitPresenceUpdate(Presence oldPresence, Presence newPresence)
        {
            bool result = PresenceUpdate == null && OncePresenceUpdate == null;
            PresenceUpdate?.Invoke(oldPresence, newPresence);
            OncePresenceUpdate?.Invoke(oldPresence, newPresence);
            OncePresenceUpdate = null;
            return result;
        }

        /// <summary>
        /// Emitted when the client hits a rate limit while making a request
        /// </summary>
        public event RateLimitEvent RateLimit;

        /// <summary>
        /// Emitted when the client hits a rate limit while making a request
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event RateLimitEvent OnceRateLimit;

        internal bool EmitRateLimitInfo(RateLimitInfo rateLimitInfo)
        {
            bool result = RateLimit == null && OnceRateLimit == null;
            RateLimit?.Invoke(rateLimitInfo);
            OnceRateLimit?.Invoke(rateLimitInfo);
            OnceRateLimit = null;
            return result;
        }

        /// <summary>
        /// Emitted when the client becomes ready to start working.
        /// </summary>
        public event ReadyEvent Ready;

        /// <summary>
        /// Emitted when the client becomes ready to start working.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event ReadyEvent OnceReady;

        internal bool EmitReady()
        {
            bool result = Ready == null && OnceReady == null;
            Ready?.Invoke();
            OnceReady?.Invoke();
            OnceReady = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a role is created.
        /// </summary>
        public event RoleCreateEvent RoleCreate;

        /// <summary>
        /// Emitted whenever a role is created.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event RoleCreateEvent OnceRoleCreate;

        internal bool EmitRoleCreate(Role role)
        {
            bool result = RoleCreate == null && OnceRoleCreate == null;
            RoleCreate?.Invoke(role);
            OnceRoleCreate?.Invoke(role);
            OnceRoleCreate = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a guild role is deleted.
        /// </summary>
        public event RoleDeleteEvent RoleDelete;

        /// <summary>
        /// Emitted whenever a guild role is deleted.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event RoleDeleteEvent OnceRoleDelete;

        internal bool EmitRoleDelete(Role role)
        {
            bool result = RoleDelete == null && OnceRoleDelete == null;
            RoleDelete?.Invoke(role);
            OnceRoleDelete?.Invoke(role);
            OnceRoleDelete = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a guild role is updated.
        /// </summary>
        public event RoleUpdateEvent RoleUpdate;

        /// <summary>
        /// Emitted whenever a guild role is updated.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event RoleUpdateEvent OnceRoleUpdate;

        internal bool EmitRoleUpdate(Role oldRole, Role newRole)
        {
            bool result = RoleUpdate == null && OnceRoleUpdate == null;
            RoleUpdate?.Invoke(oldRole, newRole);
            OnceRoleUpdate?.Invoke(oldRole, newRole);
            OnceRoleUpdate = null;
            return result;
        }

        /// <summary>
        /// Emitted when a shard's WebSocket disconnects and will no longer reconnect.
        /// </summary>
        public event ShardDisconnectEvent ShardDisconnect;

        /// <summary>
        /// Emitted when a shard's WebSocket disconnects and will no longer reconnect.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event ShardDisconnectEvent OnceShardDisconnect;

        internal bool EmitShardDisconnect(WSCloseEvent ev, int id)
        {
            bool result = ShardDisconnect == null && OnceShardDisconnect == null;
            ShardDisconnect?.Invoke(ev, id);
            OnceShardDisconnect?.Invoke(ev, id);
            OnceShardDisconnect = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a shard's WebSocket encounters a connection exception.
        /// </summary>
        public event ShardExceptionEvent ShardException;

        /// <summary>
        /// Emitted whenever a shard's WebSocket encounters a connection exception.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event ShardExceptionEvent OnceShardException;

        internal bool EmitShardException(Exception exception, int id)
        {
            var result = ShardException == null && OnceShardException == null;
            ShardException?.Invoke(exception, id);
            OnceShardException?.Invoke(exception, id);
            OnceShardException = null;
            return result;
        }

        /// <summary>
        /// Emitted when a shard turns ready.
        /// </summary>
        public event ShardReadyEvent ShardReady;

        /// <summary>
        /// Emitted when a shard turns ready.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event ShardReadyEvent OnceShardReady;

        internal bool EmitShardReady(int id, Set<string> unavailableGuilds)
        {
            bool result = ShardReady == null && OnceShardReady == null;
            ShardReady?.Invoke(id, unavailableGuilds);
            OnceShardReady?.Invoke(id, unavailableGuilds);
            OnceShardReady = null;
            return result;
        }

        /// <summary>
        /// Emitted when a shard is attempting to reconnect or re-identify.
        /// </summary>
        public event ShardReconnectingEvent ShardReconnecting;

        /// <summary>
        /// Emitted when a shard is attempting to reconnect or re-identify.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event ShardReconnectingEvent OnceShardReconnecting;

        internal bool EmitShardReconnecting(int id)
        {
            bool result = ShardReconnecting == null && OnceShardReconnecting == null;
            ShardReconnecting?.Invoke(id);
            OnceShardReconnecting?.Invoke(id);
            OnceShardReconnecting = null;
            return result;
        }

        /// <summary>
        /// Emitted when a shard resumes successfully.
        /// </summary>
        public event ShardResumeEvent ShardResume;

        /// <summary>
        /// Emitted when a shard resumes successfully.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event ShardResumeEvent OnceShardResume;

        internal bool EmitShardResume(int id, int replayedEvents)
        {
            bool result = ShardResume == null && OnceShardResume == null;
            ShardResume?.Invoke(id, replayedEvents);
            OnceShardResume?.Invoke(id, replayedEvents);
            OnceShardResume = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a user starts typing in a channel.
        /// </summary>
        public event TypingStartEvent TypingStart;

        /// <summary>
        /// Emitted whenever a user starts typing in a channel.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event TypingStartEvent OnceTypingStart;

        internal bool EmitTypingStart(Channel channel, User user)
        {
            bool result = TypingStart == null && OnceTypingStart == null;
            TypingStart?.Invoke(channel, user);
            OnceTypingStart?.Invoke(channel, user);
            OnceTypingStart = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a user's details (e.g. username) are changed.
        /// </summary>
        public event UserUpdateEvent UserUpdate;

        /// <summary>
        /// Emitted whenever a user's details (e.g. username) are changed.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event UserUpdateEvent OnceUserUpdate;

        internal bool EmitUserUpdate(User oldUser, User newUser)
        {
            bool result = UserUpdate == null && OnceUserUpdate == null;
            UserUpdate?.Invoke(oldUser, newUser);
            OnceUserUpdate?.Invoke(oldUser, newUser);
            OnceUserUpdate = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a member changes voice state - e.g. joins/leaves a channel, mutes/unmutes.
        /// </summary>
        public event VoiceStateUpdateEvent VoiceStateUpdate;

        /// <summary>
        /// Emitted whenever a member changes voice state - e.g. joins/leaves a channel, mutes/unmutes.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event VoiceStateUpdateEvent OnceVoiceStateUpdate;

        internal bool EmitVoiceStateUpdate(VoiceState oldState, VoiceState newState)
        {
            bool result = VoiceStateUpdate == null && OnceVoiceStateUpdate == null;
            VoiceStateUpdate?.Invoke(oldState, newState);
            OnceVoiceStateUpdate?.Invoke(oldState, newState);
            OnceVoiceStateUpdate = null;
            return result;
        }

        /// <summary>
        /// Emitted for general warnings.
        /// </summary>
        public event WarnEvent Warn;

        /// <summary>
        /// Emitted for general warnings.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event WarnEvent OnceWarn;

        internal bool EmitWarn(string info)
        {
            bool result = Warn == null && OnceWarn == null;
            Warn?.Invoke(info);
            OnceWarn?.Invoke(info);
            OnceWarn = null;
            return result;
        }

        /// <summary>
        /// Emitted whenever a guild text channel has its webhooks changed.
        /// </summary>
        public event WebhookUpdateEvent WebhookUpdate;

        /// <summary>
        /// Emitted whenever a guild text channel has its webhooks changed.
        /// <br/>
        /// <br/>
        /// <info><b>Listeners added to this event will be invoked only once.</b></info>
        /// </summary>
        public event WebhookUpdateEvent OnceWebhookUpdate;

        internal bool EmitWebhookUpdate(TextChannel channel)
        {
            bool result = WebhookUpdate == null && OnceWebhookUpdate == null;
            WebhookUpdate?.Invoke(channel);
            OnceWebhookUpdate?.Invoke(channel);
            OnceWebhookUpdate = null;
            return result;
        }
    }
}