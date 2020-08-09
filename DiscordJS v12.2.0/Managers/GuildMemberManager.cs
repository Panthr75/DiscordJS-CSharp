using DiscordJS.Data;
using DiscordJS.Resolvables;
using DiscordJS.Packets;
using JavaScript;
using System.Collections.Generic;

namespace DiscordJS
{
    /// <summary>
    /// Manages API methods for GuildMembers and stores their cache.
    /// </summary>
    public class GuildMemberManager : BaseManager<Collection<Snowflake, GuildMember>, GuildMember, GuildMemberData>
    {
        /// <summary>
        /// The guild this manager belongs to
        /// </summary>
        public Guild Guild { get; }

        public GuildMemberManager(Guild guild, IEnumerable<GuildMember> iterable) : base(guild.Client, iterable)
        { }

        protected override GuildMember AddItem(GuildMember item) => Add(item, true);

        public GuildMember Add(GuildMember data, bool cache = true)
        {
            Snowflake snowflake = data.ID;
            GuildMember existing = Cache.Get(snowflake);
            if (existing != null)
            {
                if (cache) existing._Patch(data);
                return existing;
            }

            var entry = new GuildMember(Client, data, Guild);
            if (cache) Cache.Set(snowflake, entry);
            return entry;
        }

        public GuildMember Add(GuildMemberData data, bool cache = true)
        {
            Snowflake snowflake = data.user.id;
            GuildMember existing = Cache.Get(snowflake);
            if (existing != null)
            {
                if (cache) existing._Patch(data);
                return existing;
            }

            var entry = new GuildMember(Client, data, Guild);
            if (cache) Cache.Set(snowflake, entry);
            return entry;
        }

        /// <summary>
        /// Resolves a GuildMemberResolvable to a GuildMember object.
        /// </summary>
        /// <param name="member">The user that is part of the guild</param>
        /// <returns></returns>
        public GuildMember Resolve(GuildMemberResolvable member) => member.Resolve(this);

        /// <summary>
        /// Resolves a GuildMemberResolvable to a member ID string.
        /// </summary>
        /// <param name="member">The user that is part of the guild</param>
        /// <returns></returns>
        public Snowflake ResolveID(GuildMemberResolvable member)
        {
            var m = member.Resolve(this);
            if (m != null) return m.ID;
            return null;
        }

        /// <summary>
        /// Fetches member(s) from Discord, even if they're offline.
        /// </summary>
        /// <param name="options">If a <see cref="UserResolvable"/>, the user to fetch.<br/>
        /// If <see langword="null"/>, fetches all members.<br/>
        /// If a query, it limits the results to users with similar usernames.</param>
        /// <returns></returns>
        public IPromise<GuildMember> Fetch(UserResolvable options)
        {
            if (options == null) return _FetchMany(null, null, 1).Then((members) => members[0]);
            var user = Client.Users.ResolveID(options);
            if (user != null) return _FetchSingle(user, true);
            return _FetchMany(null, null, 1).Then((members) => members[0]);
        }

        /// <summary>
        /// Fetches member(s) from Discord, even if they're offline.
        /// </summary>
        /// <param name="options">If a <see cref="UserResolvable"/>, the user to fetch.<br/>
        /// If <see langword="null"/>, fetches all members.<br/>
        /// If a query, it limits the results to users with similar usernames.</param>
        /// <returns></returns>
        public IPromise<GuildMember> Fetch(FetchMemberOptions options)
        {
            if (options == null) return _FetchMany(null, null, 1).Then((members) => members[0]);
            if (options.user != null)
            {
                string user = Client.Users.ResolveID(options.user);
                return _FetchSingle(user, options.cache);
            }
            return _FetchMany(null, null, 1).Then((members) => members[0]);
        }

        /// <summary>
        /// Prunes members from the guild based on how long they have been inactive.
        /// <br/>
        /// <br/>
        /// <info><b>It's recommended to set <paramref name="options"/>.<i>Count</i> to <see langword="false"/> for large guilds.</b></info>
        /// </summary>
        /// <param name="options">Prune options</param>
        /// <returns>The number of members that were/will be kicked</returns>
        public IPromise<int?> Prune(PruneOptions options)
        {
            if (options.Dry)
            {
                return Client.API.Guilds(Guild.ID).Prune.Get(new
                {
                    query = new
                    {
                        days = options.Days,
                        compute_prune_count = options.Count
                    },
                    reason = options.Reason
                }).Then((data) => data.pruned);
            }
            else
            {
                return Client.API.Guilds(Guild.ID).Prune.Post(new
                {
                    query = new
                    {
                        days = options.Days,
                        compute_prune_count = options.Count
                    },
                    reason = options.Reason
                }).Then((data) => data.pruned);
            }
        }

        /// <summary>
        /// Bans a user from the guild.
        /// </summary>
        /// <param name="user">The user to ban</param>
        /// <param name="options">Options for the ban</param>
        /// <returns>Result object will be resolved as specifically as possible.<br/>
        /// If the GuildMember cannot be resolved, the User will instead be attempted to be resolved.<br/>
        /// be resolved, the user ID will be the result.</returns>
        public IPromise<BannedMember> Ban(UserResolvable user, BanOptions options)
        {
            string id = Client.Users.ResolveID(user);
            if (id is null) return Promise<BannedMember>.Rejected(new DJSError.Error("BAN_RESOLVE_ID", true));
            return Client.API.Guilds(Guild.ID).Bans(id).Put(new { query = options }).Then((_) =>
            {
                if (user.isGuildMember) return user.member;
                var _user = Client.Users.Resolve(id);
                if (_user != null)
                {
                    var member = Resolve(id);
                    return member == null ? _user : member;
                }
                return id;
            });
        }

        /// <summary>
        /// Unbans a user from the guild.
        /// </summary>
        /// <param name="user">The user to unban</param>
        /// <param name="reason">Reason for unbanning user</param>
        /// <returns></returns>
        public IPromise<User> Unban(UserResolvable user, string reason = null)
        {
            string id = Client.Users.ResolveID(user);
            if (id is null) return Promise<User>.Rejected(new DJSError.Error("BAN_RESOLVE_ID"));
            return Client.API.Guilds(Guild.ID).Bans(id).Delete(new { reason }).Then((_) => Client.Users.Resolve(user));
        }

        /// <summary>
        /// Fetches member(s) from Discord, even if they're offline.
        /// </summary>
        /// <param name="options">If a <see cref="UserResolvable"/>, the user to fetch.<br/>
        /// If <see langword="null"/>, fetches all members.<br/>
        /// If a query, it limits the results to users with similar usernames.</param>
        /// <returns></returns>
        public IPromise<GuildMember[]> Fetch(FetchMembersOptions options)
        {
            if (options == null) return _FetchMany();
            if (options.user != null)
            {
                string[] user = new Array<UserResolvable>(options.user).Map((u) => (string)Client.Users.ResolveID(u)).ToArray();
                return _FetchMany(user, options.query, options.limit, options.withPresences, options.time);
            }
            return _FetchMany();
        }

        internal IPromise<GuildMember> _FetchSingle(string user, bool cache = true)
        {
            GuildMember existing = Cache.Get(user);
            if (existing != null && !existing.Partial) return Promise<GuildMember>.Resolved(existing);
            return Client.API.Guilds(Guild.ID).Members(user).Get().Then((GuildMemberData data) => Add(data, cache));
        }

        internal IPromise<GuildMember[]> _FetchMany(string[] user = null, string query = null, int limit = 0, bool withPresences = false, long time = 120000)
        {
            return new Promise<GuildMember[]>((resolve, reject) =>
            {
                if (Guild.MemberCount == Cache.Size && query == null && limit == 0 && withPresences == false && user == null)
                {
                    GuildMember[] res = new GuildMember[Cache.Size];
                    for (int index = 0, length = Cache.Size; index < length; index++)
                        res[index] = Cache.values[index].Value;
                    resolve(res);
                    return;
                }
                if (query == null && user == null) query = "";
                Guild.Shard.Send(new DiscordPacket(OPCode.REQUEST_GUILD_MEMBERS, new
                {
                    guild_id = Guild.ID,
                    presences = withPresences,
                    query,
                    limit
                }));

                var fetchedMembers = new Collection<Snowflake, GuildMember>();
                bool option = !string.IsNullOrEmpty(query) || limit != 0 || withPresences == true || user != null;

                Client.GuildMembersChunkEvent handler = null;
                int timeout = -1;
                handler = (members, guild) =>
                {
                    if (guild.ID != Guild.ID) return;
                    Timeout.Refresh(timeout);
                    foreach (GuildMember member in members.Values())
                    {
                        if (option) fetchedMembers.Set(member.ID, member);
                    }
                    if (
                        Guild.MemberCount <= Cache.Size ||
                        (option && members.Size < 1000) ||
                        (limit != 0 && fetchedMembers.Size >= limit)
                    )
                    {
                        Guild.Client.GuildMembersChunk -= handler;
                        Collection<Snowflake, GuildMember> fetched = option ? fetchedMembers : Cache;
                        GuildMember[] result = new GuildMember[fetched.Size];
                        int index = 0;
                        foreach (GuildMember member in fetched.Values())
                        {
                            result[index] = member;
                            index++;
                        }
                        resolve(result);
                    }
                };
                timeout = Guild.Client.SetTimeout(() =>
                {
                    Guild.Client.GuildMembersChunk -= handler;
                    reject(new DJSError.Error("GUILD_MEMBERS_TIMEOUT"));
                }, time);
                Guild.Client.GuildMembersChunk += handler;
            });
        }
    }

    /// <summary>
    /// Options for pruning
    /// </summary>
    public sealed class PruneOptions
    {
        /// <summary>
        /// Number of days of inactivity required to kick
        /// </summary>
        public int Days { get; set; } = 7;

        /// <summary>
        /// Get number of users that will be kicked, without actually kicking them
        /// </summary>
        public bool Dry { get; set; } = false;

        /// <summary>
        /// Whether or not to return the number of users that have been kicked.
        /// </summary>
        public bool Count { get; set; } = true;

        /// <summary>
        /// Reason for this prune
        /// </summary>
        public string Reason { get; set; } = null;
    }

    /// <summary>
    /// Options used to fetch a single member from a guild.
    /// </summary>
    public sealed class FetchMemberOptions
    {
        /// <summary>
        /// The user to fetch
        /// </summary>
        public UserResolvable user;

        /// <summary>
        /// Whether or not to cache the fetched member
        /// </summary>
        public bool cache = true;
    }

    /// <summary>
    /// Options used to fetch multiple members from a guild.
    /// </summary>
    public sealed class FetchMembersOptions
    {
        /// <summary>
        /// The user(s) to fetch
        /// </summary>
        public UserResolvable[] user;

        /// <summary>
        /// Limit fetch to members with similar usernames
        /// </summary>
        public string query;

        /// <summary>
        /// Maximum number of members to request
        /// </summary>
        public int limit = 0;

        /// <summary>
        /// Whether or not to include the presences
        /// </summary>
        public bool withPresences = false;

        /// <summary>
        /// Timeout for receipt of members
        /// </summary>
        public long time = 120000;
    }

    /// <summary>
    /// Represents a banned member
    /// </summary>
    public sealed class BannedMember
    {
        private enum MemberType
        {
            Member = 0,
            User = 1,
            Snowflake = 2
        }

        private MemberType type;

        internal GuildMember member;
        internal User user;
        internal Snowflake snowflake;

        public BannedMember(GuildMember member)
        {
            type = MemberType.Member;
            this.member = member;
        }

        public bool IsMember() => type == MemberType.Member;
        public bool IsUser() => type == MemberType.User;
        public bool IsSnowflake() => type == MemberType.Snowflake;

        public static explicit operator Snowflake(BannedMember bannedMember)
        {
            if (bannedMember.type == MemberType.Snowflake) return bannedMember.snowflake;
            else throw new System.InvalidCastException();
        }

        public static explicit operator User(BannedMember bannedMember)
        {
            if (bannedMember.type == MemberType.User) return bannedMember.user;
            else throw new System.InvalidCastException();
        }

        public static explicit operator GuildMember(BannedMember bannedMember)
        {
            if (bannedMember.type == MemberType.Member) return bannedMember.member;
            else throw new System.InvalidCastException();
        }
    }
}