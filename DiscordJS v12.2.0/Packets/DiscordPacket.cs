using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace DiscordJS.Packets
{
    /// <summary>
    /// Represents a generic discord packet
    /// </summary>
    public class DiscordPacket : IDiscordPacket<object>
    {
        [JsonIgnore]
        public OPCode OP { get; }

        [JsonIgnore]
        public dynamic Data { get; }

        public string ToJSON()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.None;

                Serialize(writer);;
            }
            return sb.ToString();
        }

        public void Serialize(JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("op");
            writer.WriteValue((int)OP);
            writer.WritePropertyName("d");
            writer.WriteRawValue(JsonConvert.SerializeObject(Data));
            writer.WriteEndObject();
        }

        public DiscordPacket(OPCode op, dynamic data)
        {
            OP = op;
            Data = data;
        }
    }
}