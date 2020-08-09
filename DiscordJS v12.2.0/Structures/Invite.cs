using DiscordJS.Data;
using JavaScript;

namespace DiscordJS
{
    /// <summary>
    /// Represents an invitation to a guild channel.
    /// <br/>
    /// <br/>
    /// <warn><b>The only guaranteed properties are Code, Channel, and URL. Other properties can be missing.</b></warn>
    /// </summary>
    public class Invite : Base
    {
        /// <summary>
        /// The channel the invite is for
        /// </summary>
        public Channel Channel { get; internal set; }

        /// <summary>
        /// The code for this invite
        /// </summary>
        public string Code { get; internal set; }

        /// <summary>
        /// The time the invite was created at
        /// </summary>
        public Date CreatedAt => CreatedTimestamp.HasValue ? new Date(CreatedTimestamp.Value) : null;

        /// <summary>
        /// The timestamp the invite was created at
        /// </summary>
        public long? CreatedTimestamp { get; internal set; }

        /// <summary>
        /// Whether the invite is deletable by the client user
        /// </summary>
        public bool Deletable
        {
            get
            {
                if (Guild == null || !Client.Guilds.Cache.Has(Guild.ID)) return false;
                if (Guild.Me == null) throw new DJSError.Error("GUILD_UNCACHED_ME");
                return ((GuildChannel)Channel).PermissionsFor(Client.User).Has(Permissions.FLAGS.MANAGE_CHANNELS, false) || Guild.Me.Permissions.Has(Permissions.FLAGS.MANAGE_GUILD);
            }
        }

        /// <summary>
        /// The time the invite will expire at
        /// </summary>
        public Date ExpiresAt
        {
            get
            {
                var expireTimestamp = ExpiresTimestamp;
                return expireTimestamp.HasValue ? new Date(expireTimestamp.Value) : null;
            }
        }

        /// <summary>
        /// The timestamp the invite will expire at
        /// </summary>
        public long? ExpiresTimestamp => CreatedTimestamp.HasValue && MaxAge.HasValue ? (long?)(CreatedTimestamp.Value + MaxAge.Value * 1000) : null;

        /// <summary>
        /// The guild the invite is for
        /// </summary>
        public Guild Guild { get; internal set; }

        /// <summary>
        /// The user who created this invite
        /// </summary>
        public User Inviter { get; internal set; }

        /// <summary>
        /// The maximum age of the invite, in seconds, 0 if never expires
        /// </summary>
        public int? MaxAge { get; internal set; }

        /// <summary>
        /// The maximum uses of this invite
        /// </summary>
        public int? MaxUses { get; internal set; }

        /// <summary>
        /// The approximate total number of members of the guild this invite is for
        /// </summary>
        public int? MemberCount { get; internal set; }

        /// <summary>
        /// The approximate number of online members of the guild this invite is for
        /// </summary>
        public int? PresenceCount { get; internal set; }

        /// <summary>
        /// The target user for this invite
        /// </summary>
        public User TargetUser { get; internal set; }

        /// <summary>
        /// The target user type
        /// </summary>
        public TargetUser? TargetUserType { get; internal set; }

        /// <summary>
        /// Whether or not this invite is temporary
        /// </summary>
        public bool Temporary { get; internal set; }

        /// <summary>
        /// The URL to the invite
        /// </summary>
        public string URL => Endpoints.Invite(Client.Options.http.invite, Code);

        /// <summary>
        /// How many times this invite has been used
        /// </summary>
        public int? Uses { get; internal set; }

        public Invite(Client client, InviteData data) : base(client)
        {
            //
        }

        internal virtual void _Patch(InviteData data)
        {
            Guild = data.guild == null ? null : Client.Guilds.Add(data.Guild, false);
            Code = data.code;
            PresenceCount = data.approximate_presence_count.HasValue ? (int?)data.approximate_presence_count.Value : null;
            MemberCount = data.approximate_member_count.HasValue ? (int?)data.approximate_member_count.Value : null;
            Temporary = data.temporary.HasValue ? (bool?)data.temporary.Value : null;
            MaxAge = data.max_age.HasValue ? (bool?)data.max_age.Value : null;
            Uses = data.uses.HasValue ? (bool?)data.uses.Value : null;
            MaxUses = data.max_uses.HasValue ? (bool?)data.max_uses.Value : null;
            Inviter = data.inviter == null ? null : Client.Users.Add(data.inviter);
            TargetUser = data.target_user == null ? null : Client.Users.Add(data.target_user);
            TargetUserType = data.target_user_type.HasValue ? (TargetUser?)(TargetUser)data.target_user_type : null;
            Channel = Client.Channels.Add(data.channel, Guild, false);
            CreatedTimestamp = data.created_at.HasValue ? (long?)new Date(data.created_at).GetTime() : null;
        }

        IPromise<Invite> Delete(string reason = null) => Client.API.Invites(Code).Delete(new { reason }).Then((_) => this);

        public override string ToString() => URL;
        public string ValueOf() => URL;
    }

    /// <summary>
    /// The type of the target user
    /// </summary>
    public enum TargetUser
    {
        STREAM = 1
    }
}