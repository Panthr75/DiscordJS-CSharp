namespace DiscordJS
{
    public class Endpoints
    {
        public static CDNEndpoint CDN(string root) => new CDNEndpoint(root);
        public static string Invite(string root, string code) => $"{root}/{code}";
        public static string BotGateway => "/gateway/bot";
    }

    public class CDNEndpoint
    {
        private readonly string root;

        public CDNEndpoint(string root)
        {
            this.root = root;
        }

        private static string MakeImageURL(string root, ImageURLOptions options)
            => $"{root}.{options.format}{(options.size.HasValue ? $"?size={options.size.Value}" : "")}";

        public string Emoji(string emojiID, string format = "png") => $"{root}/emojis/{emojiID}/.{format}";
        public string Asset(string name) => $"{root}/assets/{name}";
        public string DefaultAvatar(int discriminator) => $"{root}/embed/avatars/{discriminator}.png";
        public string Avatar(string userID, string hash, string format = "webp", int? size = null, bool dynamic = false)
            => MakeImageURL($"{root}/avatars/{userID}/{hash}", new ImageURLOptions(dynamic && hash.StartsWith("a_") ? "gif" : format, dynamic, size));
        public string Banner(string guildID, string hash, string format = "webp", int? size = null)
            => MakeImageURL($"{root}/banners/{guildID}/{hash}", new ImageURLOptions(format, false, size));
        public string Icon(string guildID, string hash, string format = "webp", int? size = null, bool dynamic = false)
            => MakeImageURL($"{root}/icons/{guildID}/{hash}", new ImageURLOptions(dynamic && hash.StartsWith("a_") ? "gif" : format, dynamic, size));
        public string AppIcon(string clientID, string hash, string format = "webp", int? size = null)
            => MakeImageURL($"{root}/app-icons/{clientID}/{hash}", new ImageURLOptions(format, false, size));
        public string AppAsset(string clientID, string hash, string format = "webp", int? size = null)
            => MakeImageURL($"{root}/app-assets/{clientID}/{hash}", new ImageURLOptions(format, false, size));
        public string GDMIcon(string channelID, string hash, string format = "webp", int? size = null)
            => MakeImageURL($"{root}/channel-icons/{channelID}/{hash}", new ImageURLOptions(format, false, size));
        public string Splash(string guildID, string hash, string format = "webp", int? size = null)
            => MakeImageURL($"{root}/splashes/{guildID}/{hash}", new ImageURLOptions(format, false, size));
        public string DiscoverySplash(string guildID, string hash, string format = "webp", int? size = null)
            => MakeImageURL($"{root}/discovery-splashes/{guildID}/{hash}", new ImageURLOptions(format, false, size));
        public string TeamIcon(string teamID, string hash, string format = "webp", int? size = null)
            => MakeImageURL($"{root}/team-icons/{teamID}/{hash}", new ImageURLOptions(format, false, size));
    }
}