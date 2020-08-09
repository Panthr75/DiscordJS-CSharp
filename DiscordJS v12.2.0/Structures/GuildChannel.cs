using DiscordJS.Data;
using DiscordJS.Resolvables;
using JavaScript;
using System.Collections.Generic;

namespace DiscordJS
{
    /// <summary>
    /// Represents a guild channel from any of the following:
    /// <list type="bullet">
    /// <item><see cref="TextChannel"/></item>
    /// <item><see cref="VoiceChannel"/></item>
    /// <item><see cref="CategoryChannel"/></item>
    /// <item><see cref="NewsChannel"/></item>
    /// <item><see cref="StoreChannel"/></item>
    /// </list>
    /// </summary>
    public class GuildChannel : Channel
    {
        /// <summary>
        /// Whether the channel is deletable by the client user
        /// </summary>
        public virtual bool Deletable => PermissionsFor(Client.User).Has(Permissions.FLAGS.MANAGE_CHANNELS, false);

        /// <summary>
        /// The guild the channel is in
        /// </summary>
        public Guild Guild { get; internal set; }

        /// <summary>
        /// Whether the channel is manageable by the client user
        /// </summary>
        public bool Manageable
        {
            get
            {
                if (Client.User.ID == Guild.OwnerID) return true;
                if (Type == "voice")
                {
                    if (!PermissionsFor(Client.User).Has(Permissions.FLAGS.CONNECT, false)) return false;
                }
                else if (!Viewable) return false;
                return PermissionsFor(Client.User).Has(Permissions.FLAGS.MANAGE_CHANNELS, false);
            }
        }

        /// <summary>
        /// A collection of members that can see this channel, mapped by their ID
        /// </summary>
        public virtual Collection<Snowflake, GuildMember> Members
        {
            get
            {
                var members = new Collection<Snowflake, GuildMember>();
                foreach (GuildMember member in Guild.Members.Cache.Values())
                {
                    if (PermissionsFor(member).Has(Permissions.FLAGS.VIEW_CHANNEL, false))
                    {
                        members.Set(member.ID, member);
                    }
                }
                return members;
            }
        }

        /// <summary>
        /// The name of the guild channel
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The category parent of this channel
        /// </summary>
        public CategoryChannel Parent
        {
            get
            {
                var chn = Guild.Channels.Cache.Get(ParentID);
                if (chn == null || chn.Type != "category") return null;
                return (CategoryChannel)chn;
            }
        }

        /// <summary>
        /// The ID of the category parent of this channel
        /// </summary>
        public Snowflake ParentID { get; internal set; }

        /// <summary>
        /// A map of permission overwrites in this channel for roles and users
        /// </summary>
        public Collection<Snowflake, PermissionOverwrites> PermissionOverwrites { get; internal set; }

        /// <summary>
        /// If the permissionOverwrites match the parent channel, null if no parent
        /// </summary>
        public bool? PermissionsLocked
        {
            get
            {
                var parent = Parent;
                if (parent == null) return null;
                if (PermissionOverwrites.Size != Parent.PermissionOverwrites.Size) return false;
                return PermissionOverwrites.Every((value, key) =>
                {
                    var testVal = parent.PermissionOverwrites.Get(key);
                    return testVal != null && testVal.Deny.Bit == value.Deny.Bit && testVal.Allow.Bit == value.Allow.Bit;
                });
            }
        }

        /// <summary>
        /// The position of the channel
        /// </summary>
        public int Position
        {
            get
            {
                Collection<Snowflake, GuildChannel> sorted = Guild._SortedChannels(this);
                return sorted.Array().IndexOf(sorted.Get(ID));
            }
        }

        /// <summary>
        /// The raw position of the channel from discord
        /// </summary>
        public int RawPosition { get; internal set; }

        /// <summary>
        /// Whether the channel is viewable by the client user
        /// </summary>
        public bool Viewable
        {
            get
            {
                if (Client.User.ID == Guild.OwnerID) return true;
                Permissions permissions = PermissionsFor(Client.User);
                if (permissions == null) return false;
                return permissions.Has(Permissions.FLAGS.VIEW_CHANNEL, false);
            }
        }

        /// <summary>
        /// Creates a new Guild Channel Instance
        /// </summary>
        /// <param name="guild">The guild the guild channel is part of</param>
        /// <param name="data">The data for the guild channel</param>
        public GuildChannel(Guild guild, ChannelData data) : base(guild.Client, data)
        { }

        /// <summary>
        /// Creates a new Guild Channel Instance
        /// </summary>
        /// <param name="guild">The guild the guild channel is part of</param>
        /// <param name="data">The data for the guild channel</param>
        internal GuildChannel(Guild guild, GuildChannel data) : base(guild.Client, data)
        { }

        internal override void _Patch(ChannelData data)
        {
            base._Patch(data);
            Name = data.name;
            RawPosition = data.position.Value;
            ParentID = data.parent_id;
            PermissionOverwrites = new Collection<Snowflake, PermissionOverwrites>();
            if (data.permission_overwrites != null)
            {
                for (int index = 0, length = data.permission_overwrites.Length; index < length; index++)
                {
                    var overwrite = data.permission_overwrites[index];
                    PermissionOverwrites.Set(overwrite.id, new PermissionOverwrites(this, overwrite));
                }
            }
        }

        internal virtual void _Patch(GuildChannel channel)
        {
            base._Patch(channel);
            Name = data.name;
            RawPosition = data.position.Value;
            ParentID = data.parent_id;
            PermissionOverwrites = new Collection<Snowflake, PermissionOverwrites>();
            if (data.permission_overwrites != null)
            {
                for (int index = 0, length = data.permission_overwrites.Length; index < length; index++)
                {
                    var overwrite = data.permission_overwrites[index];
                    PermissionOverwrites.Set(overwrite.id, new PermissionOverwrites(this, overwrite));
                }
            }
        }

        internal override void _Patch(Channel channel)
        {
            if (channel is GuildChannel guildChannel)
                _Patch(guildChannel);
            else 
                base._Patch(channel);
        }

        internal new GuildChannel _Clone()
        {
            return MemberwiseClone() as GuildChannel;
        }

        /// <summary>
        /// Clones this channel.
        /// </summary>
        /// <param name="options">The options</param>
        /// <returns></returns>
        public IPromise<GuildChannel> Clone(GuildChannelCloneOptions options = null)
        {
            if (options == null) options = new GuildChannelCloneOptions();
            if (options.name == null) options.name = Name;
            if (options.permissionOverwrites == null)
            {
                options.permissionOverwrites = new OverwriteResolvable[PermissionOverwrites.Size];
                int index = 0;
                foreach (PermissionOverwrites overwrite in PermissionOverwrites.Values())
                {
                    options.permissionOverwrites[index] = overwrite;
                    index++;
                }
            }
            if (options.topic == null)
            {
                if (Type == "text") options.topic = ((TextChannel)this).Topic;
                else if (Type == "news") options.topic = ((NewsChannel)this).Topic;
            }
            if (options.type == null) options.type = Type;
            if (!options.nsfw.HasValue)
            {
                if (Type == "text") options.nsfw = ((TextChannel)this).NSFW;
                else if (Type == "news") options.nsfw = ((NewsChannel)this).NSFW;
            }
            if (options.parent == null) options.parent = Parent;
            if (!options.bitrate.HasValue)
            {
                if (Type == "voice") options.bitrate = ((VoiceChannel)this).Bitrate;
            }
            if (!options.userLimit.HasValue)
            {
                if (Type == "voice") options.userLimit = ((VoiceChannel)this).UserLimit;
            }
            if (!options.rateLimitPerUser.HasValue)
            {
                if (Type == "voice") options.rateLimitPerUser = ((VoiceChannel)this).RateLimitPerUser;
            }
            options.reason = null;
            return Guild.Channels.Create(options.name, options);
        }

        /// <summary>
        /// Creates an invite to this guild channel.
        /// </summary>
        /// <param name="reason">Reason for creating this</param>
        /// <param name="temporary">Whether members that joined via the invite should be automatically<br/>
        /// kicked after 24 hours if they have not yet received a role</param>
        /// <param name="maxAge">How long the invite should last (in seconds, 0 for forever)</param>
        /// <param name="maxUses">Maximum number of uses</param>
        /// <param name="unique">Create a unique invite, or use an existing one with similar settings</param>
        /// <returns></returns>
        public IPromise<Invite> CreateInvite(string reason = null, bool temporary = false, int maxAge = 86400, int maxUses = 0, bool unique = false)
        {
            return Client.API.Channels(ID).Invites.Post(new
            {
                data = new
                {
                    temporary,
                    max_age = maxAge,
                    max_uses = maxUses,
                    unique
                },
                reason
            }).Then((invite) => new Invite(Client, invite));
        }

        /// <summary>
        /// Overwrites the permissions for a user or role in this channel. (replaces if existent)
        /// </summary>
        /// <param name="userOrRole">The user or role to update</param>
        /// <param name="options">The options for the update (dict where key is the name of a permission, and the value is a nullable <see cref="bool"/>:<br/><see langword="true"/> - override that this user/role has this permission<br/><see langword="false"/> - override that this user/role doesn't have this permission<br/><see langword="null"/> - inherit permission)</param>
        /// <param name="reason">Reason for creating/editing this overwrite</param>
        /// <returns></returns>
        public IPromise<GuildChannel> CreateOverwrite(UserResolvable userOrRole, Dictionary<string, bool?> options, string reason = null)
        {
            User user = Client.Users.Resolve(userOrRole);
            if (user == null) return Promise<GuildChannel>.Rejected(new DJSError.Error("INVALID_TYPE", "parameter", "User nor a Role", true));

            var resolvedOverwrites = DiscordJS.PermissionOverwrites.ResolveOverwriteOptions(options, 0, 0);
            var allow = resolvedOverwrites.Allow;
            var deny = resolvedOverwrites.Deny;

            return Client.API.Channels(ID).Permissions(user.ID).Put(new
            {
                data = new { id = user.ID.ToString(), type = "user", allow = allow.Bit, deny = deny.Bit },
                reason
            }).Then((_) => this);
        }

        /// <summary>
        /// Overwrites the permissions for a user or role in this channel. (replaces if existent)
        /// </summary>
        /// <param name="userOrRole">The user or role to update</param>
        /// <param name="options">The options for the update (dict where key is the name of a permission, and the value is a nullable <see cref="bool"/>:<br/><see langword="true"/> - override that this user/role has this permission<br/><see langword="false"/> - override that this user/role doesn't have this permission<br/><see langword="null"/> - inherit permission)</param>
        /// <param name="reason">Reason for creating/editing this overwrite</param>
        /// <returns></returns>
        public IPromise<GuildChannel> CreateOverwrite(RoleResolvable userOrRole, Dictionary<string, bool?> options, string reason = null)
        {
            Role role = Guild.Roles.Resolve(userOrRole);
            if (role == null) return Promise<GuildChannel>.Rejected(new DJSError.Error("INVALID_TYPE", "parameter", "User nor a Role", true));

            var resolvedOverwrites = DiscordJS.PermissionOverwrites.ResolveOverwriteOptions(options, 0, 0);
            var allow = resolvedOverwrites.Allow;
            var deny = resolvedOverwrites.Deny;

            return Client.API.Channels(ID).Permissions(role.ID).Put(new
            {
                data = new { id = role.ID.ToString(), type = "user", allow = allow.Bit, deny = deny.Bit },
                reason
            }).Then((_) => this);
        }

        /// <summary>
        /// Deletes this channel.
        /// </summary>
        /// <param name="reason">Reason for deleting this channel</param>
        /// <returns></returns>
        public IPromise<GuildChannel> Delete(string reason = null) => Client.API.Channels(ID).Delete(new { reason }).Then((_) => this);

        /// <summary>
        /// Edits the channel.
        /// </summary>
        /// <param name="data">The new data for the channel</param>
        /// <param name="reason">Reason for editing this channel</param>
        /// <returns></returns>
        public IPromise<GuildChannel> Edit(GuildChannelEditData data, string reason = null)
        {
            IPromise<object> promise;
            if (data.position.HasValue)
            {
                promise = DiscordUtil.SetPosition(this, data.position.Value, false, Guild._SortedChannels(this), Client.API.Guilds(Guild.ID).Channels(), reason).Then((updatedChannels) =>
                {
                    Client.Actions.GuildChannelsPositionUpdate.Handle(Guild.ID, updatedChannels);
                    return null;
                });
            }
            else promise = Promise<object>.Resolved(null);
            return promise.Then((_) =>
            {
                var permissionOverwritesArray = data.permissionOverwrites == null ? null : new Array<OverwriteResolvable>();
                if (data.permissionOverwrites != null) permissionOverwritesArray.Push(data.permissionOverwrites);
                var permissionOverwrites = permissionOverwritesArray == null ? null : permissionOverwritesArray.Map((o) => DiscordJS.PermissionOverwrites.Resolve(o, Guild)).ToArray();

                return Client.API.Channels(ID).Patch(new
                {
                    data = new ChannelData()
                    {
                        name = (data.name == null ? Name : data.name).Trim(),
                        topic = data.topic,
                        nsfw = data.nsfw,
                        bitrate = data.bitrate,
                        user_limit = data.userLimit.HasValue ? data.userLimit.Value : (Type == "voice" ? (int?)((VoiceChannel)this).UserLimit : null),
                        parent_id = data.parentID,
                        lock_permissions = data.lockPermissions,
                        rate_limit_per_user = data.rateLimitPerUser,
                        permission_overwrites = permissionOverwrites
                    }
                }).Then((newData) =>
                {
                    var clone = _Clone();
                    clone._Patch(newData);
                    return clone;
                });
            });
        }

        /// <summary>
        /// Checks if this channel has the same type, topic, position, name, overwrites and ID as another channel.<br/>
        /// In most cases, a simple <c>channel.ID == channel2.ID</c> will do, and is much faster too.
        /// </summary>
        /// <param name="channel">Channel to compare with</param>
        /// <returns></returns>
        public bool Equals(GuildChannel channel)
        {
            bool equal = channel != null && ID == channel.ID && Type == channel.Type && Position == channel.Position && Name == channel.Name;
            if (equal && Type == "news") equal = ((NewsChannel)this).Topic == ((NewsChannel)channel).Topic;
            else if (equal && Type == "text") equal = ((TextChannel)this).Topic == ((TextChannel)channel).Topic;

            if (equal)
            {
                if (PermissionOverwrites != null && channel.PermissionOverwrites != null)
                    equal = PermissionOverwrites.Equals(channel.PermissionOverwrites);
                else equal = PermissionOverwrites == null && channel.PermissionOverwrites == null;
            }

            return equal;
        }

        /// <summary>
        /// Fetches a collection of invites to this guild channel.<br/>
        /// Resolves with a collection mapping invites by their codes.
        /// </summary>
        /// <returns></returns>
        public IPromise<Collection<string, Invite>> FetchInvites()
        {
            return Client.API.Channels(ID).Invites.Get().Then((inviteItems) =>
            {
                Collection<string, Invite> invites = new Collection<string, Invite>();
                for (int index = 0, length = inviteItems.Length; index < length; index++)
                {
                    var inviteItem = inviteItems[index];
                    var invite = new Invite(Client, inviteItem);
                    invites.Set(invite.Code, invite);
                }
                return invites;
            });
        }

        /// <summary>
        /// Locks in the permission overwrites from the parent channel.
        /// </summary>
        /// <returns></returns>
        public IPromise<GuildChannel> LockPermissions()
        {
            var parent = Parent;
            if (parent == null) return Promise<GuildChannel>.Rejected(new DJSError.Error("GUILD_CHANNEL_ORPHAN"));
            var permissionOverwrites = Parent.PermissionOverwrites.Map((overwrite) => new OverwriteResolvable(overwrite.ToData())).ToArray();
            return Edit(new GuildChannelEditData()
            {
                permissionOverwrites = permissionOverwrites
            });
        }

        /// <summary>
        /// Gets the overall set of permissions for a member in this channel, taking into account channel overwrites.
        /// </summary>
        /// <param name="member">The member to obtain the overall permissions for</param>
        /// <returns></returns>
        public Permissions MemberPermissions(GuildMember member)
        {
            if (member.ID == Guild.OwnerID) return new Permissions(Permissions.ALL).Freeze();

            Collection<Snowflake, Role> roles = member.Roles.Cache;
            var permissions = new Permissions(roles.Map((role) => role.Permissions));

            if (permissions.Has(Permissions.FLAGS.ADMINISTRATOR)) return new Permissions(Permissions.ALL).Freeze();

            OverwritesForData overwrites = OverwritesFor(member, roles);

            return new Permissions(new PermissionResolvable(
                permissions.Remove(overwrites.everyone == null ? new BitFieldResolvable(0) : new BitFieldResolvable(new PermissionResolvable(overwrites.everyone.Deny)))
                .Add(overwrites.everyone == null ? new BitFieldResolvable(0) : new BitFieldResolvable(new PermissionResolvable(overwrites.everyone.Allow)))
                .Remove(overwrites.roles.Length > 0 ? new BitFieldResolvable(overwrites.roles.Map((role) => (PermissionResolvable)role.Deny)) : new BitFieldResolvable(0))
                .Add(overwrites.roles.Length > 0 ? new BitFieldResolvable(overwrites.roles.Map((role) => (PermissionResolvable)role.Allow)) : new BitFieldResolvable(0))
                .Remove(overwrites.member == null ? new BitFieldResolvable(0) : new BitFieldResolvable(new PermissionResolvable(overwrites.member.Deny)))
                .Add(overwrites.member == null ? new BitFieldResolvable(0) : new BitFieldResolvable(new PermissionResolvable(overwrites.member.Allow))))).Freeze();
        }

        public OverwritesForData OverwritesFor(GuildMember member, Collection<Snowflake, Role> roles = null)
        {
            if (member == null) return new OverwritesForData();
            roles = roles == null ? member.Roles.Cache : roles;
            Array<PermissionOverwrites> roleOverwrites = new Array<PermissionOverwrites>();
            PermissionOverwrites memberOverwrites = null;
            PermissionOverwrites everyoneOverwrites = null;

            foreach (PermissionOverwrites overwrite in PermissionOverwrites.Values())
            {
                if (overwrite.ID == Guild.ID) everyoneOverwrites = overwrite;
                else if (roles.Has(overwrite.ID)) roleOverwrites.Push(overwrite);
                else if (overwrite.ID == member.ID) memberOverwrites = overwrite;
            }

            return new OverwritesForData() { everyone = everyoneOverwrites, roles = roleOverwrites, member = memberOverwrites };
        }

        /// <summary>
        /// Replaces the permission overwrites in this channel.
        /// </summary>
        /// <param name="overwrites">Permission overwrites the channel gets updated with</param>
        /// <param name="reason">Reason for updating the channel overwrites</param>
        /// <returns></returns>
        public IPromise<GuildChannel> OverwritePermissions(PermissionOverwrites[] overwrites, string reason = null)
        {
            return Edit(new GuildChannelEditData() { permissionOverwrites = new Array<PermissionOverwrites>(overwrites).Map((o) => (OverwriteResolvable)o).ToArray() }, reason);
        }

        /// <summary>
        /// Replaces the permission overwrites in this channel.
        /// </summary>
        /// <param name="overwrites">Permission overwrites the channel gets updated with</param>
        /// <param name="reason">Reason for updating the channel overwrites</param>
        /// <returns></returns>
        public IPromise<GuildChannel> OverwritePermissions(OverwriteResolvable[] overwrites, string reason = null)
        {
            return Edit(new GuildChannelEditData() { permissionOverwrites = overwrites }, reason);
        }

        /// <summary>
        /// Gets the overall set of permissions for a member or role in this channel, taking into account channel overwrites.
        /// </summary>
        /// <param name="member">The member to obtain the overall permissions for</param>
        /// <returns></returns>
        public Permissions PermissionsFor(GuildMemberResolvable member) => MemberPermissions(Guild.Members.Resolve(member));

        /// <summary>
        /// Gets the overall set of permissions for a member or role in this channel, taking into account channel overwrites.
        /// </summary>
        /// <param name="role">The role to obtain the overall permissions for</param>
        /// <returns></returns>
        public Permissions PermissionsFor(RoleResolvable role) => RolePermissions(Guild.Roles.Resolve(role));

        /// <summary>
        /// Gets the overall set of permissions for a role in this channel, taking into account channel overwrites.
        /// </summary>
        /// <param name="role">The role to obtain the overall permissions for</param>
        /// <returns></returns>
        public Permissions RolePermissions(Role role)
        {
            if (role.Permissions.Has(Permissions.FLAGS.ADMINISTRATOR)) return new Permissions(Permissions.ALL).Freeze();

            var everyoneOverwrites = PermissionOverwrites.Get(Guild.ID);
            var roleOverwrites = PermissionOverwrites.Get(role.ID);

            return new Permissions(new PermissionResolvable(
                role.Permissions.Remove(everyoneOverwrites == null ? new BitFieldResolvable(0) : new BitFieldResolvable(new PermissionResolvable(everyoneOverwrites.Deny))))
                .Add(everyoneOverwrites == null ? new BitFieldResolvable(0) : new BitFieldResolvable(new PermissionResolvable(everyoneOverwrites.Allow)))
                .Remove(roleOverwrites == null ? new BitFieldResolvable(0) : new BitFieldResolvable(new PermissionResolvable(roleOverwrites.Deny)))
                .Add(roleOverwrites == null ? new BitFieldResolvable(0) : new BitFieldResolvable(new PermissionResolvable(roleOverwrites.Allow))));
        }

        /// <summary>
        /// Sets a new name for the guild channel.
        /// </summary>
        /// <param name="name">The new name for the guild channel</param>
        /// <param name="reason">Reason for changing the guild channel's name</param>
        /// <returns></returns>
        public IPromise<GuildChannel> SetName(string name, string reason = null) => Edit(new GuildChannelEditData()
        {
            name = name
        }, reason);

        /// <summary>
        /// Sets the category parent of this channel.
        /// </summary>
        /// <param name="channel">Parent channel</param>
        /// <param name="lockPermissions">Lock the permissions to what the parent's permissions are</param>
        /// <param name="reason">Reason for modifying the parent of this channel</param>
        /// <returns></returns>
        public IPromise<GuildChannel> SetParent(CategoryChannel channel, bool lockPermissions = true, string reason = null) => Edit(new GuildChannelEditData()
        {
            parentID = channel != null ? channel.ID : null,
            lockPermissions = lockPermissions
        }, reason);

        /// <summary>
        /// Sets a new position for the guild channel.
        /// </summary>
        /// <param name="position">The new position for the guild channel</param>
        /// <param name="relative">Change the position relative to its current value</param>
        /// <param name="reason">Reason for changing the position</param>
        /// <returns></returns>
        public IPromise<GuildChannel> SetPosition(int position, bool relative = false, string reason = null)
        {
            return DiscordUtil.SetPosition(this, position, relative, Guild._SortedChannels(this), Client.API.Guilds(Guild.ID).Channels(), reason).Then((updatedChannels) =>
            {
                Client.Actions.GuildChannelsPositionUpdate.Handle(Guild.ID, updatedChannels);
                return this;
            });
        }

        /// <summary>
        /// Sets a new topic for the guild channel.
        /// </summary>
        /// <param name="topic">The new topic for the guild channel</param>
        /// <param name="reason">Reason for changing the guild channel's topic</param>
        /// <returns></returns>
        public IPromise<GuildChannel> SetTopic(string topic, string reason = null) => Edit(new GuildChannelEditData()
        {
            topic = topic
        }, reason);

        /// <summary>
        /// Updates Overwrites for a user or role in this channel. (creates if non-existent)
        /// </summary>
        /// <param name="userOrRole">The user or role to update</param>
        /// <param name="options">The options for the update (dict where key is the name of a permission, and the value is a nullable <see cref="bool"/>:<br/><see langword="true"/> - override that this user/role has this permission<br/><see langword="false"/> - override that this user/role doesn't have this permission<br/><see langword="null"/> - inherit permission)</param>
        /// <param name="reason">Reason for creating/editing this overwrite</param>
        /// <returns></returns>
        public IPromise<GuildChannel> UpdateOverwrite(UserResolvable userOrRole, Dictionary<string, bool?> options, string reason = null)
        {
            User user = Client.Users.Resolve(userOrRole);
            if (user == null) return Promise<GuildChannel>.Rejected(new DJSError.Error("INVALID_TYPE", "parameter", "User nor a Role", true));

            var existing = PermissionOverwrites.Get(user.ID);
            if (existing == null) return CreateOverwrite(user, options, reason);
            else return existing.Update(options, reason).Then((_) => this);
        }

        /// <summary>
        /// Updates Overwrites for a user or role in this channel. (creates if non-existent)
        /// </summary>
        /// <param name="userOrRole">The user or role to update</param>
        /// <param name="options">The options for the update (dict where key is the name of a permission, and the value is a nullable <see cref="bool"/>:<br/><see langword="true"/> - override that this user/role has this permission<br/><see langword="false"/> - override that this user/role doesn't have this permission<br/><see langword="null"/> - inherit permission)</param>
        /// <param name="reason">Reason for creating/editing this overwrite</param>
        /// <returns></returns>
        public IPromise<GuildChannel> UpdateOverwrite(RoleResolvable userOrRole, Dictionary<string, bool?> options, string reason = null)
        {
            Role role = Guild.Roles.Resolve(userOrRole);
            if (role == null) return Promise<GuildChannel>.Rejected(new DJSError.Error("INVALID_TYPE", "parameter", "User nor a Role", true));

            var existing = PermissionOverwrites.Get(role.ID);
            if (existing == null) return CreateOverwrite(role, options, reason);
            else return existing.Update(options, reason).Then((_) => this);
        }
    }

    public sealed class GuildChannelCloneOptions : CreateGuildChannelOptions
    {
        /// <summary>
        /// Name of the new channel
        /// </summary>
        public string name;
    }

    /// <summary>
    /// The data for editing a guild channel.
    /// </summary>
    public sealed class GuildChannelEditData
    {
        /// <summary>
        /// The name of the channel
        /// </summary>
        public string name;

        /// <summary>
        /// The position of the channel
        /// </summary>
        public int? position;

        /// <summary>
        /// The topic of the text channel
        /// </summary>
        public string topic;

        /// <summary>
        /// Whether the channel is NSFW
        /// </summary>
        public bool? nsfw;

        /// <summary>
        /// The bitrate of the voice channel
        /// </summary>
        public int? bitrate;

        /// <summary>
        /// The user limit of the voice channel
        /// </summary>
        public int? userLimit;

        /// <summary>
        /// The parent ID of the channel
        /// </summary>
        public Snowflake parentID;

        /// <summary>
        /// Lock the permissions of the channel to what the parent's permissions are
        /// </summary>
        public bool? lockPermissions;

        /// <summary>
        /// Permission overwrites for the channel
        /// </summary>
        public OverwriteResolvable[] permissionOverwrites;

        /// <summary>
        /// The ratelimit per user for the channel in seconds
        /// </summary>
        public int? rateLimitPerUser;
    }

    public sealed class OverwritesForData
    {
        public PermissionOverwrites everyone;
        public Array<PermissionOverwrites> roles;
        public PermissionOverwrites member;
    }
}