using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JavaScript
{
    /// <summary>
    /// Represents a JavaScript array, but it's read-only
    /// </summary>
    /// <typeparam name="T">The type this array holds</typeparam>
    public class ReadonlyArray<T> : IList<T>, IList, IReadOnlyList<T>, ICollection<T>, ICollection, IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable
    {
        private readonly T[] values;

        /// <summary>
        /// Instantiates a new ReadonlyArray
        /// </summary>
        public ReadonlyArray()
        {
            values = new T[0] { };
        }

        /// <summary>
        /// Instantiates a new ReadonlyArray from the given array
        /// </summary>
        /// <param name="array">The array</param>
        public ReadonlyArray(Array<T> array) : this(array.ToArray())
        { }

        /// <summary>
        /// Instantiates a new Array from the given items
        /// </summary>
        /// <param name="items">The items to include in this array</param>
        public ReadonlyArray(params T[] items)
        {
            int length = items.Length;
            values = new T[length];
            Array.Copy(items, values, length);
        }

        /// <summary>
        /// Instantiates a new Array from the given items
        /// </summary>
        /// <param name="items">The items to include in this array</param>
        public ReadonlyArray(IEnumerable<T> items)
        {
            values = items == null ? new T[0] { } : new List<T>(items).ToArray();
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

            return new Array<T>(newValues);
        }

        /// <summary>
        /// Combines two or more arrays.
        /// </summary>
        /// <param name="values">Additional items to add to the end of array1.</param>
        /// <returns></returns>
        public Array<T> Concat(params ReadonlyArray<T>[] values)
        {
            List<T> newValues = new List<T>();
            for (int index = 0, length = Length; index < length; index++)
                newValues.Add(this.values[index]);

            for (int i = 0, l = values.Length; i < l; i++)
            {
                ReadonlyArray<T> v = values[i];
                for (int index = 0, length = v.Length; index < length; index++)
                    newValues.Add(v[index]);
            }

            return new Array<T>(newValues);
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
        public bool Every(Func<T, int, ReadonlyArray<T>, bool> callbackfn)
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
        public Array<T> Filter(Func<T, int, ReadonlyArray<T>, bool> callbackfn)
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
        public T Find(Func<T, int, ReadonlyArray<T>, bool> predicate)
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
        public int FindIndex(Func<T, int, ReadonlyArray<T>, bool> predicate)
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
        public void ForEach(Action<T, int, ReadonlyArray<T>> callbackfn)
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
                return new Array<T>(newValues);
            }

            for (int index = 0, length = Length; index < length; index++)
                newValues.Add(callbackfn.Invoke());

            return new Array<T>(newValues);
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
                return new Array<T>(newValues);
            }

            for (int index = 0, length = Length; index < length; index++)
                newValues.Add(callbackfn.Invoke(this[index]));

            return new Array<T>(newValues);
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
                return new Array<T>(newValues);
            }

            for (int index = 0, length = Length; index < length; index++)
                newValues.Add(callbackfn.Invoke(this[index], index));

            return new Array<T>(newValues);
        }

        /// <summary>
        /// Calls a defined callback function on each element of an array, and returns an array that contains the results.
        /// </summary>
        /// <param name="callbackfn">A function that accepts up to three arguments. The map method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public Array<T> Map(Func<T, int, ReadonlyArray<T>, T> callbackfn)
        {
            List<T> newValues = new List<T>();
            if (callbackfn == null)
            {
                for (int index = 0, length = Length; index < length; index++)
                    newValues.Add(this[index]);
                return new Array<T>(newValues);
            }

            for (int index = 0, length = Length; index < length; index++)
                newValues.Add(callbackfn.Invoke(this[index], index, this));

            return new Array<T>(newValues);
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
            if (callbackfn == null) return new Array<U>(newValues);
            for (int index = 0, length = Length; index < length; index++)
                newValues.Add(callbackfn.Invoke());
            return new Array<U>(newValues);
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
            if (callbackfn == null) return new Array<U>(newValues);
            for (int index = 0, length = Length; index < length; index++)
                newValues.Add(callbackfn.Invoke(this[index]));
            return new Array<U>(newValues);
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
            if (callbackfn == null) return new Array<U>(newValues);
            for (int index = 0, length = Length; index < length; index++)
                newValues.Add(callbackfn.Invoke(this[index], index));
            return new Array<U>(newValues);
        }

        /// <summary>
        /// Calls a defined callback function on each element of an array, and returns an array that contains the results.
        /// </summary>
        /// <typeparam name="U">The new type of array</typeparam>
        /// <param name="callbackfn">A function that accepts up to three arguments. The map method calls the callbackfn function one time for each element in the array.</param>
        /// <returns></returns>
        public Array<U> Map<U>(Func<T, int, ReadonlyArray<T>, U> callbackfn)
        {
            List<U> newValues = new List<U>();
            if (callbackfn == null) return new Array<U>(newValues);
            for (int index = 0, length = Length; index < length; index++)
                newValues.Add(callbackfn.Invoke(this[index], index, this));
            return new Array<U>(newValues);
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
        public T Reduce(Func<T, T, int, ReadonlyArray<T>, T> callbackfn) => Reduce(callbackfn, default);

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
        public T Reduce(Func<T, T, int, ReadonlyArray<T>, T> callbackfn, T initialValue)
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
        public U Reduce<U>(Func<U, T, int, ReadonlyArray<T>, U> callbackfn) => Reduce(callbackfn, default);

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
        public U Reduce<U>(Func<U, T, int, ReadonlyArray<T>, U> callbackfn, U initialValue)
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
        public T ReduceRight(Func<T, T, int, ReadonlyArray<T>, T> callbackfn) => ReduceRight(callbackfn, default);

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
        public T ReduceRight(Func<T, T, int, ReadonlyArray<T>, T> callbackfn, T initialValue)
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
        public U ReduceRight<U>(Func<U, T, int, ReadonlyArray<T>, U> callbackfn) => ReduceRight(callbackfn, default);

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
        public U ReduceRight<U>(Func<U, T, int, ReadonlyArray<T>, U> callbackfn, U initialValue)
        {
            U result = initialValue;
            if (callbackfn == null) return result;
            for (int index = Length - 1; index > -1; index--)
                result = callbackfn.Invoke(result, this[index], index, this);
            return result;
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
        public bool Some(Func<T, int, ReadonlyArray<T>, bool> callbackfn)
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
        }

        /// <summary>
        /// Gets or sets the length of the array. This is a number one higher than the highest element defined in an array.
        /// </summary>
        public int Length
        {
            get => values.Length;
        }

        /// <summary>
        /// Converts this array to a singular dimension array
        /// </summary>
        /// <returns></returns>
        public T[] ToArray() => values.ToArray();

        /// <summary>
        /// Converts this readonly array to an array
        /// </summary>
        /// <returns></returns>
        public Array<T> ToJSArray() => new Array<T>(values);

        /// <summary>
        /// Converts this array to a list
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            List<T> newList = new List<T>(values.Length);
            for (int index = 0, length = Length; index < length; index++)
                newList.Add(this[index]);
            return newList;
        }

        /// <summary>
        /// Gets the enumerator of this array
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator() => new List<T>(values).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => values.GetEnumerator();

        private bool IsCompatibleObject(object value) => (value is T) || (value == null && default(T) == null);

        T IList<T>.this[int index]
        {
            get => this[index];
            set => throw new InvalidOperationException("Values may not be inserted into a read-only Array");
        }
        int IList<T>.IndexOf(T item) => IndexOf(item);
        void IList<T>.Insert(int index, T item) => throw new InvalidOperationException("Values may not be inserted into a read-only Array");
        void IList<T>.RemoveAt(int index) => throw new InvalidOperationException("Values may not be removed from a read-only Array");
        int IList.Add(object value) => throw new InvalidOperationException("Values may not be added onto a read-only Array");
        void IList.Clear() => throw new InvalidOperationException("A read-only Array may not be cleared");
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
        void IList.Insert(int index, object value) => throw new InvalidOperationException("Values may not be inserted into a read-only Array");
        void IList.Remove(object value) => throw new InvalidOperationException("Values may not be removed from a read-only Array");
        void IList.RemoveAt(int index) => throw new InvalidOperationException("Values may not be removed from a read-only Array");
        bool IList.IsReadOnly => true;
        bool IList.IsFixedSize => true;
        object IList.this[int index]
        {
            get => this[index];
            set => throw new InvalidOperationException("Values may not be inserted into a read-only Array");
        }
        void ICollection<T>.Add(T item) => throw new InvalidOperationException("Values may not be added onto a read-only Array");
        void ICollection<T>.Clear() => throw new InvalidOperationException("A read-only Array may not be cleared");
        bool ICollection<T>.Contains(T item) => Includes(item);
        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => values.CopyTo(array, arrayIndex);
        bool ICollection<T>.Remove(T item) => throw new InvalidOperationException("Values may not be removed from a read-only Array");
        int ICollection<T>.Count => Length;
        bool ICollection<T>.IsReadOnly => true;
        void ICollection.CopyTo(Array array, int index)
        {
            if ((array != null) && (array.Rank != 1))
                throw new ArgumentException("Multi-Dimensional arrays are not supported");

            try
            {
                Array.Copy(values, 0, array, index, Length);
            }
            catch (ArrayTypeMismatchException)
            {
                throw new ArgumentException("Invalid array type given. Expected array type to be '" + typeof(T).Name + "'");
            }
        }
        int ICollection.Count => Length;
        object ICollection.SyncRoot => null;
        bool ICollection.IsSynchronized => false;
        int IReadOnlyCollection<T>.Count => Length;

        /// <summary>
        /// Explicitly converts an Array to a bool. Shorthand for !(<paramref name="array"/> <see langword="is"/> <see langword="null"/>)
        /// </summary>
        /// <param name="array">The array to convert</param>
        public static explicit operator bool(ReadonlyArray<T> array) => !(array is null);

        /// <summary>
        /// Explcitly converts an Array to a string. Shorthand for (<paramref name="array"/>.?ToString())
        /// </summary>
        /// <param name="array">The array to convert</param>
        public static explicit operator string(ReadonlyArray<T> array) => array?.ToString();

        /// <summary>
        /// Explicitly converts an array to an <see cref="sbyte"/> using the array's length property.<br/>
        /// The value will also be clamped if the length is greater than <seealso cref="sbyte.MaxValue"/>.
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <exception cref="InvalidCastException">Thrown if the given <paramref name="array"/> is <see langword="null"/></exception>
        public static explicit operator sbyte(ReadonlyArray<T> array)
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
        public static explicit operator byte(ReadonlyArray<T> array)
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
        public static explicit operator short(ReadonlyArray<T> array)
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
        public static explicit operator ushort(ReadonlyArray<T> array)
        {
            if (array == null) throw new InvalidCastException("Cannot cast 'null' to 'ushort'");
            return (ushort)Math.Min(array.Length, ushort.MaxValue);
        }

        /// <summary>
        /// Explicitly converts an array to an <see cref="int"/> using the array's length property.
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <exception cref="InvalidCastException">Thrown if the given <paramref name="array"/> is <see langword="null"/></exception>
        public static explicit operator int(ReadonlyArray<T> array)
        {
            if (array == null) throw new InvalidCastException("Cannot cast 'null' to 'int'");
            return array.Length;
        }

        /// <summary>
        /// Explicitly converts an array to an <see cref="uint"/> using the array's length property.
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <exception cref="InvalidCastException">Thrown if the given <paramref name="array"/> is <see langword="null"/></exception>
        public static explicit operator uint(ReadonlyArray<T> array)
        {
            if (array == null) throw new InvalidCastException("Cannot cast 'null' to 'uint'");
            return (uint)array.Length;
        }

        /// <summary>
        /// Explicitly converts an array to an <see cref="long"/> using the array's length property.
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <exception cref="InvalidCastException">Thrown if the given <paramref name="array"/> is <see langword="null"/></exception>
        public static explicit operator long(ReadonlyArray<T> array)
        {
            if (array == null) throw new InvalidCastException("Cannot cast 'null' to 'long'");
            return array.Length;
        }

        /// <summary>
        /// Explicitly converts an array to an <see cref="ulong"/> using the array's length property.
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <exception cref="InvalidCastException">Thrown if the given <paramref name="array"/> is <see langword="null"/></exception>
        public static explicit operator ulong(ReadonlyArray<T> array)
        {
            if (array == null) throw new InvalidCastException("Cannot cast 'null' to 'ulong'");
            return (ulong)array.Length;
        }

        /// <summary>
        /// Explicitly converts an array to an <see cref="float"/> using the array's length property.
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <exception cref="InvalidCastException">Thrown if the given <paramref name="array"/> is <see langword="null"/></exception>
        public static explicit operator float(ReadonlyArray<T> array)
        {
            if (array == null) throw new InvalidCastException("Cannot cast 'null' to 'float'");
            return array.Length;
        }

        /// <summary>
        /// Explicitly converts an array to an <see cref="double"/> using the array's length property.
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <exception cref="InvalidCastException">Thrown if the given <paramref name="array"/> is <see langword="null"/></exception>
        public static explicit operator double(ReadonlyArray<T> array)
        {
            if (array == null) throw new InvalidCastException("Cannot cast 'null' to 'double'");
            return array.Length;
        }

        /// <summary>
        /// Explicitly converts an array to an <see cref="decimal"/> using the array's length property.
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <exception cref="InvalidCastException">Thrown if the given <paramref name="array"/> is <see langword="null"/></exception>
        public static explicit operator decimal(ReadonlyArray<T> array)
        {
            if (array == null) throw new InvalidCastException("Cannot cast 'null' to 'decimal'");
            return array.Length;
        }

        /// <summary>
        /// Allows checking if an <paramref name="array"/> is <see langword="null"/>.
        /// </summary>
        /// <param name="array">The array to check</param>
        public static bool operator !(ReadonlyArray<T> array) => array is null;
    }
}