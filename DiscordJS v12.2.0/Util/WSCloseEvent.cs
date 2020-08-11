namespace DiscordJS
{
    /// <summary>
    /// An event for when the discord web socket closes
    /// </summary>
    public class WSCloseEvent
    {
        /// <summary>
        /// The code for why the websocket was closed
        /// </summary>
        public ushort Code { get; }

        /// <summary>
        /// The reason the server closed the connection
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Whether or not the conenction was cleanly closed
        /// </summary>
        public bool WasClean { get; }

        internal WSCloseEvent(ushort code, string reason, bool wasClean)
        {
            Code = code;
            Reason = reason;
            WasClean = wasClean;
        }
    }
}