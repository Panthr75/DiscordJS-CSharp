using JavaScript;
using DiscordJS.Data;

namespace DiscordJS
{
    /// <summary>
    /// Options for a client.
    /// </summary>
    public class ClientOptions
    {
        /// <summary>
        /// ID of the shard to run, or an array of shard IDs. If not specified, the client will spawn ClientOptions#shardCount shards. If set to auto, it will fetch the recommended amount of shards from Discord and spawn that amount
        /// </summary>
        public Array<int> shards; // number | Array<number> | string

        /// <summary>
        /// The total amount of shards used by all processes of this bot (e.g. recommended shard count, shard count of the ShardingManager)
        /// </summary>
        public double shardCount = 1;

        /// <summary>
        /// Maximum number of messages to cache per channel (-1 or Infinity for unlimited - don't do this without message sweeping, otherwise memory usage will climb indefinitely)
        /// </summary>
        public double messageCacheMaxSize = 200;

        /// <summary>
        /// How long a message should stay in the cache until it is considered sweepable (in seconds, 0 for forever)
        /// </summary>
        public double messageCacheLifetime = 0;

        /// <summary>
        /// How frequently to remove messages from the cache that are older than the message cache lifetime (in seconds, 0 for never)
        /// </summary>
        public double messageSweepInterval = 0;

        /// <summary>
        /// Whether to cache all guild members and users upon startup, as well as upon joining a guild (should be avoided whenever possible)
        /// </summary>
        public bool fetchAllMembers = false;

        /// <summary>
        /// Default value for MessageOptions#disableMentions
        /// </summary>
        public DisableMentionType disableMentions = DisableMentionType.None;

        /// <summary>
        /// Default value for MessageOptions#allowedMentions
        /// </summary>
        public MessageMentionOptions allowedMentions;

        /// <summary>
        /// Structures allowed to be partial. This means events can be emitted even when they're missing all the data for a particular structure. See the "Partials" topic listed in the sidebar for some important usage information, as partials require you to put checks in place when handling data.
        /// </summary>
        public Array<PartialType> partials;

        /// <summary>
        /// Maximum time permitted between REST responses and their corresponding websocket events
        /// </summary>
        public int restWsBridgeTimeout = 5000;

        /// <summary>
        /// Extra time in millseconds to wait before continuing to make REST requests (higher values will reduce rate-limiting errors on bad connections)
        /// </summary>
        public int restTimeOffset = 500;

        /// <summary>
        /// Time to wait before cancelling a REST request, in milliseconds
        /// </summary>
        public int restRequestTimeout = 15000;

        /// <summary>
        /// How frequently to delete inactive request buckets, in seconds (or 0 for never)
        /// </summary>
        public int restSweepInterval = 60;

        /// <summary>
        /// How many times to retry on 5XX errors (Infinity for indefinite amount of retries)
        /// </summary>
        public int retryLimit = 1;

        /// <summary>
        /// Presence data to use upon login
        /// </summary>
        public PresenceData presence;

        /// <summary>
        /// Options for the WebSocket
        /// </summary>
        public WebsocketOptions ws;

        /// <summary>
        /// HTTP options
        /// </summary>
        public HTTPOptions http;
    }
}