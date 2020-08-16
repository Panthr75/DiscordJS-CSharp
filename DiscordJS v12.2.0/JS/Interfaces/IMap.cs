using System;
using System.Collections.Generic;

namespace JavaScript.Interfaces
{
    /// <summary>
    /// An interface for objects that want to be a ECMAScript Compliant JavaScript Map
    /// </summary>
    /// <typeparam name="K">The type of key to use</typeparam>
    /// <typeparam name="V">The type of value to use</typeparam>
    public interface IMap<K, V>
    {
        /// <summary>
        /// Clears a Map
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes a specified key-value pair from the map using the given key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns></returns>
        bool Delete(K key);

        /// <summary>
        /// Gets the entry iterator for this map
        /// </summary>
        /// <returns></returns>
        IEnumerator<Map<K, V>.Item> Entries();

        /// <summary>
        /// Loops through every key-value pair in the map, and runs the given callback function, providing the map, the key, and the value
        /// </summary>
        /// <param name="callbackfn">The callback function to run</param>
        void ForEach(Action callbackfn);

        /// <summary>
        /// Loops through every key-value pair in the map, and runs the given callback function, providing the map, the key, and the value
        /// </summary>
        /// <param name="callbackfn">The callback function to run</param>
        void ForEach(Action<V> callbackfn);

        /// <summary>
        /// Loops through every key-value pair in the map, and runs the given callback function, providing the map, the key, and the value
        /// </summary>
        /// <param name="callbackfn">The callback function to run</param>
        void ForEach(Action<V, K> callbackfn);

        /// <summary>
        /// Loops through every key-value pair in the map, and runs the given callback function, providing the map, the key, and the value
        /// </summary>
        /// <param name="callbackfn">The callback function to run</param>
        void ForEach(Action<V, K, IMap<K, V>> callbackfn);

        /// <summary>
        /// Gets a value for a specified 
        /// </summary>
        /// <param name="key">The key to use to get the value</param>
        /// <returns></returns>
        V Get(K key);

        /// <summary>
        /// Returns whether or not this map has the given key
        /// </summary>
        /// <param name="key">The key to check for</param>
        /// <returns></returns>
        bool Has(K key);

        /// <summary>
        /// Sets a given key-value pair in the map
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The corresponding value</param>
        /// <returns><see langword="this"/></returns>
        IMap<K, V> Set(K key, V value);

        /// <summary>
        /// Gets the key iterator for this object
        /// </summary>
        /// <returns></returns>
        IEnumerator<K> Keys();

        /// <summary>
        /// Gets the value iterator for this object
        /// </summary>
        /// <returns></returns>
        IEnumerator<V> Values();

        /// <summary>
        /// The size of this map
        /// </summary>
        int Size { get; }
    }

    /// <summary>
    /// Adds methods for <see cref="IMap{K, V}"/> for <see langword="this"/>.
    /// </summary>
    /// <typeparam name="T">The inheriting object</typeparam>
    /// <typeparam name="K">The type of key to use</typeparam>
    /// <typeparam name="V">The type of value to use</typeparam>
    public interface IMap<T, K, V> where T : IMap<T, K, V>, IMap<K, V>
    {
        /// <summary>
        /// Loops through every key-value pair in the map, and runs the given callback function, providing the map, the key, and the value
        /// </summary>
        /// <param name="callbackfn">The callback function to run</param>
        void ForEach(Action<V, K, T> callbackfn);

        /// <summary>
        /// Sets a given key-value pair in the map
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The corresponding value</param>
        /// <returns><see langword="this"/></returns>
        T Set(K key, V value);
    }
}