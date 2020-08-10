namespace DiscordJS
{
    /// <summary>
    /// Info for a rate limit
    /// </summary>
    public class RateLimitInfo
    {
        /// <summary>
        /// Timeout in ms
        /// </summary>
        public long Timeout { get; internal set; }

        /// <summary>
        /// Number of requests that can be made to this endpoint
        /// </summary>
        public int Limit { get; internal set; }

        /// <summary>
        /// HTTP method used for request that triggered this event
        /// </summary>
        public string Method { get; internal set; }

        /// <summary>
        /// Path used for request that triggered this event
        /// </summary>
        public string Path { get; internal set; }

        /// <summary>
        /// Route used for request that triggered this event
        /// </summary>
        public string Route { get; internal set; }
    }
}