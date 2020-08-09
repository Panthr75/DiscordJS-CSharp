namespace DiscordJS.Data
{
    public class MessageData
    {
        public string id;
        public string channel_id;
        public UserData author;
        public GuildMemberData member;
        public string content;
        public long timestamp;
        public long? edited_timestamp;
        public bool tts;
        public bool mention_everyone;
        public MessageMentionUser[] mentions;
        public RoleData[] mention_roles;
        public ChannelData[] mention_channels;
        public MessageAttachmentData attachments;
        public MessageEmbedData[] embeds;
        public ReactionData[] reactions;
        public string nonce;
        public bool pinned;
        public string webhook_id;
        public int type;
        public MessageActivityData activity;
        public MessageApplicationData application;
        public MessageReferenceData message_reference;
        public int flags;
    }

    public class MessageMentionUser : UserData
    {
        public GuildMemberData member;
    }
}