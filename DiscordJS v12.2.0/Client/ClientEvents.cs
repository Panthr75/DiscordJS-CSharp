using System;
using System.Collections.Generic;
using JavaScript;

namespace DiscordJS
{
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
    /// Emitted whenever a user joins a guild.
    /// </summary>
    /// <param name="member">The member that has joined a guild</param>
    public delegate void GuildMemberAddEvent(GuildMember member);

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
    /// Emitted once a guild member changes speaking state.
    /// </summary>
    /// <param name="member">The member that started/stopped speaking</param>
    /// <param name="speaking">The speaking state of the member</param>
    public delegate void GuildMemberSpeakingEvent(GuildMember member, Speaking speaking);

    /// <summary>
    /// Emitted whenever a guild member changes - i.e. new role, removed role, nickname.
    /// </summary>
    /// <param name="oldMember">The member before the update</param>
    /// <param name="newMember">The member after the update</param>
    public delegate void GuildMemberUpdateEvent(GuildMember oldMember, GuildMember newMember);

    /// <summary>
    /// Emitted whenever a guild becomes unavailable, likely due to a server outage.
    /// </summary>
    /// <param name="guild">The guild that has become unavailable</param>
    public delegate void GuildUnavailableEvent(Guild guild);

    /// <summary>
    /// Emitted whenever a guild is updated - e.g. name change.
    /// </summary>
    /// <param name="oldGuild">The guild before the update</param>
    /// <param name="newGuild">The guild after the update</param>
    public delegate void GuildUpdateEvent(Guild oldGuild, Guild newGuild);

    /// <summary>
    /// Emitted when the client's session becomes invalidated. You are expected to handle closing the process gracefully and preventing a boot loop if you are listening to this event.
    /// </summary>
    public delegate void InvalidatedEvent();

    /// <summary>
    /// Emitted when an invite is created.
    /// <br/>
    /// <br/>
    /// <info><b>This event only triggers if the client has <c>MANAGE_GUILD</c> permissions for the guild, or <c>MANAGE_CHANNEL</c> permissions for the channel.</b></info>
    /// </summary>
    /// <param name="invite">The invite that was created</param>
    public delegate void InviteCreateEvent(Invite invite);

    /// <summary>
    /// Emitted when an invite is deleted.
    /// <br/>
    /// <br/>
    /// <info><b>This event only triggers if the client has <c>MANAGE_GUILD</c> permissions for the guild, or <c>MANAGE_CHANNEL</c> permissions for the channel.</b></info>
    /// </summary>
    /// <param name="invite">The invite that was deleted</param>
    public delegate void InviteDeleteEvent(Invite invite);

    /// <summary>
    /// Emitted whenever a message is created.
    /// </summary>
    /// <param name="message">The created message</param>
    public delegate void MessageEvent(Message message);

    /// <summary>
    /// Emitted whenever a message is deleted.
    /// </summary>
    /// <param name="message">The deleted message</param>
    public delegate void MessageDeleteEvent(Message message);

    /// <summary>
    /// Emitted whenever messages are deleted in bulk.
    /// </summary>
    /// <param name="messages">The deleted messages, mapped by their ID</param>
    public delegate void MessageDeleteBulkEvent(Collection<Snowflake, Message> messages);

    /// <summary>
    /// Emitted whenever a reaction is added to a cached message.
    /// </summary>
    /// <param name="messageReaction">The reaction object</param>
    /// <param name="user">The user that applied the guild or reaction emoji</param>
    public delegate void MessageReactionAddEvent(MessageReaction messageReaction, User user);

    /// <summary>
    /// Emitted whenever a reaction is removed from a cached message.
    /// </summary>
    /// <param name="messageReaction">The reaction object</param>
    /// <param name="user">The user whose emoji or reaction emoji was removed</param>
    public delegate void MessageReactionRemoveEvent(MessageReaction messageReaction, User user);

    /// <summary>
    /// Emitted whenever all reactions are removed from a cached message.
    /// </summary>
    /// <param name="message">The message the reactions were removed from</param>
    public delegate void MessageReactionRemoveAllEvent(Message message);

    /// <summary>
    /// Emitted when a bot removes an emoji reaction from a cached message.
    /// </summary>
    /// <param name="reaction">The reaction that was removed</param>
    public delegate void MessageReactionRemoveEmojiEvent(MessageReaction reaction);

    /// <summary>
    /// Emitted whenever a message is updated - e.g. embed or content change.
    /// </summary>
    /// <param name="oldMessage">The message before the update</param>
    /// <param name="newMessage">The message after the update</param>
    public delegate void MessageUpdateEvent(Message oldMessage, Message newMessage);

    /// <summary>
    /// Emitted whenever a guild member's presence (e.g. status, activity) is changed.
    /// </summary>
    /// <param name="oldPresence">The presence before the update, if one at all</param>
    /// <param name="newPresence">The presence after the update</param>
    public delegate void PresenceUpdateEvent(Presence oldPresence, Presence newPresence);

    /// <summary>
    /// Emitted when the client hits a rate limit while making a request
    /// </summary>
    /// <param name="rateLimitInfo">Object containing the rate limit info</param>
    public delegate void RateLimitEvent(RateLimitInfo rateLimitInfo);

    /// <summary>
    /// Emitted when the client becomes ready to start working.
    /// </summary>
    public delegate void ReadyEvent();

    /// <summary>
    /// Emitted whenever a role is created.
    /// </summary>
    /// <param name="role">The role that was created</param>
    public delegate void RoleCreateEvent(Role role);

    /// <summary>
    /// Emitted whenever a guild role is deleted.
    /// </summary>
    /// <param name="role">The role that was deleted</param>
    public delegate void RoleDeleteEvent(Role role);

    /// <summary>
    /// Emitted whenever a guild role is updated.
    /// </summary>
    /// <param name="oldRole">The role before the update</param>
    /// <param name="newRole">The role after the update</param>
    public delegate void RoleUpdateEvent(Role oldRole, Role newRole);

    /// <summary>
    /// Emitted when a shard's WebSocket disconnects and will no longer reconnect.
    /// </summary>
    /// <param name="ev">The WebSocket close event</param>
    /// <param name="id">The shard ID that disconnected</param>
    public delegate void ShardDisconnectEvent(WSCloseEvent ev, int id);

    /// <summary>
    /// Emitted whenever a shard's WebSocket encounters a connection exception.
    /// </summary>
    /// <param name="exception">The encountered exception</param>
    /// <param name="shardID">The shard that encountered this error</param>
    public delegate void ShardExceptionEvent(Exception exception, int shardID);

    /// <summary>
    /// Emitted when a shard turns ready.
    /// </summary>
    /// <param name="id">The shard ID that turned ready</param>
    /// <param name="unavailableGuilds">Set of unavailable guild IDs, if any</param>
    public delegate void ShardReadyEvent(int id, Set<string> unavailableGuilds);

    /// <summary>
    /// Emitted when a shard is attempting to reconnect or re-identify.
    /// </summary>
    /// <param name="id">The shard ID that is attempting to reconnect</param>
    public delegate void ShardReconnectingEvent(int id);

    /// <summary>
    /// Emitted when a shard resumes successfully.
    /// </summary>
    /// <param name="id">The shard ID that resumed</param>
    /// <param name="replayedEvents">The amount of replayed events</param>
    public delegate void ShardResumeEvent(int id, int replayedEvents);

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
}