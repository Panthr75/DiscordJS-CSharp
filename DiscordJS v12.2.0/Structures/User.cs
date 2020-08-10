using DiscordJS.Data;
using DiscordJS.Resolvables;
using JavaScript;

namespace DiscordJS
{
    /// <summary>
    /// Represents a user on Discord.
    /// </summary>
    public class User : Base, IHasID
    {
        /// <summary>
        /// The ID of the user's avatar
        /// </summary>
        public Snowflake Avatar { get; internal set; }

        /// <summary>
        /// Whether or not the user is a bot
        /// </summary>
        public bool Bot { get; internal set; }

        /// <summary>
        /// The time the user was created at
        /// </summary>
        public Date CreatedAt => Snowflake.Deconstruct(ID).Date;

        /// <summary>
        /// The timestamp the user was created at
        /// </summary>
        public long CreatedTimestamp => Snowflake.Deconstruct(ID).Timestamp;

        /// <summary>
        /// A link to the user's default avatar
        /// </summary>
        public string DefaultAvatarURL => Client.rest.CDN.DefaultAvatar(int.Parse(Discriminator) % 5);

        /// <summary>
        /// A discriminator based on username for the user
        /// </summary>
        public string Discriminator { get; internal set; }

        /// <summary>
        /// The DM between the client's user and this user
        /// </summary>
        public DMChannel DMChannel => Client.Channels.Cache.Find((c) => c.Type == "dm" && ((DMChannel)c).Recipient.ID == ID);

        /// <summary>
        /// The flags for this user
        /// </summary>
        public UserFlags Flags { get; internal set; }

        /// <summary>
        /// The ID of the user
        /// </summary>
        public Snowflake ID { get; }

        /// <summary>
        /// The Message object of the last message sent by the user, if one was sent
        /// </summary>
        public Message LastMessage
        {
            get
            {
                var channel = (ITextBasedChannel)Client.Channels.Cache.Get(LastMessageChannelID);
                return channel == null ? null : channel.Messages.Cache.Get(LastMessageID);
            }
        }

        /// <summary>
        /// The ID of the channel for the last message sent by the user, if one was sent
        /// </summary>
        public Snowflake LastMessageChannelID { get; internal set; }

        /// <summary>
        /// The ID of the last message sent by the user, if one was sent
        /// </summary>
        public Snowflake LastMessageID { get; internal set; }

        /// <summary>
        /// The locale of the user's client (ISO 639-1)
        /// </summary>
        public string Locale { get; internal set; }

        /// <summary>
        /// Whether this User is a partial
        /// </summary>
        public bool Partial => Username != null;

        /// <summary>
        /// The presence of this user
        /// </summary>
        public Presence Presence
        { 
            get
            {
                foreach (Guild guild in Client.Guilds.Cache.Values())
                {
                    if (guild.Presences.Cache.Has(ID)) return guild.Presences.Cache.Get(ID);
                }

                return new Presence(Client, new PresenceData()
                {
                    user = new UserData()
                    {
                        id = ID
                    }
                });
            }
        }

        /// <summary>
        /// Whether the user is an Official Discord System user (part of the urgent message system)
        /// </summary>
        public bool System { get; internal set; }

        /// <summary>
        /// The Discord "tag" (e.g. <c>hydrabolt#0001</c>) for this user
        /// </summary>
        public string Tag => Username is null ? null : $"{Username}#{Discriminator}";

        /// <summary>
        /// The username of the user
        /// </summary>
        public string Username { get; internal set; }

        internal UserData data;

        /// <summary>
        /// Instantiates a new User Instance
        /// </summary>
        /// <param name="client">The client that instantiated this object</param>
        /// <param name="data">The data for this user</param>
        public User(Client client, UserData data) : base(client)
        {
            this.data = data;
            ID = data.id;
            Bot = data.bot.HasValue ? data.bot.Value : false;

            _Patch(data);
        }

        internal User(Client client, User user) : base(client)
        {
            ID = user.ID;
            Bot = user.Bot;

            _Patch(user);
        }

        internal virtual void _Patch(UserData data)
        {
            if (data.username != null) Username = data.username;
            if (data.discriminator != null) Discriminator = data.discriminator;
            if (data.avatar != null) Avatar = data.avatar;
            if (data.bot.HasValue) Bot = data.bot.Value;
            if (data.system.HasValue) System = data.system.Value;
            if (data.locale != null) Locale = data.locale;
            if (data.public_flags != null) Flags = new UserFlags(data.public_flags);
            LastMessageID = null;
            LastMessageChannelID = null;
        }

        internal virtual void _Patch(User user)
        {
            if (user.Username != null) Username = user.Username;
            if (user.Discriminator != null) Discriminator = user.Discriminator;
            if (user.Avatar != null) Avatar = user.Avatar;
            Bot = user.Bot;
            System = user.System;
            if (user.Locale != null) Locale = user.Locale;
            if (user.Flags != null) Flags = user.Flags;
            LastMessageID = null;
            LastMessageChannelID = null;
        }

        /// <summary>
        /// A link to the user's avatar.
        /// </summary>
        /// <param name="options">Options for the Image URL</param>
        /// <returns></returns>
        public string AvatarURL(ImageURLOptions options) => AvatarURL(options.Format, options.Size, options.Dynamic);

        /// <summary>
        /// A link to the user's avatar.
        /// </summary>
        /// <returns></returns>
        public string AvatarURL(string format, int size, bool dynamic)
        {
            if (Avatar is null) return null;
            return Client.rest.CDN.Avatar(ID, Avatar, format, size, dynamic);
        }

        /// <summary>
        /// Creates a DM channel between the client and the user.
        /// </summary>
        /// <returns></returns>
        public IPromise<DMChannel> CreateDM()
        {
            var dmChannel = DMChannel;
            if (dmChannel != null && !dmChannel.Partial) Promise<DMChannel>.Resolved(dmChannel);
            return Client.API.Users(Client.User.ID).Channels.Post(new
            {
                data = new
                {
                    recipient_id = ID
                }
            }).Then((data) => (DMChannel)Client.Actions.ChannelCreate.Handle(data).Channel);
        }

        /// <summary>
        /// Deletes a DM channel (if one exists) between the client and the user. Resolves with the channel if successful.
        /// </summary>
        /// <returns></returns>
        public IPromise<DMChannel> DeleteDM()
        {
            var dmChannel = DMChannel;
            if (dmChannel is null) throw new DJSError.Error("USER_NO_DMCHANNEL");
            return Client.API.Channels(dmChannel.ID).Delete().Then((data) => (DMChannel)Client.Actions.ChannelDelete.Handle(data).Channel);
        }

        /// <summary>
        /// A link to the user's avatar if they have one. Otherwise a link to their default avatar will be returned.
        /// </summary>
        /// <param name="options">Options for the Image URL</param>
        /// <returns></returns>
        public string DisplayAvatarURL(ImageURLOptions options)
        {
            string normalURL = AvatarURL(options);
            return normalURL is null ? DefaultAvatarURL : normalURL;
        }

        /// <summary>
        /// Checks if the user is equal to another. It compares ID, username, discriminator, avatar, and bot flags. It is recommended to compare equality by using <c>user.ID == user2.ID</c> unless you want to compare all properties.
        /// </summary>
        /// <param name="user">User to compare with</param>
        /// <returns></returns>
        public bool Equals(User user)
        {
            return 
                user != null &&
                ID == user.ID &&
                Username == user.Username &&
                Discriminator == user.Discriminator &&
                Avatar == user.Avatar;
        }

        /// <summary>
        /// Fetches this user.
        /// </summary>
        /// <returns></returns>
        public IPromise<User> Fetch() => Client.Users.Fetch(ID, true);

        /// <summary>
        /// Fetches this user's flags.
        /// </summary>
        /// <returns></returns>
        public IPromise<UserFlags> FetchFlags()
        {
            if (Flags != null) return Promise<UserFlags>.Resolved(Flags);
            return Client.API.Users(ID).Get().Then((data) =>
            {
                _Patch(data);
                return Flags;
            });
        }

        // Send

        /// <summary>
        /// When concatenated with a string, this automatically returns the user's mention instead of the User object.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"<@{ID}>";

        /// <summary>
        /// Gets the amount of time the user has been typing in a channel for (in milliseconds), or -1 if they're not typing.
        /// </summary>
        /// <param name="channel">The channel to get the time in</param>
        /// <returns></returns>
        public long? TypingDurationIn(ChannelResolvable channel)
        {
            var chn = Client.Channels.Resolve(channel);
            return chn._typing.Has(ID) ? chn._typings.Get(ID).ElapsedTime : null;
        }

        /// <summary>
        /// Checks whether the user is typing in a channel.
        /// </summary>
        /// <param name="channel">The channel to check in</param>
        /// <returns></returns>
        public bool TypingIn(ChannelResolvable channel)
        {
            var chn = Client.Channels.Resolve(channel);
            return chn._typing.Has(ID);
        }

        /// <summary>
        /// Gets the time that the user started typing.
        /// </summary>
        /// <param name="channel">The channel to get the time in</param>
        /// <returns></returns>
        public Date TypingSinceIn(ChannelResolvable channel)
        {
            var chn = Client.Channels.Resolve(channel);
            return chn._typing.Has(ID) ? new Date(chn._typings.Get(ID).Since) : null;
        }
    }
}