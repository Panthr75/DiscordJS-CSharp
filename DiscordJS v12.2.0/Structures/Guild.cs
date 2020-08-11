using DiscordJS.Data;
using DiscordJS.Resolvables;
using JavaScript;
using System;
using System.Text.RegularExpressions;

namespace DiscordJS
{
    /// <summary>
    /// Represents a guild (or a server) on Discord.
    /// <br/>
    /// <br/>
    /// <info><b>It's recommended to see if a guild is available before performing operations or reading data from it. You can check this with <see cref="Available"/>.</b></info>
    /// </summary>
    public class Guild : Base, IHasID
    {
        /// <summary>
        /// AFK voice channel for this guild
        /// </summary>
        public VoiceChannel AFKChannel
        {
            get
            {
                var c = Client.Channels.Cache.Get(AFKChannelID);
                if (c == null || c.Type != "voice") return null;
                return (VoiceChannel)c;
            }
        }

        /// <summary>
        /// The ID of the voice channel where AFK members are moved
        /// </summary>
        public Snowflake AFKChannelID { get; internal set; }

        /// <summary>
        /// The time in seconds before a user is counted as "away from keyboard"
        /// </summary>
        public int? AFKTimeout { get; internal set; }

        /// <summary>
        /// The ID of the application that created this guild (if applicable)
        /// </summary>
        public Snowflake ApplicationID { get; internal set; }

        /// <summary>
        /// Whether the guild is available to access. If it is not available, it indicates a server outage
        /// </summary>
        public bool Available { get; internal set; }

        /// <summary>
        /// The hash of the guild banner
        /// </summary>
        public string Banner { get; internal set; }

        /// <summary>
        /// A manager of the channels belonging to this guild
        /// </summary>
        public GuildChannelManager Channels { get; internal set; }

        /// <summary>
        /// The time the guild was created at
        /// </summary>
        public Date CreatedAt => Snowflake.Deconstruct(ID).Date;

        /// <summary>
        /// The timestamp the guild was created at
        /// </summary>
        public long CreatedTimestamp => Snowflake.Deconstruct(ID).Timestamp;

        /// <summary>
        /// The value set for the guild's default message notifications
        /// </summary>
        public int DefaultMessageNotifications { get; internal set; }

        /// <summary>
        /// Whether the bot has been removed from the guild
        /// </summary>
        public bool Deleted { get; internal set; }

        /// <summary>
        /// The description of the guild, if any
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// Embed channel for this guild
        /// </summary>
        public TextChannel EmbedChannel
        {
            get
            {
                var c = Client.Channels.Cache.Get(EmbedChannelID);
                if (c == null || c.Type != "text") return null;
                return (TextChannel)c;
            }
        }

        /// <summary>
        /// The embed channel ID, if enabled
        /// </summary>
        public Snowflake EmbedChannelID { get; internal set; }

        /// <summary>
        /// Whether embedded images are enabled on this guild
        /// </summary>
        public bool EmbedEnabled { get; internal set; }

        /// <summary>
        /// A manager of the emojis belonging to this guild
        /// </summary>
        public GuildEmojiManager Emojis { get; internal set; }

        /// <summary>
        /// The explicit content filter level of the guild
        /// </summary>
        public ExplicitContentFilterLevel ExplicitContentFilter { get; internal set; }

        /// <summary>
        /// An array of guild features partnered guilds have enabled
        /// </summary>
        public Array<Features> Features { get; internal set; }

        /// <summary>
        /// The hash of the guild icon
        /// </summary>
        public string Icon { get; internal set; }

        /// <summary>
        /// The Unique ID of the guild, useful for comparisons
        /// </summary>
        public Snowflake ID { get; internal set; }

        /// <summary>
        /// The time the client user joined the guild
        /// </summary>
        public Date JoinedAt => new Date(JoinedTimestamp);

        /// <summary>
        /// The timestamp the client user joined the guild at
        /// </summary>
        public long JoinedTimestamp { get; internal set; }

        /// <summary>
        /// Whether the guild is "large" (has more than 250 members)
        /// </summary>
        public bool Large { get; internal set; }

        /// <summary>
        /// The maximum amount of members the guild can have
        /// <br/>
        /// <br/>
        /// <info><b>You will need to fetch the guild using Guild#Fetch if you want to receive this parameter</b></info>
        /// </summary>
        public int MaximumMembers { get; internal set; }

        /// <summary>
        /// The maximum amount of presences the guild can have
        /// <br/>
        /// <br/>
        /// <info><b>You will need to fetch the guild using Guild#Fetch if you want to receive this parameter</b></info>
        /// </summary>
        public int MaximumPresences { get; internal set; }

        /// <summary>
        /// The client user as a GuildMember of this guild
        /// </summary>
        public GuildMember Me
        {
            get
            {
                var member = Members.Cache.Get(Client.User.ID);
                if (member == null) return Client.Options.Partials.Includes(PartialType.GUILD_MEMBER) ? Members.Add(new GuildMemberData()
                {
                    user = new UserData()
                    {
                        id = Client.User.ID
                    }
                }, true) : null;
                else return member;
            }
        }

        /// <summary>
        /// The full amount of members in this guild
        /// </summary>
        public int MemberCount { get; internal set; }

        /// <summary>
        /// A manager of the members belonging to this guild
        /// </summary>
        public GuildMemberManager Members { get; internal set; }

        /// <summary>
        /// The required MFA level for the guild
        /// </summary>
        public int MFALevel { get; internal set; }

        /// <summary>
        /// The name of the guild
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The acronym that shows up in place of a guild icon.
        /// </summary>
        public string NameAcronym => new JavaScript.String(Name).Replace(new Regex(@"\w+", RegexOptions.Compiled), (name) => name[0].ToString());

        /// <summary>
        /// The acronym that shows up in place of a guild icon.
        /// </summary>
        public GuildMember Owner
        {
            get
            {
                var member = Members.Cache.Get(OwnerID);
                if (member == null) return Client.Options.Partials.Includes(PartialType.GUILD_MEMBER) ? Members.Add(new GuildMemberData()
                {
                    user = new UserData()
                    {
                        id = OwnerID
                    }
                }, true) : null;
                else return member;
            }
        }

        /// <summary>
        /// The acronym that shows up in place of a guild icon.
        /// </summary>
        public Snowflake OwnerID { get; internal set; }

        /// <summary>
        /// If this guild is partnered
        /// </summary>
        public bool Partnered => Features.Includes(DiscordJS.Features.PARTNERED);

        /// <summary>
        /// The total number of users currently boosting this server
        /// </summary>
        public int PremiumSubscriptionCount { get; internal set; }

        /// <summary>
        /// The premium tier on this guild
        /// </summary>
        public PremiumTier PremiumTier { get; internal set; }

        /// <summary>
        /// A manager of the presences belonging to this guild
        /// </summary>
        public PresenceManager Presences { get; internal set; }

        /// <summary>
        /// Public updates channel for this guild
        /// <br/>
        /// <br/>
        /// <info><b>This is only available on guilds with the PUBLIC feature</b></info>
        /// </summary>
        public TextChannel PublicUpdatesChannel
        {
            get
            {
                var c = Client.Channels.Cache.Get(PublicUpdatesChannelID);
                if (c == null || c.Type != "text") return null;
                return (TextChannel)c;
            }
        }

        /// <summary>
        /// Public updates channel for this guild
        /// <br/>
        /// <br/>
        /// <info><b>This is only available on guilds with the PUBLIC feature</b></info>
        /// </summary>
        public Snowflake PublicUpdatesChannelID { get; internal set; }

        /// <summary>
        /// The region the guild is located in
        /// </summary>
        public string Region { get; internal set; }

        /// <summary>
        /// A manager of the roles belonging to this guild
        /// </summary>
        public RoleManager Roles { get; internal set; }

        /// <summary>
        /// Rules channel for this guild
        /// <br/>
        /// <br/>
        /// <info><b>This is only available on guilds with the PUBLIC feature</b></info>
        /// </summary>
        public TextChannel RulesChannel
        {
            get
            {
                var c = Client.Channels.Cache.Get(RulesChannelID);
                if (c == null || c.Type != "text") return null;
                return (TextChannel)c;
            }
        }

        /// <summary>
        /// The ID of the rules channel for the guild
        /// <br/>
        /// <br/>
        /// <info><b>This is only available on guilds with the PUBLIC feature</b></info>
        /// </summary>
        public Snowflake RulesChannelID { get; internal set; }

        /// <summary>
        /// The Shard this Guild belongs to.
        /// </summary>
        public WebSocketShard Shard => Client.WS.Shards.Get(ShardID);

        /// <summary>
        /// The id of the shard this Guild belongs to.
        /// </summary>
        public int ShardID { get; internal set; }

        /// <summary>
        /// The hash of the guild splash image (VIP only)
        /// </summary>
        public string Splash { get; internal set; }

        /// <summary>
        /// System channel for this guild
        /// </summary>
        public TextChannel SystemChannel
        {
            get
            {
                var c = Client.Channels.Cache.Get(SystemChannelID);
                if (c == null || c.Type != "text") return null;
                return (TextChannel)c;
            }
        }

        /// <summary>
        /// The value set for the guild's system channel flags
        /// </summary>
        public SystemChannelFlags SystemChannelFlags { get; internal set; } // readonly

        /// <summary>
        /// The ID of the system channel
        /// </summary>
        public Snowflake SystemChannelID { get; internal set; }

        /// <summary>
        /// The vanity URL code of the guild, if any
        /// </summary>
        public string VanityURLCode { get; internal set; }

        /// <summary>
        /// The verification level of the guild
        /// </summary>
        public VerificationLevel VerificationLevel { get; internal set; }

        /// <summary>
        /// If this guild is verified
        /// </summary>
        public bool Verified => Features.Includes(DiscordJS.Features.VERIFIED);

        /// <summary>
        /// The voice state for the client user of this guild, if any
        /// </summary>
        public VoiceState Voice => VoiceStates.Cache.Get(Client.User.ID);

        /// <summary>
        /// A manager of the voice states of this guild
        /// </summary>
        public VoiceStateManager VoiceStates { get; internal set; }

        /// <summary>
        /// Widget channel for this guild
        /// </summary>
        public TextChannel WidgetChannel
        {
            get
            {
                var c = Client.Channels.Cache.Get(WidgetChannelID);
                if (c == null || c.Type != "text") return null;
                return (TextChannel)c;
            }
        }

        /// <summary>
        /// The widget channel ID, if enabled
        /// </summary>
        public Snowflake WidgetChannelID { get; internal set; }

        /// <summary>
        /// Whether widget images are enabled on this guild
        /// </summary>
        public bool WidgetEnabled { get; internal set; }

        /// <summary>
        /// Creates a new Instance of a guild
        /// </summary>
        /// <param name="client">The instantiating client</param>
        /// <param name="data">The data for the guild</param>
        public Guild(Client client, GuildData data) : base(client)
        {
            Members = new GuildMemberManager(this, null);
            Channels = new GuildChannelManager(this, null);
            Roles = new RoleManager(this, null);
            Presences = new PresenceManager(Client, null);
            VoiceStates = new VoiceStateManager(this, null);
            Deleted = false;
            if (data == null) return;
            if (data.unavailable)
            {
                Available = false;
                ID = data.id;
            }
            else
            {
                _Patch(data);
                if (data.channels == null) Available = false;
            }

            ShardID = data.shardID;
        }

        /// <summary>
        /// Sets up the guild.
        /// </summary>
        /// <param name="data">The raw data of the guild</param>
        internal void _Patch(GuildData data)
        {
            Name = data.name;
            Icon = data.icon;
            Splash = data.splash;
            Region = data.region;
            MemberCount = data.member_count.HasValue ? data.member_count.Value : MemberCount;
            Large = data.large.HasValue ? data.large.Value : Large;
            Features = data.features == null ? (Features == null ? new Array<Features>() : Features) : new Array<string>(data.features).Map<Features?>((feature) =>
            {
                if (Enum.TryParse(feature, out Features f))
                    return f;
                else
                    return null;
            }).Filter((feature) => feature.HasValue).Map((feature) => feature.Value);
            ApplicationID = data.application_id;
            AFKTimeout = data.afk_timeout;
            AFKChannelID = data.afk_channel_id;
            SystemChannelID = data.system_channel_id;
            EmbedEnabled = data.embed_enabled.HasValue ? data.embed_enabled.Value : false;
            PremiumTier = data.premium_tier.HasValue ? (PremiumTier)data.premium_tier.Value : PremiumTier.NONE;
            if (data.premium_subscription_count.HasValue)
            {
                PremiumSubscriptionCount = data.premium_subscription_count.Value;
            }
            if (data.widget_enabled.HasValue) WidgetEnabled = data.widget_enabled.Value;
            if (data.widget_channel_id != null) WidgetChannelID = data.widget_channel_id;
            if (data.embed_channel_id != null) EmbedChannelID = data.embed_channel_id;
            VerificationLevel = (VerificationLevel)data.verification_level;
            ExplicitContentFilter = (ExplicitContentFilterLevel)data.explicit_content_filter;
            MFALevel = data.mfa_level;
            JoinedTimestamp = data.joined_at.HasValue ? new Date(data.joined_at.Value).GetTime() : JoinedTimestamp;
            DefaultMessageNotifications = data.default_message_notifications;
            SystemChannelFlags = new SystemChannelFlags(data.system_channel_flags.HasValue ? data.system_channel_flags.Value : 0L).Freeze();
            if (data.max_members.HasValue) MaximumMembers = data.max_members.Value;
            else MaximumMembers = 250000;
            if (data.max_presences.HasValue) MaximumPresences = data.max_presences.Value;
            else MaximumPresences = 25000;
            VanityURLCode = data.vanity_url_code;
            Description = data.description;
            Banner = data.banner;
            ID = data.id;
            Available = !data.unavailable;
            RulesChannelID = data.rules_channel_id;
            PublicUpdatesChannelID = data.public_updates_channel_id;

            if (data.channels != null)
            {
                Channels.Cache.Clear();
                for (int index = 0, length = data.channels.Length; index < length; index++)
                    Client.Channels.Add(data.channels[index], this);
            }

            if (data.roles != null)
            {
                Roles.Cache.Clear();
                for (int index = 0, length = data.roles.Length; index < length; index++)
                    Roles.Add(data.roles[index]);
            }

            if (data.members != null)
            {
                Members.Cache.Clear();
                for (int index = 0, length = data.members.Length; index < length; index++)
                    Members.Add(data.members[index]);
            }

            if (data.owner_id != null) OwnerID = data.owner_id;

            if (data.presences != null)
            {
                for (int index = 0, length = data.presences.Length; index < length; index++)
                {
                    var presence = data.presences[index];
                    presence.guild = this;
                    Presences.Add(presence);
                }
            }

            if (data.voice_states != null)
            {
                VoiceStates.Cache.Clear();
                for (int index = 0, length = data.voice_states.Length; index < length; index++)
                    VoiceStates.Add(data.voice_states[index]);
            }

            if (Emojis == null)
            {
                Emojis = new GuildEmojiManager(this, null);
                if (data.emojis != null)
                {
                    for (int index = 0, length = data.emojis.Length; index < length; index++)
                        Emojis.Add(data.emojis[index]);
                }
            }
            else if (data.emojis != null)
            {
                Client.Actions.GuildEmojisUpdate.Handle(ID, data.emojis);
            }
        }

        /// <summary>
        /// Adds a user to the guild using OAuth2. Requires the <see cref="Permissions.FLAGS.CREATE_INSTANT_INVITE"/> permission.
        /// </summary>
        /// <param name="user">User to add to the guild</param>
        /// <param name="accessToken">An OAuth2 access token for the user with the guilds.join scope granted to the bot's application</param>
        /// <param name="nick">Nickname to give the member (requires MANAGE_NICKNAMES)</param>
        /// <param name="roles">Roles to add to the member (requires MANAGE_ROLES)</param>
        /// <param name="mute">Whether the member should be muted (requires MUTE_MEMBERS)</param>
        /// <param name="deaf">Whether the member should be deafened (requires DEAFEN_MEMBERS)</param>
        /// <returns></returns>
        public IPromise<GuildMember> AddMember(UserResolvable user, string accessToken, string nick = null, RoleResolvable[] roles = null, bool? mute = null, bool? deaf = null)
        {
            //
        }

        /// <summary>
        /// The URL to this guild's banner.
        /// </summary>
        /// <param name="options">Options for the Image URL</param>
        /// <returns></returns>
        public string BannerURL(ImageURLOptions options = null)
        {
            if (Banner == null) return null;
            if (options == null) options = new ImageURLOptions();
            return Client.rest.CDN.Banner(ID, Banner, options.format, options.size);
        }

        /// <summary>
        /// Creates an integration by attaching an integration object
        /// </summary>
        /// <param name="data">The data for the integration</param>
        /// <param name="reason">Reason for creating the integration</param>
        /// <returns></returns>
        public IPromise<Guild> CreateIntegration(IntegrationData data, string reason = null)
        {
            //
        }

        /// <summary>
        /// Deletes the guild.
        /// </summary>
        /// <returns></returns>
        public IPromise<Guild> Delete()
        {
            //
        }

        /// <summary>
        /// Updates the guild with new information - e.g. a new name.
        /// </summary>
        /// <param name="data">The data to update the guild with</param>
        /// <param name="reason">Reason for editing this guild</param>
        /// <returns></returns>
        public IPromise<Guild> Edit(GuildEditData data, string reason)
        {
            //
        }

        /// <summary>
        /// Whether this guild equals another guild. It compares all properties, so for most operations it is advisable to just compare <c>guild.ID == guild2.ID</c> as it is much faster and is often what most users need.
        /// </summary>
        /// <param name="guild">The guild to compare with</param>
        /// <returns></returns>
        public bool Equals(Guild guild)
        {
            //
        }

        /// <summary>
        /// Fetches this guild.
        /// </summary>
        /// <returns></returns>
        public IPromise<Guild> Fetch()
        {
            return Client.API.Guilds(ID).Get().Then((GuildData data) =>
            {
                _Patch(data);
                return this;
            });
        }

        /// <summary>
        /// Fetches audit logs for this guild.
        /// </summary>
        /// <param name="before">Limit to entries from before specified entry</param>
        /// <param name="limit">Limit number of entries</param>
        /// <param name="user">Only show entries involving this user</param>
        /// <param name="type">Only show entries involving this action type</param>
        /// <returns></returns>
        public IPromise<GuildAuditLogs> FetchAuditLogs(Snowflake before = null, int? limit = null, UserResolvable user = null, int? type = null)
        {
            //
        }

        /// <summary>
        /// Fetches audit logs for this guild.
        /// </summary>
        /// <param name="before">Limit to entries from before specified entry</param>
        /// <param name="limit">Limit number of entries</param>
        /// <param name="user">Only show entries involving this user</param>
        /// <param name="type">Only show entries involving this action type</param>
        /// <returns></returns>
        public IPromise<GuildAuditLogs> FetchAuditLogs(GuildAuditLogEntry before, int? limit = null, UserResolvable user = null, int? type = null)
        {
            //
        }

        /// <summary>
        /// Fetches audit logs for this guild.
        /// </summary>
        /// <param name="before">Limit to entries from before specified entry</param>
        /// <param name="limit">Limit number of entries</param>
        /// <param name="user">Only show entries involving this user</param>
        /// <param name="type">Only show entries involving this action type</param>
        /// <returns></returns>
        public IPromise<GuildAuditLogs> FetchAuditLogs(Snowflake before = null, int? limit = null, UserResolvable user = null, AuditLogAction? type = null)
        {
            //
        }

        /// <summary>
        /// Fetches audit logs for this guild.
        /// </summary>
        /// <param name="before">Limit to entries from before specified entry</param>
        /// <param name="limit">Limit number of entries</param>
        /// <param name="user">Only show entries involving this user</param>
        /// <param name="type">Only show entries involving this action type</param>
        /// <returns></returns>
        public IPromise<GuildAuditLogs> FetchAuditLogs(GuildAuditLogEntry before, int? limit = null, UserResolvable user = null, AuditLogAction? type = null)
        {
            //
        }

        /// <summary>
        /// Fetches information on a banned user from this guild.
        /// </summary>
        /// <param name="user">The User to fetch the ban info of</param>
        /// <returns></returns>
        public IPromise<BanInfo> FetchBan(UserResolvable user)
        {
            Snowflake id = Client.Users.ResolveID(user);
            if (id == null) throw new DJSError.Error("FETCH_BAN_RESOLVE_ID");
            return Client.API.Guilds(ID).Bans(id).Get((ban) => new BanInfo(Client.Users.Add(ban.user), ban.reason));
        }

        /// <summary>
        /// Fetches a collection of banned users in this guild.
        /// </summary>
        /// <returns></returns>
        public IPromise<Collection<Snowflake, BanInfo>> FetchBans()
        {
            return Client.API.Guilds(ID).Bans.Get().Then((bans) =>
            {
                return new Array<BanData>(bans).Reduce((collection, ban) =>
                {
                    collection.Set(ban.user.id, new BanInfo(Client.Users.Add(ban.user), ban.reason));
                    return collection;
                }, new Collection<Snowflake, BanInfo>());
            });
        }

        /// <summary>
        /// Fetches the guild embed.
        /// </summary>
        /// <returns></returns>
        public IPromise<GuildEmbedData> FetchEmbed()
        {
            //
        }

        /// <summary>
        /// Fetches a collection of integrations to this guild. Resolves with a collection mapping integrations by their ids.
        /// </summary>
        /// <returns></returns>
        public IPromise<Collection<Snowflake, Integration>> FetchIntegrations()
        {
            return Client.API.Guilds(ID).Integrations.Get().Then((data) =>
            {
                return new Array<IntegrationData>(data).Reduce((collection, integration) =>
                {
                    collection.Set(integration.id, new Integration(Client, integration, this));
                    return collection;
                }, new Collection<Snowflake, Integration>());
            });
        }

        /// <summary>
        /// Fetches a collection of invites to this guild. Resolves with a collection mapping invites by their codes.
        /// </summary>
        /// <returns></returns>
        public IPromise<Collection<Snowflake, Invite>> FetchInvites()
        {
            //
        }

        /// <summary>
        /// Obtains a guild preview for this guild from Discord, only available for public guilds.
        /// </summary>
        /// <returns></returns>
        public IPromise<GuildPreview> FetchPreview()
        {
            //
        }

        /// <summary>
        /// Fetches the vanity url invite code to this guild. Resolves with a string matching the vanity url invite code, not the full url.
        /// </summary>
        /// <returns></returns>
        public IPromise<string> FetchVanityCode()
        {
            //
        }

        /// <summary>
        /// Fetches available voice regions.
        /// </summary>
        /// <returns></returns>
        public IPromise<Collection<string, VoiceRegion>> FetchVoiceRegions()
        {
            //
        }

        /// <summary>
        /// Fetches all webhooks for the guild.
        /// </summary>
        /// <returns></returns>
        public IPromise<Collection<Snowflake, Webhook>> FetchWebhooks()
        {
            //
        }

        /// <summary>
        /// The URL to this guild's icon.
        /// </summary>
        /// <param name="options">Options for the Image URL</param>
        /// <returns></returns>
        public string IconURL(ImageURLOptions options = null)
        {
            if (Icon == null) return null;
            if (options == null) options = new ImageURLOptions();
            return Client.rest.CDN.Icon(ID, Icon, options.format, options.size, options.dynamic);
        }

        /// <summary>
        /// Leaves the guild.
        /// </summary>
        /// <returns></returns>
        public IPromise<Guild> Leave()
        {
            //
        }

        /// <summary>
        /// Returns the GuildMember form of a User object, if the user is present in the guild.
        /// </summary>
        /// <param name="user">The user that you want to obtain the GuildMember of</param>
        /// <returns></returns>
        public GuildMember Member(UserResolvable user)
        {
            return Members.Resolve(user);
        }

        /// <summary>
        /// Edits the AFK channel of the guild.
        /// </summary>
        /// <param name="afkChannel">The new AFK channel</param>
        /// <param name="reason">Reason for changing the guild's AFK channel</param>
        /// <returns></returns>
        public IPromise<Guild> SetAFKChannel(ChannelResolvable afkChannel, string reason = null)
        {
            //
        }

        /// <summary>
        /// Edits the AFK timeout of the guild.
        /// </summary>
        /// <param name="afkTimeout">The time in seconds that a user must be idle to be considered AFK</param>
        /// <param name="reason">Reason for changing the guild's AFK timeout</param>
        /// <returns></returns>
        public IPromise<Guild> SetAFKTimeout(int afkTimeout, string reason = null)
        {
            //
        }

        /// <summary>
        /// Sets a new guild banner.
        /// </summary>
        /// <param name="banner">The new banner of the guild</param>
        /// <param name="reason">Reason for changing the guild's banner</param>
        /// <returns></returns>
        public IPromise<Guild> SetBanner(BufferResolvable banner, string reason = null)
        {
            //
        }

        /// <summary>
        /// Batch-updates the guild's channels' positions.
        /// </summary>
        /// <param name="channelPositions">Channel positions to update</param>
        /// <returns></returns>
        public IPromise<Guild> SetChannelPositions(params ChannelPosition[] channelPositions)
        {
            //
        }

        /// <summary>
        /// Edits the setting of the default message notifications of the guild.
        /// </summary>
        /// <param name="defaultMessageNotifications">The new setting for the default message notifications</param>
        /// <param name="reason">Reason for changing the setting of the default message notifications</param>
        /// <returns></returns>
        public IPromise<Guild> SetDefaultMessageNotifications(int defaultMessageNotifications, string reason = null)
        {
            //
        }

        /// <summary>
        /// Edits the guild's embed.
        /// </summary>
        /// <param name="embed">The embed for the guild</param>
        /// <param name="reason">Reason for changing the guild's embed</param>
        /// <returns></returns>
        public IPromise<Guild> SetEmbed(GuildEmbedData embed, string reason = null)
        {
            //
        }

        /// <summary>
        /// Edits the level of the explicit content filter.
        /// </summary>
        /// <param name="explicitContentFilter">The new level of the explicit content filter</param>
        /// <param name="reason">Reason for changing the level of the guild's explicit content filter</param>
        /// <returns></returns>
        public IPromise<Guild> SetExplicitContentFilter(ExplicitContentFilterLevel explicitContentFilter, string reason = null)
        {
            //
        }

        /// <summary>
        /// Edits the level of the explicit content filter.
        /// </summary>
        /// <param name="explicitContentFilter">The new level of the explicit content filter</param>
        /// <param name="reason">Reason for changing the level of the guild's explicit content filter</param>
        /// <returns></returns>
        public IPromise<Guild> SetExplicitContentFilter(int explicitContentFilter, string reason = null)
        {
            //
        }

        /// <summary>
        /// Sets a new guild icon.
        /// </summary>
        /// <param name="icon">The new icon of the guild</param>
        /// <param name="reason">Reason for changing the guild's icon</param>
        /// <returns></returns>
        public IPromise<Guild> SetIcon(BufferResolvable icon, string reason = null)
        {
            //
        }

        /// <summary>
        /// Edits the name of the guild.
        /// </summary>
        /// <param name="name">The new name of the guild</param>
        /// <param name="reason">Reason for changing the guild's name</param>
        /// <returns></returns>
        public IPromise<Guild> SetName(string name, string reason = null)
        {
            //
        }

        /// <summary>
        /// Sets a new owner of the guild.
        /// </summary>
        /// <param name="owner">The new owner of the guild</param>
        /// <param name="reason">Reason for setting the new owner</param>
        /// <returns></returns>
        public IPromise<Guild> SetOwner(GuildMemberResolvable owner, string reason = null)
        {
            //
        }

        /// <summary>
        /// Edits the region of the guild.
        /// </summary>
        /// <param name="region">The new region of the guild</param>
        /// <param name="reason">Reason for changing the guild's region</param>
        /// <returns></returns>
        public IPromise<Guild> SetRegion(string region, string reason = null)
        {
            //
        }

        /// <summary>
        /// Batch-updates the guild's role positions
        /// </summary>
        /// <param name="rolePositions">Role positions to update</param>
        /// <returns></returns>
        public IPromise<Guild> SetRegion(params GuildRolePosition[] rolePositions)
        {
            //
        }

        /// <summary>
        /// Sets a new guild splash screen.
        /// </summary>
        /// <param name="splash">The new splash screen of the guild</param>
        /// <param name="reason">Reason for changing the guild's splash screen</param>
        /// <returns></returns>
        public IPromise<Guild> SetSplash(BufferResolvable[] splash, string reason = null)
        {
            //
        }

        /// <summary>
        /// Edits the system channel of the guild.
        /// </summary>
        /// <param name="systemChannel">The new system channel</param>
        /// <param name="reason">Reason for changing the guild's system channel</param>
        /// <returns></returns>
        public IPromise<Guild> SetSystemChannel(ChannelResolvable systemChannel, string reason = null)
        {
            //
        }

        /// <summary>
        /// Edits the flags of the default message notifications of the guild.
        /// </summary>
        /// <param name="systemChannelFlags">The new flags for the default message notifications</param>
        /// <param name="reason">Reason for changing the flags of the default message notifications</param>
        /// <returns></returns>
        public IPromise<Guild> SetSystemChannelFlags(SystemChannelFlagsResolvable systemChannelFlags, string reason = null)
        {
            //
        }

        /// <summary>
        /// Edits the verification level of the guild.
        /// </summary>
        /// <param name="verificationLevel">The new verification level of the guild</param>
        /// <param name="reason">Reason for changing the guild's verification level</param>
        /// <returns></returns>
        public IPromise<Guild> SetVerificationLevel(VerificationLevel verificationLevel, string reason = null)
        {
            //
        }

        /// <summary>
        /// Edits the verification level of the guild.
        /// </summary>
        /// <param name="verificationLevel">The new verification level of the guild</param>
        /// <param name="reason">Reason for changing the guild's verification level</param>
        /// <returns></returns>
        public IPromise<Guild> SetVerificationLevel(int verificationLevel, string reason = null)
        {
            //
        }

        /// <summary>
        /// The URL to this guild's splash.
        /// </summary>
        /// <param name="options">Options for the Image URL</param>
        /// <returns></returns>
        public string SplashURL(ImageURLOptions options = null)
        {
            if (Splash == null) return null;
            if (options == null) options = new ImageURLOptions();
            return Client.rest.CDN.Splash(ID, Splash, options.format, options.size);
        }

        /// <summary>
        /// When concatenated with a string, this automatically returns the guild's name instead of the Guild object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}