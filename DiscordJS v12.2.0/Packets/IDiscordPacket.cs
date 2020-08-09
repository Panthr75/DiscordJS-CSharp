using Newtonsoft.Json;

namespace DiscordJS.Packets
{
    /// <summary>
    /// Represents a Discord packet
    /// </summary>
    /// <typeparam name="T">The type of data this packet holds</typeparam>
    public interface IDiscordPacket<T> : IJSONConvertable
    {
        /// <summary>
        /// The OP code of this packet
        /// </summary>
        [JsonIgnore]
        OPCode OP { get; }

        /// <summary>
        /// The data this packet holds
        /// </summary>
        [JsonIgnore]
        T Data { get; }
    }
}