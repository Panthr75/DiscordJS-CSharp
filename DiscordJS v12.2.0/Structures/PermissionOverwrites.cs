using DiscordJS.Data;
using DiscordJS.Resolvables;
using JavaScript;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiscordJS
{
    /// <summary>
    /// Represents a permission overwrite for a role or member in a guild channel.
    /// </summary>
    public class PermissionOverwrites : IHasID, IJSONConvertable
    {
        /// <summary>
        /// The permissions that are allowed for the user or role.
        /// </summary>
        public Permissions Allow { get; internal set; }

        /// <summary>
        /// The GuildChannel this overwrite is for
        /// </summary>
        public GuildChannel Channel { get; }

        /// <summary>
        /// The permissions that are denied for the user or role.
        /// </summary>
        public Permissions Deny { get; internal set; }

        /// <summary>
        /// The ID of this overwrite, either a user ID or a role ID
        /// </summary>
        public Snowflake ID { get; internal set; }

        /// <summary>
        /// The type of this overwrite. It can be one of:
        /// <list type="bullet">
        /// <item>member</item>
        /// <item>role</item>
        /// </list>
        /// </summary>
        public string Type { get; internal set; }

        public PermissionOverwrites(GuildChannel channel, PermissionOverwriteData data)
        {
            Channel = channel;
            if (data != null) _Patch(data);
        }

        internal void _Patch(PermissionOverwriteData data)
        {
            ID = data.id;
            Type = data.type;
            Deny = new Permissions(data.deny.HasValue ? data.deny.Value : 0L).Freeze();
            Allow = new Permissions(data.allow.HasValue ? data.allow.Value : 0L).Freeze();
        }

        /// <summary>
        /// Converts this object to JSON string representation of <see cref="PermissionOverwriteData"/>.
        /// </summary>
        /// <returns></returns>
        public string ToJSON()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter();
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.None;

                Serialize(writer);
            }
            return sb.ToString();
        }

        internal PermissionOverwriteData ToData() => new PermissionOverwriteData() { allow = Allow.Bit, deny = Deny.Bit, id = ID, type = Type };

        /// <summary>
        /// Adds the JSON serialization of this object to the given <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer"></param>
        public void Serialize(JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("allow");
            if (Allow is null) writer.WriteNull();
            else writer.WriteValue(Allow.Bit);

            writer.WritePropertyName("deny");
            if (Deny is null) writer.WriteNull();
            else writer.WriteValue(Deny.Bit);

            writer.WritePropertyName("id");
            writer.WriteValue(ID);

            writer.WritePropertyName("type");
            writer.WriteValue(Type);

            writer.WriteEndObject();
        }

        /// <summary>
        /// Deletes this Permission Overwrite.
        /// </summary>
        /// <param name="reason">Reason for deleting this overwrite</param>
        /// <returns></returns>
        public IPromise<PermissionOverwrites> Delete(string reason = null) => Channel.Client.API.Channels(Channel.ID).Permissions(ID).Delete(new { reason }).Then((_) => this);

        /// <summary>
        /// Updates this permissionOverwrites.
        /// </summary>
        /// <param name="options">The options for the update</param>
        /// <param name="reason">Reason for creating/editing this overwrite</param>
        /// <returns></returns>
        public IPromise<PermissionOverwrites> Update(Dictionary<string, bool?> options, string reason = null)
        {
            var resolvedOptions = ResolveOverwriteOptions(options, Allow, Deny);
            Permissions allow = resolvedOptions.Allow, deny = resolvedOptions.Deny;

            return Channel.Client.API.Channels(Channel.ID).Permissions(ID).Put(new
            {
                data = new { id = ID, type = Type, allow = allow.Bit, deny = deny.Bit },
                reason
            }).Then((_) => this);
        }

        /// <summary>
        /// Resolves an overwrite into <see cref="PermissionOverwriteData"/>.
        /// </summary>
        /// <param name="overwrite">The overwrite-like data to resolve</param>
        /// <param name="guild">The guild to resolve from</param>
        /// <returns></returns>
        public static PermissionOverwriteData Resolve(OverwriteResolvable overwrite, Guild guild) => overwrite.Resolve(guild);

        /// <summary>
        /// Resolves bitfield permissions overwrites from the given <paramref name="allow"/> and <paramref name="deny"/> values.
        /// </summary>
        /// <param name="options">The options for the update</param>
        /// <param name="allow">Initial allowed permissions</param>
        /// <param name="deny">Initial denied permissions</param>
        /// <returns></returns>
        public static ResolvedOverwriteOptions ResolveOverwriteOptions(Dictionary<string, bool?> options, PermissionResolvable allow, PermissionResolvable deny)
        {
            Permissions yes = new Permissions(allow == null ? 0 : allow.Resolve(typeof(Permissions.FLAGS)));
            Permissions no = new Permissions(deny == null ? 0 : deny.Resolve(typeof(Permissions.FLAGS)));

            foreach (string perm in options.Keys)
            {
                bool? value = options[perm];
                if (Enum.TryParse(perm, out Permissions.FLAGS flag))
                {
                    if (value.HasValue)
                    {
                        if (value.Value == true)
                        {
                            yes.Add(flag);
                            no.Remove(flag);
                        }
                        else
                        {
                            yes.Remove(flag);
                            no.Add(flag);
                        }
                    }
                    else
                    {
                        yes.Remove(flag);
                        no.Remove(flag);
                    }
                }
            }

            return new ResolvedOverwriteOptions(yes, no);
        }
    }

    public class ResolvedOverwriteOptions
    {
        /// <summary>
        /// The allowed permissions
        /// </summary>
        public Permissions Allow { get; }

        /// <summary>
        /// The denied permissions
        /// </summary>
        public Permissions Deny { get; }

        public ResolvedOverwriteOptions(Permissions allow, Permissions deny)
        {
            Allow = allow;
            Deny = deny;
        }
    }
}