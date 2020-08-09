namespace DiscordJS.Data
{
    public class VoiceStateData
    {
        /// <summary>
        /// the guild id this voice state is for
        /// </summary>
        public string guild_id;

        /// <summary>
        /// the channel id this user is connected to
        /// </summary>
        public string channel_id;

        /// <summary>
        /// the user id this voice state is for
        /// </summary>
        public string user_id;

        /// <summary>
        /// the guild member this voice state is for
        /// </summary>
        public GuildMemberData member;

        /// <summary>
        /// the session id for this voice state
        /// </summary>
        public string session_id;

        /// <summary>
        /// whether this user is deafened by the server
        /// </summary>
        public bool? deaf;

        /// <summary>
        /// whether this user is muted by the server
        /// </summary>
        public bool? mute;

        /// <summary>
        /// whether this user is locally deafened
        /// </summary>
        public bool? self_deaf;

        /// <summary>
        /// whether this user is locally muted
        /// </summary>
        public bool? self_mute;

        /// <summary>
        /// whether this user is streaming using "Go Live"
        /// </summary>
        public bool? self_stream;

        /// <summary>
        /// whether this user's camera is enabled
        /// </summary>
        public bool? self_video;

        /// <summary>
        /// whether this user is muted by the current user
        /// </summary>
        public bool? suppress;
    }
}