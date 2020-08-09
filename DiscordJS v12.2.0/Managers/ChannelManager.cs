using DiscordJS.Data;
using DiscordJS.Resolvables;
using JavaScript;
using System.Collections.Generic;

namespace DiscordJS
{
    /// <summary>
    /// A manager of channels belonging to a client
    /// </summary>
    public class ChannelManager : BaseManager<Collection<Snowflake, Channel>, Channel, ChannelData>
    {
        public ChannelManager(Client client, IEnumerable<Channel> iterable) : base(client, iterable)
        { }

        protected override Channel AddItem(Channel item) => Add(item, null);

        public Channel Add(Channel data, Guild guild, bool cache = true)
        {
            Snowflake snowflake = data.ID;
            Channel existing = Cache.Get(snowflake);
            if (existing != null)
            {
                if (cache) existing._Patch(data);
                if (guild != null) guild.Channels.Add((GuildChannel)existing);
                return existing;
            }

            var entry = new Channel(Client, data);
            if (cache) Cache.Set(snowflake, entry);
            return entry;
        }

        public Channel Add(ChannelData data, Guild guild, bool cache = true)
        {
            Snowflake snowflake = data.id;
            Channel existing = Cache.Get(snowflake);
            if (existing != null)
            {
                if (cache) existing._Patch(data);
                if (guild != null) guild.Channels.Add((GuildChannel)existing);
                return existing;
            }

            var entry = new Channel(Client, data);
            if (cache) Cache.Set(snowflake, entry);
            return entry;
        }

        public void Remove(Snowflake id)
        {
            var channel = Cache.Get(id);
            if (channel != null && channel is GuildChannel guildChannel) guildChannel.Guild.Channels.Cache.Delete(id);
            Cache.Delete(id);
        }

        /// <summary>
        /// Resolves a ChannelResolvable to a Channel object.
        /// </summary>
        /// <param name="channel">The channel resolvable to resolve</param>
        /// <returns></returns>
        public Channel Resolve(ChannelResolvable channel) => channel.Resolve(this);

        /// <summary>
        /// Resolves a ChannelResolvable to a channel ID snowflake.
        /// </summary>
        /// <param name="channel">The channel resolvable to resolve</param>
        /// <returns></returns>
        public Snowflake ResolveID(ChannelResolvable channel)
        {
            var c = Resolve(channel);
            if (c == null) return null;
            return c.ID;
        }

        /// <summary>
        /// Obtains a channel from Discord, or the channel cache if it's already available.
        /// </summary>
        /// <param name="id">ID of the channel</param>
        /// <param name="cache">Whether to cache the new channel object if it isn't already</param>
        /// <returns></returns>
        public IPromise<Channel> Fetch(Snowflake id, bool cache = true)
        {
            Channel existing = Cache.Get(id);
            if (existing != null && !existing.Partial) return Promise<Channel>.Resolved(existing);
            return Client.API.Channels(id).Get().Then((data) => Add(data, null, cache));
        }
    }
}