using DiscordJS.Resolvables;
using JavaScript;

namespace DiscordJS
{
    /// <summary>
    /// Represents a message to be sent to the API.
    /// </summary>
    public class APIMessage
    {
        /// <summary>
        /// Data sendable to the API
        /// </summary>
        public object Data { get; internal set; }

        /// <summary>
        /// Files sendable to the API
        /// </summary>
        public object[] Files { get; internal set; }

        /// <summary>
        /// Whether or not the target is a message
        /// </summary>
        public bool IsMessage { get; }

        /// <summary>
        /// Whether or not the target is a user
        /// </summary>
        public bool IsUser { get; }

        /// <summary>
        /// Whether or not the target is a webhook
        /// </summary>
        public bool IsWebhook { get; }

        /// <summary>
        /// Options passed in from send
        /// </summary>
        public APIMessageOptions Options { get; internal set; }

        /// <summary>
        /// The target for this message to be sent to
        /// </summary>
        public MessageTarget Target { get; }

        /// <summary>
        /// Instantiates a new APIMessage
        /// </summary>
        /// <param name="target">The target for this message to be sent to</param>
        /// <param name="options">Options passed in from send</param>
        public APIMessage(MessageTarget target, APIMessageOptions options)
        {
            //
        }

        /// <summary>
        /// Makes the content of this message.
        /// </summary>
        /// <returns>?string or ?string[]</returns>
        public object MakeContent()
        {
            //
        }

        /// <summary>
        /// Resolves data.
        /// </summary>
        /// <returns></returns>
        public APIMessage ResolveData()
        {
            //
        }

        /// <summary>
        /// Resolves files.
        /// </summary>
        /// <returns></returns>
        public IPromise<APIMessage> ResolveFiles()
        {
            //
        }

        /// <summary>
        /// Converts this APIMessage into an array of APIMessages for each split content
        /// </summary>
        /// <returns></returns>
        public APIMessage[] Split()
        {
            //
        }

        /// <summary>
        /// Creates an APIMessage from user-level arguments.
        /// </summary>
        /// <param name="target">Target to send to</param>
        /// <param name="content">Content to send</param>
        /// <param name="options">Options to use</param>
        /// <param name="extra">Extra options to add onto transformed options</param>
        /// <returns></returns>
        public static APIMessage Create(MessageTarget target, StringResolvable content, MessageOptions options = null, MessageOptions extra = null)
        {
            var isWebhook = target.isWebhook;
            var transformed = TransformOptions(content, options, extra, isWebhook);
            return new APIMessage(target, transformed);
        }

        /// <summary>
        /// Transforms the user-level arguments into a final options object. Passing a transformed options object alone into
        /// this method will keep it the same, allowing for the reuse of the final options object.
        /// </summary>
        /// <param name="content">Content to send</param>
        /// <param name="options">Options to use</param>
        /// <param name="extra">Extra options to add onto transformed options</param>
        /// <param name="isWebhook">Whether or not to use WebhookMessageOptions as the result</param>
        /// <returns></returns>
        public static APIMessageOptions TransformOptions(StringResolvable content, MessageOptions options, MessageOptions extra = null, bool isWebhook = false)
        {
            dynamic opt;
            if (options == null && content.type == StringResolvable.Type.Object)
            {
                options = (MessageOptions)content.obj;
                content = null;
            }
            //if (!options )
        }
    }

    public class APIMessageOptions
    {
        public APIMessageOptions() { }

        public APIMessageOptions(MessageOptions options)
        {
            tts = options.tts;
            nonce = options.nonce;
            content = options.content;
            embed = options.embed;
            allowedMentions = options.allowedMentions;
            disableMentions = options.disableMentions;
            files = options.files;
            code = options.code;
            split = options.split;
            reply = options.reply;
        }

        public APIMessageOptions(WebhookMessageOptions options)
        {
            tts = options.tts;
            nonce = options.nonce;
            allowedMentions = options.allowedMentions;
            disableMentions = options.disableMentions;
            files = options.files;
            code = options.code;
            split = options.split;
            username = options.username;
            avatarURL = options.avatarURL;
            embeds = options.embeds;
        }

        /// <inheritdoc cref="MessageOptions.tts"/>
        public bool tts = false;
        /// <inheritdoc cref="MessageOptions.nonce"/>
        public string nonce = "";
        /// <inheritdoc cref="MessageOptions.content"/>
        public string content = "";
        /// <inheritdoc cref="MessageOptions.embed"/>
        public object embed = null;
        /// <inheritdoc cref="MessageOptions.allowedMentions"/>
        public MessageMentionOptions allowedMentions = null;
        /// <inheritdoc cref="MessageOptions.disableMentions"/>
        public DisableMentionType disableMentions;
        /// <inheritdoc cref="MessageOptions.files"/>
        public BufferResolvable[] files;
        /// <inheritdoc cref="MessageOptions.code"/>
        public string code = null;
        /// <inheritdoc cref="MessageOptions.split"/>
        public SplitOptions split = null;
        /// <inheritdoc cref="MessageOptions.reply"/>
        public UserResolvable reply;

        /// <inheritdoc cref="WebhookMessageOptions.username"/>
        public string username;
        /// <inheritdoc cref="WebhookMessageOptions.avatarURL"/>
        public string avatarURL;
        /// <inheritdoc cref="WebhookMessageOptions.embeds"/>
        public object[] embeds;

        public static implicit operator APIMessageOptions(MessageOptions options) => new APIMessageOptions(options);
        public static implicit operator APIMessageOptions(WebhookMessageOptions options) => new APIMessageOptions(options);
    }

    public class MessageTarget
    {
        public static implicit operator MessageTarget(TextChannel channel) => new MessageTarget(channel);
        public static implicit operator MessageTarget(DMChannel channel) => new MessageTarget(channel);
        public static implicit operator MessageTarget(GuildMember member) => new MessageTarget(member);
        public static implicit operator MessageTarget(User user) => new MessageTarget(user);
        public static implicit operator MessageTarget(Webhook webhook) => new MessageTarget(webhook);
        public static implicit operator MessageTarget(WebhookClient client) => new MessageTarget(client);
    }
}