using DiscordJS.Data;
using DiscordJS.Resolvables;
using JavaScript;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Represents a message on Discord.
    /// </summary>
    public class Message : Base, IHasID
    {
        /// <summary>
        /// Group activity
        /// </summary>
        public MessageActivity Activity { get; internal set; }

        /// <summary>
        /// Supplemental application information for group activities
        /// </summary>
        public ClientApplication Application { get; internal set; }

        /// <summary>
        /// A collection of attachments in the message - e.g. Pictures - mapped by their ID
        /// </summary>
        public Collection<Snowflake, MessageAttachment> Attachments { get; internal set; }

        /// <summary>
        /// The author of the message
        /// </summary>
        public User Author { get; internal set; }

        /// <summary>
        /// The channel that the message was sent in
        /// </summary>
        public ITextBasedChannel Channel { get; internal set; }

        /// <summary>
        /// The message contents with all mentions replaced by the equivalent text. If mentions cannot be resolved to a name, the relevant mention in the message content will not be converted.
        /// </summary>
        public string CleanContent => Content == null ? null : DiscordUtil.CleanContent(Content, this);

        /// <summary>
        /// The content of the message
        /// </summary>
        public string Content { get; internal set; }

        /// <summary>
        /// The time the message was sent at
        /// </summary>
        public Date CreatedAt => new Date(CreatedTimestamp);

        /// <summary>
        /// The timestamp the message was sent at
        /// </summary>
        public long CreatedTimestamp { get; internal set; }

        /// <summary>
        /// Whether the message is deletable by the client user
        /// </summary>
        public bool Deletable => !Deleted && (Author.ID == Client.User.ID || (Guild != null && Channel.PermissionsFor(Client.User).Has(Permissions.FLAGS.MANAGE_MESSAGES, false)));

        /// <summary>
        /// Whether this message has been deleted
        /// </summary>
        public bool Deleted { get; internal set; }

        /// <summary>
        /// Whether the message is editable by the client user
        /// </summary>
        public bool Editable => Author.ID == Client.User.ID;

        /// <summary>
        /// The time the message was last edited at (if applicable)
        /// </summary>
        public Date EditedAt => EditedTimestamp.HasValue ? new Date(EditedTimestamp.Value) : null;

        /// <summary>
        /// The timestamp the message was last edited at (if applicable)
        /// </summary>
        public long? EditedTimestamp { get; internal set; }

        /// <summary>
        /// An array of cached versions of the message, including the current version Sorted from latest (first) to oldest (last)
        /// </summary>
        public Array<Message> Edits
        {
            get
            {
                var copy = _edits.Slice();
                copy.Unshift(this);
                return copy;
            }
        }

        /// <summary>
        /// A list of embeds in the message - e.g. YouTube Player
        /// </summary>
        public Array<MessageEmbed> Embeds { get; internal set; }

        /// <summary>
        /// Flags that are applied to the message
        /// </summary>
        public MessageFlags Flags { get; internal set; }

        /// <summary>
        /// The guild the message was sent in (if in a guild channel)
        /// </summary>
        public Guild Guild => Channel.Guild;

        /// <summary>
        /// The ID of the message
        /// </summary>
        public Snowflake ID { get; internal set; }

        /// <summary>
        /// Represents the author of the message as a guild member. Only available if the message comes from a guild where the author is still a member
        /// </summary>
        public GuildMember Member => Guild == null ? null : Guild.Member(Author);

        /// <summary>
        /// All valid mentions that the message contains
        /// </summary>
        public MessageMentions Mentions { get; internal set; }

        /// <summary>
        /// A random number or string used for checking message delivery
        /// <br/>
        /// <br/>
        /// <warn><b>This is only received after the message was sent successfully, and lost if re-fetched</b></warn>
        /// </summary>
        public string Nonce { get; internal set; }

        /// <summary>
        /// Whether or not this message is a partial
        /// </summary>
        public bool Partial => Content == null || Author == null;

        /// <summary>
        /// Whether the message is pinnable by the client user
        /// </summary>
        public bool Pinnable => Type == MessageType.DEFAULT && (Guild == null || Channel.PermissionsFor(Client.User).Has(Permissions.FLAGS.MANAGE_MESSAGES, false));


        /// <summary>
        /// Whether or not this message is pinned
        /// </summary>
        public bool Pinned { get; internal set; }

        /// <summary>
        /// A manager of the reactions belonging to this message
        /// </summary>
        public ReactionManager Reactions { get; internal set; }

        /// <summary>
        /// Message reference data
        /// </summary>
        public MessageReference Reference { get; internal set; }

        /// <summary>
        /// Whether or not this message was sent by Discord, not actually a user (e.g. pin notifications)
        /// </summary>
        public bool System { get; internal set; }

        /// <summary>
        /// Whether or not the message was Text-To-Speech
        /// </summary>
        public bool TTS { get; internal set; }

        /// <summary>
        /// The type of the message
        /// </summary>
        public MessageType Type { get; internal set; }

        /// <summary>
        /// The url to jump to this message
        /// </summary>
        public string URL => $"https://discord.com/channels/{(Guild == null ? "@me" : Guild.ID.ToString())}/{Channel.ID}/{ID}";

        /// <summary>
        /// ID of the webhook that sent the message, if applicable
        /// </summary>
        public Snowflake WebhookID { get; internal set; }

        /// <summary>
        /// The previous versions of the message, sorted with the most recent first
        /// </summary>
        Array<Message> _edits;

        /// <summary>
        /// Instantiates a new Message Object
        /// </summary>
        /// <param name="client">The instantiating client</param>
        /// <param name="data">The data for the message</param>
        /// <param name="channel">The channel the message was sent in</param>
        public Message(Client client, MessageData data, ITextBasedChannel channel) : base(client)
        {
            Channel = channel;
            Deleted = false;
            if (data != null) _Patch(data);
        }

        internal Message(Client client, Message message, ITextBasedChannel channel) : base(client)
        {
            Channel = channel;
            Deleted = false;
            if (message != null) _Patch(message);
        }

        internal virtual void _Patch(MessageData data)
        {
            ID = data.id;
            Type = (MessageType)data.type;
            Content = data.content;
            Author = data.author == null ? null : Client.Users.Add(data.author, data.webhook_id == null);
            Pinned = data.pinned;
            TTS = data.tts;
            Nonce = data.nonce;
            Embeds = new Array<MessageEmbedData>(data.embeds == null ? new MessageEmbedData[0] { } : data.embeds).Map((d) => new MessageEmbed(d, true));
            Attachments = new Collection<Snowflake, MessageAttachment>();
            if (data.attachments != null)
            {
                for (int index = 0, length = data.attachments.Length; index < length; index++)
                {
                    var attachment = data.attachments[index];
                    Attachments.Set(attachment.id, new MessageAttachment(attachment.url, attachment.filename, attachment));
                }
            }
            CreatedTimestamp = new Date(data.timestamp).GetTime();
            EditedTimestamp = data.edited_timestamp.HasValue ? (long?)new Date(data.edited_timestamp.Value).GetTime() : null;
            Reactions = new ReactionManager(this);
            if (data.reactions != null)
            {
                for (int index = 0, length = data.reactions.Length; index < length; index++)
                {
                    var reaction = data.reactions[index];
                    Reactions.Add(reaction);
                }
            }
            Mentions = new MessageMentions(this, data.mentions, data.mention_roles, data.mention_everyone, data.mention_channels);
            WebhookID = data.webhook_id;
            Application = data.application == null ? null : new ClientApplication(Client, data.application);
            Activity = data.activity == null ? null : new MessageActivity(data.activity);
            _edits = new Array<Message>();
            if (Member != null && data.member != null)
            {
                Member._Patch(data.member);
            }
            else if (data.member != null && Guild != null && Author != null)
            {
                Guild.Members.Add(data.member);
            }

            Flags = new MessageFlags(data.flags).Freeze();
            Reference = data.message_reference == null ? null : new MessageReference() { GuildID = data.message_reference.guild_id, ChannelID = data.message_reference.channel_id, MessageID = data.message_reference.message_id };
        }

        internal virtual void _Patch(Message message)
        {
            ID = message.ID;
            Type = message.Type;
            Content = message.Content;
            Author = message.Author;
            Pinned = message.Pinned;
            TTS = message.TTS;
            Nonce = message.Nonce;
            Embeds = message.Embeds.Slice();
            Attachments = message.Attachments;
            CreatedTimestamp = message.CreatedTimestamp;
            EditedTimestamp = message.EditedTimestamp;
            Reactions = message.Reactions;
            Mentions = message.Mentions;
            WebhookID = message.WebhookID;
            Application = message.Application;
            Activity = message.Activity;
            _edits = message._edits;

            Flags = message.Flags;
            Reference = message.Reference;
        }

        internal new Message _Clone() => MemberwiseClone() as Message;

        /// <summary>
        /// Similar to CreateReactionCollector but in promise form. Resolves with a collection of reactions that pass the specified filter.
        /// </summary>
        /// <param name="filter">The filter function to use</param>
        /// <param name="options">Optional options to pass to the internal collector</param>
        /// <returns></returns>
        public IPromise<Collection<Snowflake, MessageReaction>> AwaitReactions(Func<MessageReaction, User, Collection<Snowflake, MessageReaction>, bool> filter, AwaitReactionsOptions options = null)
        {
            return new Promise<ICollection<Snowflake, MessageReaction>>((resolve, reject) =>
            {
                var collector = CreateReactionCollector(filter, options);
                collector.OnceEnd += (reactions, reason) =>
                {
                    if (options.errors != null && new Array<string>(options.errors).Includes(reason)) reject(new ReactionCollectorCollectionException(reactions, reason));
                    else resolve(reactions);
                };
            });
        }

        /// <summary>
        /// Creates a reaction collector.
        /// </summary>
        /// <param name="filter">The filter to apply</param>
        /// <param name="options">Options to send to the collector</param>
        /// <returns></returns>
        public ReactionCollector CreateReactionCollector(Func<MessageReaction, User, Collection<Snowflake, MessageReaction>, bool> filter, ReactionCollectorOptions options = null)
        {
            return new ReactionCollector(this, filter, options);
        }

        /// <summary>
        /// Deletes the message.
        /// </summary>
        /// <param name="timeout">How long to wait to delete the message in milliseconds</param>
        /// <param name="reason">Reason for deleting this message, if it does not belong to the client user</param>
        /// <returns></returns>
        public IPromise<Message> Delete(long timeout = 0, string reason = null)
        {
            if (timeout <= 0)
                return Channel.Messages.Delete(ID, reason).Then((_) => this);
            else
                return new Promise((resolve, reject) => Client.SetTimeout(() => resolve(), timeout)).Then(() => Delete(0, reason));
        }

        /// <summary>
        /// Edits the content of the message.
        /// </summary>
        /// <param name="content">The new content for the message</param>
        /// <param name="options">The options to provide</param>
        /// <returns></returns>
        public IPromise<Message> Edit(StringResolvable content, MessageEditOptions options)
        {
            var data = APIMessage.Create(this, content, options).ResolveData().Data;
            return Client.API.Channels(Channel.ID).Messages(ID).Patch(new { data }).Then((d) =>
            {
                var clone = _Clone();
                clone._Patch(d);
                return clone;
            });
        }

        /// <summary>
        /// Edits the content of the message.
        /// </summary>
        /// <param name="content">The new content for the message</param>
        /// <param name="options">The options to provide</param>
        /// <returns></returns>
        public IPromise<Message> Edit(APIMessage content, MessageEditOptions options)
        {
            var data = content.ResolveData().Data;
            return Client.API.Channels(Channel.ID).Messages(ID).Patch(new { data }).Then((d) =>
            {
                var clone = _Clone();
                clone._Patch(d);
                return clone;
            });
        }

        /// <summary>
        /// Edits the content of the message.
        /// </summary>
        /// <param name="content">The new content for the message</param>
        /// <param name="options">The options to provide</param>
        /// <returns></returns>
        public IPromise<Message> Edit(StringResolvable content, MessageEmbed options)
        {
            var data = APIMessage.Create(this, content, options).ResolveData().Data;
            return Client.API.Channels(Channel.ID).Messages(ID).Patch(new { data }).Then((d) =>
            {
                var clone = _Clone();
                clone._Patch(d);
                return clone;
            });
        }

        /// <summary>
        /// Edits the content of the message.
        /// </summary>
        /// <param name="content">The new content for the message</param>
        /// <param name="options">The options to provide</param>
        /// <returns></returns>
        public IPromise<Message> Edit(APIMessage content, MessageEmbed options)
        {
            var data = content.ResolveData().Data;
            return Client.API.Channels(Channel.ID).Messages(ID).Patch(new { data }).Then((d) =>
            {
                var clone = _Clone();
                clone._Patch(d);
                return clone;
            });
        }

        /// <summary>
        /// Used mainly internally. Whether two messages are identical in properties. If you want to compare messages without checking all the properties, use <c>message.ID == message2.ID</c>, which is much more efficient. This method allows you to see if there are differences in content, embeds, attachments, nonce and tts properties.
        /// </summary>
        /// <param name="message">The message to compare it to</param>
        /// <param name="rawData">Raw data passed through the WebSocket about this message</param>
        /// <returns></returns>
        public bool Equals(Message message, MessageData rawData = null)
        {
            if (message == null) return false;
            var embedUpdate = message.Author == null && message.Attachments == null;
            if (embedUpdate) return ID == message.ID && Embeds.Length == message.Embeds.Length;

            bool equal = ID == message.ID && Author.ID == message.Author.ID && Content == message.Content && TTS == message.TTS && Attachments.Size == message.Attachments.Size;

            if (equal && rawData != null)
            {
                equal = Mentions.Everyone == message.Mentions.Everyone && CreatedTimestamp == new Date(rawData.timestamp).GetTime() && EditedTimestamp == (rawData.edited_timestamp.HasValue ? (long?)new Date(rawData.edited_timestamp.Value).GetTime() : null);
            }
            return equal;
        }

        /// <summary>
        /// Fetch this message.
        /// </summary>
        /// <returns></returns>
        public IPromise<Message> Fetch() => Channel.Messages.Fetch(ID, true);

        /// <summary>
        /// Fetches the webhook used to create this message.
        /// </summary>
        /// <returns></returns>
        public IPromise<Webhook> FetchWebhook()
        {
            if (!WebhookID) return Promise<Webhook>.Rejected(new DJSError.Error("WEBHOOK_MESSAGE"));
            return Client.FetchWebhook(WebhookID);
        }

        /// <summary>
        /// Pins this message to the channel's pinned messages.
        /// </summary>
        /// <returns></returns>
        public IPromise<Message> Pin() => Client.API.Channels(Channel.ID).Pins(ID).Put().Then((_) => this);

        /// <summary>
        /// Adds a reaction to the message.
        /// </summary>
        /// <param name="emoji">The emoji to react with</param>
        /// <returns></returns>
        public IPromise<MessageReaction> React(EmojiIdentifierResolvable emoji)
        {
            var e = Client.Emojis.ResolveIdentifier(emoji);
            if (e == null) throw new DJSError.Error("EMOJI_TYPE");

            return Client.API.Channels(Channel.ID).Messages(ID).Reactions(e, "@me").Put().Then((_) =>
                Client.Actions.MessageReactionAdd.Handle(Client.User, Channel, this, DiscordUtil.ParseEmoji(e)).reaction
            );
        }

        /// <summary>
        /// Replies to the message.
        /// </summary>
        /// <param name="content">The content for the message</param>
        /// <param name="options">The options to provide</param>
        /// <returns></returns>
        public IPromise<Array<Message>> Reply(StringResolvable content, MessageOptions options = null) => Channel.Send(APIMessage.TransformOptions(content, options, new APIMessageTransformOptions()
        {
            reply = Member == null ? Author : Member.User
        }));

        /// <summary>
        /// Replies to the message.
        /// </summary>
        /// <param name="content">The content for the message</param>
        /// <param name="options">The options to provide</param>
        /// <returns></returns>
        public IPromise<Array<Message>> Reply(APIMessage content, MessageOptions options = null) => Channel.Send(content);

        /// <summary>
        /// Replies to the message.
        /// </summary>
        /// <param name="content">The content for the message</param>
        /// <param name="options">The options to provide</param>
        /// <returns></returns>
        public IPromise<Array<Message>> Reply(StringResolvable content, MessageAdditions options = null) => Channel.Send(APIMessage.TransformOptions(content, options, new APIMessageTransformOptions()
        {
            reply = Member == null ? Author : Member.User
        }));

        /// <summary>
        /// Replies to the message.
        /// </summary>
        /// <param name="content">The content for the message</param>
        /// <param name="options">The options to provide</param>
        /// <returns></returns>
        public IPromise<Array<Message>> Reply(APIMessage content, MessageAdditions options = null) => Channel.Send(content);

        /// <summary>
        /// Suppresses or unsuppresses embeds on a message
        /// </summary>
        /// <param name="suppress">If the embeds should be suppressed or not</param>
        /// <returns></returns>
        public IPromise<Message> SuppressEmbed(bool suppress = true)
        {
            var flags = new MessageFlags(Flags.Bit);

            if (suppress)
                flags.Add(MessageFlags.FLAGS.SUPPRESS_EMBEDS);
            else
                flags.Remove(MessageFlags.FLAGS.SUPPRESS_EMBEDS);

            return Edit(new APIMessage() { flags = flags });
        }

        /// <summary>
        /// When concatenated with a string, this automatically concatenates the message's content instead of the object.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Content;

        /// <summary>
        /// Unpins this message from the channel's pinned messages.
        /// </summary>
        /// <returns></returns>
        public IPromise<Message> Unpin() => Client.API.Channels(Channel.ID).Pins(ID).Delete().Then((_) => this);
    }

    /// <summary>
    /// Reference data sent in a crossposted message.
    /// </summary>
    public sealed class MessageReference
    {
        /// <summary>
        /// ID of the channel the message was crossposted from
        /// </summary>
        public Snowflake ChannelID { get; internal set; }

        /// <summary>
        /// ID of the guild the message was crossposted from
        /// </summary>
        public Snowflake GuildID { get; internal set; }

        /// <summary>
        /// ID of the message that was crossposted
        /// </summary>
        public Snowflake MessageID { get; internal set; }
    }

    /// <summary>
    /// An object containing the same properties as CollectorOptions, but a few more:
    /// </summary>
    public sealed class AwaitReactionsOptions : ReactionCollectorOptions
    {
        /// <summary>
        /// Stop/end reasons that cause the promise to reject
        /// </summary>
        public string[] errors;
    }

    /// <summary>
    /// Options that can be passed into editMessage.
    /// </summary>
    public sealed class MessageEditOptions
    {
        /// <summary>
        /// Content to be edited
        /// </summary>
        public string content;

        /// <summary>
        /// An embed to be added/edited
        /// </summary>
        public object embed;

        /// <summary>
        /// Language for optional codeblock formatting to apply
        /// </summary>
        public string code;

        /// <summary>
        /// Which mentions should be parsed from the message content
        /// </summary>
        public MessageMentionOptions allowedMentions;
    }
}