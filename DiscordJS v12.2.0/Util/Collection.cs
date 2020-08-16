using JavaScript;
using System;

namespace DiscordJS
{
    /// <summary>
    /// A Map with additional utility methods. This is used throughout discord.js rather than Arrays for anything that has
    /// an ID, for significantly improved performance and ease-of-use.
    /// </summary>
    public class Collection<K, V> : Map<K, V>, ICollection<K, V>
    {
        /// <summary>
        /// Cached array for the <c>Array()</c> method - will be reset to <c>null</c> whenever <c>Set()</c> or <c>Delete()</c> are called
        /// </summary>
        private Array<V> _array;

        /// <summary>
        /// Cached array for the <c>KeyArray()</c> method - will be reset to <c>null</c> whenever <c>Set()</c> or <c>Delete()</c> are called
        /// </summary>
        private Array<K> _keyArray;

        /// <summary>
        /// Instantiates a new Collection
        /// </summary>
        public Collection()
        {
            _array = null;
            _keyArray = null;
        }

        /// <inheritdoc/>
        public new V Get(K key) => base.Get(key);

        /// <summary>
        /// Behaves Identical to <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map/set">Map.set()</see>.
        /// <br/>
        /// Sets a new element in the collection with the specified key and value.
        /// </summary>
        /// <param name="key">The key of the element to add</param>
        /// <param name="value">The value of the element to add</param>
        /// <returns></returns>
        public new virtual Collection<K, V> Set(K key, V value)
        {
            _keyArray = null;
            _array = null;
            base.Set(key, value);
            return this;
        }

        /// <inheritdoc/>
        public new bool Has(K key) => base.Has(key);

        /// <inheritdoc/>
        public new bool Delete(K key)
        {
            _keyArray = null;
            _array = null;
            return base.Delete(key);
        }

        /// <inheritdoc/>
        public new void Clear()
        {
            base.Clear();
        }

        /// <inheritdoc/>
        public Array<V> Array()
        {
            if (!_array || _array.Length != Size) _array = Array<V>.From(Values());
            return _array;
        }

        /// <inheritdoc/>
        public Array<K> KeyArray()
        {
            if (!_array || _array.Length != Size) _keyArray = Array<K>.From(Keys());
            return _keyArray;
        }

        /// <inheritdoc/>
        public V First()
        {
            if (Size > 0) return values[0].Value;
            return default;
        }

        /// <inheritdoc/>
        public Array<V> First(int amount)
        {
            if (amount < 0) return Last(amount * -1);
            Array<V> result = new Array<V>();
            for (int index = 0, length = Math.Min(amount, Size); index < length; index++)
                result.Push(values[index].Value);
            return result;
        }

        /// <inheritdoc/>
        public K FirstKey()
        {
            if (Size > 0) return values[0].Key;
            return default;
        }

        /// <inheritdoc/>
        public Array<K> FirstKey(int amount)
        {
            if (amount < 0) return LastKey(amount * -1);
            Array<K> result = new Array<K>();
            for (int index = 0, length = Math.Min(amount, Size); index < length; index++)
                result.Push(values[index].Key);
            return result;
        }

        /// <inheritdoc/>
        public V Last()
        {
            if (Size > 0) return values[Size - 1].Value;
            return default;
        }

        /// <summary>
        /// Obtains the last value(s) in this collection. This relies on <see cref="Collection{K, V}.Array()"/>, and thus the caching<br/>
        /// mechanism applies here as well.
        /// </summary>
        /// <param name="amount">Amount of values to obtain from the end</param>
        /// <returns>A single value if no amount is provided or an array of values, starting from the start if<br/>
        /// amount is negative</returns>
        public Array<V> Last(int amount)
        {
            if (amount < 0) return First(amount * -1);
            return Array().Slice(-amount);
        }

        /// <inheritdoc/>
        public K LastKey()
        {
            if (Size > 0) return values[Size - 1].Key;
            return default;
        }

        /// <summary>
        /// Obtains the last key(s) in this collection. This relies on <see cref="Collection{K, V}.KeyArray()"/>, and thus the caching<br/>
        /// mechanism applies here as well.
        /// </summary>
        /// <param name="amount">Amount of keys to obtain from the end</param>
        /// <returns>A single key if no amount is provided or an array of keys, starting from the start if<br/>
        /// amount is negative</returns>
        public Array<K> LastKey(int amount)
        {
            if (amount < 0) return FirstKey(amount * -1);
            return KeyArray().Slice(-amount);
        }

        /// <summary>
        /// Obtains unique random value(s) from this collection. This relies on <see cref="Collection{K, V}.Array()"/>, and thus the caching<br/>
        /// mechanism applies here as well.
        /// </summary>
        /// <returns>A single value if no amount is provided or an array of values</returns>
        public V Random()
        {
            var arr = Array();
            return arr[new Random().Next(0, Size)];
        }

        /// <summary>
        /// Obtains unique random value(s) from this collection. This relies on <see cref="Collection{K, V}.Array()"/>, and thus the caching<br/>
        /// mechanism applies here as well.
        /// </summary>
        /// <param name="amount">Amount of values to obtain randomly</param>
        /// <returns>A single value if no amount is provided or an array of values</returns>
        public Array<V> Random(int amount)
        {
            var arr = Array().Slice();
            int len = Math.Min(amount, arr.Length);
            Array<V> result = new Array<V>(len);
            for (int index = 0; index < len; index++)
                result.Push(arr.Splice(new Random().Next(0, len), 1)[0]);
            return result;
        }

        /// <summary>
        /// Obtains unique random key(s) from this collection. This relies on <see cref="Collection{K, V}.KeyArray()"/>, and thus the caching<br/>
        /// mechanism applies here as well.
        /// </summary>
        /// <returns>A single key if no amount is provided or an array of keys</returns>
        public K RandomKey()
        {
            var arr = KeyArray();
            return arr[new Random().Next(0, Size)];
        }

        /// <summary>
        /// Obtains unique random key(s) from this collection. This relies on <see cref="Collection{K, V}.KeyArray()"/>, and thus the caching<br/>
        /// mechanism applies here as well.
        /// </summary>
        /// <param name="amount">Amount of keys to obtain randomly</param>
        /// <returns>A single key if no amount is provided or an array of keys</returns>
        public Array<K> RandomKey(int amount)
        {
            var arr = KeyArray().Slice();
            int len = Math.Min(amount, arr.Length);
            Array<K> result = new Array<K>(len);
            for (int index = 0; index < len; index++)
                result.Push(arr.Splice(new Random().Next(0, len), 1)[0]);
            return result;
        }

        /// <inheritdoc/>
        public V Find(Func<bool> fn)
        {
            if (fn is null) return default;
            foreach (Item item in values)
                if (fn()) return item.Value;
            return default;
        }

        /// <inheritdoc/>
        public V Find(Func<V, bool> fn)
        {
            if (fn is null) return default;
            foreach (Item item in values)
                if (fn(item.Value)) return item.Value;
            return default;
        }

        /// <inheritdoc/>
        public V Find(Func<V, K, bool> fn)
        {
            if (fn is null) return default;
            foreach (Item item in values)
                if (fn(item.Value, item.Key)) return item.Value;
            return default;
        }

        /// <inheritdoc/>
        public V Find(Func<V, K, ICollection<K, V>, bool> fn)
        {
            if (fn is null) return default;
            foreach (Item item in values)
                if (fn(item.Value, item.Key, this)) return item.Value;
            return default;
        }

        /// <inheritdoc/>
        public K FindKey(Func<bool> fn)
        {
            if (fn is null) return default;
            foreach (Item item in values)
                if (fn()) return item.Key;
            return default;
        }

        /// <inheritdoc/>
        public K FindKey(Func<V, bool> fn)
        {
            if (fn is null) return default;
            foreach (Item item in values)
                if (fn(item.Value)) return item.Key;
            return default;
        }

        /// <inheritdoc/>
        public K FindKey(Func<V, K, bool> fn)
        {
            if (fn is null) return default;
            foreach (Item item in values)
                if (fn(item.Value, item.Key)) return item.Key;
            return default;
        }

        /// <inheritdoc/>
        public K FindKey(Func<V, K, ICollection<K, V>, bool> fn)
        {
            if (fn is null) return default;
            foreach (Item item in values)
                if (fn(item.Value, item.Key, this)) return item.Key;
            return default;
        }

        /// <inheritdoc/>
        public int Sweep(Func<bool> fn)
        {
            int prevSize = Size;
            if (fn is null) return prevSize;
            foreach (Item item in values)
                if (fn()) Delete(item.Key);
            return prevSize - Size;
        }

        /// <inheritdoc/>
        public int Sweep(Func<V, bool> fn)
        {
            int prevSize = Size;
            if (fn is null) return prevSize;
            foreach (Item item in values)
                if (fn(item.Value)) Delete(item.Key);
            return prevSize - Size;
        }

        /// <inheritdoc/>
        public int Sweep(Func<V, K, bool> fn)
        {
            int prevSize = Size;
            if (fn is null) return prevSize;
            foreach (Item item in values)
                if (fn(item.Value, item.Key)) Delete(item.Key);
            return prevSize - Size;
        }

        /// <inheritdoc/>
        public int Sweep(Func<V, K, ICollection<K, V>, bool> fn)
        {
            int prevSize = Size;
            if (fn is null) return prevSize;
            foreach (Item item in values)
                if (fn(item.Value, item.Key, this)) Delete(item.Key);
            return prevSize - Size;
        }

        /// <summary>
        /// Identical to
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/filter">Array.Filter()</see>,
        /// but returns a Collection instead of an Array.
        /// </summary>
        /// <param name="fn">The function to test with (should return boolean)</param>
        /// <example>
        /// Collection collection;
        /// collection.Filter((User user) =&gt; user.Username == "Bob");
        /// </example>
        /// <returns></returns>
        public Collection<K, V> Filter(Func<bool> fn)
        {
            var results = new Collection<K, V>();
            if (fn is null) return results;
            foreach (Item item in values)
                if (fn()) results.Set(item.Key, item.Value);
            return results;
        }

        /// <summary>
        /// Identical to
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/filter">Array.Filter()</see>,
        /// but returns a Collection instead of an Array.
        /// </summary>
        /// <param name="fn">The function to test with (should return boolean)</param>
        /// <example>
        /// Collection collection;
        /// collection.Filter((User user) =&gt; user.Username == "Bob");
        /// </example>
        /// <returns></returns>
        public Collection<K, V> Filter(Func<V, bool> fn)
        {
            var results = new Collection<K, V>();
            if (fn is null) return results;
            foreach (Item item in values)
                if (fn(item.Value)) results.Set(item.Key, item.Value);
            return results;
        }

        /// <summary>
        /// Identical to
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/filter">Array.Filter()</see>,
        /// but returns a Collection instead of an Array.
        /// </summary>
        /// <param name="fn">The function to test with (should return boolean)</param>
        /// <example>
        /// Collection collection;
        /// collection.Filter((User user) =&gt; user.Username == "Bob");
        /// </example>
        /// <returns></returns>
        public Collection<K, V> Filter(Func<V, K, bool> fn)
        {
            var results = new Collection<K, V>();
            if (fn is null) return results;
            foreach (Item item in values)
                if (fn(item.Value, item.Key)) results.Set(item.Key, item.Value);
            return results;
        }

        /// <summary>
        /// Identical to
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/filter">Array.Filter()</see>,
        /// but returns a Collection instead of an Array.
        /// </summary>
        /// <param name="fn">The function to test with (should return boolean)</param>
        /// <example>
        /// Collection collection;
        /// collection.Filter((User user) =&gt; user.Username == "Bob");
        /// </example>
        /// <returns></returns>
        public Collection<K, V> Filter(Func<V, K, ICollection<K, V>, bool> fn)
        {
            var results = new Collection<K, V>();
            if (fn is null) return results;
            foreach (Item item in values)
                if (fn(item.Value, item.Key, this)) results.Set(item.Key, item.Value);
            return results;
        }

        /// <summary>
        /// Partitions the collection into two collections where the first collection<br/>
        /// contains the items that passed and the second contains the items that failed.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <example>
        /// Collection collection;
        /// var result = collection.Partition((Guild guild) =&gt; guild.MemberCount &gt; 250);
        /// ICollection big = result[0], small = result[1];
        /// </example>
        /// <returns>A Collection Array of length <c>2</c></returns>
        public Collection<K, V>[] Partition(Func<bool> fn)
        {
            Collection<K, V> passed = new Collection<K, V>(), failed = new Collection<K, V>();
            Collection<K, V>[] results = new Collection<K, V>[2] { passed, failed };
            if (fn is null) return results;
            foreach (Item item in values)
            {
                if (fn()) results[0].Set(item.Key, item.Value);
                else results[1].Set(item.Key, item.Value);
            }
            return results;
        }

        /// <summary>
        /// Partitions the collection into two collections where the first collection<br/>
        /// contains the items that passed and the second contains the items that failed.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <example>
        /// Collection collection;
        /// var result = collection.Partition((Guild guild) =&gt; guild.MemberCount &gt; 250);
        /// ICollection big = result[0], small = result[1];
        /// </example>
        /// <returns>A Collection Array of length <c>2</c></returns>
        public Collection<K, V>[] Partition(Func<V, bool> fn)
        {
            Collection<K, V> passed = new Collection<K, V>(), failed = new Collection<K, V>();
            Collection<K, V>[] results = new Collection<K, V>[2] { passed, failed };
            if (fn is null) return results;
            foreach (Item item in values)
            {
                if (fn(item.Value)) results[0].Set(item.Key, item.Value);
                else results[1].Set(item.Key, item.Value);
            }
            return results;
        }

        /// <summary>
        /// Partitions the collection into two collections where the first collection<br/>
        /// contains the items that passed and the second contains the items that failed.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <example>
        /// Collection collection;
        /// var result = collection.Partition((Guild guild) =&gt; guild.MemberCount &gt; 250);
        /// ICollection big = result[0], small = result[1];
        /// </example>
        /// <returns>A Collection Array of length <c>2</c></returns>
        public Collection<K, V>[] Partition(Func<V, K, bool> fn)
        {
            Collection<K, V> passed = new Collection<K, V>(), failed = new Collection<K, V>();
            Collection<K, V>[] results = new Collection<K, V>[2] { passed, failed };
            if (fn is null) return results;
            foreach (Item item in values)
            {
                if (fn(item.Value, item.Key)) results[0].Set(item.Key, item.Value);
                else results[1].Set(item.Key, item.Value);
            }
            return results;
        }

        /// <summary>
        /// Partitions the collection into two collections where the first collection<br/>
        /// contains the items that passed and the second contains the items that failed.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <example>
        /// Collection collection;
        /// var result = collection.Partition((Guild guild) =&gt; guild.MemberCount &gt; 250);
        /// ICollection big = result[0], small = result[1];
        /// </example>
        /// <returns>A Collection Array of length <c>2</c></returns>
        public Collection<K, V>[] Partition(Func<V, K, ICollection<K, V>, bool> fn)
        {
            Collection<K, V> passed = new Collection<K, V>(), failed = new Collection<K, V>();
            Collection<K, V>[] results = new Collection<K, V>[2] { passed, failed };
            if (fn is null) return results;
            foreach (Item item in values)
            {
                if (fn(item.Value, item.Key, this)) results[0].Set(item.Key, item.Value);
                else results[1].Set(item.Key, item.Value);
            }
            return results;
        }

        /// <inheritdoc/>
        public Array<T> Map<T>(Func<T> fn)
        {
            Array<T> result = new Array<T>();
            if (fn is null) return result;
            foreach (Item item in values)
                result.Push(fn());
            return result;
        }

        /// <inheritdoc/>
        public Array<T> Map<T>(Func<V, T> fn)
        {
            Array<T> result = new Array<T>();
            if (fn is null) return result;
            foreach (Item item in values)
                result.Push(fn(item.Value));
            return result;
        }

        /// <inheritdoc/>
        public Array<T> Map<T>(Func<V, K, T> fn)
        {
            Array<T> result = new Array<T>();
            if (fn is null) return result;
            foreach (Item item in values)
                result.Push(fn(item.Value, item.Key));
            return result;
        }

        /// <inheritdoc/>
        public Array<T> Map<T>(Func<V, K, ICollection<K, V>, T> fn)
        {
            Array<T> result = new Array<T>();
            if (fn is null) return result;
            foreach (Item item in values)
                result.Push(fn(item.Value, item.Key, this));
            return result;
        }

        /// <summary>
        /// Maps each item to another value into a collection. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/map">Array.Map()</see>.
        /// </summary>
        /// <typeparam name="T">The type to map to</typeparam>
        /// <param name="fn">Function that produces an element of the new collection, taking three arguments</param>
        /// <example>
        /// Collection collection;
        /// collection.MapValues(user =&gt; user.Tag);
        /// </example>
        /// <returns></returns>
        public Collection<K, T> MapValues<T>(Func<T> fn)
        {
            Collection<K, T> result = new Collection<K, T>();
            if (fn is null) return result;
            foreach (Item item in values)
                result.Set(item.Key, fn());
            return result;
        }

        /// <summary>
        /// Maps each item to another value into a collection. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/map">Array.Map()</see>.
        /// </summary>
        /// <typeparam name="T">The type to map to</typeparam>
        /// <param name="fn">Function that produces an element of the new collection, taking three arguments</param>
        /// <example>
        /// Collection collection;
        /// collection.MapValues(user =&gt; user.Tag);
        /// </example>
        /// <returns></returns>
        public Collection<K, T> MapValues<T>(Func<V, T> fn)
        {
            Collection<K, T> result = new Collection<K, T>();
            if (fn is null) return result;
            foreach (Item item in values)
                result.Set(item.Key, fn(item.Value));
            return result;
        }

        /// <summary>
        /// Maps each item to another value into a collection. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/map">Array.Map()</see>.
        /// </summary>
        /// <typeparam name="T">The type to map to</typeparam>
        /// <param name="fn">Function that produces an element of the new collection, taking three arguments</param>
        /// <example>
        /// Collection collection;
        /// collection.MapValues(user =&gt; user.Tag);
        /// </example>
        /// <returns></returns>
        public Collection<K, T> MapValues<T>(Func<V, K, T> fn)
        {
            Collection<K, T> result = new Collection<K, T>();
            if (fn is null) return result;
            foreach (Item item in values)
                result.Set(item.Key, fn(item.Value, item.Key));
            return result;
        }

        /// <summary>
        /// Maps each item to another value into a collection. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/map">Array.Map()</see>.
        /// </summary>
        /// <typeparam name="T">The type to map to</typeparam>
        /// <param name="fn">Function that produces an element of the new collection, taking three arguments</param>
        /// <example>
        /// Collection collection;
        /// collection.MapValues(user =&gt; user.Tag);
        /// </example>
        /// <returns></returns>
        public Collection<K, T> MapValues<T>(Func<V, K, ICollection<K, V>, T> fn)
        {
            Collection<K, T> result = new Collection<K, T>();
            if (fn is null) return result;
            foreach (Item item in values)
                result.Set(item.Key, fn(item.Value, item.Key, this));
            return result;
        }

        /// <inheritdoc/>
        public bool Some(Func<bool> fn)
        {
            if (fn is null) return false;
            foreach (Item item in values)
                if (fn()) return true;
            return false;
        }

        /// <inheritdoc/>
        public bool Some(Func<V, bool> fn)
        {
            if (fn is null) return false;
            foreach (Item item in values)
                if (fn(item.Value)) return true;
            return false;
        }

        /// <inheritdoc/>
        public bool Some(Func<V, K, bool> fn)
        {
            if (fn is null) return false;
            foreach (Item item in values)
                if (fn(item.Value, item.Key)) return true;
            return false;
        }

        /// <inheritdoc/>
        public bool Some(Func<V, K, ICollection<K, V>, bool> fn)
        {
            if (fn is null) return false;
            foreach (Item item in values)
                if (fn(item.Value, item.Key, this)) return true;
            return false;
        }

        /// <inheritdoc/>
        public bool Every(Func<bool> fn)
        {
            if (fn is null) return false;
            foreach (Item item in values)
                if (!fn()) return false;
            return true;
        }

        /// <inheritdoc/>
        public bool Every(Func<V, bool> fn)
        {
            if (fn is null) return false;
            foreach (Item item in values)
                if (!fn(item.Value)) return false;
            return true;
        }

        /// <inheritdoc/>
        public bool Every(Func<V, K, bool> fn)
        {
            if (fn is null) return false;
            foreach (Item item in values)
                if (!fn(item.Value, item.Key)) return false;
            return true;
        }

        /// <inheritdoc/>
        public bool Every(Func<V, K, ICollection<K, V>, bool> fn)
        {
            if (fn is null) return false;
            foreach (Item item in values)
                if (!fn(item.Value, item.Key, this)) return false;
            return true;
        }

        /// <inheritdoc/>
        public T Reduce<T>(Func<T> fn) => Reduce(fn, default);

        /// <inheritdoc/>
        public T Reduce<T>(Func<T, T> fn) => Reduce(fn, default);

        /// <inheritdoc/>
        public T Reduce<T>(Func<T, V, T> fn) => Reduce(fn, default);

        /// <inheritdoc/>
        public T Reduce<T>(Func<T, V, K, T> fn) => Reduce(fn, default);

        /// <inheritdoc/>
        public T Reduce<T>(Func<T, V, K, ICollection<K, V>, T> fn) => Reduce(fn, default);

        /// <inheritdoc/>
        public T Reduce<T>(Func<T> fn, T initialValue)
        {
            T accumulator = initialValue;
            if (fn is null) return accumulator;
            foreach (Item item in values) accumulator = fn();
            return accumulator;
        }

        /// <inheritdoc/>
        public T Reduce<T>(Func<T, T> fn, T initialValue)
        {
            T accumulator = initialValue;
            if (fn is null) return accumulator;
            foreach (Item item in values) accumulator = fn(accumulator);
            return accumulator;
        }

        /// <inheritdoc/>
        public T Reduce<T>(Func<T, V, T> fn, T initialValue)
        {
            T accumulator = initialValue;
            if (fn is null) return accumulator;
            foreach (Item item in values) accumulator = fn(accumulator, item.Value);
            return accumulator;
        }

        /// <inheritdoc/>
        public T Reduce<T>(Func<T, V, K, T> fn, T initialValue)
        {
            T accumulator = initialValue;
            if (fn is null) return accumulator;
            foreach (Item item in values) accumulator = fn(accumulator, item.Value, item.Key);
            return accumulator;
        }

        /// <inheritdoc/>
        public T Reduce<T>(Func<T, V, K, ICollection<K, V>, T> fn, T initialValue)
        {
            T accumulator = initialValue;
            if (fn is null) return accumulator;
            foreach (Item item in values) accumulator = fn(accumulator, item.Value, item.Key, this);
            return accumulator;
        }

        /// <summary>
        /// Identical to
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map/forEach">Map.ForEach()</see>,
        /// but returns the collection.
        /// </summary>
        /// <param name="fn">Function to execute for each element</param>
        /// <returns></returns>
        public Collection<K, V> Each(Action fn)
        {
            if (fn is null) return this; // Don't do work if no fn is defined
            for (int index = 0, length = values.Count; index < length; index++)
            {
                Item item = values[index];
                fn();
            }
            return this;
        }

        /// <summary>
        /// Identical to
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map/forEach">Map.ForEach()</see>,
        /// but returns the collection.
        /// </summary>
        /// <param name="fn">Function to execute for each element</param>
        /// <returns></returns>
        public Collection<K, V> Each(Action<V> fn)
        {
            if (fn is null) return this; // Don't do work if no fn is defined
            for (int index = 0, length = values.Count; index < length; index++)
            {
                Item item = values[index];
                fn(item.Value);
            }
            return this;
        }

        /// <summary>
        /// Identical to
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map/forEach">Map.ForEach()</see>,
        /// but returns the collection.
        /// </summary>
        /// <param name="fn">Function to execute for each element</param>
        /// <returns></returns>
        public Collection<K, V> Each(Action<V, K> fn)
        {
            if (fn is null) return this; // Don't do work if no fn is defined
            for (int index = 0, length = values.Count; index < length; index++)
            {
                Item item = values[index];
                fn(item.Value, item.Key);
            }
            return this;
        }

        /// <summary>
        /// Identical to
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map/forEach">Map.ForEach()</see>,
        /// but returns the collection.
        /// </summary>
        /// <param name="fn">Function to execute for each element</param>
        /// <returns></returns>
        public Collection<K, V> Each(Action<V, K, ICollection<K, V>> fn)
        {
            if (fn is null) return this; // Don't do work if no fn is defined
            for (int index = 0, length = values.Count; index < length; index++)
            {
                Item item = values[index];
                fn(item.Value, item.Key, this);
            }
            return this;
        }

        /// <summary>
        /// Runs a function on the collection and returns the collection.
        /// </summary>
        /// <param name="fn">Function to execute</param>
        /// <returns></returns>
        public Collection<K, V> Tap(Action<ICollection<K, V>> fn)
        {
            if (!(fn is null)) fn(this);
            return this;
        }

        /// <summary>
        /// Creates an identical shallow copy of this collection.
        /// </summary>
        /// <returns></returns>
        public Collection<K, V> Clone()
        {
            System.Collections.Generic.List<Item> newValues = new System.Collections.Generic.List<Item>();
            for (int index = 0, length = values.Count; index < length; index++)
                newValues.Add(values[index]);
            return new Collection<K, V>() { values = newValues };
        }

        /// <summary>
        /// Combines this collection with others into a new collection. None of the source collections are modified.
        /// </summary>
        /// <param name="collections">Collections to merge</param>
        /// <returns></returns>
        public Collection<K, V> Concat(params ICollection<K, V>[] collections)
        {
            var newColl = Clone();
            foreach (ICollection<K, V> coll in collections)
            {
                foreach (System.Collections.Generic.KeyValuePair<K, V> item in coll)
                    newColl.Set(item.Key, item.Value);
            }
            return newColl;
        }

        /// <inheritdoc/>
        public bool Equals(ICollection<K, V> collection)
        {
            if (collection is null) return false;
            else if (Size != collection.Size) return false;
            foreach (System.Collections.Generic.KeyValuePair<K, V> item in (ICollection<K, V>)this)
            {
                if (!collection.Has(item.Key) || !System.Collections.Generic.EqualityComparer<V>.Default.Equals(item.Value, collection.Get(item.Key)))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// The sort method sorts the items of a collection in place and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <returns></returns>
        public Collection<K, V> Sort() => Sort((x, y) => x.ToString().CompareTo(y.ToString()));

        /// <summary>
        /// The sort method sorts the items of a collection in place and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        public Collection<K, V> Sort(Func<int> compareFunction)
        {
            var entries = new Array<Item>(values);
            entries.Sort(new Comparison<Item>((Item a, Item b) => compareFunction()));

            // perform clean-up
            base.Clear();
            _array = null;
            _keyArray = null;

            // Set the new entries
            for (int index = 0, length = entries.Length; index < length; index++)
            {
                Item item = entries[index];
                base.Set(item.Key, item.Value);
            }

            return this;
        }

        /// <summary>
        /// The sort method sorts the items of a collection in place and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        public Collection<K, V> Sort(Func<V, int> compareFunction)
        {
            var entries = new Array<Item>(values);
            entries.Sort(new Comparison<Item>((Item a, Item b) => compareFunction(a.Value)));

            // perform clean-up
            base.Clear();
            _array = null;
            _keyArray = null;

            // Set the new entries
            for (int index = 0, length = entries.Length; index < length; index++)
            {
                Item item = entries[index];
                base.Set(item.Key, item.Value);
            }

            return this;
        }

        /// <summary>
        /// The sort method sorts the items of a collection in place and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        public Collection<K, V> Sort(Func<V, V, int> compareFunction)
        {
            var entries = new Array<Item>(values);
            entries.Sort(new Comparison<Item>((Item a, Item b) => compareFunction(a.Value, b.Value)));

            // perform clean-up
            base.Clear();
            _array = null;
            _keyArray = null;

            // Set the new entries
            for (int index = 0, length = entries.Length; index < length; index++)
            {
                Item item = entries[index];
                base.Set(item.Key, item.Value);
            }

            return this;
        }

        /// <summary>
        /// The sort method sorts the items of a collection in place and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        public Collection<K, V> Sort(Func<V, V, K, int> compareFunction)
        {
            var entries = new Array<Item>(values);
            entries.Sort(new Comparison<Item>((Item a, Item b) => compareFunction(a.Value, b.Value, a.Key)));

            // perform clean-up
            base.Clear();
            _array = null;
            _keyArray = null;

            // Set the new entries
            for (int index = 0, length = entries.Length; index < length; index++)
            {
                Item item = entries[index];
                base.Set(item.Key, item.Value);
            }

            return this;
        }

        /// <summary>
        /// The sort method sorts the items of a collection in place and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        public Collection<K, V> Sort(Func<V, V, K, K, int> compareFunction)
        {
            var entries = new Array<Item>(values);
            entries.Sort(new Comparison<Item>((Item a, Item b) => compareFunction(a.Value, b.Value, a.Key, b.Key)));

            // perform clean-up
            base.Clear();
            _array = null;
            _keyArray = null;

            // Set the new entries
            for (int index = 0, length = entries.Length; index < length; index++)
            {
                Item item = entries[index];
                base.Set(item.Key, item.Value);
            }

            return this;
        }

        /// <summary>
        /// The intersect method returns a new structure containing items where the keys are present in both original structures.
        /// </summary>
        /// <param name="other">The other Collection to filter against</param>
        /// <returns></returns>
        public Collection<K, V> Intersect(ICollection<K, V> other) => Filter((_, k) => other.Has(k));

        /// <summary>
        /// The difference method returns a new structure containing items where the key is present in one of the original structures but not the other.
        /// </summary>
        /// <param name="other">The other Collection to filter against</param>
        /// <returns></returns>
        public Collection<K, V> Difference(ICollection<K, V> other) => Filter((_, k) => !other.Has(k)).Concat(other.Filter((_, k) => !Has(k)));

        /// <summary>
        /// The sorted method sorts the items of a collection and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <returns></returns>
        public Collection<K, V> Sorted() => Clone().Sort();

        /// <summary>
        /// The sorted method sorts the items of a collection and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        public Collection<K, V> Sorted(Func<int> compareFunction) => Clone().Sort(compareFunction);

        /// <summary>
        /// The sorted method sorts the items of a collection and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        public Collection<K, V> Sorted(Func<V, int> compareFunction) => Clone().Sort(compareFunction);

        /// <summary>
        /// The sorted method sorts the items of a collection and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        public Collection<K, V> Sorted(Func<V, V, int> compareFunction) => Clone().Sort(compareFunction);

        /// <summary>
        /// The sorted method sorts the items of a collection and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        public Collection<K, V> Sorted(Func<V, V, K, int> compareFunction) => Clone().Sort(compareFunction);

        /// <summary>
        /// The sorted method sorts the items of a collection and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        public Collection<K, V> Sorted(Func<V, V, K, K, int> compareFunction) => Clone().Sort(compareFunction);

        System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<K, V>> System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<K, V>>.GetEnumerator() => GetEnumerator();
        ICollection<K, V> ICollection<K, V>.Set(K key, V value) => Set(key, value);
        ICollection<K, V> ICollection<K, V>.Filter(Func<bool> fn) => Filter(fn);
        ICollection<K, V> ICollection<K, V>.Filter(Func<V, bool> fn) => Filter(fn);
        ICollection<K, V> ICollection<K, V>.Filter(Func<V, K, bool> fn) => Filter(fn);
        ICollection<K, V> ICollection<K, V>.Filter(Func<V, K, ICollection<K, V>, bool> fn) => Filter(fn);
        ICollection<K, V>[] ICollection<K, V>.Partition(Func<bool> fn)
        {
            Collection<K, V> passed = new Collection<K, V>(), failed = new Collection<K, V>();
            ICollection<K, V>[] results = new ICollection<K, V>[2] { passed, failed };
            if (fn is null) return results;
            {
                foreach (Item item in values)
                {
                    if (fn()) results[0].Set(item.Key, item.Value);
                    else results[1].Set(item.Key, item.Value);
                }
            }
            return results;
        }
        ICollection<K, V>[] ICollection<K, V>.Partition(Func<V, bool> fn)
        {
            Collection<K, V> passed = new Collection<K, V>(), failed = new Collection<K, V>();
            ICollection<K, V>[] results = new ICollection<K, V>[2] { passed, failed };
            if (fn is null) return results;
            {
                foreach (Item item in values)
                {
                    if (fn(item.Value)) results[0].Set(item.Key, item.Value);
                    else results[1].Set(item.Key, item.Value);
                }
            }
            return results;
        }
        ICollection<K, V>[] ICollection<K, V>.Partition(Func<V, K, bool> fn)
        {
            Collection<K, V> passed = new Collection<K, V>(), failed = new Collection<K, V>();
            ICollection<K, V>[] results = new ICollection<K, V>[2] { passed, failed };
            if (fn is null) return results;
            {
                foreach (Item item in values)
                {
                    if (fn(item.Value, item.Key)) results[0].Set(item.Key, item.Value);
                    else results[1].Set(item.Key, item.Value);
                }
            }
            return results;
        }
        ICollection<K, V>[] ICollection<K, V>.Partition(Func<V, K, ICollection<K, V>, bool> fn)
        {
            Collection<K, V> passed = new Collection<K, V>(), failed = new Collection<K, V>();
            ICollection<K, V>[] results = new ICollection<K, V>[2] { passed, failed };
            if (fn is null) return results;
            {
                foreach (Item item in values)
                {
                    if (fn(item.Value, item.Key, this)) results[0].Set(item.Key, item.Value);
                    else results[1].Set(item.Key, item.Value);
                }
            }
            return results;
        }
        ICollection<K, T> ICollection<K, V>.MapValues<T>(Func<T> fn) => MapValues(fn);
        ICollection<K, T> ICollection<K, V>.MapValues<T>(Func<V, T> fn) => MapValues(fn);
        ICollection<K, T> ICollection<K, V>.MapValues<T>(Func<V, K, T> fn) => MapValues(fn);
        ICollection<K, T> ICollection<K, V>.MapValues<T>(Func<V, K, ICollection<K, V>, T> fn) => MapValues(fn);
        ICollection<K, V> ICollection<K, V>.Each(Action fn) => Each(fn);
        ICollection<K, V> ICollection<K, V>.Each(Action<V> fn) => Each(fn);
        ICollection<K, V> ICollection<K, V>.Each(Action<V, K> fn) => Each(fn);
        ICollection<K, V> ICollection<K, V>.Each(Action<V, K, ICollection<K, V>> fn) => Each(fn);
        ICollection<K, V> ICollection<K, V>.Tap(Action<ICollection<K, V>> fn) => Tap(fn);
        ICollection<K, V> ICollection<K, V>.Clone() => Clone();
        ICollection<K, V> ICollection<K, V>.Concat(params ICollection<K, V>[] collections) => Concat(collections);
        ICollection<K, V> ICollection<K, V>.Sort() => Sort();
        ICollection<K, V> ICollection<K, V>.Sort(Func<int> fn) => Sort(fn);
        ICollection<K, V> ICollection<K, V>.Sort(Func<V, int> fn) => Sort(fn);
        ICollection<K, V> ICollection<K, V>.Sort(Func<V, V, int> fn) => Sort(fn);
        ICollection<K, V> ICollection<K, V>.Sort(Func<V, V, K, int> fn) => Sort(fn);
        ICollection<K, V> ICollection<K, V>.Sort(Func<V, V, K, K, int> fn) => Sort(fn);
        ICollection<K, V> ICollection<K, V>.Intersect(ICollection<K, V> other) => Intersect(other);
        ICollection<K, V> ICollection<K, V>.Difference(ICollection<K, V> other) => Difference(other);
        ICollection<K, V> ICollection<K, V>.Sorted() => Sort();
        ICollection<K, V> ICollection<K, V>.Sorted(Func<int> fn) => Sorted(fn);
        ICollection<K, V> ICollection<K, V>.Sorted(Func<V, int> fn) => Sorted(fn);
        ICollection<K, V> ICollection<K, V>.Sorted(Func<V, V, int> fn) => Sorted(fn);
        ICollection<K, V> ICollection<K, V>.Sorted(Func<V, V, K, int> fn) => Sorted(fn);
        ICollection<K, V> ICollection<K, V>.Sorted(Func<V, V, K, K, int> fn) => Sorted(fn);
    }
}