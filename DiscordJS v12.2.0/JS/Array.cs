using System.Collections.Generic;
using System.Collections;
using System;

namespace JavaScript
{
    /// <summary>
    /// Represents a JavaScript array
    /// </summary>
    /// <typeparam name="T">The type this array holds</typeparam>
    public class Array<T> : IList<T>, IList, IReadOnlyList<T>, ICollection<T>, ICollection, IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable
    {
        private List<T> values = new List<T>();

        /// <summary>
        /// Instantiates a new Array
        /// </summary>
        public Array()
        {
            values = new List<T>();
        }

        /// <summary>
        /// Instantiates a new Array with a given length
        /// </summary>
        /// <param name="length">The length to use</param>
        public Array(int length)
        {
            values = new List<T>(length);
        }

        /// <summary>
        /// Instantiates a new Array from the given items
        /// </summary>
        /// <param name="items">The items to include in this array</param>
        public Array(params T[] items)
        {
            values = new List<T>(items);
        }

        /// <summary>
        /// Instantiates a new Array from the given items
        /// </summary>
        /// <param name="items">The items to include in this array</param>
        public Array(IEnumerable<T> items)
        {
            values = new List<T>(items);
        }

        /// <summary>
        /// Returns the this object after copying a section of the array identified by start and end<br/>
        /// to the same array starting at position target
        /// </summary>
        /// <param name="target">If target is negative, it is treated as length+target where length is the<br/>
        /// length of the array.</param>
        /// <param name="start">If start is negative, it is treated as length+start. If end is negative, it<br/>
        /// is treated as length+end.</param>
        /// <returns></returns>
        public Array<T> CopyWithin(int target, int start) => CopyWithin(target, start, Length);

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
        public Array<T> CopyWithin(int target, int start, int end)
        {
            int len = Length;
            int to = target < 0 ? Math.Max(len + target, 0) : Math.Min(target, len);
            int from = start < 0 ? Math.Max(len + start, 0) : Math.Min(start, len);
            int final = end < 0 ? Math.Max(len + end, 0) : Math.Min(end, len);
            int count = Math.Min(final - from, len - to);
            Array<int> indexesToDelete = new Array<int>();
            int direction;
            if (from < to && to < from + count)
            {
                direction = -1;
                from = from + count - 1;
                to = to + count - 1;
            }
            else direction = 1;
            while (count > 0)
            {
                bool fromPresent = from > -1 && from < len;
                if (fromPresent) this[to] = this[from];
                else indexesToDelete.Push(to);
                from += direction;
                to += direction;
                count--;
            }
            for (int index = indexesToDelete.Length - 1; index > -1; index--)
            {
                int i = indexesToDelete[index];
                values.RemoveAt(i);
            }
            return this;
        }

        /// <summary>
        /// Combines two or more arrays.
        /// </summary>
        /// <param name="values">Additional items to add to the end of array1.</param>
        /// <returns></returns>
        public Array<T> Concat(params Array<T>[] values)
        {
            List<T> newValues = new List<T>();
            for (int index = 0, length = Length; index < length; index++)
                newValues.Add(this.values[index]);

            for (int i = 0, l = values.Length; i < l; i++)
            {
                Array<T> v = values[i];
                for (int index = 0, length = v.Length; index < length; index++)
                    newValues.Add(v[index]);
            }

            return new Array<T>() { values = newValues };
        }

        /// <summary>
        /// Determines whether all the members of an array satisfy the specified test.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The every method calls
        /// the callbackfn function for each element in the array until the callbackfn returns a value
        /// which is coercible to the Boolean value false, or until the end of the array.</param>
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
        /// <param name="callbackfn">A function that accepts up to three arguments. The every method calls
        /// the callbackfn function for each element in the array until the callbackfn returns a value
        /// which is coercible to the Boolean value false, or until the end of the array.</param>
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
        /// <param name="callbackfn">A function that accepts up to three arguments. The every method calls
        /// the callbackfn function for each element in the array until the callbackfn returns a value
        /// which is coercible to the Boolean value false, or until the end of the array.</param>
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
        /// <param name="callbackfn">A function that accepts up to three arguments. The every method calls
        /// the callbackfn function for each element in the array until the callbackfn returns a value
        /// which is coercible to the Boolean value false, or until the end of the array.</param>
        /// <returns></returns>
        public bool Every(Func<T, int, Array<T>, bool> callbackfn)
        {
            if (callbackfn == null) return false;
            for (int index = 0, length = Length; index < length; index++)
            {
                if (!callbackfn.Invoke(this[index], index, this))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the this object after filling the section identified by start and end with value
        /// </summary>
        /// <param name="value">value to fill array section with</param>
        /// <param name="start">index to start filling the array at. If start is negative, it is treated as
        /// length+start where length is the length of the array.</param>
        /// <returns></returns>
        public Array<T> Fill(T value, int start = 0) => Fill(value, start, Length - 1);

        /// <summary>
        /// Returns the this object after filling the section identified by start and end with value
        /// </summary>
        /// <param name="value">value to fill array section with</param>
        /// <param name="start">index to start filling the array at. If start is negative, it is treated as
        /// length+start where length is the length of the array.</param>
        /// <param name="end">index to stop filling the array at. If end is negative, it is treated as
        /// length+end.</param>
        /// <returns></returns>
        public Array<T> Fill(T value, int start, int end)
        {
            int len = Length - 1;
            int k = start < 0 ? Math.Max(len + start, 0) : Math.Min(start, len);
            int final = end < 0 ? Math.Max(len + end, 0) : Math.Min(end, len);
            for (; k < final; k++)
                this[k] = value;
            return this;
        }

        /// <summary>
        /// Returns the elements of an array that meet the condition specified in a callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The filter method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public Array<T> Filter(Func<bool> callbackfn)
        {
            Array<T> newArray = new Array<T>();
            if (callbackfn == null) return newArray;
            for (int index = 0, length = Length; index < length; index++)
            {
                T item = this[index];
                if (callbackfn.Invoke())
                    newArray.Push(item);
            }
            return newArray;
        }

        /// <summary>
        /// Returns the elements of an array that meet the condition specified in a callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The filter method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public Array<T> Filter(Func<T, bool> callbackfn)
        {
            Array<T> newArray = new Array<T>();
            if (callbackfn == null) return newArray;
            for (int index = 0, length = Length; index < length; index++)
            {
                T item = this[index];
                if (callbackfn.Invoke(item))
                    newArray.Push(item);
            }
            return newArray;
        }

        /// <summary>
        /// Returns the elements of an array that meet the condition specified in a callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The filter method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public Array<T> Filter(Func<T, int, bool> callbackfn)
        {
            Array<T> newArray = new Array<T>();
            if (callbackfn == null) return newArray;
            for (int index = 0, length = Length; index < length; index++)
            {
                T item = this[index];
                if (callbackfn.Invoke(item, index))
                    newArray.Push(item);
            }
            return newArray;
        }

        /// <summary>
        /// Returns the elements of an array that meet the condition specified in a callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The filter method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public Array<T> Filter(Func<T, int, Array<T>, bool> callbackfn)
        {
            Array<T> newArray = new Array<T>();
            if (callbackfn == null) return newArray;
            for (int index = 0, length = Length; index < length; index++)
            {
                T item = this[index];
                if (callbackfn.Invoke(item, index, this))
                    newArray.Push(item);
            }
            return newArray;
        }

        /// <summary>
        /// Returns the value of the first element in the array where predicate is true, and default
        /// otherwise.
        /// </summary>
        /// <param name="predicate">find calls predicate once for each element of the array, in ascending
        /// order, until it finds one where predicate returns true. If such an element is found, find
        /// immediately returns that element value. Otherwise, find returns default.</param>
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
        /// Returns the value of the first element in the array where predicate is true, and default
        /// otherwise.
        /// </summary>
        /// <param name="predicate">find calls predicate once for each element of the array, in ascending
        /// order, until it finds one where predicate returns true. If such an element is found, find
        /// immediately returns that element value. Otherwise, find returns default.</param>
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
        /// Returns the value of the first element in the array where predicate is true, and default
        /// otherwise.
        /// </summary>
        /// <param name="predicate">find calls predicate once for each element of the array, in ascending
        /// order, until it finds one where predicate returns true. If such an element is found, find
        /// immediately returns that element value. Otherwise, find returns default.</param>
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
        /// Returns the value of the first element in the array where predicate is true, and default
        /// otherwise.
        /// </summary>
        /// <param name="predicate">find calls predicate once for each element of the array, in ascending
        /// order, until it finds one where predicate returns true. If such an element is found, find
        /// immediately returns that element value. Otherwise, find returns default.</param>
        /// <returns></returns>
        public T Find(Func<T, int, Array<T>, bool> predicate)
        {
            if (predicate == null) return default;
            for (int index = 0, length = Length; index < length; index++)
            {
                T item = this[index];
                if (predicate.Invoke(item, index, this))
                    return item;
            }
            return default;
        }

        /// <summary>
        /// Returns the index of the first element in the array where predicate is true, and -1
        /// otherwise.
        /// </summary>
        /// <param name="predicate">find calls predicate once for each element of the array, in ascending
        /// order, until it finds one where predicate returns true. If such an element is found,
        /// findIndex immediately returns that element index. Otherwise, findIndex returns -1.</param>
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
        /// Returns the index of the first element in the array where predicate is true, and -1
        /// otherwise.
        /// </summary>
        /// <param name="predicate">find calls predicate once for each element of the array, in ascending
        /// order, until it finds one where predicate returns true. If such an element is found,
        /// findIndex immediately returns that element index. Otherwise, findIndex returns -1.</param>
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
        /// Returns the index of the first element in the array where predicate is true, and -1
        /// otherwise.
        /// </summary>
        /// <param name="predicate">find calls predicate once for each element of the array, in ascending
        /// order, until it finds one where predicate returns true. If such an element is found,
        /// findIndex immediately returns that element index. Otherwise, findIndex returns -1.</param>
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
        /// Returns the index of the first element in the array where predicate is true, and -1
        /// otherwise.
        /// </summary>
        /// <param name="predicate">find calls predicate once for each element of the array, in ascending
        /// order, until it finds one where predicate returns true. If such an element is found,
        /// findIndex immediately returns that element index. Otherwise, findIndex returns -1.</param>
        /// <returns></returns>
        public int FindIndex(Func<T, int, Array<T>, bool> predicate)
        {
            if (predicate == null) return -1;
            for (int index = 0, length = Length; index < length; index++)
            {
                if (predicate.Invoke(this[index], index, this))
                    return index;
            }
            return -1;
        }

        /// <summary>
        /// Performs the specified action for each element in an array.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. forEach calls the callbackfn function one time for each element in the array.</param>
        public void ForEach(Action callbackfn)
        {
            if (callbackfn == null) return;
            for (int index = 0, length = Length; index < length; index++)
                callbackfn.Invoke();
        }

        /// <summary>
        /// Performs the specified action for each element in an array.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. forEach calls the callbackfn function one time for each element in the array.</param>
        public void ForEach(Action<T> callbackfn)
        {
            if (callbackfn == null) return;
            for (int index = 0, length = Length; index < length; index++)
                callbackfn.Invoke(this[index]);
        }

        /// <summary>
        /// Performs the specified action for each element in an array.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. forEach calls the callbackfn function one time for each element in the array.</param>
        public void ForEach(Action<T, int> callbackfn)
        {
            if (callbackfn == null) return;
            for (int index = 0, length = Length; index < length; index++)
                callbackfn.Invoke(this[index], index);
        }

        /// <summary>
        /// Performs the specified action for each element in an array.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. forEach calls the callbackfn function one time for each element in the array.</param>
        public void ForEach(Action<T, int, Array<T>> callbackfn)
        {
            if (callbackfn == null) return;
            for (int index = 0, length = Length; index < length; index++)
                callbackfn.Invoke(this[index], index, this);
        }

        /// <summary>
        /// Determines whether an array includes a certain element, returning true or false as appropriate.
        /// </summary>
        /// <param name="value">The element to search for.</param>
        /// <param name="fromIndex">The position in this array at which to begin searching for searchElement.</param>
        /// <returns></returns>
        public bool Includes(T value, int fromIndex = 0)
        {
            for (int index = fromIndex, length = Length; index < length; index++)
            {
                if (EqualityComparer<T>.Default.Equals(this[index], value))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the index of the first occurrence of a value in an array.
        /// </summary>
        /// <param name="value">The value to locate in the array.</param>
        /// <param name="fromIndex">The array index at which to begin the search. If fromIndex is omitted, the search starts at index 0.</param>
        /// <returns></returns>
        public int IndexOf(T value, int fromIndex = 0)
        {
            for (int index = fromIndex, length = Length; index < length; index++)
            {
                if (EqualityComparer<T>.Default.Equals(this[index], value))
                    return index;
            }
            return -1;
        }

        /// <summary>
        /// Adds all the elements of an array separated by the specified separator string.
        /// </summary>
        /// <param name="seperator">A string used to separate one element of an array from the next in the resulting String. If omitted, the array elements are separated with a comma.</param>
        /// <returns></returns>
        public string Join(string seperator = ",")
        {
            string result = "";
            for (int index = 0, length = Length; index < length; index++)
            {
                if (index > 0)
                    result += seperator;
                result += this[index].ToString();
            }
            return result;
        }

        /// <summary>
        /// Returns the index of the last occurrence of a specified value in an array.
        /// </summary>
        /// <param name="value">The value to locate in the array.</param>
        /// <returns></returns>
        public int LastIndexOf(T value) => LastIndexOf(value, Length - 1);

        /// <summary>
        /// Returns the index of the last occurrence of a specified value in an array.
        /// </summary>
        /// <param name="value">The value to locate in the array.</param>
        /// <param name="fromIndex">The array index at which to begin the search. If fromIndex is omitted, the search starts at the last index in the array.</param>
        /// <returns></returns>
        public int LastIndexOf(T value, int fromIndex)
        {
            for (int index = fromIndex; index > -1; index--)
            {
                if (EqualityComparer<T>.Default.Equals(this[index], value))
                    return index;
            }
            return -1;
        }

        /// <summary>
        /// Calls a defined callback function on each element of an array, and returns an array that contains the results.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The map method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public Array<T> Map(Func<T> callbackfn)
        {
            List<T> newValues = new List<T>();
            if (callbackfn == null)
            {
                for (int index = 0, length = Length; index < length; index++)
                    newValues.Add(this[index]);
                return new Array<T>() { values = newValues };
            }

            for (int index = 0, length = Length; index < length; index++)
                newValues.Add(callbackfn.Invoke());

            return new Array<T>() { values = newValues };
        }

        /// <summary>
        /// Calls a defined callback function on each element of an array, and returns an array that contains the results.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The map method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public Array<T> Map(Func<T, T> callbackfn)
        {
            List<T> newValues = new List<T>();
            if (callbackfn == null)
            {
                for (int index = 0, length = Length; index < length; index++)
                    newValues.Add(this[index]);
                return new Array<T>() { values = newValues };
            }

            for (int index = 0, length = Length; index < length; index++)
                newValues.Add(callbackfn.Invoke(this[index]));

            return new Array<T>() { values = newValues };
        }

        /// <summary>
        /// Calls a defined callback function on each element of an array, and returns an array that contains the results.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The map method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public Array<T> Map(Func<T, int, T> callbackfn)
        {
            List<T> newValues = new List<T>();
            if (callbackfn == null)
            {
                for (int index = 0, length = Length; index < length; index++)
                    newValues.Add(this[index]);
                return new Array<T>() { values = newValues };
            }

            for (int index = 0, length = Length; index < length; index++)
                newValues.Add(callbackfn.Invoke(this[index], index));

            return new Array<T>() { values = newValues };
        }

        /// <summary>
        /// Calls a defined callback function on each element of an array, and returns an array that contains the results.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The map method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public Array<T> Map(Func<T, int, Array<T>, T> callbackfn)
        {
            List<T> newValues = new List<T>();
            if (callbackfn == null)
            {
                for (int index = 0, length = Length; index < length; index++)
                    newValues.Add(this[index]);
                return new Array<T>() { values = newValues };
            }

            for (int index = 0, length = Length; index < length; index++)
                newValues.Add(callbackfn.Invoke(this[index], index, this));

            return new Array<T>() { values = newValues };
        }

        /// <summary>
        /// Calls a defined callback function on each element of an array, and returns an array that contains the results.
        /// </summary>
        /// <typeparam name="U">The new type of array</typeparam>
        /// <param name="callbackfn">A function that accepts up to three arguments. The map method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public Array<U> Map<U>(Func<U> callbackfn)
        {
            List<U> newValues = new List<U>();
            if (callbackfn == null) return new Array<U>() { values = newValues };
            for (int index = 0, length = Length; index < length; index++)
                newValues.Add(callbackfn.Invoke());
            return new Array<U>() { values = newValues };
        }

        /// <summary>
        /// Calls a defined callback function on each element of an array, and returns an array that contains the results.
        /// </summary>
        /// <typeparam name="U">The new type of array</typeparam>
        /// <param name="callbackfn">A function that accepts up to three arguments. The map method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public Array<U> Map<U>(Func<T, U> callbackfn)
        {
            List<U> newValues = new List<U>();
            if (callbackfn == null) return new Array<U>() { values = newValues };
            for (int index = 0, length = Length; index < length; index++)
                newValues.Add(callbackfn.Invoke(this[index]));
            return new Array<U>() { values = newValues };
        }

        /// <summary>
        /// Calls a defined callback function on each element of an array, and returns an array that contains the results.
        /// </summary>
        /// <typeparam name="U">The new type of array</typeparam>
        /// <param name="callbackfn">A function that accepts up to three arguments. The map method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public Array<U> Map<U>(Func<T, int, U> callbackfn)
        {
            List<U> newValues = new List<U>();
            if (callbackfn == null) return new Array<U>() { values = newValues };
            for (int index = 0, length = Length; index < length; index++)
                newValues.Add(callbackfn.Invoke(this[index], index));
            return new Array<U>() { values = newValues };
        }

        /// <summary>
        /// Calls a defined callback function on each element of an array, and returns an array that contains the results.
        /// </summary>
        /// <typeparam name="U">The new type of array</typeparam>
        /// <param name="callbackfn">A function that accepts up to three arguments. The map method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public Array<U> Map<U>(Func<T, int, Array<T>, U> callbackfn)
        {
            List<U> newValues = new List<U>();
            if (callbackfn == null) return new Array<U>() { values = newValues };
            for (int index = 0, length = Length; index < length; index++)
                newValues.Add(callbackfn.Invoke(this[index], index, this));
            return new Array<U>() { values = newValues };
        }

        /// <summary>
        /// Removes the last element from an array and returns it.
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            int indexToRemove = Length - 1;
            if (indexToRemove < 0) return default;
            T item = this[indexToRemove];
            values.RemoveAt(indexToRemove);
            return item;
        }

        /// <summary>
        /// Appends new elements to an array, and returns the new length of the array.
        /// </summary>
        /// <param name="values">New elements of the Array.</param>
        /// <returns></returns>
        public int Push(params T[] values)
        {
            for (long index = 0, length = values.LongLength; index < length; index++)
                this.values.Add(values[index]);
            return Length;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public T Reduce(Func<T> callbackfn) => Reduce(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public T Reduce(Func<T, T> callbackfn) => Reduce(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public T Reduce(Func<T, T, T> callbackfn) => Reduce(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public T Reduce(Func<T, T, int, T> callbackfn) => Reduce(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public T Reduce(Func<T, T, int, Array<T>, T> callbackfn) => Reduce(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public T Reduce(Func<T> callbackfn, T initialValue)
        {
            T result = initialValue;
            if (callbackfn == null) return result;
            for (int index = 0, length = Length; index < length; index++)
                result = callbackfn.Invoke();
            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public T Reduce(Func<T, T> callbackfn, T initialValue)
        {
            T result = initialValue;
            if (callbackfn == null) return result;
            for (int index = 0, length = Length; index < length; index++)
                result = callbackfn.Invoke(result);
            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public T Reduce(Func<T, T, T> callbackfn, T initialValue)
        {
            T result = initialValue;
            if (callbackfn == null) return result;
            for (int index = 0, length = Length; index < length; index++)
                result = callbackfn.Invoke(result, this[index]);
            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public T Reduce(Func<T, T, int, T> callbackfn, T initialValue)
        {
            T result = initialValue;
            if (callbackfn == null) return result;
            for (int index = 0, length = Length; index < length; index++)
                result = callbackfn.Invoke(result, this[index], index);
            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public T Reduce(Func<T, T, int, Array<T>, T> callbackfn, T initialValue)
        {
            T result = initialValue;
            if (callbackfn == null) return result;
            for (int index = 0, length = Length; index < length; index++)
                result = callbackfn.Invoke(result, this[index], index, this);
            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public U Reduce<U>(Func<U> callbackfn) => Reduce(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public U Reduce<U>(Func<U, U> callbackfn) => Reduce(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public U Reduce<U>(Func<U, T, U> callbackfn) => Reduce(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public U Reduce<U>(Func<U, T, int, U> callbackfn) => Reduce(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public U Reduce<U>(Func<U, T, int, Array<T>, U> callbackfn) => Reduce(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public U Reduce<U>(Func<U> callbackfn, U initialValue)
        {
            U result = initialValue;
            if (callbackfn == null) return result;
            for (int index = 0, length = Length; index < length; index++)
                result = callbackfn.Invoke();
            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public U Reduce<U>(Func<U, U> callbackfn, U initialValue)
        {
            U result = initialValue;
            if (callbackfn == null) return result;
            for (int index = 0, length = Length; index < length; index++)
                result = callbackfn.Invoke(result);
            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public U Reduce<U>(Func<U, T, U> callbackfn, U initialValue)
        {
            U result = initialValue;
            if (callbackfn == null) return result;
            for (int index = 0, length = Length; index < length; index++)
                result = callbackfn.Invoke(result, this[index]);
            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public U Reduce<U>(Func<U, T, int, U> callbackfn, U initialValue)
        {
            U result = initialValue;
            if (callbackfn == null) return result;
            for (int index = 0, length = Length; index < length; index++)
                result = callbackfn.Invoke(result, this[index], index);
            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduce method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public U Reduce<U>(Func<U, T, int, Array<T>, U> callbackfn, U initialValue)
        {
            U result = initialValue;
            if (callbackfn == null) return result;
            for (int index = 0, length = Length; index < length; index++)
                result = callbackfn.Invoke(result, this[index], index, this);
            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public T ReduceRight(Func<T> callbackfn) => ReduceRight(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public T ReduceRight(Func<T, T> callbackfn) => ReduceRight(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public T ReduceRight(Func<T, T, T> callbackfn) => ReduceRight(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public T ReduceRight(Func<T, T, int, T> callbackfn) => ReduceRight(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public T ReduceRight(Func<T, T, int, Array<T>, T> callbackfn) => ReduceRight(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public T ReduceRight(Func<T> callbackfn, T initialValue)
        {
            T result = initialValue;
            if (callbackfn == null) return result;
            for (int index = Length - 1; index > -1; index--)
                result = callbackfn.Invoke();
            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public T ReduceRight(Func<T, T> callbackfn, T initialValue)
        {
            T result = initialValue;
            if (callbackfn == null) return result;
            for (int index = Length - 1; index > -1; index--)
                result = callbackfn.Invoke(result);
            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public T ReduceRight(Func<T, T, T> callbackfn, T initialValue)
        {
            T result = initialValue;
            if (callbackfn == null) return result;
            for (int index = Length - 1; index > -1; index--)
                result = callbackfn.Invoke(result, this[index]);
            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public T ReduceRight(Func<T, T, int, T> callbackfn, T initialValue)
        {
            T result = initialValue;
            if (callbackfn == null) return result;
            for (int index = Length - 1; index > -1; index--)
                result = callbackfn.Invoke(result, this[index], index);
            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public T ReduceRight(Func<T, T, int, Array<T>, T> callbackfn, T initialValue)
        {
            T result = initialValue;
            if (callbackfn == null) return result;
            for (int index = Length - 1; index > -1; index--)
                result = callbackfn.Invoke(result, this[index], index, this);
            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public U ReduceRight<U>(Func<U> callbackfn) => ReduceRight(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public U ReduceRight<U>(Func<U, U> callbackfn) => ReduceRight(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public U ReduceRight<U>(Func<U, T, U> callbackfn) => ReduceRight(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public U ReduceRight<U>(Func<U, T, int, U> callbackfn) => ReduceRight(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public U ReduceRight<U>(Func<U, T, int, Array<T>, U> callbackfn) => ReduceRight(callbackfn, default);

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public U ReduceRight<U>(Func<U> callbackfn, U initialValue)
        {
            U result = initialValue;
            if (callbackfn == null) return result;
            for (int index = Length - 1; index > -1; index--)
                result = callbackfn.Invoke();
            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public U ReduceRight<U>(Func<U, U> callbackfn, U initialValue)
        {
            U result = initialValue;
            if (callbackfn == null) return result;
            for (int index = Length - 1; index > -1; index--)
                result = callbackfn.Invoke(result);
            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public U ReduceRight<U>(Func<U, T, U> callbackfn, U initialValue)
        {
            U result = initialValue;
            if (callbackfn == null) return result;
            for (int index = Length - 1; index > -1; index--)
                result = callbackfn.Invoke(result, this[index]);
            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public U ReduceRight<U>(Func<U, T, int, U> callbackfn, U initialValue)
        {
            U result = initialValue;
            if (callbackfn == null) return result;
            for (int index = Length - 1; index > -1; index--)
                result = callbackfn.Invoke(result, this[index], index);
            return result;
        }

        /// <summary>
        /// Calls the specified callback function for all the elements in an array, in descending order. The return value of the callback function is the accumulated result, and is provided as an argument in the next call to the callback function.
        /// </summary>
        /// <typeparam name="U">The type to reduce to</typeparam>
        /// <param name="callbackfn">A function that accepts up to four arguments. The reduceRight method calls the callbackfn function one time for each element in the array.</param>
        /// <param name="initialValue">If initialValue is specified, it is used as the initial value to start the accumulation. The first call to the callbackfn function provides this value as an argument instead of an array value.</param>
        /// <returns></returns>
        public U ReduceRight<U>(Func<U, T, int, Array<T>, U> callbackfn, U initialValue)
        {
            U result = initialValue;
            if (callbackfn == null) return result;
            for (int index = Length - 1; index > -1; index--)
                result = callbackfn.Invoke(result, this[index], index, this);
            return result;
        }

        /// <summary>
        /// Reverses the elements in an Array.
        /// </summary>
        /// <returns>This</returns>
        public Array<T> Reverse()
        {
            values.Reverse();
            return this;
        }

        /// <summary>
        /// Removes the first element from an array and returns it.
        /// </summary>
        /// <returns></returns>
        public T Shift()
        {
            if (Length == 0) return default;
            T item = this[0];
            values.RemoveAt(0);
            return item;
        }

        /// <summary>
        /// Returns a section of an array.
        /// </summary>
        /// <returns></returns>
        public Array<T> Slice() => Slice(0, Length);

        /// <summary>
        /// Returns a section of an array.
        /// </summary>
        /// <param name="start">The beginning of the specified portion of the array.</param>
        /// <returns></returns>
        public Array<T> Slice(int start) => Slice(start, Length);

        /// <summary>
        /// Returns a section of an array.
        /// </summary>
        /// <param name="start">The beginning of the specified portion of the array.</param>
        /// <param name="end">The end of the specified portion of the array. This is exclusive of the element at the index 'end'.</param>
        /// <returns></returns>
        public Array<T> Slice(int start, int end)
        {
            int len = Length - 1;
            int k = start < 0 ? Math.Max(len + start, 0) : Math.Min(start, len);
            int final = end < 0 ? Math.Max(len + end, 0) : Math.Min(end, len);
            Array<T> A = new Array<T>();
            for (int n = 0; k < final; k++, n++)
                A[n] = this[k];
            return A;
        }

        /// <summary>
        /// Determines whether the specified callback function returns true for any element of an array.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The some method calls
        /// the callbackfn function for each element in the array until the callbackfn returns true, or
        /// until the end of the array.</param>
        /// <returns></returns>
        public bool Some(Func<bool> callbackfn)
        {
            if (callbackfn == null) return false;
            for (int index = 0, length = Length; index < length; index++)
            {
                if (callbackfn.Invoke())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified callback function returns true for any element of an array.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The some method calls
        /// the callbackfn function for each element in the array until the callbackfn returns true, or
        /// until the end of the array.</param>
        /// <returns></returns>
        public bool Some(Func<T, bool> callbackfn)
        {
            if (callbackfn == null) return false;
            for (int index = 0, length = Length; index < length; index++)
            {
                if (callbackfn.Invoke(this[index]))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified callback function returns true for any element of an array.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The some method calls
        /// the callbackfn function for each element in the array until the callbackfn returns true, or
        /// until the end of the array.</param>
        /// <returns></returns>
        public bool Some(Func<T, int, bool> callbackfn)
        {
            if (callbackfn == null) return false;
            for (int index = 0, length = Length; index < length; index++)
            {
                if (callbackfn.Invoke(this[index], index))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified callback function returns true for any element of an array.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The some method calls
        /// the callbackfn function for each element in the array until the callbackfn returns true, or
        /// until the end of the array.</param>
        /// <returns></returns>
        public bool Some(Func<T, int, Array<T>, bool> callbackfn)
        {
            if (callbackfn == null) return false;
            for (int index = 0, length = Length; index < length; index++)
            {
                if (callbackfn.Invoke(this[index], index, this))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Sorts an array. If compareFn is omitted, uses the default comparer
        /// </summary>
        /// <returns></returns>
        public Array<T> Sort()
        {
            values.Sort();
            return this;
        }

        /// <summary>
        /// Sorts an array. If compareFn is omitted, uses the default comparer
        /// </summary>
        /// <param name="compareFn">Function used to determine the order of the elements. It is expected to return
        /// a negative value if first argument is less than second argument, zero if they're equal and a positive
        /// value otherwise. If omitted, the elements are sorted in ascending, ASCII character order.</param>
        /// <returns></returns>
        public Array<T> Sort(Func<T, T, int> compareFn)
        {
            values.Sort(new Comparison<T>(compareFn));
            return this;
        }

        /// <summary>
        /// Sorts an array. If compareFn is omitted, uses the default comparer
        /// </summary>
        /// <param name="compareFn">Function used to determine the order of the elements. It is expected to return
        /// a negative value if first argument is less than second argument, zero if they're equal and a positive
        /// value otherwise. If omitted, the elements are sorted in ascending, ASCII character order.</param>
        /// <returns></returns>
        public Array<T> Sort(Comparison<T> compareFn)
        {
            values.Sort(compareFn);
            return this;
        }

        /// <summary>
        /// Removes elements from an array and, if necessary, inserts new elements in their place, returning the deleted elements.
        /// </summary>
        /// <param name="start">The zero-based location in the array from which to start removing elements.</param>
        /// <returns></returns>
        public Array<T> Splice(int start) => Splice(start, Length - start);

        /// <summary>
        /// Removes elements from an array and, if necessary, inserts new elements in their place, returning the deleted elements.
        /// </summary>
        /// <param name="start">The zero-based location in the array from which to start removing elements.</param>
        /// <param name="deleteCount">The number of elements to remove.</param>
        /// <param name="items">Elements to insert into the array in place of the deleted elements.</param>
        /// <returns></returns>
        public Array<T> Splice(int start, int deleteCount, params T[] items)
        {
            int len = Length - 1;
            int actualStart = start < 0 ? Math.Max((len + start), 0) : Math.Min(start, len);
            int itemCount = items.Length;
            int actualDeleteCount = Math.Min(Math.Max(deleteCount, 0), len - actualStart);
            Array<T> A = new Array<T>();
            for (int k = 0; k < actualDeleteCount; k++)
            {
                int from = actualStart + k;
                if (from > -1 && from < Length)
                    A[k] = this[from];
            }
            if (itemCount < actualDeleteCount)
            {
                int to = itemCount;
                for (int k = actualStart, l = (len - actualDeleteCount); k < l; k++)
                {
                    int from = k + actualDeleteCount;
                    if (from > -1 && from < Length)
                        this[to + k] = this[from];
                    else
                        values.RemoveAt(to);
                }

                for (int k = len, l = len - actualDeleteCount + itemCount; k > l; k--)
                    values.RemoveAt(k - 1);
            }
            else if (itemCount > actualDeleteCount)
            {
                int to = len - actualDeleteCount;
                for (int k = to; k > actualStart; k--)
                {
                    int from = k + actualDeleteCount - 1;
                    if (from > -1 && from < Length)
                        this[to + itemCount - 1] = this[from];
                    else
                        values.RemoveAt(to);
                }
            }

            for (int k = actualStart, i = 0; i < itemCount; i++, k++)
                this[k] = items[i];
            return A;
        }

        /// <summary>
        /// Inserts new elements at the start of an array.
        /// </summary>
        /// <param name="values">Elements to insert at the start of the Array.</param>
        /// <returns></returns>
        public int Unshift(params T[] values)
        {
            for (int index = 0, length = values.Length; index < length; index++)
                this.values.Insert(index, values[index]);
            return Length;
        }

        /// <summary>
        /// Returns a string representation of an array.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Join();

        /// <inheritdoc/>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Length)
                    return default;
                return values[index];
            }
            set
            {
                if (index < 0)
                    return;

                int length = Length;
                if (index >= length)
                {
                    for (int i = Length, l = index; i < l; i++)
                        values.Add(default);
                    values.Add(value);
                }
                else
                    values[index] = value;
            }
        }

        /// <summary>
        /// Gets or sets the length of the array. This is a number one higher than the highest element defined in an array.
        /// </summary>
        public int Length
        {
            get => values.Count;
            set
            {
                List<T> newValues = new List<T>();
                for (int index = 0, length = Math.Min(value, values.Count); index < length; index++)
                    newValues.Add(values[index]);
                for (int index = values.Count, length = value; index < length; index++)
                    newValues.Add(default);
                values = newValues;
            }
        }

        /// <summary>
        /// Converts this array to a singular dimension array
        /// </summary>
        /// <returns></returns>
        public T[] ToArray() => values.ToArray();

        /// <summary>
        /// Converts this array to a list
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            List<T> newList = new List<T>(values.Count);
            for (int index = 0, length = Length; index < length; index++)
                newList.Add(this[index]);
            return newList;
        }

        /// <summary>
        /// Converts an enumerable to an Array
        /// </summary>
        /// <param name="items">The enumerable to convert</param>
        /// <returns></returns>
        public static Array<T> From(IEnumerable<T> items)
        {
            Array<T> result = new Array<T>();
            foreach (T item in items)
                result.Push(item);
            return result;
        }

        /// <summary>
        /// Converts an enumerable to an Array, mapping each item to a new one using <paramref name="mapFn"/>
        /// </summary>
        /// <param name="items">The enumerable to convert</param>
        /// <param name="mapFn">The map function to use</param>
        /// <returns></returns>
        public static Array<T> From(IEnumerable<T> items, Func<T> mapFn)
        {
            if (mapFn is null) return From(items);
            Array<T> result = new Array<T>();
            foreach (T _ in items)
                result.Push(mapFn());
            return result;
        }

        /// <summary>
        /// Converts an enumerable to an Array, mapping each item to a new one using <paramref name="mapFn"/>
        /// </summary>
        /// <param name="items">The enumerable to convert</param>
        /// <param name="mapFn">The map function to use</param>
        /// <returns></returns>
        public static Array<T> From(IEnumerable<T> items, Func<T, T> mapFn)
        {
            if (mapFn is null) return From(items);
            Array<T> result = new Array<T>();
            foreach (T item in items)
                result.Push(mapFn(item));
            return result;
        }

        /// <summary>
        /// Converts an enumerable to an Array, mapping each item to a new one using <paramref name="mapFn"/>
        /// </summary>
        /// <param name="items">The enumerable to convert</param>
        /// <param name="mapFn">The map function to use</param>
        /// <returns></returns>
        public static Array<T> From(IEnumerable<T> items, Func<T, int, T> mapFn)
        {
            if (mapFn is null) return From(items);
            Array<T> result = new Array<T>();
            int index = 0;
            foreach (T item in items)
            {
                result.Push(mapFn(item, index));
                index++;
            }
            return result;
        }

        /// <summary>
        /// Converts an enumerable of type <typeparamref name="U"/> to an Array of type <typeparamref name="T"/>, mapping each item to a new one using <paramref name="mapFn"/>
        /// </summary>
        /// <typeparam name="U">The original type of enumerable</typeparam>
        /// <param name="items">The enumerable to convert</param>
        /// <param name="mapFn">The map function to use</param>
        /// <returns></returns>
        public static Array<T> From<U>(IEnumerable<U> items, Func<T> mapFn)
        {
            if (mapFn is null) return new Array<T>();
            Array<T> result = new Array<T>();
            foreach (U _ in items)
                result.Push(mapFn());
            return result;
        }

        /// <summary>
        /// Converts an enumerable of type <typeparamref name="U"/> to an Array of type <typeparamref name="T"/>, mapping each item to a new one using <paramref name="mapFn"/>
        /// </summary>
        /// <typeparam name="U">The original type of enumerable</typeparam>
        /// <param name="items">The enumerable to convert</param>
        /// <param name="mapFn">The map function to use</param>
        /// <returns></returns>
        public static Array<T> From<U>(IEnumerable<U> items, Func<U, T> mapFn)
        {
            if (mapFn is null) return new Array<T>();
            Array<T> result = new Array<T>();
            foreach (U item in items)
                result.Push(mapFn(item));
            return result;
        }

        /// <summary>
        /// Converts an enumerable of type <typeparamref name="U"/> to an Array of type <typeparamref name="T"/>, mapping each item to a new one using <paramref name="mapFn"/>
        /// </summary>
        /// <typeparam name="U">The original type of enumerable</typeparam>
        /// <param name="items">The enumerable to convert</param>
        /// <param name="mapFn">The map function to use</param>
        /// <returns></returns>
        public static Array<T> From<U>(IEnumerable<U> items, Func<U, int, T> mapFn)
        {
            if (mapFn is null) return new Array<T>();
            Array<T> result = new Array<T>();
            int index = 0;
            foreach (U item in items)
            {
                result.Push(mapFn(item, index));
                index++;
            }
            return result;
        }

        /// <summary>
        /// Converts an iterator to an Array
        /// </summary>
        /// <param name="items">The iterator to convert</param>
        /// <returns></returns>
        public static Array<T> From(Iterator<T> items)
        {
            Array<T> result = new Array<T>();
            IteratorResult<T> item = items.Next();
            while (!item.Done)
            {
                result.Push(item.Value);
                item = items.Next();
            }
            items.CallReset();
            return result;
        }

        /// <summary>
        /// Converts an iterator to an Array, mapping each item to a new one using <paramref name="mapFn"/>
        /// </summary>
        /// <param name="items">The iterator to convert</param>
        /// <param name="mapFn">The map function to use</param>
        /// <returns></returns>
        public static Array<T> From(Iterator<T> items, Func<T> mapFn)
        {
            if (mapFn is null) return From(items);
            Array<T> result = new Array<T>();
            IteratorResult<T> item = items.Next();
            while (!item.Done)
            {
                result.Push(mapFn());
                item = items.Next();
            }
            items.CallReset();
            return result;
        }

        /// <summary>
        /// Converts an iterator to an Array, mapping each item to a new one using <paramref name="mapFn"/>
        /// </summary>
        /// <param name="items">The iterator to convert</param>
        /// <param name="mapFn">The map function to use</param>
        /// <returns></returns>
        public static Array<T> From(Iterator<T> items, Func<T, T> mapFn)
        {
            if (mapFn is null) return From(items);
            Array<T> result = new Array<T>();
            IteratorResult<T> item = items.Next();
            while (!item.Done)
            {
                result.Push(mapFn(item.Value));
                item = items.Next();
            }
            items.CallReset();
            return result;
        }

        /// <summary>
        /// Converts an iterator to an Array, mapping each item to a new one using <paramref name="mapFn"/>
        /// </summary>
        /// <param name="items">The iterator to convert</param>
        /// <param name="mapFn">The map function to use</param>
        /// <returns></returns>
        public static Array<T> From(Iterator<T> items, Func<T, int, T> mapFn)
        {
            if (mapFn is null) return From(items);
            Array<T> result = new Array<T>();
            IteratorResult<T> item = items.Next();
            int index = 0;
            while (!item.Done)
            {
                result.Push(mapFn(item.Value, index));
                item = items.Next();
                index++;
            }
            items.CallReset();
            return result;
        }

        /// <summary>
        /// Converts an iterator of type <typeparamref name="U"/> to an Array of type <typeparamref name="T"/>, mapping each item to a new one using <paramref name="mapFn"/>
        /// </summary>
        /// <typeparam name="U">The original type of iterator</typeparam>
        /// <param name="items">The iterator to convert</param>
        /// <param name="mapFn">The map function to use</param>
        /// <returns></returns>
        public static Array<T> From<U>(Iterator<U> items, Func<T> mapFn)
        {
            if (mapFn is null) return new Array<T>();
            Array<T> result = new Array<T>();
            IteratorResult<U> item = items.Next();
            while (!item.Done)
            {
                result.Push(mapFn());
                item = items.Next();
            }
            items.CallReset();
            return result;
        }

        /// <summary>
        /// Converts an iterator of type <typeparamref name="U"/> to an Array of type <typeparamref name="T"/>, mapping each item to a new one using <paramref name="mapFn"/>
        /// </summary>
        /// <typeparam name="U">The original type of iterator</typeparam>
        /// <param name="items">The iterator to convert</param>
        /// <param name="mapFn">The map function to use</param>
        /// <returns></returns>
        public static Array<T> From<U>(Iterator<U> items, Func<U, T> mapFn)
        {
            if (mapFn is null) return new Array<T>();
            Array<T> result = new Array<T>();
            IteratorResult<U> item = items.Next();
            while (!item.Done)
            {
                result.Push(mapFn(item.Value));
                item = items.Next();
            }
            items.CallReset();
            return result;
        }

        /// <summary>
        /// Converts an iterator of type <typeparamref name="U"/> to an Array of type <typeparamref name="T"/>, mapping each item to a new one using <paramref name="mapFn"/>
        /// </summary>
        /// <typeparam name="U">The original type of iterator</typeparam>
        /// <param name="items">The iterator to convert</param>
        /// <param name="mapFn">The map function to use</param>
        /// <returns></returns>
        public static Array<T> From<U>(Iterator<U> items, Func<U, int, T> mapFn)
        {
            if (mapFn is null) return new Array<T>();
            Array<T> result = new Array<T>();
            IteratorResult<U> item = items.Next();
            int index = 0;
            while (!item.Done)
            {
                result.Push(mapFn(item.Value, index));
                item = items.Next();
                index++;
            }
            items.CallReset();
            return result;
        }

        /// <summary>
        /// Returns whether the given object is a JavaScript array.
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <returns></returns>
        public static bool IsArray(object obj) => obj is Array<T>;

        /// <summary>
        /// Constructs an Array out of the given items
        /// </summary>
        /// <param name="items">The items to use to construct the array</param>
        /// <returns></returns>
        public static Array<T> Of(IEnumerable<T> items)
        {
            Array<T> result = new Array<T>();
            foreach (T item in items)
                result.Push(item);
            return result;
        }

        /// <summary>
        /// Constructs an Array out of the given items
        /// </summary>
        /// <param name="items">The items to use to construct the array</param>
        /// <returns></returns>
        public static Array<T> Of(params T[] items)
        {
            Array<T> result = new Array<T>();
            for (int index = 0, length = items.Length; index < length; index++)
                result.Push(items[index]);
            return result;
        }

        /// <summary>
        /// Gets the enumerator of this array
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator() => values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => values.GetEnumerator();

        private bool IsCompatibleObject(object value) => (value is T) || (value == null && default(T) == null);
        private void ThrowIfCantBeNull(object value)
        {
            if (value == null && Nullable.GetUnderlyingType(typeof(T)) == null)
                throw new ArgumentException("Null is not allowed to be added to a nonnullable-type array");
        }

        private void RemoveAt(int index) => Splice(index, 1);
        private bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }
        private void Clear() => values = new List<T>();

        int IList<T>.IndexOf(T item) => IndexOf(item);
        void IList<T>.Insert(int index, T item) => this[index] = item;
        void IList<T>.RemoveAt(int index) => RemoveAt(index);
        int IList.Add(object value)
        {
            ThrowIfCantBeNull(value);

            try
            {
                Push((T)value);
            }
            catch(InvalidCastException)
            {
                throw new ArgumentException("Invalid argument type given. Expected '" + typeof(T).Name + "'");
            }

            return Length - 1;
        }
        void IList.Clear() => Clear();
        bool IList.Contains(object value)
        {
            if (IsCompatibleObject(value))
                return Includes((T)value);
            return false;
        }
        int IList.IndexOf(object value)
        {
            if (IsCompatibleObject(value))
                return IndexOf((T)value);
            return -1;
        }
        void IList.Insert(int index, object value)
        {
            ThrowIfCantBeNull(value);

            try
            {
                Splice(index, 0, (T)value);
            }
            catch(InvalidCastException)
            {
                throw new ArgumentException("Invalid argument type given. Expected '" + typeof(T).Name + "'");
            }
        }
        void IList.Remove(object value)
        {
            if (IsCompatibleObject(value))
                Remove((T)value);
        }
        void IList.RemoveAt(int index) => RemoveAt(index);
        bool IList.IsReadOnly => false;
        bool IList.IsFixedSize => false;
        object IList.this[int index]
        {
            get => this[index];
            set
            {
                ThrowIfCantBeNull(value);

                try
                {
                    this[index] = (T)value;
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException("Invalid argument type given. Expected '" + typeof(T).Name + "'");
                }
            }
        }
        void ICollection<T>.Add(T item) => Push(item);
        void ICollection<T>.Clear() => Clear();
        bool ICollection<T>.Contains(T item) => Includes(item);
        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => values.CopyTo(array, arrayIndex);
        bool ICollection<T>.Remove(T item) => Remove(item);
        int ICollection<T>.Count => Length;
        bool ICollection<T>.IsReadOnly => false;
        void ICollection.CopyTo(Array array, int index)
        {
            if ((array != null) && (array.Rank != 1))
                throw new ArgumentException("Multi-Dimensional arrays are not supported");

            try
            {
                Array.Copy(values.ToArray(), 0, array, index, Length);
            }
            catch(ArrayTypeMismatchException)
            {
                throw new ArgumentException("Invalid array type given. Expected array type to be '" + typeof(T).Name + "'");
            }
        }
        int ICollection.Count => Length;
        object ICollection.SyncRoot => ((ICollection)values).SyncRoot;
        bool ICollection.IsSynchronized => false;
        int IReadOnlyCollection<T>.Count => Length;

        /// <summary>
        /// Explicitly converts an Array to a bool. Shorthand for !(<paramref name="array"/> <see langword="is"/> <see langword="null"/>)
        /// </summary>
        /// <param name="array">The array to convert</param>
        public static explicit operator bool(Array<T> array) => !(array is null);

        /// <summary>
        /// Explcitly converts an Array to a string. Shorthand for (<paramref name="array"/>.?ToString())
        /// </summary>
        /// <param name="array">The array to convert</param>
        public static explicit operator string(Array<T> array) => array?.ToString();

        /// <summary>
        /// Explicitly converts an array to an <see cref="sbyte"/> using the array's length property.<br/>
        /// The value will also be clamped if the length is greater than <seealso cref="sbyte.MaxValue"/>.
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <exception cref="InvalidCastException">Thrown if the given <paramref name="array"/> is <see langword="null"/></exception>
        public static explicit operator sbyte(Array<T> array)
        {
            if (array == null) throw new InvalidCastException("Cannot cast 'null' to 'sbyte'");
            return (sbyte)Math.Min(array.Length, sbyte.MaxValue);
        }

        /// <summary>
        /// Explicitly converts an array to an <see cref="byte"/> using the array's length property.<br/>
        /// The value will also be clamped if the length is greater than <seealso cref="byte.MaxValue"/>.
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <exception cref="InvalidCastException">Thrown if the given <paramref name="array"/> is <see langword="null"/></exception>
        public static explicit operator byte(Array<T> array)
        {
            if (array == null) throw new InvalidCastException("Cannot cast 'null' to 'byte'");
            return (byte)Math.Min(array.Length, byte.MaxValue);
        }

        /// <summary>
        /// Explicitly converts an array to an <see cref="short"/> using the array's length property.<br/>
        /// The value will also be clamped if the length is greater than <seealso cref="short.MaxValue"/>.
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <exception cref="InvalidCastException">Thrown if the given <paramref name="array"/> is <see langword="null"/></exception>
        public static explicit operator short(Array<T> array)
        {
            if (array == null) throw new InvalidCastException("Cannot cast 'null' to 'short'");
            return (short)Math.Min(array.Length, short.MaxValue);
        }

        /// <summary>
        /// Explicitly converts an array to an <see cref="ushort"/> using the array's length property.<br/>
        /// The value will also be clamped if the length is greater than <seealso cref="ushort.MaxValue"/>.
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <exception cref="InvalidCastException">Thrown if the given <paramref name="array"/> is <see langword="null"/></exception>
        public static explicit operator ushort(Array<T> array)
        {
            if (array == null) throw new InvalidCastException("Cannot cast 'null' to 'ushort'");
            return (ushort)Math.Min(array.Length, ushort.MaxValue);
        }

        /// <summary>
        /// Explicitly converts an array to an <see cref="int"/> using the array's length property.
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <exception cref="InvalidCastException">Thrown if the given <paramref name="array"/> is <see langword="null"/></exception>
        public static explicit operator int(Array<T> array)
        {
            if (array == null) throw new InvalidCastException("Cannot cast 'null' to 'int'");
            return array.Length;
        }

        /// <summary>
        /// Explicitly converts an array to an <see cref="uint"/> using the array's length property.
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <exception cref="InvalidCastException">Thrown if the given <paramref name="array"/> is <see langword="null"/></exception>
        public static explicit operator uint(Array<T> array)
        {
            if (array == null) throw new InvalidCastException("Cannot cast 'null' to 'uint'");
            return (uint)array.Length;
        }

        /// <summary>
        /// Explicitly converts an array to an <see cref="long"/> using the array's length property.
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <exception cref="InvalidCastException">Thrown if the given <paramref name="array"/> is <see langword="null"/></exception>
        public static explicit operator long(Array<T> array)
        {
            if (array == null) throw new InvalidCastException("Cannot cast 'null' to 'long'");
            return array.Length;
        }

        /// <summary>
        /// Explicitly converts an array to an <see cref="ulong"/> using the array's length property.
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <exception cref="InvalidCastException">Thrown if the given <paramref name="array"/> is <see langword="null"/></exception>
        public static explicit operator ulong(Array<T> array)
        {
            if (array == null) throw new InvalidCastException("Cannot cast 'null' to 'ulong'");
            return (ulong)array.Length;
        }

        /// <summary>
        /// Explicitly converts an array to an <see cref="float"/> using the array's length property.
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <exception cref="InvalidCastException">Thrown if the given <paramref name="array"/> is <see langword="null"/></exception>
        public static explicit operator float(Array<T> array)
        {
            if (array == null) throw new InvalidCastException("Cannot cast 'null' to 'float'");
            return array.Length;
        }

        /// <summary>
        /// Explicitly converts an array to an <see cref="double"/> using the array's length property.
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <exception cref="InvalidCastException">Thrown if the given <paramref name="array"/> is <see langword="null"/></exception>
        public static explicit operator double(Array<T> array)
        {
            if (array == null) throw new InvalidCastException("Cannot cast 'null' to 'double'");
            return array.Length;
        }

        /// <summary>
        /// Explicitly converts an array to an <see cref="decimal"/> using the array's length property.
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <exception cref="InvalidCastException">Thrown if the given <paramref name="array"/> is <see langword="null"/></exception>
        public static explicit operator decimal(Array<T> array)
        {
            if (array == null) throw new InvalidCastException("Cannot cast 'null' to 'decimal'");
            return array.Length;
        }

        /// <summary>
        /// Allows checking if an <paramref name="array"/> is <see langword="null"/>.
        /// </summary>
        /// <param name="array">The array to check</param>
        public static bool operator !(Array<T> array) => array is null;

        public static T[] Map(T[] array, Func<T> fn)
        {
            var length = array.Length;
            T[] result = new T[length];
            for (int index = 0; index < length; index++)
                result[index] = fn();
            return result;
        }

        public static T[] Map(T[] array, Func<T, T> fn)
        {
            var length = array.Length;
            T[] result = new T[length];
            for (int index = 0; index < length; index++)
                result[index] = fn(array[index]);
            return result;
        }

        public static T[] Map(T[] array, Func<T, int, T> fn)
        {
            var length = array.Length;
            T[] result = new T[length];
            for (int index = 0; index < length; index++)
                result[index] = fn(array[index], index);
            return result;
        }

        public static T[] Map(T[] array, Func<T, int, T[], T> fn)
        {
            var length = array.Length;
            T[] result = new T[length];
            for (int index = 0; index < length; index++)
                result[index] = fn(array[index], index, array);
            return result;
        }

        public static U[] Map<U>(T[] array, Func<U> fn)
        {
            var length = array.Length;
            U[] result = new U[length];
            for (int index = 0; index < length; index++)
                result[index] = fn();
            return result;
        }

        public static U[] Map<U>(T[] array, Func<T, U> fn)
        {
            var length = array.Length;
            U[] result = new U[length];
            for (int index = 0; index < length; index++)
                result[index] = fn(array[index]);
            return result;
        }

        public static U[] Map<U>(T[] array, Func<T, int, U> fn)
        {
            var length = array.Length;
            U[] result = new U[length];
            for (int index = 0; index < length; index++)
                result[index] = fn(array[index], index);
            return result;
        }

        public static U[] Map<U>(T[] array, Func<T, int, T[], U> fn)
        {
            var length = array.Length;
            U[] result = new U[length];
            for (int index = 0; index < length; index++)
                result[index] = fn(array[index], index, array);
            return result;
        }

        public static T[] Map(IEnumerable<T> array, Func<T> fn)
        {
            List<T> list = new List<T>();
            int index = 0;
            foreach (T item in array)
            {
                list.Add(fn());
                index++;
            }
            return list.ToArray();
        }

        public static T[] Map(IEnumerable<T> array, Func<T, T> fn)
        {
            List<T> list = new List<T>();
            int index = 0;
            foreach (T item in array)
            {
                list.Add(fn(item));
                index++;
            }
            return list.ToArray();
        }

        public static T[] Map(IEnumerable<T> array, Func<T, int, T> fn)
        {
            List<T> list = new List<T>();
            int index = 0;
            foreach (T item in array)
            {
                list.Add(fn(item, index));
                index++;
            }
            return list.ToArray();
        }

        public static T[] Map(IEnumerable<T> array, Func<T, int, IEnumerable<T>, T> fn)
        {
            List<T> list = new List<T>();
            int index = 0;
            foreach (T item in array)
            {
                list.Add(fn(item, index, array));
                index++;
            }
            return list.ToArray();
        }

        public static U[] Map<U>(IEnumerable<T> array, Func<U> fn)
        {
            List<U> list = new List<U>();
            int index = 0;
            foreach (T item in array)
            {
                list.Add(fn());
                index++;
            }
            return list.ToArray();
        }

        public static U[] Map<U>(IEnumerable<T> array, Func<T, U> fn)
        {
            List<U> list = new List<U>();
            int index = 0;
            foreach (T item in array)
            {
                list.Add(fn(item));
                index++;
            }
            return list.ToArray();
        }

        public static U[] Map<U>(IEnumerable<T> array, Func<T, int, U> fn)
        {
            List<U> list = new List<U>();
            int index = 0;
            foreach (T item in array)
            {
                list.Add(fn(item, index));
                index++;
            }
            return list.ToArray();
        }

        public static U[] Map<U>(IEnumerable<T> array, Func<T, int, IEnumerable<T>, U> fn)
        {
            List<U> list = new List<U>();
            int index = 0;
            foreach (T item in array)
            {
                list.Add(fn(item, index, array));
                index++;
            }
            return list.ToArray();
        }
    }
}