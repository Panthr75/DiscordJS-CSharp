using JavaScript;
using System.Collections.Generic;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Manages the API methods of a data model and holds its cache.
    /// </summary>
    /// <typeparam name="T">The cache type</typeparam>
    /// <typeparam name="V">The value this manager holds</typeparam>
    /// <typeparam name="D">The data the <typeparamref name="V"/> uses and can be used to instantiate it</typeparam>
    public abstract class BaseManager<T, V, D> where T : class, ICollection<Snowflake, V>, new() where V : class, IHasID//, IHasData<D>
    {
        /// <summary>
        /// The client that instantiated this Manager
        /// </summary>
        public Client Client { get; }

        /// <summary>
        /// Holds the cache for the data model
        /// </summary>
        public T Cache { get; }

        /// <summary>
        /// The data structure belonging to this manager
        /// </summary>
        internal Type Holds
        {
            get
            {
                var s = Structures.Get(holds.Name);
                if (s is null) return holds;
                else return s;
            }
        }

        private readonly Type holds;

        public BaseManager(Client client, IEnumerable<V> iterable)
        {
            Client = client;
            Cache = new T();
            if (iterable != null)
            {
                foreach (V i in iterable)
                    AddItem(i);
            }
        }

        protected abstract V AddItem(V item);

        //public V Add(D data, bool cache = true, Snowflake id = null, Type[] extrasType = null, params object[] extras)
        //{
        //    V existing = Cache.Get(id);
        //    if (!(existing is null))
        //    {
        //        if (cache) existing._Patch(data);
        //        return existing;
        //    }

        //    var h = Holds;
        //    V entry;
        //    if (h is null) entry 
        //}

        /// <summary>
        /// Resolves a data entry to a data Object.
        /// </summary>
        /// <param name="idOrInstance">The id or instance of something in this Manager</param>
        /// <returns>An instance from this Manager</returns>
        public V Resolve(object idOrInstance)
        {
            if (idOrInstance is V value) return value;
            else if (idOrInstance is string str) return Cache.Get(str);
            else if (idOrInstance is Snowflake snowflake) return Cache.Get(snowflake);
            return null;
        }

        /// <summary>
        /// Resolves a data entry to a instance ID.
        /// </summary>
        /// <param name="idOrInstance">The id or instance of something in this Manager</param>
        /// <returns></returns>
        public Snowflake ResolveID(object idOrInstance)
        {
            if (idOrInstance is V value) return value.ID;
            else if (idOrInstance is string str) return str;
            else if (idOrInstance is Snowflake snowflake) return snowflake;
            return null;
        }
    }
}