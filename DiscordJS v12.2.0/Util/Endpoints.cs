namespace DiscordJS
{
    public class Endpoints
    {
        public static CDNEndpoint CDN(string root) => new CDNEndpoint(root);
    }

    public class CDNEndpoint
    {
        private readonly string root;

        public CDNEndpoint(string root)
        {
            this.root = root;
        }

        public string Emoji(string emojiID, string format = "png") => $"{root}/emojis/{emojiID}/.{format}";
    }
}