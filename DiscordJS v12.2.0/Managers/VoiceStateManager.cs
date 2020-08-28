using DiscordJS.Data;
using DiscordJS.Resolvables;
using System.Collections.Generic;

namespace DiscordJS
{
    /// <summary>
    /// Manages API methods for VoiceStates and stores their cache.
    /// </summary>
    public class VoiceStateManager : BaseManager<Collection<Snowflake, VoiceState>, VoiceState, VoiceStateData>
    {
        /// <summary>
        /// The guild this manager belongs to
        /// </summary>
        public Guild Guild { get; }

        public VoiceStateManager(Guild guild) : this(guild, null)
        { }

        public VoiceStateManager(Guild guild, IEnumerable<VoiceState> iterable) : base(guild.Client, iterable)
        {
            Guild = guild;
        }

        protected override VoiceState AddItem(VoiceState item)
        {
            Cache.Set(item.ID, item);
            return item;
        }

        public VoiceState Add(VoiceStateData data, bool cache = true)
        {
            VoiceState existing = Cache.Get(data.user_id);
            if (existing != null) return existing._Patch(data);

            VoiceState entry = new VoiceState(Guild, data);
            if (cache) Cache.Set(data.user_id, entry);
            return entry;
        }

        /// <summary>
        /// Resolves a VoiceStateResolvable to a VoiceState object.
        /// </summary>
        /// <param name="voiceState">The VoiceStateResolvable to identify</param>
        /// <returns></returns>
        public VoiceState Resolve(VoiceStateResolvable voiceState) => voiceState.Resolve(this);

        /// <summary>
        /// Resolves a VoiceStateResolvable to a voiceState ID Snowflake.
        /// </summary>
        /// <param name="voiceState">The VoiceStateResolvable to identify</param>
        /// <returns></returns>
        public Snowflake ResolveID(VoiceStateResolvable voiceState) => voiceState.ResolveID();
    }
}