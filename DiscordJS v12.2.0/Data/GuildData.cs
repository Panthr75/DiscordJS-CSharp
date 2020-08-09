namespace DiscordJS.Data
{
    public class GuildData
    {
        /// <summary>
        /// guild id
        /// </summary>
        public string id;

        /// <summary>
        /// guild name (2-100 characters, excluding trailing and leading whitespace)
        /// </summary>
        public string name;

        /// <summary>
        /// icon hash
        /// </summary>
        public string icon;

        /// <summary>
        /// splash hash
        /// </summary>
        public string splash;

        /// <summary>
        /// discovery splash hash; only present for guilds with the "DISCOVERABLE" feature
        /// </summary>
        public string discovery_splash;

        /// <summary>
        /// true if the user is the owner of the guild
        /// </summary>
        public bool? owner;

        /// <summary>
        /// id of owner
        /// </summary>
        public string owner_id;

        /// <summary>
        /// legacy total permissions for the user in the guild (excludes overrides)
        /// </summary>
        public int? permissions;

        /// <summary>
        /// voice region id for the guild
        /// </summary>
        public string region;

        /// <summary>
        /// id of afk channel
        /// </summary>
        public string afk_channel_id;

        /// <summary>
        /// afk timeout in seconds
        /// </summary>
        public int? afk_timeout;

        /// <summary>
        /// true if the server widget is enabled (deprecated, replaced with widget_enabled)
        /// </summary>
        public bool? embed_enabled;

        /// <summary>
        /// the channel id that the widget will generate an invite to, or null if set to no invite (deprecated, replaced with widget_channel_id)
        /// </summary>
        public string embed_channel_id;

        /// <summary>
        /// verification level required for the guild
        /// </summary>
        public int verification_level;

        /// <summary>
        /// default message notifications level
        /// </summary>
        public int default_message_notifications;

        /// <summary>
        /// explicit content filter level
        /// </summary>
        public int explicit_content_filter;

        /// <summary>
        /// roles in the guild
        /// </summary>
        public RoleData[] roles;

        /// <summary>
        /// custom guild emojis
        /// </summary>
        public EmojiData[] emojis;

        /// <summary>
        /// enabled guild features
        /// </summary>
        public string[] features;

        /// <summary>
        /// required MFA level for the guild
        /// </summary>
        public int mfa_level;

        /// <summary>
        /// application id of the guild creator if it is bot-created
        /// </summary>
        public bool? widget_enabled;

        /// <summary>
        /// the channel id that the widget will generate an invite to, or null if set to no invite
        /// </summary>
        public string widget_channel_id;

        /// <summary>
        /// the id of the channel where guild notices such as welcome messages and boost events are posted
        /// </summary>
        public string system_channel_id;

        /// <summary>
        /// system channel flags
        /// </summary>
        public long? system_channel_flags;

        /// <summary>
        /// the id of the channel where guilds with the "PUBLIC" feature can display rules and/or guidelines
        /// </summary>
        public string rules_channel_id;

        /// <summary>
        /// when this guild was joined at
        /// </summary>
        public long? joined_at;

        /// <summary>
        /// true if this is considered a large guild
        /// </summary>
        public bool? large;

        /// <summary>
        /// total number of members in this guild
        /// </summary>
        public int? member_count;

        /// <summary>
        /// states of members currently in voice channels; lacks the guild_id key
        /// </summary>
        public VoiceStateData[] voice_states;

        /// <summary>
        /// users in the guild
        /// </summary>
        public GuildMemberData[] members;

        /// <summary>
        /// channels in the guild
        /// </summary>
        public ChannelData[] channels;

        /// <summary>
        /// presences of the members in the guild, will only include non-offline members if the size is greater than large threshold
        /// </summary>
        public PresenceData[] presences;

        /// <summary>
        /// the maximum number of presences for the guild (the default value, currently 25000, is in effect when null is returned)
        /// </summary>
        public int? max_presences;

        /// <summary>
        /// the maximum number of members for the guild
        /// </summary>
        public int? max_members;

        /// <summary>
        /// the vanity url code for the guild
        /// </summary>
        public string vanity_url_code;

        /// <summary>
        /// the description for the guild, if the guild is discoverable
        /// </summary>
        public string description;

        /// <summary>
        /// banner hash
        /// </summary>
        public string banner;

        /// <summary>
        /// premium tier (Server Boost level)
        /// </summary>
        public int? premium_tier;

        /// <summary>
        /// the number of boosts this guild currently has
        /// </summary>
        public int? premium_subscription_count;

        /// <summary>
        /// the preferred locale of a guild with the "PUBLIC" feature; used in server discovery and notices from Discord; defaults to "en-US"
        /// </summary>
        public string preferred_locale;

        /// <summary>
        /// the id of the channel where admins and moderators of guilds with the "PUBLIC" feature receive notices from Discord
        /// </summary>
        public string public_updates_channel_id;

        /// <summary>
        /// the maximum amount of users in a video channel
        /// </summary>
        public int? max_video_channel_users;

        /// <summary>
        /// approximate number of members in this guild, returned from the GET /guild/&lt;id&gt; endpoint when with_counts is true
        /// </summary>
        public int? approximate_member_count;

        /// <summary>
        /// approximate number of non-offline members in this guild, returned from the GET /guild/&lt;id&gt; endpoint when with_counts is true
        /// </summary>
        public int? approximate_presence_count;
    }
}