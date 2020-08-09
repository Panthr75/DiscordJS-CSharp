using DiscordJS.Resolvables;
using JavaScript;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Data structure that makes it easy to interact with a permission bitfield. All <see cref="GuildMember"/>s have a set of<br/>
    /// permissions in their guild, and each channel in the guild may also have <see cref="PermissionOverwrites"/> for the member<br/>
    /// that override their default permissions.
    /// </summary>
    public class Permissions : BitField
    {
        protected override Type FlagsType => typeof(FLAGS);

        /// <summary>
        /// Creates a new Permissions BitField with the default value of 0.
        /// </summary>
        public Permissions() : this(0)
        { }

        /// <summary>
        /// Creates a new Permissions BitField.
        /// </summary>
        /// <param name="bits">Bit(s) to read from</param>
        public Permissions(PermissionResolvable bits) : base(bits)
        { }

        /// <summary>
        /// Numeric permission flags. All available properties:
        /// <list type="bullet">
        /// <item><b>ADMINISTRATOR</b> (implicitly has <i>all</i> permissions, and bypasses all channel overwrites)</item>
        /// <item><b>CREATE_INSTANT_INVITE</b> (create invitations to the guild)</item>
        /// <item><b>KICK_MEMBERS</b></item>
        /// <item><b>BAN_MEMBERS</b></item>
        /// <item><b>MANAGE_CHANNELS</b> (edit and reorder channels)</item>
        /// <item><b>MANAGE_GUILD</b> (edit the guild information, region, etc.)</item>
        /// <item><b>ADD_REACTIONS</b> (add new reactions to messages)</item>
        /// <item><b>VIEW_AUDIT_LOG</b></item>
        /// <item><b>PRIORITY_SPEAKER</b></item>
        /// <item><b>STREAM</b></item>
        /// <item><b>VIEW_CHANNEL</b></item>
        /// <item><b>SEND_MESSAGES</b></item>
        /// <item><b>SEND_TTS_MESSAGES</b></item>
        /// <item><b>MANAGE_MESSAGES</b> (delete messages and reactions)</item>
        /// <item><b>EMBED_LINKS</b> (links posted will have a preview embedded)</item>
        /// <item><b>ATTACH_FILES</b></item>
        /// <item><b>READ_MESSAGE_HISTORY</b> (view messages that were posted prior to opening Discord)</item>
        /// <item><b>MENTION_EVERYONE</b></item>
        /// <item><b>USE_EXTERNAL_EMOJIS</b> (use emojis from different guilds)</item>
        /// <item><b>VIEW_GUILD_INSIGHTS</b></item>
        /// <item><b>CONNECT</b> (connect to a voice channel)</item>
        /// <item><b>SPEAK</b> (speak in a voice channel)</item>
        /// <item><b>MUTE_MEMBERS</b> (mute members across all voice channels)</item>
        /// <item><b>DEAFEN_MEMBERS</b> (deafen members across all voice channels)</item>
        /// <item><b>MOVE_MEMBERS</b> (move members between voice channels)</item>
        /// <item><b>USE_VAD</b> (use voice activity detection)</item>
        /// <item><b>CHANGE_NICKNAME</b></item>
        /// <item><b>MANAGE_NICKNAMES</b> (change other members' nicknames)</item>
        /// <item><b>MANAGE_ROLES</b></item>
        /// <item><b>MANAGE_WEBHOOKS</b></item>
        /// <item><b>MANAGE_EMOJIS</b></item>
        /// </list>
        /// <b>See: <see href="https://discord.com/developers/docs/topics/permissions"/></b>
        /// </summary>
        public enum FLAGS : long
        {
            CREATE_INSTANT_INVITE = 1L << 0,
            KICK_MEMBERS = 1L << 1,
            BAN_MEMBERS = 1L << 2,
            ADMINISTRATOR = 1L << 3,
            MANAGE_CHANNELS = 1L << 4,
            MANAGE_GUILD = 1L << 5,
            ADD_REACTIONS = 1L << 6,
            VIEW_AUDIT_LOG = 1L << 7,
            PRIORITY_SPEAKER = 1L << 8,
            STREAM = 1L << 9,
            VIEW_CHANNEL = 1L << 10,
            SEND_MESSAGES = 1L << 11,
            SEND_TTS_MESSAGES = 1L << 12,
            MANAGE_MESSAGES = 1L << 13,
            EMBED_LINKS = 1L << 14,
            ATTACH_FILES = 1L << 15,
            READ_MESSAGE_HISTORY = 1L << 16,
            MENTION_EVERYONE = 1L << 17,
            USE_EXTERNAL_EMOJIS = 1L << 18,
            VIEW_GUILD_INSIGHTS = 1L << 19,
            CONNECT = 1L << 20,
            SPEAK = 1L << 21,
            MUTE_MEMBERS = 1L << 22,
            DEAFEN_MEMBERS = 1L << 23,
            MOVE_MEMBERS = 1L << 24,
            USE_VAD = 1L << 25,
            CHANGE_NICKNAME = 1L << 26,
            MANAGE_NICKNAMES = 1L << 27,
            MANAGE_ROLES = 1L << 28,
            MANAGE_WEBHOOKS = 1L << 29,
            MANAGE_EMOJIS = 1L << 30
        }

        /// <summary>
        /// Checks whether the bitfield has a permission, or any of multiple permissions.
        /// </summary>
        /// <param name="permission">Permission(s) to check for</param>
        /// <param name="checkAdmin">Whether to allow the administrator permission to override</param>
        /// <returns></returns>
        public bool Any(PermissionResolvable permission, bool checkAdmin = true)
            => (checkAdmin && base.Has(FLAGS.ADMINISTRATOR)) || base.Any(permission);

        /// <summary>
        /// Checks whether the bitfield has a permission, or multiple permissions.
        /// </summary>
        /// <param name="permission">Permission(s) to check for</param>
        /// <param name="checkAdmin">Whether to allow the administrator permission to override</param>
        /// <returns></returns>
        public bool Has(PermissionResolvable permission, bool checkAdmin = true)
            => (checkAdmin && base.Has(FLAGS.ADMINISTRATOR)) || base.Has(permission);

        /// <summary>
        /// Freezes these bits, making them immutable.
        /// </summary>
        /// <returns>These bits</returns>
        public new Permissions Freeze()
        {
            base.Freeze();
            return this;
        }

        /// <summary>
        /// Bitfield representing every permission combined
        /// </summary>
        public static long ALL { get; }

        /// <summary>
        /// Bitfield representing the default permissions for users
        /// </summary>
        public static long DEFAULT { get; }

        static Permissions()
        {
            ALL = new Array<long>(Enum.GetValues(typeof(FLAGS)) as long[]).Reduce((all, p) => all | p, 0L);
            DEFAULT = 104324673;
        }
    }
}
