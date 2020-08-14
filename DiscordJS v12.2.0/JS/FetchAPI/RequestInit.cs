namespace JavaScript.Web
{
    public class RequestInit
    {
        public string Method { get; set; }
        public HeadersInit Headers { get; set; }
        public BodyInit Body { get; set; }
        public string Referrer { get; set; }
        // public ReferrerPolicy Policy { get; set; }
        /// <summary>
        /// <list type="bullet">
        /// <item>"navigate"</item>
        /// <item>"same-origin"</item>
        /// <item>"no-cors"</item>
        /// <item>"cors"</item>
        /// </list>
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// <list type="bullet">
        /// <item>"omit"</item>
        /// <item>"same-origin"</item>
        /// <item>"include"</item>
        /// </summary>
        public string Credentials { get; set; }
        /// <summary>
        /// <list type="bullet">
        /// <item>"default"</item>
        /// <item>"no-store"</item>
        /// <item>"reload"</item>
        /// <item>"no-cache"</item>
        /// <item>"force-cache"</item>
        /// <item>"only-if-cached"</item>
        /// </summary>
        public string Cache { get; set; }
    }
}