/*
ABOUT ACTIONS
Actions are similar to WebSocket Packet Handlers, but since introducing
the REST API methods, in order to prevent rewriting code to handle data,
"actions" have been introduced. They're basically what Packet Handlers
used to be but they're strictly for manipulating data and making sure
that WebSocket events don't clash with REST methods.
*/

using DiscordJS.Data;
using System;

namespace DiscordJS.Actions
{
    /// <summary>
    /// Represents a generic action
    /// </summary>
    /// <typeparam name="T">The type this action returns</typeparam>
    public abstract class GenericAction<T>
    {
        /// <summary>
        /// The client that instantiated this action
        /// </summary>
        public Client Client { get; }

        public GenericAction(Client client)
        {
            Client = client;
        }

        /// <summary>
        /// Handles packet data
        /// </summary>
        /// <param name="data">The data to handle</param>
        /// <returns></returns>
        public abstract T Handle(dynamic data);

        protected V GetPayload<D, V, C>(D data, BaseManager<C, V, D> manager, Snowflake id, PartialType partialType, bool cache = true) where V : class, IHasID where C : class, ICollection<Snowflake, V>, new()
        {
            V existing = manager.Cache.Get(id);
            if (existing == null && Client.Options.partials.Includes(partialType))
            {
                dynamic m = manager;
                return m.Add(data, cache);
            }
            return existing;
        }

        /// <summary>
        /// Gets the channel for the given data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected dynamic GetChannel(dynamic data)
        {
            string id;
            try
            {
                id = data.channel_id;
                if (id == null)
                    throw new Exception();
            }
            catch(Exception)
            {
                id = data.id;
            }
            try
            {
                if (data.channel != null)
                    return data.channel;
                else
                    throw new Exception();
            }
            catch(Exception)
            {
                UserData recipient;
                try
                {
                    if (data.author != null)
                        recipient = data.author;
                    else
                        throw new Exception();
                }
                catch (Exception)
                {
                    recipient = new UserData() { id = data.user_id };
                }
                return GetPayload(new ChannelData()
                {
                    id = id, guild_id = data.guild_id, recipients = new UserData[1] { recipient }
                }, Client.Channels, id, PartialType.CHANNEL);
            }
        }

        /// <summary>
        /// Gets the message for the given channel data, and data
        /// </summary>
        /// <param name="data">The data</param>
        /// <param name="channel">The channel data</param>
        /// <param name="cache">Whether to cache the created message</param>
        /// <returns></returns>
        protected dynamic GetMessage(dynamic data, dynamic channel, bool cache = true)
        {
            string id;
            try
            {
                id = data.channel_id;
                if (id == null)
                    throw new Exception();
            }
            catch (Exception)
            {
                id = data.id;
            }
            bool isChannel = channel is Channel;
            Guild guild = isChannel ? channel.Guild : new Guild(Client, (channel.guild == null ? Client.Guilds.Cache.Get(channel.guild_id) : channel.guild));
            Channel c = isChannel ? channel : Channel.Create(channel, Client, guild);
            ITextBasedChannel t = c as ITextBasedChannel;
            try
            {
                if (t != null && data.message != null)
                    return data.message;
                else
                    throw new Exception();
            }
            catch (Exception)
            {
                string guild_id;
                try
                {
                    if (data.guild_id != null)
                        guild_id = data.guild_id;
                    else
                        throw new Exception();
                }
                catch(Exception)
                {
                    guild_id = guild == null ? null : guild.ID;
                }

                return GetPayload(new MessageData()
                {
                    id = id,
                    channel_id = isChannel ? (string)channel.ID : channel.id,
                    guild_id = guild_id
                }, t.Messages, id, PartialType.MESSAGE, cache);
            }
        }
    }
}