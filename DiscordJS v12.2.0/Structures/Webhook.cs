using DiscordJS.Data;
using DiscordJS.Resolvables;
using JavaScript;

namespace DiscordJS
{
    /// <summary>
    /// Represents a webhook.
    /// </summary>
    public class Webhook : IHasID
    {
        /// <summary>
        /// The avatar for the webhook
        /// </summary>
        public string Avatar { get; internal set; }

        /// <summary>
        /// The channel the webhook belongs to
        /// </summary>
        public Snowflake ChannelID { get; internal set; }

        /// <summary>
        /// The client that instantiated the webhook
        /// </summary>
        public Client Client { get; }

        /// <summary>
        /// The time the webhook was created at
        /// </summary>
        public Date CreatedAt => Snowflake.Deconstruct(ID).Date;

        /// <summary>
        /// The timestamp the webhook was created at
        /// </summary>
        public long CreatedTimestamp => Snowflake.Deconstruct(ID).Timestamp;

        /// <summary>
        /// The guild the webhook belongs to
        /// </summary>
        public Snowflake GuildID { get; internal set; }

        /// <summary>
        /// The ID of the webhook
        /// </summary>
        public Snowflake ID { get; internal set; }

        /// <summary>
        /// The name of the webhook
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The owner of the webhook
        /// </summary>
        public User Owner { get; internal set; }

        /// <summary>
        /// The token for the webhook
        /// </summary>
        public string Token { get; internal set; }

        /// <summary>
        /// The type of the webhook
        /// </summary>
        public WebhookTypes Type { get; internal set; }

        /// <summary>
        /// The url of this webhook
        /// </summary>
        public string URL => Client.Options.http.api + Client.API.Webhooks(ID, Token);

        public Webhook(Client client, WebhookData data)
        {
            Client = client;
        }

        internal void _Patch(WebhookData data)
        {
            Name = data.name;
            Token = data.token;
            Avatar = data.avatar;
            Type = (WebhookTypes)data.type;
            ChannelID = data.channel_id;
            Owner = data.user == null ? null : (Client.Users != null ? Client.Users.Cache.Get(data.user.id) : new User(Client, data.user));
        }

        /// <summary>
        /// A link to the webhook's avatar.
        /// </summary>
        /// <param name="options">Options for the Image URL</param>
        /// <returns></returns>
        public string AvatarURL(ImageURLOptions options)
        {
            if (Avatar == null) return null;
            return Client.rest.CDN.Avatar(ID, Avatar, options.format, options.size);
        }

        /// <summary>
        /// Deletes the webhook.
        /// </summary>
        /// <param name="reason">Reason for deleting this webhook</param>
        /// <returns></returns>
        public IPromise Delete(string reason = null) => Client.API.Webhooks(ID, Token).Delete(new { reason });

        /// <summary>
        /// Edits the webhook.
        /// </summary>
        /// <param name="options">Options</param>
        /// <param name="reason">Reason for editing this webhook</param>
        /// <returns></returns>
        public IPromise<Webhook> Edit(WebhookEditOptions options = null, string reason = null)
        {
            return options.avatar.Resolve().Then((avatar) =>
            {
                string channel = options.channel.snowflake == null ? options.channel.channel.ID : options.channel.snowflake;
                return Client.API.Webhooks(ID, channel == null ? null : Token).Patch(new
                {
                    data = new { name = options.name == null ? Name : options.name, channel_id = channel },
                    reason
                }).Then((data) =>
                {
                    Name = data.name;
                    Avatar = data.avatar;
                    ChannelID = data.channel_id;
                    return this;
                });
            });
        }

        /// <summary>
        /// Sends a message with this webhook.
        /// </summary>
        /// <param name="content">The content to send</param>
        /// <param name="options">The options to provide</param>
        /// <returns></returns>
        public IPromise<Message> Send(StringResolvable content, WebhookMessageOptions options)
        {
            var apiMessage = APIMessage.Create(this, content, options).ResolveData();
            return apiMessage.ResolveFiles().Then((resolvedStuff) =>
            {
                var data = resolvedStuff.Data;
                var files = resolvedStuff.Files;
                return Client.Webhooks.Post(new
                {
                    data,
                    files,
                    query = new { wait = true },
                    auth = false
                }).Then((MessageData d) =>
                {
                    var channel = Client.Channels == null ? null : Client.Channels.Cache.Get(d.channel_id);
                    if (channel == null) return new Message(Client, d, null);
                    return ((ITextBasedChannel)channel).Messages.Add(d, false);
                });
            });
        }

        /// <summary>
        /// Sends a message with this webhook.
        /// </summary>
        /// <param name="content">The content to send</param>
        /// <param name="options">The options to provide</param>
        /// <returns></returns>
        public IPromise<Message> Send(APIMessage content, WebhookMessageOptions options)
        {
            var apiMessage = content.ResolveData();
            return apiMessage.ResolveFiles().Then((resolvedStuff) =>
            {
                var data = resolvedStuff.Data;
                var files = resolvedStuff.Files;
                return Client.Webhooks.Post(new
                {
                    data,
                    files,
                    query = new { wait = true },
                    auth = false
                }).Then((MessageData d) =>
                {
                    var channel = Client.Channels == null ? null : Client.Channels.Cache.Get(d.channel_id);
                    if (channel == null) return new Message(Client, d, null);
                    return ((ITextBasedChannel)channel).Messages.Add(d, false);
                });
            });
        }

        /// <summary>
        /// Sends a message with this webhook.
        /// </summary>
        /// <param name="content">The content to send</param>
        /// <param name="options">The options to provide</param>
        /// <returns></returns>
        public IPromise<Message> Send(StringResolvable content, MessageAdditions options)
        {
            var apiMessage = APIMessage.Create(this, content, options).ResolveData();
            return apiMessage.ResolveFiles().Then((resolvedStuff) =>
            {
                var data = resolvedStuff.Data;
                var files = resolvedStuff.Files;
                return Client.Webhooks.Post(new
                {
                    data,
                    files,
                    query = new { wait = true },
                    auth = false
                }).Then((MessageData d) =>
                {
                    var channel = Client.Channels == null ? null : Client.Channels.Cache.Get(d.channel_id);
                    if (channel == null) return new Message(Client, d, null);
                    return ((ITextBasedChannel)channel).Messages.Add(d, false);
                });
            });
        }

        /// <summary>
        /// Sends a message with this webhook.
        /// </summary>
        /// <param name="content">The content to send</param>
        /// <param name="options">The options to provide</param>
        /// <returns></returns>
        public IPromise<Message> Send(APIMessage content, MessageAdditions options)
        {
            var apiMessage = content.ResolveData();
            return apiMessage.ResolveFiles().Then((resolvedStuff) =>
            {
                var data = resolvedStuff.Data;
                var files = resolvedStuff.Files;
                return Client.Webhooks.Post(new
                {
                    data,
                    files,
                    query = new { wait = true },
                    auth = false
                }).Then((MessageData d) =>
                {
                    var channel = Client.Channels == null ? null : Client.Channels.Cache.Get(d.channel_id);
                    if (channel == null) return new Message(Client, d, null);
                    return ((ITextBasedChannel)channel).Messages.Add(d, false);
                });
            });
        }

        /// <summary>
        /// Sends a raw slack message with this webhook.
        /// </summary>
        /// <param name="body">The raw body to send</param>
        /// <returns></returns>
        public IPromise<bool> SendSlackMessage(dynamic body)
        {
            return Client.API.Webhooks(ID, Token).Slack.Post(new
            {
                query = new { wait = true },
                auth = false,
                data = body
            }).Then((data) => data.ToString() == "ok");
        }
    }

    public sealed class WebhookEditOptions
    {
        /// <summary>
        /// New name for this webhook
        /// </summary>
        public string name;

        /// <summary>
        /// New avatar for this webhook
        /// </summary>
        public BufferResolvable avatar;

        /// <summary>
        /// New channel for this webhook
        /// </summary>
        public ChannelResolvable channel;
    }

    /// <summary>
    /// Options that can be passed into send.
    /// </summary>
    public sealed class WebhookMessageOptions
    {
        /// <summary>
        /// Username override for the message
        /// </summary>
        public string username = null;

        /// <summary>
        /// Avatar URL override for the message
        /// </summary>
        public string avatarURL = null;

        /// <summary>
        /// Whether or not the message should be spoken aloud
        /// </summary>
        public bool tts = false;

        /// <summary>
        /// The nonce for the message
        /// </summary>
        public string nonce = "";

        /// <summary>
        /// An array of embeds for the message
        /// </summary>
        public object[] embeds;

        /// <summary>
        /// Which mentions should be parsed from the message content (see <see href="https://discordapp.com/developers/docs/resources/channel#embed-object">here</see> for more details)
        /// </summary>
        public MessageMentionOptions allowedMentions = null;

        /// <summary>
        /// Whether or not all mentions or everyone/here mentions should be sanitized to prevent unexpected mentions
        /// </summary>
        public DisableMentionType disableMentions;

        /// <summary>
        /// Files to send with the message
        /// </summary>
        public BufferResolvable[] files = null;

        /// <summary>
        /// Language for optional codeblock formatting to apply
        /// </summary>
        public string code = null;

        /// <summary>
        /// Whether or not the message should be split into multiple messages if it exceeds the character limit. If an object is provided, these are the options for splitting the message.
        /// </summary>
        public SplitOptions split = null;
    }
}