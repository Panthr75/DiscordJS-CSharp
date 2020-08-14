namespace DiscordJS
{
    /// <summary>
    /// A utility class that makes multi-process sharding of a bot an easy and painless experience.
    /// </summary>
    public class ShardingManager
    {
    }

    /// <summary>
    /// Options for the sharding manager
    /// </summary>
    public class ShardingManagerOptions : IDefaultableObject
    {
        /// <summary>
        /// Number of total shards of all shard managers, or <see langword="null"/> for "auto"
        /// </summary>
        public int? TotalShards { get; set; }

        /// <summary>
        /// List of shards to spawn or <see langword="null"/> for "auto"
        /// </summary>
        public int[] ShardList { get; set; }

        /// <summary>
        /// Whether shards should automatically respawn upon exiting
        /// </summary>
        public bool? Respawn { get; set; }

        /// <summary>
        /// Token to use for automatic shard count and passing to shards
        /// </summary>
        public string Token { get; set; }

        internal void ImplementDefault()
        {
            if (!Respawn.HasValue) Respawn = true;
        }

        internal void FromDefault(ShardingManagerOptions def)
        {
            if (!Respawn.HasValue) Respawn = def.Respawn;
        }

        void IDefaultableObject.ImplementDefault() => ImplementDefault();
        void IDefaultableObject.FromDefault(object def)
        {
            if (def is ShardingManagerOptions defOptions)
                FromDefault(defOptions);
        }
    }
}
