using DiscordJS.Data;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Represents the voice state for a Guild Member.
    /// </summary>
    public class VoiceState : Base, IHasID
    {
        /// <summary>
        /// The channel that the member is connected to
        /// </summary>
        public VoiceChannel Channel => (VoiceChannel)Guild.Channels.Cache.Get(ChannelID);

        /// <summary>
        /// The ID of the voice channel that this member is in
        /// </summary>
        public Snowflake ChannelID { get; internal set; }

        /// <summary>
        /// If this is a voice state of the client user, then this will refer to the active VoiceConnection for this guild
        /// </summary>
        public VoiceConnection Connection => ID != Client.User.ID ? null : Client.Voice.Connections.Get(Guild.ID);

        /// <summary>
        /// Whether this member is either self-deafened or server-deafened
        /// </summary>
        public bool Deaf => (ServerDeaf.HasValue && ServerDeaf.Value) || (SelfDeaf.HasValue && SelfDeaf.Value);

        /// <summary>
        /// The guild of this voice state
        /// </summary>
        public Guild Guild { get; internal set; }

        /// <summary>
        /// The ID of the member of this voice state
        /// </summary>
        public Snowflake ID { get; internal set; }

        /// <summary>
        /// The member that this voice state belongs to
        /// </summary>
        public GuildMember Member => Guild.Members.Cache.Get(ID);

        /// <summary>
        /// Whether this member is either self-muted or server-muted
        /// </summary>
        public bool Mute => (ServerMute.HasValue && ServerMute.Value) || (SelfMute.HasValue && SelfMute.Value);

        /// <summary>
        /// Whether this member is self-deafened
        /// </summary>
        public bool? SelfDeaf { get; internal set; }

        /// <summary>
        /// Whether this member is self-muted
        /// </summary>
        public bool? SelfMute { get; internal set; }

        /// <summary>
        /// Whether this member is deafened server-wide
        /// </summary>
        public bool? ServerDeaf { get; internal set; }

        /// <summary>
        /// Whether this member is muted server-wide
        /// </summary>
        public bool? ServerMute { get; internal set; }

        /// <summary>
        /// The session ID of this member's connection
        /// </summary>
        public string SessionID { get; internal set; }

        /// <summary>
        /// Whether this member is currently speaking. A <see cref="bool"/> if the information is available (aka the bot is connected to any voice channel in the guild), otherwise this is <see langword="null"/>
        /// </summary>
        public bool? Speaking
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Whether this member is streaming using "Go Live"
        /// </summary>
        public bool Streaming { get; internal set; }

        /// <summary>
        /// Instantiates a new voice state
        /// </summary>
        /// <param name="guild">The guild the voice state is part of</param>
        /// <param name="data">The data for the voice state</param>
        public VoiceState(Guild guild, VoiceStateData data) : base(guild.Client)
        {
            Guild = guild;
            ID = data.user_id;
            _Patch(data);
        }

        internal VoiceState _Patch(VoiceStateData data)
        {
            ServerDeaf = data.deaf;
            ServerMute = data.mute;
            SelfDeaf = data.self_deaf;
            SelfMute = data.self_mute;
            SessionID = data.session_id;
            Streaming = data.self_stream.HasValue ? data.self_stream.Value : false;
            ChannelID = data.channel_id;
            return this;
        }
    }
}