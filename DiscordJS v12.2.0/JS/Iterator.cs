using System.Collections.Generic;
using System.Collections;

namespace JavaScript
{
    /// <summary>
    /// Represents a JavaScript Iterator
    /// </summary>
    public abstract class Iterator<T> : IEnumerator<T>, IEnumerator, IEnumerable<T>, IEnumerable
    {
        /// <summary>
        /// The current value in this iterator
        /// </summary>
        public T Current { get; private set; }
        private T defaultValue;

        /// <summary>
        /// Instantiates this iterator with the default value of <see langword="default"/>.
        /// </summary>
        public Iterator() : this(default)
        { }

        /// <summary>
        /// Instantiates this iterator with the default value of <paramref name="defaultValue"/>
        /// </summary>
        /// <param name="defaultValue">The default value for this iterator</param>
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

        /// <inheritdoc/>
        public bool MoveNext()
        {
            var result = Next();
            return !result.Done;
        }

        /// <summary>
        /// Gets the enumerator of this, actually returning <see langword="this"/>.<br/>
        /// This is so you can run a <see langword="foreach"/> on <see cref="Map{K, V}.Values"/>, for example.
        /// </summary>
        /// <returns><see langword="this"/></returns>
        public IEnumerator<T> GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => this;

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

        /// <summary>
        /// Instantiates a new iterator result
        /// </summary>
        /// <param name="value">The value for this result</param>
        /// <param name="done">Whether or not the iterator is done</param>
        public IteratorResult(T value, bool done)
        {
            Value = value;
            Done = done;
        }
    }
}