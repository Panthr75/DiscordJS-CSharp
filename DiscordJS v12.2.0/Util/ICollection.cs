using JavaScript;
using System;
using System.Collections.Generic;

namespace DiscordJS
{
    /// <summary>
    /// Represents a Collection
    /// </summary>
    public interface ICollection<K, V> : IEnumerable<KeyValuePair<K, V>>
    {
        /// <summary>
        /// Behaves Identical to <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map/get">Map.get()</see>.
        /// <br/>
        /// Gets an element with the specified key, and returns its value, or <c>default</c> if the element does not exist.
        /// </summary>
        /// <param name="key">The key to get from this collection</param>
        /// <returns></returns>
        V Get(K key);

        /// <summary>
        /// Behaves Identical to <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map/has">Map.has()</see>.
        /// <br/>
        /// Checks if an element exists in the collection.
        /// </summary>
        /// <param name="key">The key of the element to check for</param>
        /// <returns><c>true</c> if the element exists, <c>false</c> if it does not exist.</returns>
        bool Has(K key);

        /// <summary>
        /// Behaves Identical to <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map/delete">Map.delete()</see>.
        /// <br/>
        /// Deletes an element from the collection.
        /// </summary>
        /// <param name="key">The key to delete from the collection</param>
        /// <returns><c>true</c> if the element was remove, <c>false</c> if it does not exist.</returns>
        bool Delete(K key);

        /// <summary>
        /// Behaves Identical to <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map/clear">Map.clear()</see>.
        /// <br/>
        /// Removes all elements from the collection.
        /// </summary>
        void Clear();

        /// <summary>
        /// Behaves Identical to <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map/set">Map.set()</see>.
        /// <br/>
        /// Sets a new element in the collection with the specified key and value.
        /// </summary>
        /// <param name="key">The key of the element to add</param>
        /// <param name="value">The value of the element to add</param>
        /// <returns></returns>
        ICollection<K, V> Set(K key, V value);

        /// <summary>
        /// Creates an ordered array of the values of this collection, and caches it internally. The array will only be<br/>
        /// reconstructed if an item is added to or removed from the collection, or if you change the length of the array<br/>
        /// itself. If you don't want this caching behavior, use <c>Array&lt;V&gt;.From(collection.Values())</c> instead.
        /// </summary>
        /// <returns></returns>
        Array<V> Array();

        /// <summary>
        /// Creates an ordered array of the keys of this collection, and caches it internally. The array will only be<br/>
        /// reconstructed if an item is added to or removed from the collection, or if you change the length of the array<br/>
        /// itself. If you don't want this caching behavior, use <c>Array&lt;K&gt;.From(collection.Keys())</c> instead.
        /// </summary>
        /// <returns></returns>
        Array<K> KeyArray();

        /// <summary>
        /// Obtains the first value(s) in this collection.
        /// </summary>
        /// <returns>A single value if no amount is provided or an array of values, starting from the end if<br/>
        /// amount is negative</returns>
        V First();

        /// <summary>
        /// Obtains the first value(s) in this collection.
        /// </summary>
        /// <param name="amount">Amount of values to obtain from the beginning</param>
        /// <returns>A single value if no amount is provided or an array of values, starting from the end if<br/>
        /// amount is negative</returns>
        Array<V> First(int amount);

        /// <summary>
        /// Obtains the first key(s) in this collection.
        /// </summary>
        /// <returns>A single key if no amount is provided or an array of keys, starting from the end if<br/>
        /// amount is negative</returns>
        K FirstKey();

        /// <summary>
        /// Obtains the first key(s) in this collection.
        /// </summary>
        /// <param name="amount">Amount of keys to obtain from the beginning</param>
        /// <returns>A single key if no amount is provided or an array of keys, starting from the end if<br/>
        /// amount is negative</returns>
        Array<K> FirstKey(int amount);

        /// <summary>
        /// Obtains the last value(s) in this collection.
        /// </summary>
        /// <returns>A single value if no amount is provided or an array of values, starting from the start if<br/>
        /// amount is negative</returns>
        V Last();

        /// <summary>
        /// Obtains the last value(s) in this collection.
        /// </summary>
        /// <param name="amount">Amount of values to obtain from the end</param>
        /// <returns>A single value if no amount is provided or an array of values, starting from the start if<br/>
        /// amount is negative</returns>
        Array<V> Last(int amount);

        /// <summary>
        /// Obtains the last key(s) in this collection.
        /// </summary>
        /// <returns>A single key if no amount is provided or an array of keys, starting from the start if<br/>
        /// amount is negative</returns>
        K LastKey();

        /// <summary>
        /// Obtains the last key(s) in this collection.
        /// </summary>
        /// <param name="amount">Amount of keys to obtain from the end</param>
        /// <returns>A single key if no amount is provided or an array of keys, starting from the start if<br/>
        /// amount is negative</returns>
        Array<K> LastKey(int amount);

        /// <summary>
        /// Obtains unique random value(s) from this collection.
        /// </summary>
        /// <returns>A single value if no amount is provided or an array of values</returns>
        V Random();

        /// <summary>
        /// Obtains unique random value(s) from this collection.
        /// </summary>
        /// <param name="amount">Amount of values to obtain randomly</param>
        /// <returns>A single value if no amount is provided or an array of values</returns>
        Array<V> Random(int amount);

        /// <summary>
        /// Obtains unique random key(s) from this collection.
        /// </summary>
        /// <returns>A single key if no amount is provided or an array of keys</returns>
        K RandomKey();

        /// <summary>
        /// Obtains unique random key(s) from this collection.
        /// </summary>
        /// <param name="amount">Amount of keys to obtain randomly</param>
        /// <returns>A single key if no amount is provided or an array of keys</returns>
        Array<K> RandomKey(int amount);

        /// <summary>
        /// Searches for a single item where the given function returns a truthy value. This behaves like<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/find">Array.Find()</see>.<br/><br/>
        /// <warn><b>All collections used in DiscordJS are mapped using their <c>ID</c> property, and if you want to find by id you<br/>
        /// should use the <c>Get</c> method. See
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map/get">MDN</see> for details.</b></warn>
        /// </summary>
        /// <param name="fn">The function to test with (should return boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.Find((User user) =&gt; user.Username == "Bob");
        /// </example>
        /// <returns><see langword="default"/> if <paramref name="fn"/> is <see langword="null"/>, otherwise <typeparamref name="V"/></returns>
        V Find(Func<bool> fn);

        /// <summary>
        /// Searches for a single item where the given function returns a truthy value. This behaves like<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/find">Array.Find()</see>.<br/><br/>
        /// <warn><b>All collections used in DiscordJS are mapped using their <c>ID</c> property, and if you want to find by id you<br/>
        /// should use the <c>Get</c> method. See
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map/get">MDN</see> for details.</b></warn>
        /// </summary>
        /// <param name="fn">The function to test with (should return boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.Find((User user) =&gt; user.Username == "Bob");
        /// </example>
        /// <returns><see langword="default"/> if <paramref name="fn"/> is <see langword="null"/>, otherwise <typeparamref name="V"/></returns>
        V Find(Func<V, bool> fn);

        /// <summary>
        /// Searches for a single item where the given function returns a truthy value. This behaves like<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/find">Array.Find()</see>.<br/><br/>
        /// <warn><b>All collections used in DiscordJS are mapped using their <c>ID</c> property, and if you want to find by id you<br/>
        /// should use the <c>Get</c> method. See
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map/get">MDN</see> for details.</b></warn>
        /// </summary>
        /// <param name="fn">The function to test with (should return boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.Find((User user) =&gt; user.Username == "Bob");
        /// </example>
        /// <returns><see langword="default"/> if <paramref name="fn"/> is <see langword="null"/>, otherwise <typeparamref name="V"/></returns>
        V Find(Func<V, K, bool> fn);

        /// <summary>
        /// Searches for a single item where the given function returns a truthy value. This behaves like<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/find">Array.Find()</see>.<br/><br/>
        /// <warn><b>All collections used in DiscordJS are mapped using their <c>ID</c> property, and if you want to find by id you<br/>
        /// should use the <c>Get</c> method. See
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map/get">MDN</see> for details.</b></warn>
        /// </summary>
        /// <param name="fn">The function to test with (should return boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.Find((User user) =&gt; user.Username == "Bob");
        /// </example>
        /// <returns><see langword="default"/> if <paramref name="fn"/> is <see langword="null"/>, otherwise <typeparamref name="V"/></returns>
        V Find(Func<V, K, ICollection<K, V>, bool> fn);

        /// <summary>
        /// Searches for a single item where the given function returns a truthy value. This behaves like<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/findIndex">Array.FindIndex()</see><br/>
        /// but returns the key rather than the positional index.
        /// </summary>
        /// <param name="fn">The function to test with (should return boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.FindKey((User user) =&gt; user.Username == "Bob");
        /// </example>
        /// <returns><see langword="default"/> if <paramref name="fn"/> is <see langword="null"/>, otherwise <typeparamref name="K"/></returns>
        K FindKey(Func<bool> fn);

        /// <summary>
        /// Searches for a single item where the given function returns a truthy value. This behaves like<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/findIndex">Array.FindIndex()</see><br/>
        /// but returns the key rather than the positional index.
        /// </summary>
        /// <param name="fn">The function to test with (should return boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.FindKey((User user) =&gt; user.Username == "Bob");
        /// </example>
        /// <returns><see langword="default"/> if <paramref name="fn"/> is <see langword="null"/>, otherwise <typeparamref name="K"/></returns>
        K FindKey(Func<V, bool> fn);

        /// <summary>
        /// Searches for a single item where the given function returns a truthy value. This behaves like<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/findIndex">Array.FindIndex()</see><br/>
        /// but returns the key rather than the positional index.
        /// </summary>
        /// <param name="fn">The function to test with (should return boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.FindKey((User user) =&gt; user.Username == "Bob");
        /// </example>
        /// <returns><see langword="default"/> if <paramref name="fn"/> is <see langword="null"/>, otherwise <typeparamref name="K"/></returns>
        K FindKey(Func<V, K, bool> fn);

        /// <summary>
        /// Searches for a single item where the given function returns a truthy value. This behaves like<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/findIndex">Array.FindIndex()</see><br/>
        /// but returns the key rather than the positional index.
        /// </summary>
        /// <param name="fn">The function to test with (should return boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.FindKey((User user) =&gt; user.Username == "Bob");
        /// </example>
        /// <returns><see langword="default"/> if <paramref name="fn"/> is <see langword="null"/>, otherwise <typeparamref name="K"/></returns>
        K FindKey(Func<V, K, ICollection<K, V>, bool> fn);

        /// <summary>
        /// Removes items that satisfy the provided filter function.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <returns>The number of removed entries</returns>
        int Sweep(Func<bool> fn);

        /// <summary>
        /// Removes items that satisfy the provided filter function.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <returns>The number of removed entries</returns>
        int Sweep(Func<V, bool> fn);

        /// <summary>
        /// Removes items that satisfy the provided filter function.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <returns>The number of removed entries</returns>
        int Sweep(Func<V, K, bool> fn);

        /// <summary>
        /// Removes items that satisfy the provided filter function.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <returns>The number of removed entries</returns>
        int Sweep(Func<V, K, ICollection<K, V>, bool> fn);

        /// <summary>
        /// Identical to
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/filter">Array.Filter()</see>,
        /// but returns a Collection instead of an Array.
        /// </summary>
        /// <param name="fn">The function to test with (should return boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.Filter((User user) =&gt; user.Username == "Bob");
        /// </example>
        /// <returns></returns>
        ICollection<K, V> Filter(Func<bool> fn);

        /// <summary>
        /// Identical to
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/filter">Array.Filter()</see>,
        /// but returns a Collection instead of an Array.
        /// </summary>
        /// <param name="fn">The function to test with (should return boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.Filter((User user) =&gt; user.Username == "Bob");
        /// </example>
        /// <returns></returns>
        ICollection<K, V> Filter(Func<V, bool> fn);

        /// <summary>
        /// Identical to
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/filter">Array.Filter()</see>,
        /// but returns a Collection instead of an Array.
        /// </summary>
        /// <param name="fn">The function to test with (should return boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.Filter((User user) =&gt; user.Username == "Bob");
        /// </example>
        /// <returns></returns>
        ICollection<K, V> Filter(Func<V, K, bool> fn);

        /// <summary>
        /// Identical to
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/filter">Array.Filter()</see>,
        /// but returns a Collection instead of an Array.
        /// </summary>
        /// <param name="fn">The function to test with (should return boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.Filter((User user) =&gt; user.Username == "Bob");
        /// </example>
        /// <returns></returns>
        ICollection<K, V> Filter(Func<V, K, ICollection<K, V>, bool> fn);

        /// <summary>
        /// Partitions the collection into two collections where the first collection<br/>
        /// contains the items that passed and the second contains the items that failed.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <example>
        /// ICollection collection;
        /// var result = collection.Partition((Guild guild) =&gt; guild.MemberCount &gt; 250);
        /// ICollection big = result[0], small = result[1];
        /// </example>
        /// <returns>A Collection Array of length <c>2</c></returns>
        ICollection<K, V>[] Partition(Func<bool> fn);

        /// <summary>
        /// Partitions the collection into two collections where the first collection<br/>
        /// contains the items that passed and the second contains the items that failed.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <example>
        /// ICollection collection;
        /// var result = collection.Partition((Guild guild) =&gt; guild.MemberCount &gt; 250);
        /// ICollection big = result[0], small = result[1];
        /// </example>
        /// <returns>A Collection Array of length <c>2</c></returns>
        ICollection<K, V>[] Partition(Func<V, bool> fn);

        /// <summary>
        /// Partitions the collection into two collections where the first collection<br/>
        /// contains the items that passed and the second contains the items that failed.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <example>
        /// ICollection collection;
        /// var result = collection.Partition((Guild guild) =&gt; guild.MemberCount &gt; 250);
        /// ICollection big = result[0], small = result[1];
        /// </example>
        /// <returns>A Collection Array of length <c>2</c></returns>
        ICollection<K, V>[] Partition(Func<V, K, bool> fn);

        /// <summary>
        /// Partitions the collection into two collections where the first collection<br/>
        /// contains the items that passed and the second contains the items that failed.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <example>
        /// ICollection collection;
        /// var result = collection.Partition((Guild guild) =&gt; guild.MemberCount &gt; 250);
        /// ICollection big = result[0], small = result[1];
        /// </example>
        /// <returns>A Collection Array of length <c>2</c></returns>
        ICollection<K, V>[] Partition(Func<V, K, ICollection<K, V>, bool> fn);

        /// <summary>
        /// Maps each item to another value into an array. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/map">Array.Map()</see>.
        /// </summary>
        /// <typeparam name="T">The type to map to</typeparam>
        /// <param name="fn">Function that produces an element of the new array, taking three arguments</param>
        /// <example>
        /// ICollection collection;
        /// collection.Map&lt;string&gt;((User user) =&gt; user.Tag);
        /// </example>
        /// <returns></returns>
        Array<T> Map<T>(Func<T> fn);

        /// <summary>
        /// Maps each item to another value into an array. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/map">Array.Map()</see>.
        /// </summary>
        /// <typeparam name="T">The type to map to</typeparam>
        /// <param name="fn">Function that produces an element of the new array, taking three arguments</param>
        /// <example>
        /// ICollection collection;
        /// collection.Map&lt;string&gt;((User user) =&gt; user.Tag);
        /// </example>
        /// <returns></returns>
        Array<T> Map<T>(Func<V, T> fn);

        /// <summary>
        /// Maps each item to another value into an array. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/map">Array.Map()</see>.
        /// </summary>
        /// <typeparam name="T">The type to map to</typeparam>
        /// <param name="fn">Function that produces an element of the new array, taking three arguments</param>
        /// <example>
        /// ICollection collection;
        /// collection.Map&lt;string&gt;((User user) =&gt; user.Tag);
        /// </example>
        /// <returns></returns>
        Array<T> Map<T>(Func<V, K, T> fn);

        /// <summary>
        /// Maps each item to another value into an array. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/map">Array.Map()</see>.
        /// </summary>
        /// <typeparam name="T">The type to map to</typeparam>
        /// <param name="fn">Function that produces an element of the new array, taking three arguments</param>
        /// <example>
        /// ICollection collection;
        /// collection.Map&lt;string&gt;((User user) =&gt; user.Tag);
        /// </example>
        /// <returns></returns>
        Array<T> Map<T>(Func<V, K, ICollection<K, V>, T> fn);

        /// <summary>
        /// Maps each item to another value into a collection. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/map">Array.Map()</see>.
        /// </summary>
        /// <typeparam name="T">The type to map to</typeparam>
        /// <param name="fn">Function that produces an element of the new collection, taking three arguments</param>
        /// <example>
        /// ICollection collection;
        /// collection.MapValues(user =&gt; user.Tag);
        /// </example>
        /// <returns></returns>
        ICollection<K, T> MapValues<T>(Func<T> fn);

        /// <summary>
        /// Maps each item to another value into a collection. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/map">Array.Map()</see>.
        /// </summary>
        /// <typeparam name="T">The type to map to</typeparam>
        /// <param name="fn">Function that produces an element of the new collection, taking three arguments</param>
        /// <example>
        /// ICollection collection;
        /// collection.MapValues(user =&gt; user.Tag);
        /// </example>
        /// <returns></returns>
        ICollection<K, T> MapValues<T>(Func<V, T> fn);

        /// <summary>
        /// Maps each item to another value into a collection. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/map">Array.Map()</see>.
        /// </summary>
        /// <typeparam name="T">The type to map to</typeparam>
        /// <param name="fn">Function that produces an element of the new collection, taking three arguments</param>
        /// <example>
        /// ICollection collection;
        /// collection.MapValues(user =&gt; user.Tag);
        /// </example>
        /// <returns></returns>
        ICollection<K, T> MapValues<T>(Func<V, K, T> fn);

        /// <summary>
        /// Maps each item to another value into a collection. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/map">Array.Map()</see>.
        /// </summary>
        /// <typeparam name="T">The type to map to</typeparam>
        /// <param name="fn">Function that produces an element of the new collection, taking three arguments</param>
        /// <example>
        /// ICollection collection;
        /// collection.MapValues(user =&gt; user.Tag);
        /// </example>
        /// <returns></returns>
        ICollection<K, T> MapValues<T>(Func<V, K, ICollection<K, V>, T> fn);

        /// <summary>
        /// Checks if there exists an item that passes a test. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/some">Array.Some()</see>.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.Some((User user) =&gt; user.Discriminator == "0000");
        /// </example>
        /// <returns></returns>
        bool Some(Func<bool> fn);

        /// <summary>
        /// Checks if there exists an item that passes a test. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/some">Array.Some()</see>.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.Some((User user) =&gt; user.Discriminator == "0000");
        /// </example>
        /// <returns></returns>
        bool Some(Func<V, bool> fn);

        /// <summary>
        /// Checks if there exists an item that passes a test. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/some">Array.Some()</see>.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.Some((User user) =&gt; user.Discriminator == "0000");
        /// </example>
        /// <returns></returns>
        bool Some(Func<V, K, bool> fn);

        /// <summary>
        /// Checks if there exists an item that passes a test. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/some">Array.Some()</see>.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.Some((User user) =&gt; user.Discriminator == "0000");
        /// </example>
        /// <returns></returns>
        bool Some(Func<V, K, ICollection<K, V>, bool> fn);

        /// <summary>
        /// Checks if all items passes a test. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/every">Array.Every()</see>.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.Every((User user) =&gt; !user.Bot);
        /// </example>
        /// <returns></returns>
        bool Every(Func<bool> fn);

        /// <summary>
        /// Checks if all items passes a test. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/every">Array.Every()</see>.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.Every((User user) =&gt; !user.Bot);
        /// </example>
        /// <returns></returns>
        bool Every(Func<V, bool> fn);

        /// <summary>
        /// Checks if all items passes a test. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/every">Array.Every()</see>.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.Every((User user) =&gt; !user.Bot);
        /// </example>
        /// <returns></returns>
        bool Every(Func<V, K, bool> fn);

        /// <summary>
        /// Checks if all items passes a test. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/every">Array.Every()</see>.
        /// </summary>
        /// <param name="fn">Function used to test (should return a boolean)</param>
        /// <example>
        /// ICollection collection;
        /// collection.Every((User user) =&gt; !user.Bot);
        /// </example>
        /// <returns></returns>
        bool Every(Func<V, K, ICollection<K, V>, bool> fn);

        /// <summary>
        /// Applies a function to produce a single value. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/reduce">Array.Reduce()</see>.
        /// </summary>
        /// <typeparam name="T">The type to reduce to</typeparam>
        /// <param name="fn">Function used to reduce, taking four arguments; <c>accumulator</c>, <c>currentValue</c>, <c>currentKey</c>,<br/>
        /// and <c>collection</c></param>
        /// <example>
        /// ICollection collection;
        /// collection.Reduce&lt;int&gt;((int acc, Guild guild) =&gt; acc + guild.memberCount, 0);
        /// </example>
        /// <returns></returns>
        T Reduce<T>(Func<T> fn);

        /// <summary>
        /// Applies a function to produce a single value. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/reduce">Array.Reduce()</see>.
        /// </summary>
        /// <typeparam name="T">The type to reduce to</typeparam>
        /// <param name="fn">Function used to reduce, taking four arguments; <c>accumulator</c>, <c>currentValue</c>, <c>currentKey</c>,<br/>
        /// and <c>collection</c></param>
        /// <example>
        /// ICollection collection;
        /// collection.Reduce&lt;int&gt;((int acc, Guild guild) =&gt; acc + guild.memberCount, 0);
        /// </example>
        /// <returns></returns>
        T Reduce<T>(Func<T, T> fn);

        /// <summary>
        /// Applies a function to produce a single value. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/reduce">Array.Reduce()</see>.
        /// </summary>
        /// <typeparam name="T">The type to reduce to</typeparam>
        /// <param name="fn">Function used to reduce, taking four arguments; <c>accumulator</c>, <c>currentValue</c>, <c>currentKey</c>,<br/>
        /// and <c>collection</c></param>
        /// <example>
        /// ICollection collection;
        /// collection.Reduce&lt;int&gt;((int acc, Guild guild) =&gt; acc + guild.memberCount, 0);
        /// </example>
        /// <returns></returns>
        T Reduce<T>(Func<T, V, T> fn);

        /// <summary>
        /// Applies a function to produce a single value. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/reduce">Array.Reduce()</see>.
        /// </summary>
        /// <typeparam name="T">The type to reduce to</typeparam>
        /// <param name="fn">Function used to reduce, taking four arguments; <c>accumulator</c>, <c>currentValue</c>, <c>currentKey</c>,<br/>
        /// and <c>collection</c></param>
        /// <example>
        /// ICollection collection;
        /// collection.Reduce&lt;int&gt;((int acc, Guild guild) =&gt; acc + guild.memberCount, 0);
        /// </example>
        /// <returns></returns>
        T Reduce<T>(Func<T, V, K, T> fn);

        /// <summary>
        /// Applies a function to produce a single value. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/reduce">Array.Reduce()</see>.
        /// </summary>
        /// <typeparam name="T">The type to reduce to</typeparam>
        /// <param name="fn">Function used to reduce, taking four arguments; <c>accumulator</c>, <c>currentValue</c>, <c>currentKey</c>,<br/>
        /// and <c>collection</c></param>
        /// <example>
        /// ICollection collection;
        /// collection.Reduce&lt;int&gt;((int acc, Guild guild) =&gt; acc + guild.memberCount, 0);
        /// </example>
        /// <returns></returns>
        T Reduce<T>(Func<T, V, K, ICollection<K, V>, T> fn);

        /// <summary>
        /// Applies a function to produce a single value. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/reduce">Array.Reduce()</see>.
        /// </summary>
        /// <typeparam name="T">The type to reduce to</typeparam>
        /// <param name="fn">Function used to reduce, taking four arguments; <c>accumulator</c>, <c>currentValue</c>, <c>currentKey</c>,<br/>
        /// and <c>collection</c></param>
        /// <param name="initialValue">Starting value for the accumulator</param>
        /// <example>
        /// ICollection collection;
        /// collection.Reduce&lt;int&gt;((int acc, Guild guild) =&gt; acc + guild.memberCount, 0);
        /// </example>
        /// <returns></returns>
        T Reduce<T>(Func<T> fn, T initialValue);

        /// <summary>
        /// Applies a function to produce a single value. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/reduce">Array.Reduce()</see>.
        /// </summary>
        /// <typeparam name="T">The type to reduce to</typeparam>
        /// <param name="fn">Function used to reduce, taking four arguments; <c>accumulator</c>, <c>currentValue</c>, <c>currentKey</c>,<br/>
        /// and <c>collection</c></param>
        /// <param name="initialValue">Starting value for the accumulator</param>
        /// <example>
        /// ICollection collection;
        /// collection.Reduce&lt;int&gt;((int acc, Guild guild) =&gt; acc + guild.memberCount, 0);
        /// </example>
        /// <returns></returns>
        T Reduce<T>(Func<T, T> fn, T initialValue);

        /// <summary>
        /// Applies a function to produce a single value. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/reduce">Array.Reduce()</see>.
        /// </summary>
        /// <typeparam name="T">The type to reduce to</typeparam>
        /// <param name="fn">Function used to reduce, taking four arguments; <c>accumulator</c>, <c>currentValue</c>, <c>currentKey</c>,<br/>
        /// and <c>collection</c></param>
        /// <param name="initialValue">Starting value for the accumulator</param>
        /// <example>
        /// ICollection collection;
        /// collection.Reduce&lt;int&gt;((int acc, Guild guild) =&gt; acc + guild.memberCount, 0);
        /// </example>
        /// <returns></returns>
        T Reduce<T>(Func<T, V, T> fn, T initialValue);

        /// <summary>
        /// Applies a function to produce a single value. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/reduce">Array.Reduce()</see>.
        /// </summary>
        /// <typeparam name="T">The type to reduce to</typeparam>
        /// <param name="fn">Function used to reduce, taking four arguments; <c>accumulator</c>, <c>currentValue</c>, <c>currentKey</c>,<br/>
        /// and <c>collection</c></param>
        /// <param name="initialValue">Starting value for the accumulator</param>
        /// <example>
        /// ICollection collection;
        /// collection.Reduce&lt;int&gt;((int acc, Guild guild) =&gt; acc + guild.memberCount, 0);
        /// </example>
        /// <returns></returns>
        T Reduce<T>(Func<T, V, K, T> fn, T initialValue);

        /// <summary>
        /// Applies a function to produce a single value. Identical in behavior to<br/>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/reduce">Array.Reduce()</see>.
        /// </summary>
        /// <typeparam name="T">The type to reduce to</typeparam>
        /// <param name="fn">Function used to reduce, taking four arguments; <c>accumulator</c>, <c>currentValue</c>, <c>currentKey</c>,<br/>
        /// and <c>collection</c></param>
        /// <param name="initialValue">Starting value for the accumulator</param>
        /// <example>
        /// ICollection collection;
        /// collection.Reduce&lt;int&gt;((int acc, Guild guild) =&gt; acc + guild.memberCount, 0);
        /// </example>
        /// <returns></returns>
        T Reduce<T>(Func<T, V, K, ICollection<K, V>, T> fn, T initialValue);

        /// <summary>
        /// Identical to
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map/forEach">Map.ForEach()</see>,
        /// but returns the collection.
        /// </summary>
        /// <param name="fn">Function to execute for each element</param>
        /// <returns></returns>
        ICollection<K, V> Each(Action fn);

        /// <summary>
        /// Identical to
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map/forEach">Map.ForEach()</see>,
        /// but returns the collection.
        /// </summary>
        /// <param name="fn">Function to execute for each element</param>
        /// <returns></returns>
        ICollection<K, V> Each(Action<V> fn);

        /// <summary>
        /// Identical to
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map/forEach">Map.ForEach()</see>,
        /// but returns the collection.
        /// </summary>
        /// <param name="fn">Function to execute for each element</param>
        /// <returns></returns>
        ICollection<K, V> Each(Action<V, K> fn);

        /// <summary>
        /// Identical to
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map/forEach">Map.ForEach()</see>,
        /// but returns the collection.
        /// </summary>
        /// <param name="fn">Function to execute for each element</param>
        /// <returns></returns>
        ICollection<K, V> Each(Action<V, K, ICollection<K, V>> fn);

        /// <summary>
        /// Runs a function on the collection and returns the collection.
        /// </summary>
        /// <param name="fn">Function to execute</param>
        /// <returns></returns>
        ICollection<K, V> Tap(Action<ICollection<K, V>> fn);

        /// <summary>
        /// Creates an identical shallow copy of this collection.
        /// </summary>
        /// <returns></returns>
        ICollection<K, V> Clone();

        /// <summary>
        /// Combines this collection with others into a new collection. None of the source collections are modified.
        /// </summary>
        /// <param name="collections">Collections to merge</param>
        /// <returns></returns>
        ICollection<K, V> Concat(params ICollection<K, V>[] collections);

        /// <summary>
        /// Checks if this collection shares identical items with another.<br/>
        /// This is different to checking for equality using equal-signs, because<br/>
        /// the collections may be different objects, but contain the same data.
        /// </summary>
        /// <param name="collection">Collection to compare with</param>
        /// <returns>Whether the collections have identical contents</returns>
        bool Equals(ICollection<K, V> collection);

        /// <summary>
        /// The sort method sorts the items of a collection in place and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <returns></returns>
        ICollection<K, V> Sort();

        /// <summary>
        /// The sort method sorts the items of a collection in place and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        ICollection<K, V> Sort(Func<int> compareFunction);

        /// <summary>
        /// The sort method sorts the items of a collection in place and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        ICollection<K, V> Sort(Func<V, int> compareFunction);

        /// <summary>
        /// The sort method sorts the items of a collection in place and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        ICollection<K, V> Sort(Func<V, V, int> compareFunction);

        /// <summary>
        /// The sort method sorts the items of a collection in place and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        ICollection<K, V> Sort(Func<V, V, K, int> compareFunction);

        /// <summary>
        /// The sort method sorts the items of a collection in place and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        ICollection<K, V> Sort(Func<V, V, K, K, int> compareFunction);

        /// <summary>
        /// The intersect method returns a new structure containing items where the keys are present in both original structures.
        /// </summary>
        /// <param name="other">The other Collection to filter against</param>
        /// <returns></returns>
        ICollection<K, V> Intersect(ICollection<K, V> other);

        /// <summary>
        /// The difference method returns a new structure containing items where the key is present in one of the original structures but not the other.
        /// </summary>
        /// <param name="other">The other Collection to filter against</param>
        /// <returns></returns>
        ICollection<K, V> Difference(ICollection<K, V> other);

        /// <summary>
        /// The sorted method sorts the items of a collection and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        ICollection<K, V> Sorted();

        /// <summary>
        /// The sorted method sorts the items of a collection and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        ICollection<K, V> Sorted(Func<int> compareFunction);

        /// <summary>
        /// The sorted method sorts the items of a collection and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        ICollection<K, V> Sorted(Func<V, int> compareFunction);

        /// <summary>
        /// The sorted method sorts the items of a collection and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        ICollection<K, V> Sorted(Func<V, V, int> compareFunction);

        /// <summary>
        /// The sorted method sorts the items of a collection and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        ICollection<K, V> Sorted(Func<V, V, K, int> compareFunction);

        /// <summary>
        /// The sorted method sorts the items of a collection and returns it.<br/>
        /// The default sort order is according to string Unicode code points.
        /// </summary>
        /// <param name="compareFunction">Specifies a function that defines the sort order.<br/>
        /// If omitted, the collection is sorted according to each character's Unicode code point value,<br/>
        /// according to the string conversion of each element.</param>
        /// <returns></returns>
        ICollection<K, V> Sorted(Func<V, V, K, K, int> compareFunction);

        /// <summary>
        /// The size of the collection.
        /// </summary>
        int Size { get; }
    }
}