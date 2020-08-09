using JavaScript;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using DiscordJS.Data;
using DiscordJS.Resolvables;

namespace DiscordJS
{
    /// <summary>
    /// Represents a member of a guild on Discord.
    /// </summary>
    public class GuildMember : Base, IHasID
    {
        /// <summary>
        /// Whether this member is bannable by the client user
        /// </summary>
        public bool Bannable => Manageable && Guild.Me.Permissions.Has(Permissions.FLAGS.BAN_MEMBERS);

        /// <summary>
        /// Whether the member has been removed from the guild
        /// </summary>
        public bool Deleted { get; internal set; }

        /// <summary>
        /// The displayed color of this member in base 10
        /// </summary>
        public int DisplayColor
        {
            get
            {
                var role = Roles.Color;
                if (role != null && role.Color.HasValue) return role.Color.Value;
                else return 0;
            }
        }

        /// <summary>
        /// The displayed color of this member in hexadecimal
        /// </summary>
        public string DisplayHexColor
        {
            get
            {
                var role = Roles.Color;
                if (role != null && role.HexColor != null) return role.HexColor;
                else return "#000000";
            }
        }

        /// <summary>
        /// The nickname of this member, or their username if they don't have one
        /// </summary>
        public string DisplayName
        {
            get
            {
                var nick = Nickname;
                return nick == null ? User.Username : nick;
            }
        }

        /// <summary>
        /// The guild that this member is part of
        /// </summary>
        public Guild Guild { get; internal set; }

        /// <summary>
        /// The ID of this member
        /// </summary>
        public Snowflake ID => User != null ? User.ID : null;

        /// <summary>
        /// The time this member joined the guild
        /// </summary>
        public Date JoinedAt => JoinedTimestamp.HasValue ? new Date(JoinedTimestamp.Value) : null;

        /// <summary>
        /// The timestamp the member joined the guild at
        /// </summary>
        public long? JoinedTimestamp { get; internal set; }

        /// <summary>
        /// Whether this member is kickable by the client user
        /// </summary>
        public bool Kickable => Manageable && Guild.Me.Permissions.Has(Permissions.FLAGS.KICK_MEMBERS);

        /// <summary>
        /// The Message object of the last message sent by the member in their guild, if one was sent
        /// </summary>
        public Message LastMessage
        {
            get
            {
                var channel = Guild.Channels.Cache.Get(LastMessageChannelID);
                return channel == null ? null : channel.Messages.Cache.Get(LastMessageID);
            }
        }

        /// <summary>
        /// The ID of the channel for the last message sent by the member in their guild, if one was sent
        /// </summary>
        public Snowflake LastMessageChannelID { get; internal set; }

        /// <summary>
        /// The ID of the last message sent by the member in their guild, if one was sent
        /// </summary>
        public Snowflake LastMessageID { get; internal set; }

        /// <summary>
        /// Whether the client user is above this user in the hierarchy, according to role position and guild ownership. This is a prerequisite for many moderative actions.
        /// </summary>
        public bool Manageable
        {
            get
            {
                if (User.ID == Guild.OwnerID) return false;
                else if (User.ID == Client.User.ID) return false;
                else if (Client.User.ID == Guild.OwnerID) return true;
                else if (Guild.Me == null) throw new DJSError.Error("GUILD_UNCACHED_ME");
                else return Guild.Me.Roles.Highest.ComparePositionTo(Roles.Highest) > 0;
            }
        }

        /// <summary>
        /// The nickname of this member, if they have one
        /// </summary>
        public string Nickname { get; internal set; }

        /// <summary>
        /// Whether this GuildMember is a partial
        /// </summary>
        public bool Partial => JoinedTimestamp.HasValue;

        /// <summary>
        /// The overall set of permissions for this member, taking only roles into account
        /// </summary>
        public Permissions Permissions
        {
            get
            {
                if (User.ID == Guild.OwnerID) return new Permissions(Permissions.ALL).Freeze();
                return new Permissions(Roles.Cache.Map((role) => role.Permissions)).Freeze();
            }
        }

        /// <summary>
        /// The time of when the member used their Nitro boost on the guild, if it was used
        /// </summary>
        public Date PremiumSince => PremiumSinceTimestamp.HasValue ? new Date(PremiumSinceTimestamp.Value) : null;

        /// <summary>
        /// The timestamp of when the member used their Nitro boost on the guild, if it was used
        /// </summary>
        public long? PremiumSinceTimestamp { get; internal set; }

        /// <summary>
        /// The presence of this guild member
        /// </summary>
        public Presence Presence
        {
            get
            {
                var existing = Guild.Presences.Cache.Get(ID);
                if (existing != null) return existing;
                else return new Presence(Client, new PresenceData()
                {
                    user = new PresenceUserData()
                    {
                        id = ID
                    },
                    guild = Guild
                });
            }
        }

        /// <summary>
        /// A manager for the roles belonging to this member
        /// </summary>
        public GuildMemberRoleManager Roles => new GuildMemberRoleManager(this);

        /// <summary>
        /// The user that this guild member instance represents
        /// </summary>
        public User User { get; internal set; }

        /// <summary>
        /// The voice state of this member
        /// </summary>
        public VoiceState Voice
        {
            get
            {
                var voiceState = Guild.VoiceStates.Cache.Get(ID);
                if (voiceState == null) return new VoiceState(Guild, new VoiceStateData()
                {
                    user_id = ID
                });
                else return voiceState;
            }
        }

        internal Array<string> _roles;
        internal GuildMemberData data;

        public GuildMember(Client client, GuildMemberData data, Guild guild) : base(client)
        {
            Guild = guild;
            if (data != null && data.user != null) User = Client.Users.Add(data.user, true);
            JoinedTimestamp = null;
            LastMessageID = null;
            LastMessageChannelID = null;
            PremiumSinceTimestamp = null;
            Deleted = false;
            this.data = data;
            if (data != null) _Patch(data);
        }

        internal GuildMember(Client client, GuildMember member, Guild guild) : base(client)
        {
            Guild = guild;
            if (member != null && member.User != null) User = Client.Users.Add(member.User, true);
            JoinedTimestamp = null;
            LastMessageID = null;
            PremiumSinceTimestamp = null;
            Deleted = false;
            if (member != null) _Patch(data);
        }

        internal virtual void _Patch(GuildMemberData data)
        {
            if (data.nick != null) Nickname = data.nick;
            if (data.joined_at.HasValue) JoinedTimestamp = new Date(data.joined_at.Value).GetTime();
            if (data.premium_since.HasValue) PremiumSinceTimestamp = new Date(data.premium_since.Value).GetTime();
            if (data.user != null) User = Guild.Client.Users.Add(data.user);
            if (data.roles != null) _roles = new Array<string>(data.roles);
        }

        internal virtual void _Patch(GuildMember member)
        {
            if (member.Nickname != null) Nickname = member.Nickname;
            if (member.JoinedTimestamp.HasValue) JoinedTimestamp = member.JoinedTimestamp.Value;
            if (member.PremiumSinceTimestamp.HasValue) PremiumSinceTimestamp = member.PremiumSinceTimestamp.Value;
            if (member.User != null) User = Guild.Client.Users.Add(member.User);
            if (member._roles != null) _roles = member._roles.Slice();
        }

        internal new GuildMember _Clone()
        {
            var clone = MemberwiseClone() as GuildMember;
            clone._roles = _roles.Slice();
            return clone;
        }

        /// <summary>
        /// Bans this guild member.
        /// </summary>
        /// <param name="options">Options for the ban</param>
        /// <returns></returns>
        public IPromise<GuildMember> Ban(BanOptions options) => Guild.Members.Ban(this, options);

        /// <summary>
        /// Creates a DM channel between the client and this member.
        /// </summary>
        /// <returns></returns>
        public IPromise<DMChannel> CreateDM() => User.CreateDM();

        /// <summary>
        /// Deletes any DMs with this member.
        /// </summary>
        /// <returns></returns>
        public IPromise<DMChannel> DeleteDM() => User.DeleteDM();

        /// <summary>
        /// Edits this member.
        /// </summary>
        /// <param name="data">The data to edit the member with</param>
        /// <param name="reason">Reason for editing this user</param>
        /// <returns></returns>
        public IPromise<GuildMember> Edit(EditData data, string reason = null)
        {
            if (data.Channel != null)
            {
                GuildChannel channel = Guild.Channels.Resolve(data.Channel);
                if (channel == null || channel.Type != "voice") throw new DJSError.Error("GUILD_VOICE_CHANNEL_RESOLVE");
                data.channel_id = channel.ID;
                data.Channel = null;
            }
            else if (data.KickFromVoice)
            {
                data.Channel = null;
                data.channel_id = null;
            }
            var roles = data.Roles;
            if (roles != null)
            {
                string[] roleIds = new string[roles.Length];
                for (int index = 0, length = roles.Length; index < length; index++)
                {
                    var role = roles[index];
                    roleIds[index] = role.isRole ? role.role.ID : role.id.ToString();
                }
                data.roles = roleIds;
            }
            dynamic endpoint = Client.API.Guilds(Guild.ID);
            if (User.ID == Client.User.ID)
            {
                if (data.Nick != null && data.Roles == null && !data.Mute.HasValue && !data.Deaf.HasValue && data.Channel == null && data.KickFromVoice == false)
                    endpoint = endpoint.Members("@me").Nick;
                else endpoint = endpoint.Members(ID);
            }
            else endpoint = endpoint.Members(ID);
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter writer = new JsonTextWriter(sw);

            writer.WriteStartObject();

            writer.WritePropertyName("data");
            data.Serialize(writer);
            writer.WritePropertyName("reason");
            if (reason == null) writer.WriteNull();
            else writer.WriteValue(reason);

            writer.Close();

            IPromise<object> promise = endpoint.Patch(sb.ToString());
            return promise.Then((_) =>
            {
                var clone = _Clone();
                var d = new GuildMemberData()
                {
                    user = User != null ? User.data : null,
                    roles = data.roles,
                    mute = data.Mute,
                    deaf = data.Deaf,
                    nick = data.Nick
                };
                clone._Patch(d);
                return clone;
            });
        }

        /// <summary>
        /// Fetches this GuildMember.
        /// </summary>
        /// <returns></returns>
        public IPromise<GuildMember> Fetch() => Guild.Members.Fetch(ID, true);

        /// <summary>
        /// Checks if any of this member's roles have a permission.
        /// </summary>
        /// <param name="permission">Permission(s) to check for</param>
        /// <param name="options">Options</param>
        /// <returns></returns>
        public bool HasPermission(PermissionResolvable permission) => HasPermission(permission, new Permissions.CheckOptions());

        /// <summary>
        /// Checks if any of this member's roles have a permission.
        /// </summary>
        /// <param name="permission">Permission(s) to check for</param>
        /// <param name="options">Options</param>
        /// <returns></returns>
        public bool HasPermission(PermissionResolvable permission, Permissions.CheckOptions options)
        {
            if (options.checkOwner && User.ID == Guild.OwnerID) return true;
            return Roles.Cache.Some((r) => r.Permissions.Has(permission, options.checkAdmin));
        }

        /// <summary>
        /// Kicks this member from the guild.
        /// </summary>
        /// <param name="reason">Reason for kicking user</param>
        /// <returns></returns>
        public IPromise<GuildMember> Kick(string reason = null)
        {
            IPromise<object> promise = Client.API.Guilds(Guild.ID).Members(User.ID).Delete(new { reason });
            return promise.Then((_) => this);
        }

        /// <summary>
        /// Returns channel.PermissionsFor(guildMember). Returns permissions for a member in a guild channel, taking into account roles and permission overwrites.
        /// </summary>
        /// <param name="channel">The guild channel to use as context</param>
        /// <returns></returns>
        public Permissions PermissionsIn(ChannelResolvable channel)
        {
            GuildChannel chn = Guild.Channels.Resolve(channel);
            if (chn == null) throw new DJSError.Error("GUILD_CHANNEL_RESOLVE");
            return chn.MemberPermissions(this);
        }

        // Send

        /// <summary>
        /// Sets the nickname for this member.
        /// </summary>
        /// <param name="nick">The nickname for the guild member</param>
        /// <param name="reason">Reason for setting the nickname</param>
        /// <returns></returns>
        public IPromise<GuildMember> SetNickname(string nick, string reason = null) => Edit(new EditData()
        {
            Nick = nick
        }, reason);

        /// <summary>
        /// When concatenated with a string, this automatically returns the user's mention instead of the GuildMember object.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"<@{(Nickname != null ? "!" : "")}{User.ID}>";

        /// <summary>
        /// The data for editing a guild member.
        /// </summary>
        public sealed class EditData : IJSONConvertable
        {
            /// <summary>
            /// The nickname to set for the member
            /// </summary>
            [JsonIgnore]
            public string Nick { get; set; }

            /// <summary>
            /// The roles or role IDs to apply
            /// </summary>
            [JsonIgnore]
            public RoleResolvable[] Roles { get; set; }

            /// <summary>
            /// Whether or not the member should be muted
            /// </summary>
            [JsonIgnore]
            public bool? Mute { get; set; } = null;

            /// <summary>
            /// Whether or not the member should be deafened
            /// </summary>
            [JsonIgnore]
            public bool? Deaf { get; set; } = null;

            /// <summary>
            /// Channel to move member to (if they are connected to voice)
            /// </summary>
            [JsonIgnore]
            public ChannelResolvable Channel { get; set; }

            /// <summary>
            /// Whether to kick the member from a voice channel
            /// </summary>
            [JsonIgnore]
            public bool KickFromVoice { get; set; } = false;

            internal string channel_id = null;
            internal string[] roles = null;

            internal string ToJSON()
            {
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.None;

                    Serialize(writer);
                }

                return sb.ToString();
            }

            internal void Serialize(JsonWriter writer)
            {
                writer.WriteStartObject();

                writer.WritePropertyName("nick");
                if (Nick == null) writer.WriteNull();
                else writer.WriteValue(Nick);

                if (channel_id != null)
                {
                    writer.WritePropertyName("channel_id");
                    writer.WriteValue(channel_id);
                }
                else if (channel_id == null && KickFromVoice == true)
                {
                    writer.WritePropertyName("channel_id");
                    writer.WriteNull();
                }

                if (roles != null)
                {
                    writer.WritePropertyName("roles");
                    writer.WriteStartArray();
                    for (int index = 0, length = roles.Length; index < length; index++)
                        writer.WriteValue(roles[index]);
                    writer.WriteEndArray();
                }

                writer.WriteEndObject();
            }

            string IJSONConvertable.ToJSON() => ToJSON();
            void IJSONConvertable.Serialize(JsonWriter writer) => Serialize(writer);
        }
    }

    public sealed class BanOptions
    {
        /// <summary>
        /// Number of days of messages to delete
        /// </summary>
        public int days = 0;

        /// <summary>
        /// Reason for banning
        /// </summary>
        public string reason = null;
    }
}