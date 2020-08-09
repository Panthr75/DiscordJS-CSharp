using DiscordJS.Data;
using JavaScript;

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
        public VoiceChannel AFKChannel { get; }

        /// <summary>
        /// The ID of the voice channel where AFK members are moved
        /// </summary>
        public Snowflake AFKChannelID { get; internal set; }

        /// <summary>
        /// The time in seconds before a user is counted as "away from keyboard"
        /// </summary>
        public int AFKTimeout { get; internal set; }

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
        public Date CreatedAt { get; }

        /// <summary>
        /// The timestamp the guild was created at
        /// </summary>
        public long CreatedTimestamp { get; }

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
        public TextChannel EmbedChannel { get; }

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
        public Date JoinedAt { get; }

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
        public GuildMember Me { get; internal set; }

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
        public string Name { get; }

        /// <summary>
        /// The acronym that shows up in place of a guild icon.
        /// </summary>
        public GuildMember Owner { get; }

        /// <summary>
        /// The acronym that shows up in place of a guild icon.
        /// </summary>
        public Snowflake OwnerID { get; internal set; }

        /// <summary>
        /// If this guild is partnered
        /// </summary>
        public bool Partnered { get; }

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
        public TextChannel PublicUpdatesChannel { get; }

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
        public TextChannel RulesChannel { get; }

        /// <summary>
        /// The ID of the rules channel for the guild
        /// <br/>
        /// <br/>
        /// <info><b>This is only available on guilds with the PUBLIC feature</b></info>
        /// </summary>
        public TextChannel RulesChannelID { get; internal set; }

        /// <summary>
        /// The Shard this Guild belongs to.
        /// </summary>
        public WebSocketShard Shard { get; }

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
        public TextChannel SystemChannel { get; internal set; }

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
        public bool Verified { get; }

        /// <summary>
        /// The voice state for the client user of this guild, if any
        /// </summary>
        public VoiceState Voice { get; }

        /// <summary>
        /// A manager of the voice states of this guild
        /// </summary>
        public VoiceStateManager VoiceStates { get; internal set; }

        /// <summary>
        /// Widget channel for this guild
        /// </summary>
        public TextChannel WidgetChannel { get; }

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
            //
        }

        internal void _Patch(GuildData data)
        {
            //
        }
    }
}