using DiscordJS.Data;
using DiscordJS.Resolvables;
using JavaScript;
using System;
using System.Collections.Generic;

namespace DiscordJS
{
    /// <summary>
    /// Manages API methods for GuildChannels and stores their cache.
    /// </summary>
    public class GuildChannelManager : BaseManager<Collection<Snowflake, GuildChannel>, GuildChannel, ChannelData>
    {
        /// <summary>
        /// The guild this Manager belongs to
        /// </summary>
        public Guild Guild { get; }

        public GuildChannelManager(Guild guild, IEnumerable<GuildChannel> iterable) : base(guild.Client, iterable)
        {
            Guild = guild;
        }

        protected override GuildChannel AddItem(GuildChannel item) => Add(item);

        public GuildChannel Add(GuildChannel data)
        {
            Snowflake snowflake = data.ID;
            GuildChannel existing = Cache.Get(snowflake);
            if (existing != null) return existing;

            var entry = new GuildChannel(Guild, data);
            Cache.Set(snowflake, entry);
            return entry;
        }

        public GuildChannel Add(ChannelData data)
        {
            Snowflake snowflake = data.id;
            GuildChannel existing = Cache.Get(snowflake);
            if (existing != null) return existing;

            var entry = new GuildChannel(Guild, data);
            Cache.Set(snowflake, entry);
            return entry;
        }

        /// <summary>
        /// Resolves a GuildChannelResolvable to a Channel object.
        /// </summary>
        /// <param name="channel">The GuildChannel resolvable to resolve</param>
        /// <returns></returns>
        public GuildChannel Resolve(GuildChannelResolvable channel) => channel.Resolve(this);

        /// <summary>
        /// Resolves a GuildChannelResolvable to a channel ID Snowflake.
        /// </summary>
        /// <param name="channel">The GuildChannel resolvable to resolve</param>
        /// <returns></returns>
        public Snowflake ResolveID(GuildChannelResolvable channel)
        {
            var c = Resolve(channel);
            if (c == null) return null;
            return c.ID;
        }

        /// <summary>
        /// Creates a new channel in the guild.
        /// </summary>
        /// <param name="name">The name of the new channel</param>
        /// <param name="options">Options</param>
        /// <returns></returns>
        public IPromise<GuildChannel> Create(string name, CreateGuildChannelOptions options = null)
        {
            if (options == null) options = new CreateGuildChannelOptions();
            string parent = options.parent != null ? Client.Channels.ResolveID(options.parent) : null;
            PermissionOverwriteData[] permissionOverwrites = options.permissionOverwrites != null ? new Array<OverwriteResolvable>(options.permissionOverwrites).Map((o) => PermissionOverwrites.Resolve(o, Guild)).ToArray() : null;
            return Client.API.Guilds(Guild.ID).Channels.Post(new
            {
                data = new ChannelData()
                {
                    name = name,
                    topic = options.topic,
                    type = options.type != null && Enum.TryParse(options.type.ToUpper(), out ChannelTypes channelType) ? (int)channelType : (int)ChannelTypes.TEXT,
                    nsfw = options.nsfw,
                    bitrate = options.bitrate,
                    user_limit = options.userLimit,
                    parent_id = parent,
                    position = options.position,
                    permission_overwrites = permissionOverwrites,
                    rate_limit_per_user = options.rateLimitPerUser
                },
                options.reason
            }).Then((ChannelData data) => Client.Actions.ChannelCreate.Handle(data).Channel);
        }
    }

    public class CreateGuildChannelOptions
    {
        /// <summary>
        /// The type of the new channel, either "text", "voice", or "category"
        /// </summary>
        public string type;

        /// <summary>
        /// The topic for the new channel
        /// </summary>
        public string topic = null;

        /// <summary>
        /// Whether the new channel is nsfw
        /// </summary>
        public bool? nsfw = null;

        /// <summary>
        /// Bitrate of the new channel in bits (only voice)
        /// </summary>
        public int? bitrate = null;

        /// <summary>
        /// Maximum amount of users allowed in the new channel (only voice)
        /// </summary>
        public int? userLimit = null;

        /// <summary>
        /// Parent of the new channel
        /// </summary>
        public ChannelResolvable parent = null;

        /// <summary>
        /// Permission overwrites of the new channel
        /// </summary>
        public OverwriteResolvable[] permissionOverwrites = null;

        /// <summary>
        /// Position of the new channel
        /// </summary>
        public int? position = null;

        /// <summary>
        /// The ratelimit per user for the channel
        /// </summary>
        public int? rateLimitPerUser = null;

        /// <summary>
        /// Reason for creating the channel
        /// </summary>
        public string reason = null;
    }
}