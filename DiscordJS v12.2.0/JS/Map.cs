using System;
using System.Collections;
using System.Collections.Generic;

namespace JavaScript
{
    /// <summary>
    /// Represents an ECMAScript Compliant JavaScript Map
    /// </summary>
    /// <typeparam name="K">The type of key to use</typeparam>
    /// <typeparam name="V">The type of value to use</typeparam>
    public class Map<K, V> : IEnumerable<Map<K, V>.Item>, IEnumerable
    {
        internal List<Item> values;

        /// <summary>
        /// Represents a specific item in a map
        /// </summary>
        public class Item
        {
            /// <summary>
            /// Instantiates a new Item with <see cref="Key"/> and <see cref="Value"/> set to <see langword="default"/>.
            /// </summary>
            public Item()
            {
                Key = default;
                Value = default;
            }

            /// <summary>
            /// Instantiates a new Item with <see cref="Key"/> set to <paramref name="key"/> and <see cref="Value"/> set to <paramref name="value"/>.
            /// </summary>
            /// <param name="key">The key of the item</param>
            /// <param name="value">The value of the item</param>
            public Item(K key, V value)
            {
                Key = key;
                Value = value;
            }


            /// <summary>
            /// The key used to identify the item
            /// </summary>
            public K Key { get; internal set; }

            /// <summary>
            /// The value of the item
            /// </summary>
            public V Value { get; internal set; }
        }

        /// <summary>
        /// Clears a Map
        /// </summary>
        public void Clear() => values.Clear();

        /// <summary>
        /// Removes a specified key-value pair from the map using the given key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns></returns>
        public bool Delete(K key)
        {
            for (int index = 0, length = values.Count; index < length; index++)
            {
                Item item = values[index];
                if (EqualityComparer<K>.Default.Equals(item.Key, key))
                {
                    values.RemoveAt(index);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the entry iterator for this map
        /// </summary>
        /// <returns></returns>
        public EntryIterator Entries() => new EntryIterator(this);

        /// <summary>
        /// Loops through every key-value pair in the map, and runs the given callback function, providing the map, the key, and the value
        /// </summary>
        /// <param name="callbackfn">The callback function to run</param>
        public void ForEach(Action callbackfn)
        {
            if (callbackfn is null) return; // Don't do work if no callbackfn is defined
            for (int index = 0, length = values.Count; index < length; index++)
            {
                Item item = values[index];
                callbackfn.Invoke();
            }
        }

        /// <summary>
        /// Loops through every key-value pair in the map, and runs the given callback function, providing the map, the key, and the value
        /// </summary>
        /// <param name="callbackfn">The callback function to run</param>
        public void ForEach(Action<V> callbackfn)
        {
            if (callbackfn is null) return; // Don't do work if no callbackfn is defined
            for (int index = 0, length = values.Count; index < length; index++)
            {
                Item item = values[index];
                callbackfn.Invoke(item.Value);
            }
        }

        /// <summary>
        /// Loops through every key-value pair in the map, and runs the given callback function, providing the map, the key, and the value
        /// </summary>
        /// <param name="callbackfn">The callback function to run</param>
        public void ForEach(Action<V, K> callbackfn)
        {
            if (callbackfn is null) return; // Don't do work if no callbackfn is defined
            for (int index = 0, length = values.Count; index < length; index++)
            {
                Item item = values[index];
                callbackfn.Invoke(item.Value, item.Key);
            }
        }

        /// <summary>
        /// Loops through every key-value pair in the map, and runs the given callback function, providing the map, the key, and the value
        /// </summary>
        /// <param name="callbackfn">The callback function to run</param>
        public void ForEach(Action<V, K, Map<K, V>> callbackfn)
        {
            if (callbackfn is null) return; // Don't do work if no callbackfn is defined
            for (int index = 0, length = values.Count; index < length; index++)
            {
                Item item = values[index];
                callbackfn.Invoke(item.Value, item.Key, this);
            }
        }

        /// <summary>
        /// Gets a value for a specified 
        /// </summary>
        /// <param name="key">The key to use to get the value</param>
        /// <returns></returns>
        public V Get(K key)
        {
            for (int index = 0, length = values.Count; index < length; index++)
            {
                Item item = values[index];
                if (EqualityComparer<K>.Default.Equals(item.Key, key))
                    return item.Value;
            }
            return default;
        }

        /// <summary>
        /// Returns whether or not this map has the given key
        /// </summary>
        /// <param name="key">The key to check for</param>
        /// <returns></returns>
        public bool Has(K key)
        {
            for (int index = 0, length = values.Count; index < length; index++)
            {
                Item item = values[index];
                if (EqualityComparer<K>.Default.Equals(item.Key, key))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Sets a given key-value pair in the map
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The corresponding value</param>
        /// <returns></returns>
        public Map<K, V> Set(K key, V value)
        {
            for (int index = 0, length = values.Count; index < length; index++)
            {
                Item item = values[index];
                if (EqualityComparer<K>.Default.Equals(item.Key, key))
                {
                    item.Value = value;
                    return this;
                }
            }
            values.Add(new Item()
            {
                Key = key,
                Value = value
            });
            return this;
        }

        /// <summary>
        /// Gets the key iterator for this object
        /// </summary>
        /// <returns></returns>
        public KeyIterator Keys() => new KeyIterator(this);

        /// <summary>
        /// Gets the value iterator for this object
        /// </summary>
        /// <returns></returns>
        public ValueIterator Values() => new ValueIterator(this);

        /// <summary>
        /// The size of this map
        /// </summary>
        public int Size => values.Count;

        /// <summary>
        /// Gets the entry iterator for this map
        /// </summary>
        /// <returns></returns>
        public EntryIterator GetEnumerator() => new EntryIterator(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator<Item> IEnumerable<Item>.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Represents the value iterator for this map
        /// </summary>
        [Serializable]
        public sealed class ValueIterator : Iterator<V>
        {
            private Map<K, V> map;
            private int index;

            internal ValueIterator(Map<K, V> map) : base()
            {
                this.map = map;
                index = 0;
            }

            /// <inheritdoc/>
            protected override IteratorResult<V> GetNext()
            {
                while (index < map.Size)
                {
                    var entry = map.values[index];
                    if (entry != null && entry.GetHashCode() >= 0)
                    {
                        index++;
                        return new IteratorResult<V>(entry.Value, false);
                    }
                    index++;
                }

                index = map.Size + 1;
                return new IteratorResult<V>(default, true);
            }

            /// <inheritdoc/>
            protected override void Reset()
            {
                index = 0;
                base.Reset();
            }
        }

        /// <summary>
        /// Represents the key iterator for this map
        /// </summary>
        [Serializable]
        public sealed class KeyIterator : Iterator<K>
        {
            private Map<K, V> map;
            private int index;

            internal KeyIterator(Map<K, V> map) : base()
            {
                this.map = map;
                index = 0;
            }

            /// <inheritdoc/>
            protected override IteratorResult<K> GetNext()
            {
                while (index < map.Size)
                {
                    var entry = map.values[index];
                    if (entry != null && entry.GetHashCode() >= 0)
                    {
                        index++;
                        return new IteratorResult<K>(entry.Key, false);
                    }
                    index++;
                }

                index = map.Size + 1;
                return new IteratorResult<K>(default, true);
            }

            /// <inheritdoc/>
            protected override void Reset()
            {
                index = 0;
                base.Reset();
            }
        }

        /// <summary>
        /// Represents the entry iterator for this map
        /// </summary>
        [Serializable]
        public sealed class EntryIterator : Iterator<Item>, IEnumerator<KeyValuePair<K, V>>
        {
            private Map<K, V> map;
            private int index;

            internal EntryIterator(Map<K, V> map) : base(new Item())
            {
                this.map = map;
                index = 0;
            }

            /// <inheritdoc/>
            protected override IteratorResult<Item> GetNext()
            {
                while (index < map.Size)
                {
                    var entry = map.values[index];
                    if (entry != null && entry.GetHashCode() >= 0)
                    {
                        index++;
                        return new IteratorResult<Item>(new Item(entry.Key, entry.Value), false);
                    }
                    index++;
                }

                index = map.Size + 1;
                return new IteratorResult<Item>(null, true);
            }

            /// <inheritdoc/>
            protected override void Reset()
            {
                index = 0;
                base.Reset();
            }

            /// <summary>
            /// The current entry
            /// </summary>
            public Item Entry => Current;

            /// <summary>
            /// The key for the current entry
            /// </summary>
            public K Key => Current.Key;

            /// <summary>
            /// The value for the current entry
            /// </summary>
            public V Value => Current.Value;

            KeyValuePair<K, V> IEnumerator<KeyValuePair<K, V>>.Current => new KeyValuePair<K, V>(Current.Key, Current.Value);
        }
    }
}