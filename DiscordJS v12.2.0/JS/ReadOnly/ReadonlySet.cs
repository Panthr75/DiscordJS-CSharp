using System;
using System.Collections;
using System.Collections.Generic;

namespace JavaScript
{
    /// <summary>
    /// A read-only variant of <see cref="Set{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type this set holds</typeparam>
    public class ReadonlySet<T> : IEnumerable<T>, IEnumerable
    {
        internal readonly T[] values;

        internal IEqualityComparer<T> comparer;

        /// <summary>
        /// The number of entries in this set
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Instantiates a new set object with the default equality comparer, and no values
        /// </summary>
        public ReadonlySet() : this(null, null)
        { }

        /// <summary>
        /// Instantiates the set with the given equality comparer, and no values
        /// </summary>
        /// <param name="comparer">The comparer</param>
        public ReadonlySet(IEqualityComparer<T> comparer) : this(null, comparer)
        { }

        /// <summary>
        /// Instantiates the set with the the default equality comparer, and the values from the given enumerable
        /// </summary>
        /// <param name="enumerable">The enumerable</param>
        public ReadonlySet(IEnumerable<T> enumerable) : this(enumerable, null)
        { }

        /// <summary>
        /// Instantiates the set with the given enumerable to extract values from, and a comparer
        /// </summary>
        /// <param name="enumerable">The enumerable</param>
        /// <param name="comparer">The comparer</param>
        public ReadonlySet(IEnumerable<T> enumerable, IEqualityComparer<T> comparer)
        {
            this.comparer = comparer == null ? EqualityComparer<T>.Default : comparer;
            if (enumerable != null)
            {
                List<T> list = new List<T>();
                foreach (T value in enumerable)
                    list.Add(value);
                values = list.ToArray();
            }
            else
                values = new T[0] { };
        }

        /// <summary>
        /// Gets the entry iterator for this set
        /// </summary>
        /// <returns></returns>
        public EntryIterator Entries() => new EntryIterator(this);

        /// <summary>
        /// Loops through each item in the set, and runs the given callback function, providing the set, the key, and the value.<br/>
        /// Since the set does not have a key value, the key will be the value
        /// </summary>
        /// <param name="callbackfn">The callback function to run</param>
        public void ForEach(Action callbackfn)
        {
            if (callbackfn == null) return;
            for (int index = 0, length = values.Length; index < length; index++)
                callbackfn();
        }

        /// <summary>
        /// Loops through each item in the set, and runs the given callback function, providing the set, the key, and the value.<br/>
        /// Since the set does not have a key value, the key will be the value
        /// </summary>
        /// <param name="callbackfn">The callback function to run</param>
        public void ForEach(Action<T> callbackfn)
        {
            if (callbackfn == null) return;
            for (int index = 0, length = values.Length; index < length; index++)
                callbackfn(values[index]);
        }

        /// <summary>
        /// Loops through each item in the set, and runs the given callback function, providing the set, the key, and the value.<br/>
        /// Since the set does not have a key value, the key will be the value
        /// </summary>
        /// <param name="callbackfn">The callback function to run</param>
        public void ForEach(Action<T, T> callbackfn)
        {
            if (callbackfn == null) return;
            for (int index = 0, length = values.Length; index < length; index++)
            {
                T e = values[index];
                callbackfn(e, e);
            }
        }

        /// <summary>
        /// Loops through each item in the set, and runs the given callback function, providing the set, the key, and the value.<br/>
        /// Since the set does not have a key value, the key will be the value
        /// </summary>
        /// <param name="callbackfn">The callback function to run</param>
        public void ForEach(Action<T, T, ReadonlySet<T>> callbackfn)
        {
            if (callbackfn == null) return;
            for (int index = 0, length = values.Length; index < length; index++)
            {
                T e = values[index];
                callbackfn(e, e, this);
            }
        }

        /// <summary>
        /// Gets the entry iterator for this map
        /// </summary>
        /// <returns></returns>
        public EntryIterator GetEnumerator() => new EntryIterator(this);
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Returns whether or not this set has the given value
        /// </summary>
        /// <param name="value">The value to check for</param>
        /// <returns></returns>
        public bool Has(T value)
        {
            for (int index = 0, length = values.Length; index < length; index++)
            {
                if (comparer.Equals(values[index], value))
                    return true;
            }
            return false;
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
        /// Represents the value iterator for this set
        /// </summary>
        [Serializable]
        public sealed class ValueIterator : Iterator<T>
        {
            private ReadonlySet<T> set;
            private int index;

            internal ValueIterator(ReadonlySet<T> set) : base()
            {
                this.set = set;
                index = 0;
            }

            /// <inheritdoc/>
            protected override IteratorResult<T> GetNext()
            {
                while (index < set.Size)
                {
                    var entry = set.values[index];
                    if (entry == null || entry.GetHashCode() >= 0)
                    {
                        index++;
                        return new IteratorResult<T>(entry, false);
                    }
                    index++;
                }

                index = set.Size + 1;
                return new IteratorResult<T>(default, true);
            }

            /// <inheritdoc/>
            protected override void Reset()
            {
                index = 0;
                base.Reset();
            }
        }

        /// <summary>
        /// Represents the key iterator for this set
        /// </summary>
        [Serializable]
        public sealed class KeyIterator : Iterator<T>
        {
            private ReadonlySet<T> set;
            private int index;

            internal KeyIterator(ReadonlySet<T> set) : base()
            {
                this.set = set;
                index = 0;
            }

            /// <inheritdoc/>
            protected override IteratorResult<T> GetNext()
            {
                while (index < set.Size)
                {
                    var entry = set.values[index];
                    if (entry == null || entry.GetHashCode() >= 0)
                    {
                        index++;
                        return new IteratorResult<T>(entry, false);
                    }
                    index++;
                }

                index = set.Size + 1;
                return new IteratorResult<T>(default, true);
            }

            /// <inheritdoc/>
            protected override void Reset()
            {
                index = 0;
                base.Reset();
            }
        }

        /// <summary>
        /// Represents the entry iterator for this set
        /// </summary>
        [Serializable]
        public sealed class EntryIterator : Iterator<T>, IEnumerator<KeyValuePair<T, T>>
        {
            private ReadonlySet<T> set;
            private int index;

            internal EntryIterator(ReadonlySet<T> set) : base(default)
            {
                this.set = set;
                index = 0;
            }

            /// <inheritdoc/>
            protected override IteratorResult<T> GetNext()
            {
                while (index < set.Size)
                {
                    var entry = set.values[index];
                    if (entry == null || entry.GetHashCode() >= 0)
                    {
                        index++;
                        return new IteratorResult<T>(entry, false);
                    }
                    index++;
                }

                index = set.Size + 1;
                return new IteratorResult<T>(default, true);
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
            public T Entry => Current;

            /// <summary>
            /// The key for the current entry
            /// </summary>
            public T Key => Current;

            /// <summary>
            /// The value for the current entry
            /// </summary>
            public T Value => Current;

            KeyValuePair<T, T> IEnumerator<KeyValuePair<T, T>>.Current => new KeyValuePair<T, T>(Key, Value);
        }
    }
}
