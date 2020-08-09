using Newtonsoft.Json;

namespace DiscordJS
{
    /// <summary>
    /// Represents an object that can be converted to a JSON string
    /// </summary>
    public interface IJSONConvertable
    {
        /// <summary>
        /// Converts this object to a JSON string
        /// </summary>
        /// <returns></returns>
        string ToJSON();

        /// <summary>
        /// Serializes this object into JSON
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        void Serialize(JsonWriter writer);
    }
}