using DiscordJS.Data;
using JavaScript;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Represents any channel on Discord.
    /// </summary>
    public class Channel : Base, IHasID
    {
        /// <summary>
        /// The time the channel was created at
        /// </summary>
        public Date CreatedAt => Snowflake.Deconstruct(ID).Date;

        /// <summary>
        /// The timestamp the channel was created at
        /// </summary>
        public long CreatedTimestamp => Snowflake.Deconstruct(ID).Timestamp;

        /// <summary>
        /// Whether the channel has been deleted
        /// </summary>
        public bool Deleted { get; internal set; }

        /// <summary>
        /// The unique ID of the channel
        /// </summary>
        public Snowflake ID { get; internal set; }

        /// <summary>
        /// The type of the channel, either:
        /// <list type="table">
        /// <item>
        /// <term>dm</term>
        /// <description>A DM Channel</description>
        /// </item>
        /// <item>
        /// <term>group</term>
        /// <description>A Group DM Channel</description>
        /// </item>
        /// <item>
        /// <term>text</term>
        /// <description>A Guild Text Channel</description>
        /// </item>
        /// <item>
        /// <term>voice</term>
        /// <description>A Guild Voice Channel</description>
        /// </item>
        /// <item>
        /// <term>category</term>
        /// <description>A Guild Category Channel</description>
        /// </item>
        /// <item>
        /// <term>news</term>
        /// <description>A Guild News Channel</description>
        /// </item>
        /// <item>
        /// <term>store</term>
        /// <description>A Guild Store Channel</description>
        /// </item>
        /// <item>
        /// <term>unknown</term>
        /// <description>a Generic Channel of unknown type, could be Channel or GuildChannel</description>
        /// </item>
        /// </list>
        /// </summary>
        public string Type { get; internal set; }

        /// <summary>
        /// Whether this Channel is a partial
        /// </summary>
        public virtual bool Partial => false;

        internal ChannelData data;
        internal Map<Snowflake, TypingInfo> _typing;

        public Channel(Client client, ChannelData data) : base(client)
        {
            string type;
            try
            {
                type = System.Enum.GetName(typeof(ChannelTypes), (ChannelTypes)data.type);
            }
            catch(System.Exception)
            {
                type = "unknown";
            }
            Type = type == null ? "unknown" : type.ToLower();
            Deleted = false;
            if (data != null) _Patch(data);
        }

        internal Channel(Client client, Channel channel) : base(client)
        {
            Type = channel.Type == null ? "unknown" : Type.ToLower();
            Deleted = channel.Deleted;
            _Patch(channel);
        }

        internal virtual void _Patch(ChannelData data)
        {
            ID = data.id;
            this.data = data;
        }

        internal virtual void _Patch(Channel channel)
        {
            ID = channel.ID;
            data = channel.data;
        }

        /// <summary>
        /// Deletes this channel.
        /// </summary>
        /// <returns></returns>
        public IPromise<Channel> Delete()
        {
            return Client.API
                .Channels(ID)
                .Delete(new { })
                .Then((_) => this);
        }

        /// <summary>
        /// Fetches this channel.
        /// </summary>
        /// <returns></returns>
        public IPromise<Channel> Fetch() => Client.Channels.Fetch(ID, true);

        /// <summary>
        /// When concatenated with a string, this automatically returns the channel's mention instead of the Channel object.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"<#{ID}>";

        internal static Channel Create(ChannelData data, Client client, Guild guild = null)
        {
            switch ((ChannelTypes)data.type)
            {
                case ChannelTypes.CATEGORY:
                    return new CategoryChannel(guild, data);
                case ChannelTypes.DM:
                    return new DMChannel(client, data);
                case ChannelTypes.GROUP:
                    return null;
                case ChannelTypes.NEWS:
                    return new NewsChannel(guild, data);
                case ChannelTypes.STORE:
                    return new StoreChannel(guild, data);
                case ChannelTypes.TEXT:
                    return new TextChannel(guild, data);
                case ChannelTypes.VOICE:
                    return new VoiceChannel(guild, data);
                default:
                    return null;
            }
        }
    }
}