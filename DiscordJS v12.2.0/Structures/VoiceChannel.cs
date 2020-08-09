using DiscordJS.Data;
using JavaScript;

namespace DiscordJS
{
    /// <summary>
    /// Represents a guild voice channel on Discord.
    /// </summary>
    public class VoiceChannel : GuildChannel
    {
        /// <summary>
        /// The bitrate of this voice channel
        /// </summary>
        public int Bitrate { get; internal set; }

        /// <summary>
        /// Whether the channel is deletable by the client user
        /// </summary>
        public override bool Deletable => base.Deletable && PermissionsFor(Client.User).Has(Permissions.FLAGS.CONNECT, false);

        /// <summary>
        /// Whether the channel is editable by the client user
        /// </summary>
        public bool Editable => Manageable && PermissionsFor(Client.User).Has(Permissions.FLAGS.CONNECT, false);

        /// <summary>
        /// Checks if the voice channel is full
        /// </summary>
        public bool Full => UserLimit > 0 && Members.Size >= UserLimit;

        /// <summary>
        /// Whether the channel is joinable by the client user
        /// </summary>
        public bool Joinable => Viewable && PermissionsFor(Client.User).Has(Permissions.FLAGS.CONNECT, false) && !(Full && PermissionsFor(Client.User).Has(Permissions.FLAGS.MOVE_MEMBERS, false));

        /// <summary>
        /// The members in this voice channel
        /// </summary>
        public override Collection<Snowflake, GuildMember> Members
        {
            get
            {
                var coll = new Collection<Snowflake, GuildMember>();
                foreach (VoiceState state in Guild.VoiceStates.Cache.Values())
                {
                    if (state.ChannelID == ID && state.Member != null)
                        coll.Set(state.ID, state.Member);
                }
                return coll;
            }
        }

        /// <summary>
        /// Checks if the client has permission to send audio to the voice channel
        /// </summary>
        public bool Speakable => PermissionsFor(Client.User).Has(Permissions.FLAGS.SPEAK, false);

        /// <summary>
        /// The maximum amount of users allowed in this channel - 0 means unlimited.
        /// </summary>
        public int UserLimit { get; internal set; }

        public VoiceChannel(Guild guild, ChannelData data) : base(guild, data)
        { }

        internal override void _Patch(ChannelData data)
        {
            base._Patch(data);

            Bitrate = data.bitrate.HasValue ? data.bitrate.Value : 0;
            UserLimit = data.user_limit.HasValue ? data.user_limit.Value : 0;
        }

        internal override void _Patch(Channel channel)
        {
            if (channel is VoiceChannel voiceChannel)
                _Patch(voiceChannel);
            else
                base._Patch(channel);
        }

        internal override void _Patch(GuildChannel channel)
        {
            if (channel.Type == "voice")
                _Patch((VoiceChannel)channel);
            else
                base._Patch(channel);
        }

        internal virtual void _Patch(VoiceChannel channel)
        {
            base._Patch(channel);

            Bitrate = channel.Bitrate;
            UserLimit = channel.UserLimit;
        }

        /// <summary>
        /// Attempts to join this voice channel.
        /// </summary>
        /// <returns></returns>
        public IPromise<VoiceConnection> Join() => Client.Voice.JoinChannel(this);

        /// <summary>
        /// Leaves this voice channel.
        /// </summary>
        public void Leave()
        {
            var connection = Client.Voice.Connections.Get(Guild.ID);
            if (connection != null && connection.Channel.ID == ID) connection.Disconnect();
        }

        /// <summary>
        /// Sets the bitrate of the channel.
        /// </summary>
        /// <param name="bitrate">The new bitrate</param>
        /// <param name="reason">Reason for changing the channel's bitrate</param>
        /// <returns></returns>
        public IPromise<VoiceChannel> SetBitrate(int bitrate, string reason = null)
        {
            return Edit(new GuildChannelEditData() { bitrate = bitrate }, reason).Then((_) => this);
        }

        /// <summary>
        /// Sets the user limit of the channel.
        /// </summary>
        /// <param name="userLimit">The new user limit</param>
        /// <param name="reason">Reason for changing the user limit</param>
        /// <returns></returns>
        public IPromise<VoiceChannel> SetUserLimit(int userLimit, string reason = null)
        {
            return Edit(new GuildChannelEditData() { userLimit = userLimit }, reason).Then((_) => this);
        }
    }
}