using System;

namespace JavaScript
{
    /// <summary>
    /// The base class all typed arrays inherit from.
    /// </summary>
    /// <typeparam name="T">The type the array holds</typeparam>
    /// <typeparam name="This">The inheriting object</typeparam>
    public abstract class TypedArray<T, This> where This : TypedArray<T, This>, new()
    {
        internal ArrayBuffer buffer;

        /// <summary>
        /// The ArrayBuffer instance referenced by the array.
        /// </summary>
        public ArrayBuffer Buffer { get; }

        public int Length { get; }

        /// <summary>
        /// The length in bytes of the array.
        /// </summary>
        public int ByteLength => buffer.byteLength;

        /// <summary>
        /// The offset in bytes of the array.
        /// </summary>
        public int ByteOffset => buffer.byteOffset;

        /// <summary>
        /// Returns the this object after copying a section of the array identified by start and end<br/>
        /// to the same array starting at position target
        /// </summary>
        /// <param name="target">If target is negative, it is treated as length+target where length is the<br/>
        /// length of the array.</param>
        /// <param name="start">If start is negative, it is treated as length+start. If end is negative, it<br/>
        /// is treated as length+end.</param>
        /// <returns></returns>
        public This CopyWithin(int target, int start) => CopyWithin(target, start, Length);

        /// <summary>
        /// Returns the this object after copying a section of the array identified by start and end<br/>
        /// to the same array starting at position target
        /// </summary>
        /// <param name="target">If target is negative, it is treated as length+target where length is the<br/>
        /// length of the array.</param>
        /// <param name="start">If start is negative, it is treated as length+start. If end is negative, it<br/>
        /// is treated as length+end.</param>
        /// <param name="end">If not specified, length of the this object is used as its default value.</param>
        /// <returns></returns>
        public This CopyWithin(int target, int start, int end)
        {
            //
        }

        /// <summary>
        /// Determines whether all the members of an array satisfy the specified test.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The every method calls<br/>
        /// the <paramref name="callbackfn"/> function for each element in the array until the callbackfn returns <see langword="false"/><br/>
        /// or until the end of the array.</param>
        /// <returns></returns>
        public bool Every(Func<bool> callbackfn)
        {
            if (callbackfn == null) return false;
            for (int index = 0, length = Length; index < length; index++)
            {
                if (!callbackfn.Invoke())
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether all the members of an array satisfy the specified test.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The every method calls<br/>
        /// the <paramref name="callbackfn"/> function for each element in the array until the callbackfn returns <see langword="false"/><br/>
        /// or until the end of the array.</param>
        /// <returns></returns>
        public bool Every(Func<T, bool> callbackfn)
        {
            if (callbackfn == null) return false;
            for (int index = 0, length = Length; index < length; index++)
            {
                if (!callbackfn.Invoke(this[index]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether all the members of an array satisfy the specified test.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The every method calls<br/>
        /// the <paramref name="callbackfn"/> function for each element in the array until the callbackfn returns <see langword="false"/><br/>
        /// or until the end of the array.</param>
        /// <returns></returns>
        public bool Every(Func<T, int, bool> callbackfn)
        {
            if (callbackfn == null) return false;
            for (int index = 0, length = Length; index < length; index++)
            {
                if (!callbackfn.Invoke(this[index], index))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether all the members of an array satisfy the specified test.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The every method calls<br/>
        /// the <paramref name="callbackfn"/> function for each element in the array until the callbackfn returns <see langword="false"/><br/>
        /// or until the end of the array.</param>
        /// <returns></returns>
        public bool Every(Func<T, int, This, bool> callbackfn)
        {
            if (callbackfn == null) return false;
            This thisObj = (This)this;
            for (int index = 0, length = Length; index < length; index++)
            {
                if (!callbackfn.Invoke(this[index], index, thisObj))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the elements of an array that meet the condition specified in a callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The filter method calls<br/>
        /// the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public abstract This Filter(Func<bool> callbackfn);

        /// <summary>
        /// Returns the value of the first element in the array where <paramref name="predicate"/> is <see langword="true"/>, and <see langword="default"/><br/>
        /// otherwise.
        /// </summary>
        /// <param name="predicate">Find calls <paramref name="predicate"/> once for each element of the array, in ascending<br/>
        /// order, until it finds one where <paramref name="predicate"/> returns <see langword="true"/>. If such an element is found, Find<br/>
        /// immediately returns that element value. Otherwise, Find returns <see langword="default"/>.</param>
        /// <returns></returns>
        public T Find(Func<bool> predicate)
        {
            if (predicate == null) return default;
            for (int index = 0, length = Length; index < length; index++)
            {
                T item = this[index];
                if (predicate.Invoke())
                    return item;
            }
            return default;
        }

        /// <summary>
        /// Returns the value of the first element in the array where <paramref name="predicate"/> is <see langword="true"/>, and <see langword="default"/><br/>
        /// otherwise.
        /// </summary>
        /// <param name="predicate">Find calls <paramref name="predicate"/> once for each element of the array, in ascending<br/>
        /// order, until it finds one where <paramref name="predicate"/> returns <see langword="true"/>. If such an element is found, Find<br/>
        /// immediately returns that element value. Otherwise, Find returns <see langword="default"/>.</param>
        /// <returns></returns>
        public T Find(Func<T, bool> predicate)
        {
            if (predicate == null) return default;
            for (int index = 0, length = Length; index < length; index++)
            {
                T item = this[index];
                if (predicate.Invoke(item))
                    return item;
            }
            return default;
        }

        /// <summary>
        /// Returns the value of the first element in the array where <paramref name="predicate"/> is <see langword="true"/>, and <see langword="default"/><br/>
        /// otherwise.
        /// </summary>
        /// <param name="predicate">Find calls <paramref name="predicate"/> once for each element of the array, in ascending<br/>
        /// order, until it finds one where <paramref name="predicate"/> returns <see langword="true"/>. If such an element is found, Find<br/>
        /// immediately returns that element value. Otherwise, Find returns <see langword="default"/>.</param>
        /// <returns></returns>
        public T Find(Func<T, int, bool> predicate)
        {
            if (predicate == null) return default;
            for (int index = 0, length = Length; index < length; index++)
            {
                T item = this[index];
                if (predicate.Invoke(item, index))
                    return item;
            }
            return default;
        }

        /// <summary>
        /// Returns the value of the first element in the array where <paramref name="predicate"/> is <see langword="true"/>, and <see langword="default"/><br/>
        /// otherwise.
        /// </summary>
        /// <param name="predicate">Find calls <paramref name="predicate"/> once for each element of the array, in ascending<br/>
        /// order, until it finds one where <paramref name="predicate"/> returns <see langword="true"/>. If such an element is found, Find<br/>
        /// immediately returns that element value. Otherwise, Find returns <see langword="default"/>.</param>
        /// <returns></returns>
        public T Find(Func<T, int, This, bool> predicate)
        {
            if (predicate == null) return default;
            This thisObj = (This)this;
            for (int index = 0, length = Length; index < length; index++)
            {
                T item = this[index];
                if (predicate.Invoke(item, index, thisObj))
                    return item;
            }
            return default;
        }

        /// <summary>
        /// Returns the index of the first element in the array where <paramref name="predicate"/> is <see langword="true"/>, and -1<br/>
        /// otherwise.
        /// </summary>
        /// <param name="predicate">FindIndex calls <paramref name="predicate"/> once for each element of the array, in ascending<br/>
        /// order, until it finds one where <paramref name="predicate"/> returns <see langword="true"/>. If such an element is found, FindIndex<br/>
        /// immediately returns that element index. Otherwise, FindIndex returns -1.</param>
        /// <returns></returns>
        public int FindIndex(Func<bool> predicate)
        {
            if (predicate == null) return -1;
            for (int index = 0, length = Length; index < length; index++)
            {
                if (predicate.Invoke())
                    return index;
            }
            return -1;
        }

        /// <summary>
        /// Returns the index of the first element in the array where <paramref name="predicate"/> is <see langword="true"/>, and -1<br/>
        /// otherwise.
        /// </summary>
        /// <param name="predicate">FindIndex calls <paramref name="predicate"/> once for each element of the array, in ascending<br/>
        /// order, until it finds one where <paramref name="predicate"/> returns <see langword="true"/>. If such an element is found, FindIndex<br/>
        /// immediately returns that element index. Otherwise, FindIndex returns -1.</param>
        /// <returns></returns>
        public int FindIndex(Func<T, bool> predicate)
        {
            if (predicate == null) return -1;
            for (int index = 0, length = Length; index < length; index++)
            {
                if (predicate.Invoke(this[index]))
                    return index;
            }
            return -1;
        }

        /// <summary>
        /// Returns the index of the first element in the array where <paramref name="predicate"/> is <see langword="true"/>, and -1<br/>
        /// otherwise.
        /// </summary>
        /// <param name="predicate">FindIndex calls <paramref name="predicate"/> once for each element of the array, in ascending<br/>
        /// order, until it finds one where <paramref name="predicate"/> returns <see langword="true"/>. If such an element is found, FindIndex<br/>
        /// immediately returns that element index. Otherwise, FindIndex returns -1.</param>
        /// <returns></returns>
        public int FindIndex(Func<T, int, bool> predicate)
        {
            if (predicate == null) return -1;
            for (int index = 0, length = Length; index < length; index++)
            {
                if (predicate.Invoke(this[index], index))
                    return index;
            }
            return -1;
        }

        /// <summary>
        /// Returns the index of the first element in the array where <paramref name="predicate"/> is <see langword="true"/>, and -1<br/>
        /// otherwise.
        /// </summary>
        /// <param name="predicate">FindIndex calls <paramref name="predicate"/> once for each element of the array, in ascending<br/>
        /// order, until it finds one where <paramref name="predicate"/> returns <see langword="true"/>. If such an element is found, FindIndex<br/>
        /// immediately returns that element index. Otherwise, FindIndex returns -1.</param>
        /// <returns></returns>
        public int FindIndex(Func<T, int, This, bool> predicate)
        {
            if (predicate == null) return -1;
            This thisObj = (This)this;
            for (int index = 0, length = Length; index < length; index++)
            {
                if (predicate.Invoke(this[index], index, thisObj))
                    return index;
            }
            return -1;
        }

        /// <summary>
        /// Performs the specified action for each element in an array.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. ForEach calls the<br/>
        /// <paramref name="callbackfn"/> function one time for each element in the array.</param>
        public void ForEach(Action callbackfn)
        {
            if (callbackfn == null) return;
            for (int index = 0, length = Length; index < length; index++)
                callbackfn.Invoke();
        }

        /// <summary>
        /// Performs the specified action for each element in an array.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. ForEach calls the<br/>
        /// <paramref name="callbackfn"/> function one time for each element in the array.</param>
        public void ForEach(Action<T> callbackfn)
        {
            if (callbackfn == null) return;
            for (int index = 0, length = Length; index < length; index++)
                callbackfn.Invoke(this[index]);
        }

        /// <summary>
        /// Performs the specified action for each element in an array.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. ForEach calls the<br/>
        /// <paramref name="callbackfn"/> function one time for each element in the array.</param>
        public void ForEach(Action<T, int> callbackfn)
        {
            if (callbackfn == null) return;
            for (int index = 0, length = Length; index < length; index++)
                callbackfn.Invoke(this[index], index);
        }

        /// <summary>
        /// Performs the specified action for each element in an array.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. ForEach calls the<br/>
        /// <paramref name="callbackfn"/> function one time for each element in the array.</param>
        public void ForEach(Action<T, int, This> callbackfn)
        {
            if (callbackfn == null) return;
            This thisObj = (This)this;
            for (int index = 0, length = Length; index < length; index++)
                callbackfn.Invoke(this[index], index, thisObj);
        }

        public abstract T this[int index] { get; set; }
    }
}