using DiscordJS.Data;
using JavaScript;

namespace DiscordJS
{
    /// <summary>
    /// Represents a Client OAuth2 Application.
    /// </summary>
    public class ClientApplication : Base
    {
        /// <summary>
        /// If this app's bot is public
        /// </summary>
        public bool? BotPublic { get; internal set; }

        /// <summary>
        /// If this app's bot requires a code grant when using the OAuth2 flow
        /// </summary>
        public bool? BotRequireCodeGrant { get; internal set; }

        /// <summary>
        /// The app's cover image
        /// </summary>
        public string Cover { get; internal set; }

        /// <summary>
        /// The time the app was created at
        /// </summary>
        public Date CreatedAt => Snowflake.Deconstruct(ID).Date;

        /// <summary>
        /// The timestamp the app was created at
        /// </summary>
        public long CreatedTimestamp => Snowflake.Deconstruct(ID).Timestamp;

        /// <summary>
        /// The app's description
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// The app's icon hash
        /// </summary>
        public string Icon { get; internal set; }

        /// <summary>
        /// The ID of the app
        /// </summary>
        public Snowflake ID { get; internal set; }

        /// <summary>
        /// The name of the app
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The owner of this OAuth application
        /// </summary>
        public ClientApplicationOwner Owner { get; internal set; }

        /// <summary>
        /// The app's RPC origins, if enabled
        /// </summary>
        public Array<string> RPCOrigins { get; internal set; }

        public ClientApplication(Client client, ClientApplicationData data) : base(client)
        {
            //
        }

        internal virtual void _Patch(ClientApplicationData data)
        {
            ID = data.id;
            Name = data.name;
            Description = data.description;
            Icon = data.icon;
            Cover = data.cover_image;
            RPCOrigins = data.rpc_origins == null ? new Array<string>() : new Array<string>(data.rpc_origins);
            BotRequireCodeGrant = data.bot_require_code_grant;
            BotPublic = data.bot_require_code_grant;
            Owner = data.team == null ? (ClientApplicationOwner)(data.owner == null ? null : Client.Users.Add(data.owner)) : (ClientApplicationOwner)new Team(Client, data.team);
        }

        /// <summary>
        /// A link to this application's cover image.
        /// </summary>
        /// <param name="options">Options for the Image URL</param>
        /// <returns>URL to the cover image</returns>
        public string CoverImage(ImageURLOptions options = null)
        {
            if (Cover == null) return null;
            if (options == null) options = new ImageURLOptions();
            return Endpoints.CDN(Client.Options.http.cdn).AppIcon(ID, Cover, options.format, options.size);
        }

        /// <summary>
        /// Gets the clients rich presence assets.
        /// </summary>
        /// <returns></returns>
        public IPromise<Array<ClientAsset>> FetchAssets()
        {
            return Client.API.OAuth2.Applications(ID).Assets.Get().Then((assets) =>
            {
                Array<ClientAsset> result = new Array<ClientAsset>();
                for (int index = 0, length = assets.Length; index < length; index++)
                    result.Push(new ClientAsset(assets[index]));
            });
        }

        /// <summary>
        /// A link to the application's icon.
        /// </summary>
        /// <param name="options">Options for the Image URL</param>
        /// <returns>URL to the icon</returns>
        public string IconURL(ImageURLOptions options = null)
        {
            if (Icon == null) return null;
            if (options == null) options = new ImageURLOptions();
            return Client.rest.CDN.AppIcon(ID, Icon, options.format, options.size);
        }

        /// <summary>
        /// When concatenated with a string, this automatically returns the application's name instead of the ClientApplication object.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;
    }

    /// <summary>
    /// Asset data.
    /// </summary>
    public class ClientAsset
    {
        /// <summary>
        /// The asset ID
        /// </summary>
        public Snowflake AssetID { get; }

        /// <summary>
        /// The asset name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The asset type
        /// </summary>
        public AssetTypes Type { get; }

        internal ClientAsset(AssetData data)
        {
            AssetID = data.id;
            Name = data.name;
            Type = (AssetTypes)data.type;
        }
    }
}