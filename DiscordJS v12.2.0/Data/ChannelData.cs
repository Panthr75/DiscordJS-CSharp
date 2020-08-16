using Newtonsoft.Json;
using System;

namespace DiscordJS.Data
{
    [Data]
    public sealed class ChannelData
    {
        public string id;
        public int type;
        public string guild_id;
        public int? position;
        public PermissionOverwriteData[] permission_overwrites;
        public string name;
        public string topic;
        public bool? nsfw;
        public string last_message_id;
        public int? bitrate;
        public int? user_limit;
        public int? rate_limit_per_user;
        public UserData[] recipients;
        public string icon;
        public string owner_id;
        public string application_id;
        public string parent_id;
        public long? last_pin_timestamp;
        public bool? lock_permissions;
        [JsonIgnore]
        [NotData]
        internal Guild guild;
    }
}