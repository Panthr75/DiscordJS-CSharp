using System;

namespace DiscordJS.Resolvables
{
    /// <summary>
    /// Data that resolves to give a Guild object. This can be:
    /// <list type="bullet">
    /// <item>A Guild object</item>
    /// <item>A GuildMember object</item>
    /// <item>A GuildChannel object</item>
    /// <item>A GuildEmoji object</item>
    /// <item>A Role object</item>
    /// <item>A Snowflake</item>
    /// <item>An Invite object</item>
    /// </list>
    /// </summary>
    public class GuildResolvable
    {
        internal enum Type
        {
            Guild,
            GuildMember,
            GuildChannel,
            GuildEmoji,
            Role,
            Snowflake,
            Invite
        }

        internal readonly Type type;

        internal readonly Guild guild;
        internal readonly GuildMember member;
        internal readonly GuildChannel channel;
        internal readonly GuildEmoji emoji;
        internal readonly Role role;
        internal readonly Snowflake snowflake;
        internal readonly Invite invite;

        internal Guild Resolve(GuildManager manager)
        {
            if (type == Type.Guild)
                return guild;
            else if (type == Type.GuildChannel)
                return channel == null ? null : channel.Guild;
            else if (type == Type.GuildEmoji)
                return emoji == null ? null : emoji.Guild;
            else if (type == Type.GuildMember)
                return member == null ? null : member.Guild;
            else if (type == Type.Invite)
                return invite == null ? null : invite.Guild;
            else if (type == Type.Role)
                return role == null ? null : role.Guild;
            else if (type == Type.Snowflake)
                return manager.Cache.Get(snowflake);
            else
                throw new ArgumentException("The given type can't be cast to this resolvable");
        }

        internal Snowflake ResolveID()
        {
            if (type == Type.Guild)
                return guild == null ? null : guild.ID;
            else if (type == Type.GuildChannel)
                return channel == null || channel.Guild == null ? null : channel.Guild.ID;
            else if (type == Type.GuildEmoji)
                return emoji == null || emoji.Guild == null ? null : emoji.Guild.ID;
            else if (type == Type.GuildMember)
                return member == null || member.Guild == null ? null : member.Guild.ID;
            else if (type == Type.Invite)
                return invite == null || invite.Guild == null ? null : invite.Guild.ID;
            else if (type == Type.Role)
                return role == null || role.Guild == null ? null : role.Guild.ID;
            else if (type == Type.Snowflake)
                return snowflake;
            else
                throw new ArgumentException("The given type can't be cast to this resolvable");
        }

        public GuildResolvable(Guild guild)
        {
            this.guild = guild;
            type = Type.Guild;
        }

        public GuildResolvable(GuildMember member)
        {
            this.member = member;
            type = Type.GuildMember;
        }

        public GuildResolvable(GuildChannel channel)
        {
            this.channel = channel;
            type = Type.GuildChannel;
        }

        public GuildResolvable(GuildEmoji emoji)
        {
            this.emoji = emoji;
            type = Type.GuildEmoji;
        }

        public GuildResolvable(Role role)
        {
            this.role = role;
            type = Type.Role;
        }

        public GuildResolvable(Snowflake snowflake)
        {
            this.snowflake = snowflake;
            type = Type.Snowflake;
        }

        public GuildResolvable(string snowflake) : this((Snowflake)snowflake)
        { }

        public GuildResolvable(JavaScript.String snowflake) : this((Snowflake)snowflake.ToString())
        { }

        public GuildResolvable(Invite invite)
        {
            this.invite = invite;
            type = Type.Invite;
        }
    }
}
