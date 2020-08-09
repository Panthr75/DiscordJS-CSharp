using DiscordJS.Data;
using System;

namespace DiscordJS.Resolvables
{
    /// <summary>
    /// Data that can be resolved into <see cref="PermissionOverwriteData"/>.
    /// </summary>
    public class OverwriteResolvable : IResolvable<Guild, PermissionOverwriteData>
    {
        internal bool isData = false;
        internal PermissionOverwrites overwrite;
        internal PermissionOverwriteData overwriteData;

        internal PermissionOverwriteData Resolve(Guild guild)
        {
            if (isData)
            {
                if (overwriteData.type == "role" || overwriteData.type == "member")
                {
                    return new PermissionOverwriteData()
                    {
                        allow = overwriteData.allow.HasValue ? (long?)new PermissionResolvable(overwriteData.allow.Value).Resolve(typeof(Permissions.FLAGS)) : null,
                        deny = overwriteData.deny.HasValue ? (long?)new PermissionResolvable(overwriteData.deny.Value).Resolve(typeof(Permissions.FLAGS)) : null,
                        id = overwriteData.id,
                        type = overwriteData.type
                    };
                }
                string type;
                if (guild.Roles.Resolve(overwriteData.id) != null) type = "role";
                else if (guild.Client.Users.Resolve(overwriteData.id) != null) type = "member";
                else throw new ArgumentException("The given overwrite data does is not a User nor a Role");

                return new PermissionOverwriteData()
                {
                    id = overwriteData.id,
                    type = type,
                    allow = overwriteData.allow.HasValue ? (long?)new PermissionResolvable(overwriteData.allow.Value).Resolve(typeof(Permissions.FLAGS)) : null,
                    deny = overwriteData.deny.HasValue ? (long?)new PermissionResolvable(overwriteData.deny.Value).Resolve(typeof(Permissions.FLAGS)) : null
                };
            }
            else
            {
                return new PermissionOverwriteData()
                {
                    allow = overwrite.Allow.Bit,
                    deny = overwrite.Deny.Bit,
                    id = overwrite.ID,
                    type = overwrite.Type
                };
            }
        }

        PermissionOverwriteData IResolvable<Guild, PermissionOverwriteData>.Resolve(Guild arg1) => Resolve(arg1);

        public OverwriteResolvable(PermissionOverwriteData data)
        {
            isData = true;
            overwriteData = data;
        }

        public OverwriteResolvable(PermissionOverwrites overwrites)
        {
            isData = false;
            overwrite = overwrites;
        }

        public static implicit operator OverwriteResolvable(PermissionOverwriteData data) => new OverwriteResolvable(data);
        public static implicit operator OverwriteResolvable(PermissionOverwrites overwrites) => new OverwriteResolvable(overwrites);
    }
}