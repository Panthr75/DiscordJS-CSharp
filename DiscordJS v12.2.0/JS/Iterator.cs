using System.Collections.Generic;
using System.Collections;

namespace JavaScript
{
    /// <summary>
    /// Represents a JavaScript Iterator
    /// </summary>
    public abstract class Iterator<T> : IEnumerator<T>, IEnumerator
    {
        public T Current { get; private set; }
        private T defaultValue;

        public Iterator() : this(default)
        { }

        public Iterator(T defaultValue)
        {
            this.defaultValue = defaultValue;
            Current = defaultValue;
        }

        /// <summary>
        /// Gets the next item in the iterator
        /// </summary>
        /// <returns></returns>
        public IteratorResult<T> Next()
        {
            var result = GetNext();
            if (result.Done) Current = defaultValue;
            else Current = result.Value;
            return result;
        }

        public bool MoveNext()
        {
            var result = Next();
            return !result.Done;
        }

        void IEnumerator.Reset() => Reset();

        object IEnumerator.Current => Current;

        T IEnumerator<T>.Current => Current;

        /// <summary>
        /// Gets the next iterator value in the iterator
        /// </summary>
        protected abstract IteratorResult<T> GetNext();

        /// <summary>
        /// Resets the iterator. Override if such a thing is needed. By default, sets <see cref="Current"/><br/>
        /// to the default value specified in the constructor, or <see langword="default"/> if it was not provided.
        /// </summary>
        protected virtual void Reset()
        {
            Current = defaultValue;
        }

        internal void CallReset() => Reset();

        /// <summary>
        /// Disposes the iterator. Override if such a thing needs to occur.
        /// </summary>
        public virtual void Dispose() { }
    }

    /// <summary>
    /// Represents a result from an iterator
    /// </summary>
    /// <typeparam name="T">The type of result</typeparam>
    public class IteratorResult<T>
    {
        /// <summary>
        /// The value of this result
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Whether or not the iterator is done.
        /// </summary>
        public bool Done { get; }

        public IteratorResult(T value, bool done)
        {
            Value = value;
            Done = done;
        }
    }
}