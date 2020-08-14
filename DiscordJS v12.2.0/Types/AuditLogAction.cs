namespace DiscordJS
{
    /// <summary>
    /// The action of an entry. Here are the available actions:
    /// <list type="table">
    /// <item>
    /// <term>ALL</term>
    /// <description>null</description>
    /// </item>
    /// <item>
    /// <term>GUILD_UPDATE</term>
    /// <description>1</description>
    /// </item>
    /// <item>
    /// <term>CHANNEL_CREATE</term>
    /// <description>10</description>
    /// </item>
    /// <item>
    /// <term>CHANNEL_UPDATE</term>
    /// <description>11</description>
    /// </item>
    /// <item>
    /// <term>CHANNEL_DELETE</term>
    /// <description>12</description>
    /// </item>
    /// <item>
    /// <term>CHANNEL_OVERWRITE_CREATE</term>
    /// <description>13</description>
    /// </item>
    /// <item>
    /// <term>CHANNEL_OVERWRITE_UPDATE</term>
    /// <description>14</description>
    /// </item>
    /// <item>
    /// <term>CHANNEL_OVERWRITE_DELETE</term>
    /// <description>15</description>
    /// </item>
    /// <item>
    /// <term>MEMBER_KICK</term>
    /// <description>20</description>
    /// </item>
    /// <item>
    /// <term>MEMBER_PRUNE</term>
    /// <description>21</description>
    /// </item>
    /// <item>
    /// <term>MEMBER_BAN_ADD</term>
    /// <description>22</description>
    /// </item>
    /// <item>
    /// <term>MEMBER_BAN_REMOVE</term>
    /// <description>23</description>
    /// </item>
    /// <item>
    /// <term>MEMBER_UPDATE</term>
    /// <description>24</description>
    /// </item>
    /// <item>
    /// <term>MEMBER_ROLE_UPDATE</term>
    /// <description>25</description>
    /// </item>
    /// <item>
    /// <term>MEMBER_MOVE</term>
    /// <description>26</description>
    /// </item>
    /// <item>
    /// <term>MEMBER_DISCONNECT</term>
    /// <description>27</description>
    /// </item>
    /// <item>
    /// <term>BOT_ADD</term>
    /// <description>28</description>
    /// </item>
    /// <item>
    /// <term>ROLE_CREATE</term>
    /// <description>30</description>
    /// </item>
    /// <item>
    /// <term>ROLE_UPDATE</term>
    /// <description>31</description>
    /// </item>
    /// <item>
    /// <term>ROLE_DELETE</term>
    /// <description>32</description>
    /// </item>
    /// <item>
    /// <term>INVITE_CREATE</term>
    /// <description>40</description>
    /// </item>
    /// <item>
    /// <term>INVITE_UPDATE</term>
    /// <description>41</description>
    /// </item>
    /// <item>
    /// <term>INVITE_DELETE</term>
    /// <description>42</description>
    /// </item>
    /// <item>
    /// <term>WEBHOOK_CREATE</term>
    /// <description>50</description>
    /// </item>
    /// <item>
    /// <term>WEBHOOK_UPDATE</term>
    /// <description>51</description>
    /// </item>
    /// <item>
    /// <term>WEBHOOK_DELETE</term>
    /// <description>52</description>
    /// </item>
    /// <item>
    /// <term>EMOJI_CREATE</term>
    /// <description>60</description>
    /// </item>
    /// <item>
    /// <term>EMOJI_UPDATE</term>
    /// <description>61</description>
    /// </item>
    /// <item>
    /// <term>EMOJI_DELETE</term>
    /// <description>62</description>
    /// </item>
    /// <item>
    /// <term>MESSAGE_DELETE</term>
    /// <description>72</description>
    /// </item>
    /// <item>
    /// <term>MESSAGE_BULK_DELETE</term>
    /// <description>73</description>
    /// </item>
    /// <item>
    /// <term>MESSAGE_PIN</term>
    /// <description>74</description>
    /// </item>
    /// <item>
    /// <term>MESSAGE_UNPIN</term>
    /// <description>75</description>
    /// </item>
    /// <item>
    /// <term>INTEGRATION_CREATE</term>
    /// <description>80</description>
    /// </item>
    /// <item>
    /// <term>INTEGRATION_UPDATE</term>
    /// <description>81</description>
    /// </item>
    /// <item>
    /// <term>INTEGRATION_DELETE</term>
    /// <description>82</description>
    /// </item>
    /// </list>
    /// </summary>
    public enum AuditLogAction
    {
        /// <summary>
        /// <see langword="null"/>
        /// </summary>
        ALL = 0,

        /// <summary>
        /// The <see cref="Guild"/> was updated
        /// </summary>
        GUILD_UPDATE = 1,

        /// <summary>
        /// A <see cref="Channel"/> (<see cref="GuildChannel"/>) in the <see cref="Guild"/> was created
        /// </summary>
        CHANNEL_CREATE = 10,

        /// <summary>
        /// A <see cref="Channel"/> (<see cref="GuildChannel"/>) in the <see cref="Guild"/> was updated
        /// </summary>
        CHANNEL_UPDATE = 11,

        /// <summary>
        /// A <see cref="Channel"/> (<see cref="GuildChannel"/>) in the <see cref="Guild"/> was deleted
        /// </summary>
        CHANNEL_DELETE = 12,

        /// <summary>
        /// <see cref="PermissionOverwrites"/> for a <see cref="Channel"/> (<see cref="GuildChannel"/>) was created
        /// </summary>
        CHANNEL_OVERWRITE_CREATE = 13,

        /// <summary>
        /// <see cref="PermissionOverwrites"/> for a <see cref="Channel"/> (<see cref="GuildChannel"/>) was updated
        /// </summary>
        CHANNEL_OVERWRITE_UPDATE = 14,

        /// <summary>
        /// <see cref="PermissionOverwrites"/> for a <see cref="Channel"/> (<see cref="GuildChannel"/>) was deleted
        /// </summary>
        CHANNEL_OVERWRITE_DELETE = 15,

        /// <summary>
        /// A <see cref="GuildMember"/> was kicked from the <see cref="Guild"/>
        /// </summary>
        MEMBER_KICK = 20,

        /// <summary>
        /// A <see cref="GuildMember"/> was pruned from the <see cref="Guild"/>
        /// </summary>
        MEMBER_PRUNE = 21,

        /// <summary>
        /// A <see cref="GuildMember"/> was banned from the <see cref="Guild"/>
        /// </summary>
        MEMBER_BAN_ADD = 22,

        /// <summary>
        /// A <see cref="GuildMember"/> was unbanned from the <see cref="Guild"/>
        /// </summary>
        MEMBER_BAN_REMOVE = 23,

        /// <summary>
        /// A <see cref="GuildMember"/> was updated in the <see cref="Guild"/>
        /// </summary>
        MEMBER_UPDATE = 24,

        /// <summary>
        /// A <see cref="GuildMember"/> was assigned/removed roles in the <see cref="Guild"/>
        /// </summary>
        MEMBER_ROLE_UPDATE = 25,
        
        /// <summary>
        /// A <see cref="GuildMember"/> was moved between <see cref="VoiceChannel"/>s in the <see cref="Guild"/>
        /// </summary>
        MEMBER_MOVE = 26,

        /// <summary>
        /// A <see cref="GuildMember"/> was kicked/disconnected from a <see cref="VoiceChannel"/> in the <see cref="Guild"/>
        /// </summary>
        MEMBER_DISCONNECT = 27,
        
        /// <summary>
        /// A bot was added to the <see cref="Guild"/>
        /// </summary>
        BOT_ADD = 28,
        
        /// <summary>
        /// A <see cref="Role"/> was created in the <see cref="Guild"/>
        /// </summary>
        ROLE_CREATE = 30,

        /// <summary>
        /// A <see cref="Role"/> was updated in the <see cref="Guild"/>
        /// </summary>
        ROLE_UPDATE = 31,

        /// <summary>
        /// A <see cref="Role"/> was deleted in the <see cref="Guild"/>
        /// </summary>
        ROLE_DELETE = 32,

        /// <summary>
        /// An <see cref="Invite"/> was created in the <see cref="Guild"/>
        /// </summary>
        INVITE_CREATE = 40,

        /// <summary>
        /// An <see cref="Invite"/> was updated in the <see cref="Guild"/>
        /// </summary>
        INVITE_UPDATE = 41,

        /// <summary>
        /// An <see cref="Invite"/> was deleted in the <see cref="Guild"/>
        /// </summary>
        INVITE_DELETE = 42,

        /// <summary>
        /// A <see cref="Webhook"/> was created in the <see cref="Guild"/>
        /// </summary>
        WEBHOOK_CREATE = 50,

        /// <summary>
        /// A <see cref="Webhook"/> was updated in the <see cref="Guild"/>
        /// </summary>
        WEBHOOK_UPDATE = 51,

        /// <summary>
        /// A <see cref="Webhook"/> was deleted in the <see cref="Guild"/>
        /// </summary>
        WEBHOOK_DELETE = 52,

        /// <summary>
        /// An <see cref="Emoji"/> (<see cref="GuildEmoji"/>) was created in the <see cref="Guild"/>
        /// </summary>
        EMOJI_CREATE = 60,

        /// <summary>
        /// An <see cref="Emoji"/> (<see cref="GuildEmoji"/>) was updated in the <see cref="Guild"/>
        /// </summary>
        EMOJI_UPDATE = 61,

        /// <summary>
        /// An <see cref="Emoji"/> (<see cref="GuildEmoji"/>) was deleted in the <see cref="Guild"/>
        /// </summary>
        EMOJI_DELETE = 62,

        /// <summary>
        /// A <see cref="Message"/> was deleted in the <see cref="Guild"/>
        /// </summary>
        MESSAGE_DELETE = 72,

        /// <summary>
        /// Multiple <see cref="Message"/>s were deleted in the <see cref="Guild"/>
        /// </summary>
        MESSAGE_BULK_DELETE = 73,

        /// <summary>
        /// A <see cref="Message"/> was pinned in the <see cref="Guild"/>
        /// </summary>
        MESSAGE_PIN = 74,

        /// <summary>
        /// A <see cref="Message"/> was unpinned in the <see cref="Guild"/>
        /// </summary>
        MESSAGE_UNPIN = 75,

        /// <summary>
        /// An <see cref="Integration"/> was created in the <see cref="Guild"/>
        /// </summary>
        INTEGRATION_CREATE = 80,

        /// <summary>
        /// An <see cref="Integration"/> was updated in the <see cref="Guild"/>
        /// </summary>
        INTEGRATION_UPDATE = 81,

        /// <summary>
        /// An <see cref="Integration"/> was deleted in the <see cref="Guild"/>
        /// </summary>
        INTEGRATION_DELETE = 82
    }
}