namespace DiscordJS
{
    /// <summary>
    /// An Audio Player for a Voice Connection.
    /// </summary>
    public class AudioPlayer : BasePlayer
    {
        /// <summary>
        /// The voice connection that the player serves
        /// </summary>
        public override VoiceConnection VoiceConnection { get; }

        public AudioPlayer(VoiceConnection voiceConnection)
        {
            VoiceConnection = voiceConnection;
        }

        public override StreamDispatcher PlayBroadcast(VoiceBroadcast broadcast, StreamOptions options)
        {
            var dispatcher = CreateDispatcher(options, new StreamDispatcher.Streams()
            {
                Broadcast = broadcast
            });
            broadcast.Add(dispatcher);
            return dispatcher;
        }
    }
}