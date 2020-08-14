using DiscordJS.Data;
using JavaScript;
using System;
using System.Collections.Generic;

namespace DiscordJS
{
    /// <summary>
    /// Audit logs entry.
    /// </summary>
    public class GuildAuditLogsEntry : IHasID
    {
        /// <summary>
        /// Specific action type of this entry in its enum presentation
        /// </summary>
        public AuditLogAction Action { get; internal set; }

        /// <summary>
        /// The action type of this entry
        /// </summary>
        public AuditLogActionType ActionType { get; internal set; }

        /// <summary>
        /// Specific property changes
        /// </summary>
        public Array<GuildAuditLogChange> Changes { get; internal set; }

        /// <summary>
        /// The time this entry was created at
        /// </summary>
        public Date CreatedAt => Snowflake.Deconstruct(ID).Date;

        /// <summary>
        /// The timestamp this entry was created at
        /// </summary>
        public long CreatedTimestamp => Snowflake.Deconstruct(ID).Timestamp;

        /// <summary>
        /// The user that executed this entry
        /// </summary>
        public User Executor { get; internal set; }

        /// <summary>
        /// Any extra data from the entry
        /// <br/>
        /// <br/>
        /// <see cref="object"/> or <see cref="Role"/> or <see cref="GuildMember"/>
        /// </summary>
        public dynamic Extra { get; internal set; }

        /// <summary>
        /// The ID of this entry
        /// </summary>
        public Snowflake ID { get; internal set; }

        /// <summary>
        /// The reason of this entry
        /// </summary>
        public string Reason { get; internal set; }

        /// <summary>
        /// The target of this entry
        /// </summary>
        public AuditLogEntryTarget Target { get; internal set; }

        /// <summary>
        /// The target type of this entry
        /// </summary>
        public AuditLogTargetType TargetType { get; internal set; }

        /// <summary>
        /// Instantiates a new GuildAuditLogsEntry
        /// </summary>
        /// <param name="logs">The instantiating logs</param>
        /// <param name="guild">The guild these logs are for</param>
        /// <param name="data">The data</param>
        public GuildAuditLogsEntry(GuildAuditLogs logs, Guild guild, AuditLogEntryData data)
        {
            TargetType = GuildAuditLogs.TargetType(data.action_type);
            ActionType = GuildAuditLogs.ActionType(data.action_type);
            Action = (AuditLogAction)data.action_type;
            Reason = data.reason;
            Executor = guild.Client.Options.partials.Includes(PartialType.USER) ?
                guild.Client.Users.Add(new UserData() { id = data.user_id }) :
                guild.Client.Users.Cache.Get(data.user_id);
            if (data.changes != null)
            {
                Changes = new Array<AuditLogChangeData>(data.changes).Map((c) => new GuildAuditLogChange()
                {
                    Key = c.key,
                    Old = c.old_value,
                    New = c.new_value
                });
            }
            else
                Changes = null;
            ID = data.id;
            Extra = null;
            switch (Action)
            {
                case AuditLogAction.MEMBER_PRUNE:
                    Extra = new
                    {
                        Removed = data.options.members_removed == null ? null : (int?)int.Parse(data.options.members_removed),
                        Days = data.options.delete_member_days == null ? null : (int?)int.Parse(data.options.delete_member_days)
                    };
                    break;

                case AuditLogAction.MEMBER_MOVE:
                case AuditLogAction.MESSAGE_DELETE:
                case AuditLogAction.MESSAGE_BULK_DELETE:
                    var chn = guild.Channels.Cache.Get(data.options.channel_id);
                    Extra = new
                    {
                        Channel = chn == null ? new GuildChannel(guild, new ChannelData() { id = data.options.channel_id }) : chn,
                        Count = data.options.count == null ? null : (int?)int.Parse(data.options.count)
                    };
                    break;

                case AuditLogAction.MESSAGE_PIN:
                case AuditLogAction.MESSAGE_UNPIN:
                    var msgchn = guild.Channels.Cache.Get(data.options.channel_id);
                    Extra = new
                    {
                        Channel = msgchn == null ? new GuildChannel(guild, new ChannelData() { id = data.options.channel_id }) : msgchn,
                        MessageID = data.options.message_id
                    };
                    break;

                case AuditLogAction.MEMBER_DISCONNECT:
                    Extra = new
                    {
                        Count = data.options.count == null ? null : (int?)int.Parse(data.options.count)
                    };
                    break;

                case AuditLogAction.CHANNEL_OVERWRITE_CREATE:
                case AuditLogAction.CHANNEL_OVERWRITE_UPDATE:
                case AuditLogAction.CHANNEL_OVERWRITE_DELETE:
                    switch (data.options.type)
                    {
                        case "member":
                            var member = guild.Members.Cache.Get(data.options.id);
                            if (member == null)
                            {
                                Extra = new
                                {
                                    ID = data.options.id,
                                    Type = "member"
                                };
                            }
                            else Extra = member;
                            break;

                        case "role":
                            var role = guild.Roles.Cache.Get(data.options.id);
                            if (role == null)
                            {
                                Extra = new
                                {
                                    ID = data.options.id,
                                    Name = data.options.role_name,
                                    Type = "role"
                                };
                            }
                            break;

                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }

            Target = null;
            if (TargetType == AuditLogTargetType.UNKNOWN)
            {
                Target = Changes.Reduce((o, c) =>
                {
                    o[c.Key] = c.New == null ? c.Old : c.New;
                    return o;
                }, new Dictionary<string, dynamic>());
                Target.ID = data.target_id;
            }
            else if (TargetType == AuditLogTargetType.USER && data.target_id != null)
            {
                Target = guild.Client.Options.partials.Includes(PartialType.USER) ? guild.Client.Users.Add(new UserData() { id = data.target_id }) : guild.Client.Users.Cache.Get(data.target_id);
            }
            else if (TargetType == AuditLogTargetType.GUILD)
            {
                Target = guild.Client.Guilds.Cache.Get(data.target_id);
            }
            else if (TargetType == AuditLogTargetType.WEBHOOK)
            {
                var hook = logs.Webhooks.Get(data.target_id);
                Target = hook == null ? new Webhook(guild.Client, (WebhookData)Changes.Reduce((o, c) =>
                {
                    o[c.Key] = c.New == null ? c.Old : c.New;
                    return o;
                }, new Dictionary<string, dynamic>()
                {
                    ["id"] = data.target_id,
                    ["guild_id"] = (string)guild.ID
                })) : hook;
            }
            else if (TargetType == AuditLogTargetType.INVITE)
            {
                guild.Members.Fetch(guild.Client.User.ID).Then((me) =>
                {
                    if (me.Permissions.Has("MANAGE_GUILD"))
                    {
                        var change = Changes.Find((c) => c.Key == "code");
                        return guild.FetchInvites().Then((invites) =>
                        {
                            var c = change == null ? null : (change.New == null ? change.Old : change.New);
                            Target = invites.Find((i) => i.Code == c.ToString());
                        });
                    }
                    else
                    {
                        Target = Changes.Reduce((o, c) =>
                        {
                            o[c.Key] = c.New == null ? c.Old : c.New;
                            return o;
                        }, new Dictionary<string, dynamic>());
                    }
                    return null;
                });
            }
            else if (TargetType == AuditLogTargetType.MESSAGE)
            {
                // Discord sends a channel id for the MESSAGE_BULK_DELETE action type.
                if (Action == AuditLogAction.MESSAGE_BULK_DELETE)
                {
                    Target = guild.Channels.Cache.Get(data.target_id);
                    if (Target == null) Target = AuditLogEntryTarget.FromDynamic(new
                    {
                        ID = data.target_id
                    });
                }
                else
                {
                    Target = guild.Client.Users.Cache.Get(data.target_id);
                }
            }
            else if (TargetType == AuditLogTargetType.INTEGRATION)
            {
                Target = logs.Integrations.Get(data.target_id);
                if (Target == null)
                {
                    Target = new Integration(guild.Client, Changes.Reduce((o, c) =>
                    {
                        o[c.Key] = c.New == null ? c.Old : c.New;
                        return o;
                    }, new Dictionary<string, dynamic>()), guild);
                }
            }
            else if (data.target_id != null)
            {
                var t = guild.GetType();
                var n = (JavaScript.String)TargetType.ToString();
                var field = t.GetField(n.CharAt(0).ToUpperCase() + n.Slice(1).ToLowerCase());
                if (field != null)
                {
                    dynamic val = field.GetValue(guild);
                    Target = val.Cache.Get(data.target_id);
                }
                if (Target == null)
                {
                    Target = AuditLogEntryTarget.FromDynamic(new
                    {
                        ID = (Snowflake)data.target_id
                    });
                }
            }
        }
    }
}
