using DiscordJS.Data;
using DiscordJS.Resolvables;
using JavaScript;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Represents a guild text channel on Discord.
    /// </summary>
    public class TextChannel : GuildChannel, ITextBasedChannel
    {
        /// <inheritdoc cref="TextBasedChannel.LastMessageDoc"/>
        public Message LastMessage => TextBasedChannel.LastMessage(Messages, LastMessageID);

        /// <inheritdoc cref="TextBasedChannel.LastMessageIDDoc"/>
        public Snowflake LastMessageID { get; internal set; }

        /// <inheritdoc cref="TextBasedChannel.MessagesDoc"/>
        public MessageManager Messages { get; }

        /// <inheritdoc cref="TextBasedChannel.LastPinAtDoc"/>
        public Date LastPinAt => TextBasedChannel.LastPinAt(LastPinTimestamp);

        /// <inheritdoc cref="TextBasedChannel.LastPinTimestampDoc"/>
        public long? LastPinTimestamp { get; internal set; }

        /// <summary>
        /// If the guild considers this channel NSFW
        /// </summary>
        public bool NSFW { get; internal set; }

        /// <summary>
        /// The ratelimit per user for this channel in seconds
        /// </summary>
        public int RateLimitPerUser { get; internal set; }

        /// <summary>
        /// The topic of the text channel
        /// </summary>
        public string Topic { get; internal set; }

        /// <summary>
        /// Whether or not the typing indicator is being shown in the channel
        /// </summary>
        public bool Typing => TextBasedChannel.Typing(Client, ID);

        /// <summary>
        /// Number of times StartTyping has been called
        /// </summary>
        public int TypingCount => TextBasedChannel.TypingCount(Client, ID);

        /// <summary>
        /// Instantiates a new TextChannel object
        /// </summary>
        /// <param name="guild">The guild the text channel is part of</param>
        /// <param name="data">The data for the text channel</param>
        public TextChannel(Guild guild, ChannelData data) : base(guild, data)
        {
            //
        }

        internal override void _Patch(ChannelData data)
        {
            base._Patch(data);

            Topic = data.topic;
            NSFW = data.nsfw.HasValue ? data.nsfw.Value : false;
            LastMessageID = data.last_message_id;
            RateLimitPerUser = data.rate_limit_per_user.HasValue ? data.rate_limit_per_user.Value : 0;
            LastPinTimestamp = data.last_pin_timestamp.HasValue ? (long?)new Date(data.last_pin_timestamp.Value).GetTime() : null;
            if (data.messages != null)
            {
                for (int index = 0, length = data.messages.Length; index < length; index++)
                {
                    var msg = data.messages[index];
                    Messages.Add(msg);
                }
            }
        }

        public IPromise<Collection<Snowflake, Message>> AwaitMessages(Func<Message, Collection<Snowflake, Message>, bool> filter, AwaitMessageOptions options = null) => TextBasedChannel.AwaitMessages(filter, options);

        public IPromise<Collection<Snowflake, Message>> BulkDelete(Collection<Snowflake, Message> messages, bool filterOld = false) => TextBasedChannel.BulkDelete(Client, ID, this, messages, filterOld);

        public IPromise<Collection<Snowflake, Message>> BulkDelete(Array<Message> messages, bool filterOld = false) => TextBasedChannel.BulkDelete(Client, ID, this, messages, filterOld);

        public IPromise<Collection<Snowflake, Message>> BulkDelete(Array<Snowflake> messages, bool filterOld = false) => TextBasedChannel.BulkDelete(Client, ID, this, messages, filterOld);

        public IPromise<Collection<Snowflake, Message>> BulkDelete(int messages, bool filterOld = false) => TextBasedChannel.BulkDelete(Client, ID, this, Messages, messages, filterOld);

        public MessageCollector CreateMessageCollector(Func<Message, Collection<Snowflake, Message>, bool> filter, MessageCollectorOptions options = null) => TextBasedChannel.CreateMessageCollector(this, filter, options);

        /// <summary>
        /// Creates a webhook for the channel.
        /// </summary>
        /// <param name="name">The name of the webhook</param>
        /// <param name="avatar">Avatar for the webhook</param>
        /// <param name="reason">Reason for creating the webhook</param>
        /// <returns>The created webhook</returns>
        public IPromise<Webhook> CreateWebhook(string name, BufferResolvable avatar, string reason = null)
        {
            var avt = avatar.Resolve(null);
            Client.API.Channels(ID).Webhooks.Post(new
            {
                data = new
                {
                    name,
                    avatar = avt
                },
                reason
            }).Then((data) => new Webhook(Client, data));
        }

        /// <summary>
        /// Fetches all webhooks for the channel.
        /// </summary>
        /// <returns></returns>
        public IPromise<Collection<Snowflake, Webhook>> FetchWebhooks()
        {
            return Client.API.Channels(ID).Webhooks.Get().Then((data) =>
            {
                var hooks = new Collection<Snowflake, Webhook>();
                for (int index = 0, length = data.Length; index < length; index++)
                {
                    var hook = data[index];
                    hooks.Set(hook.id, new Webhook(Client, hook));
                }
                return hooks;
            });
        }

        /// <summary>
        /// Sets whether this channel is flagged as NSFW.
        /// </summary>
        /// <param name="nsfw">Whether the channel should be considered NSFW</param>
        /// <param name="reason">Reason for changing the channel's NSFW flag</param>
        /// <returns></returns>
        public IPromise<TextChannel> SetNSFW(bool nsfw, string reason = null) => Edit(new GuildChannelEditData() { nsfw = nsfw }, reason).Then((_) => this);

        /// <summary>
        /// Sets the rate limit per user for this channel.
        /// </summary>
        /// <param name="rateLimitPerUser">The new ratelimit in seconds</param>
        /// <param name="reason">Reason for changing the channel's ratelimits</param>
        /// <returns></returns>
        public IPromise<TextChannel> SetRateLimitPerUser(int rateLimitPerUser, string reason = null) => Edit(new GuildChannelEditData() { rateLimitPerUser = rateLimitPerUser }, reason).Then((_) => this);

        public IPromise StartTyping(int? count = 1) => TextBasedChannel.StartTyping(Client, ID, count);

        public void StopTyping(bool force = false) => TextBasedChannel.StopTyping(Client, ID, force);
    }
}