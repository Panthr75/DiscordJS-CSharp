using System;

namespace DiscordJS
{
    /// <summary>
    /// A Collection which holds a max amount of entries. The first key is deleted if the Collection has<br/>
    /// reached max size.
    /// </summary>
    public class LimitedCollection<K, V> : Collection<K, V>
    {
        /// <summary>
        /// The max size of the Collection.
        /// </summary>
        public int MaxSize { get; }

        /// <summary>
        /// Instantiates a new LimitedCollection.
        /// </summary>
        /// <param name="maxSize">The maximum size of the Collection</param>
        public LimitedCollection(int maxSize = 0)
        {
            MaxSize = maxSize;
        }

        /// <inheritdoc/>
        public override Collection<K, V> Set(K key, V value)
        {
            if (MaxSize == 0) return this;
            if (Size >= MaxSize && !Has(key)) Delete(FirstKey());
            return base.Set(key, value);
        }
    }
}