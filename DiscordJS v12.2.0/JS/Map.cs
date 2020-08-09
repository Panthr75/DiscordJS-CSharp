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
        /// <param name="key"></param>
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

        public Array<K> Keys()
        {
            Array<K> result = new Array<K>();
            ForEach((_, key) =>
            {
                result.Push(key);
            });
            return result;
        }

        public Array<V> Values()
        {
            Array<V> result = new Array<V>();
            ForEach((value) =>
            {
                result.Push(value);
            });
            return result;
        }

        /// <summary>
        /// The size of this map
        /// </summary>
        public int Size => values.Count;

        public Enumerator GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator<Item> IEnumerable<Item>.GetEnumerator() => GetEnumerator();

        [Serializable]
        public sealed class ValueEnumerator : Iterator<V>
        {
            private Map<K, V> map;
            private int index;

            internal ValueEnumerator(Map<K, V> map) : base()
            {
                this.map = map;
                index = 0;
            }

            protected override IteratorResult<V> GetNext()
            {
                while (index < map.Size)
                {
                    if (map.values[index].GetHashCode() >= 0)
                    {
                        var entry = map.values[index];
                        index++;
                        return new IteratorResult<V>(entry.Value, false);
                    }
                    index++;
                }

                index = map.Size + 1;
                return new IteratorResult<V>(default, true);
            }

            protected override void Reset()
            {
                index = 0;
                base.Reset();
            }
        }

        [Serializable]
        public sealed class Enumerator : Iterator<Item>, IEnumerator<KeyValuePair<K, V>>
        {
            private Map<K, V> map;
            private int index;

            internal Enumerator(Map<K, V> map) : base(new Item())
            {
                this.map = map;
                index = 0;
            }

            protected override IteratorResult<Item> GetNext()
            {
                while (index < map.Size)
                {
                    if (map.values[index].GetHashCode() >= 0)
                    {
                        var entry = map.values[index];
                        index++;
                        return new IteratorResult<Item>(new Item(entry.Key, entry.Value), false);
                    }
                    index++;
                }

                index = map.Size + 1;
                return new IteratorResult<Item>(null, true);
            }

            protected override void Reset()
            {
                index = 0;
                base.Reset();
            }

            public Item Entry => Current;

            public K Key => Current.Key;

            public V Value => Current.Value;

            KeyValuePair<K, V> IEnumerator<KeyValuePair<K, V>>.Current => new KeyValuePair<K, V>(Current.Key, Current.Value);
        }
    }
}