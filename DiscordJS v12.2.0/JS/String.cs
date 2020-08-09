using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JavaScript
{
    /// <summary>
    /// Represents a JavaScript String
    /// </summary>
    public class String : IComparable, IComparable<string>
    {
        private readonly string value;

        /// <summary>
        /// Creates a new Empty String
        /// </summary>
        public String()
        {
            value = "";
        }

        /// <summary>
        /// Creates a new String using the given string value. If it is null, then will use the Empty String.
        /// </summary>
        /// <param name="str">The string</param>
        public String(string str)
        {
            if (str == null) value = "";
            else value = str;
        }

        /// <summary>
        /// Creates a new String using the given value. Is converted to a string with <see cref="object.ToString"/>. If value is null, then will use the Empty String.
        /// </summary>
        /// <param name="value">The value to convert to a String</param>
        public String(object value)
        {
            if (value is null) this.value = "";
            else
            {
                string strValue = value.ToString();
                this.value = strValue == null ? "" : strValue;
            }
        }

        /// <summary>
        /// Returns the character at the specified index.
        /// </summary>
        /// <param name="pos">The zero-based index of the desired character.</param>
        /// <returns></returns>
        public String CharAt(int pos)
        {
            if (pos < 0 || pos >= Length) return new String("");
            return new String(value[pos].ToString());
        }

        /// <summary>
        /// Returns the Unicode value of the character at the specified location.
        /// </summary>
        /// <param name="pos">The zero-based index of the desired character. If there is no character at the specified index, NaN is returned.</param>
        /// <returns></returns>
        public int CharCodeAt(int pos)
        {
            if (pos < 0 || pos >= Length) return -1;
            return value[pos];
        }

        /// <summary>
        /// Returns a nonnegative integer Number less than 1114112 (0x110000) that is the code point
        /// value of the UTF-16 encoded code point starting at the string element at position pos in
        /// the String resulting from converting this object to a String.
        /// If there is no element at that position, the result is undefined.
        /// If a valid UTF-16 surrogate pair does not begin at pos, the result is the code unit at pos.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public int CharPointAt(int pos)
        {
            if (pos < 0 || pos >= Length) return -1;
            char first = value[pos];
            if (first < 0xD800 || first > 0xDBFF || pos + 1 == Length) return first;
            char second = value[pos + 1];
            if (second < 0xDC00 || second > 0xDFFF) return first;
            return char.ConvertToUtf32(first, second);
        }

        /// <summary>
        /// Returns a string that contains the concatenation of two or more strings.
        /// </summary>
        /// <param name="args">The strings to append to the end of the string.</param>
        /// <returns></returns>
        public String Concat(params object[] args)
        {
            string R = value;
            for (int index = 0, length = args.Length; index < length; index++)
            {
                object nextObj = args[index];
                string nextString = nextObj is null ? "" : nextObj.ToString();
                if (nextString is null) nextString = "";
                R += nextString;
            }
            return new String(R);
        }

        /// <summary>
        /// Returns true if the sequence of elements of searchString converted to a String is the
        /// same as the corresponding elements of this object (converted to a String) starting at
        /// endPosition – length(this). Otherwise returns false.
        /// </summary>
        /// <param name="searchString">The search string</param>
        /// <returns></returns>
        public bool EndsWith(string searchString) => EndsWith(searchString, Length);

        /// <summary>
        /// Returns true if the sequence of elements of searchString converted to a String is the
        /// same as the corresponding elements of this object (converted to a String) starting at
        /// endPosition – length(this). Otherwise returns false.
        /// </summary>
        /// <param name="searchString">The search string</param>
        /// <param name="endPosition"></param>
        /// <returns></returns>
        public bool EndsWith(string searchString, int endPosition)
        {
            if (string.IsNullOrEmpty(searchString)) return true;
            int end = Math.Min(Math.Max(endPosition, 0), Length);
            int searchLength = searchString.Length;
            int start = end - searchLength;
            if (start < 0) return false;
            for (int index = 0; index < searchLength; index++)
            {
                if (value[index + start] != searchString[index])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if searchString appears as a substring of the result of converting this
        /// object to a String, at one or more positions that are
        /// greater than or equal to position; otherwise, returns false.
        /// </summary>
        /// <param name="searchString">search string</param>
        /// <returns></returns>
        public bool Includes(string searchString) => Includes(searchString, 0);

        /// <summary>
        /// Returns true if searchString appears as a substring of the result of converting this
        /// object to a String, at one or more positions that are
        /// greater than or equal to position; otherwise, returns false.
        /// </summary>
        /// <param name="searchString">search string</param>
        /// <param name="position">If position is undefined, 0 is assumed, so as to search all of the String.</param>
        /// <returns></returns>
        public bool Includes(string searchString, int position)
        {
            if (string.IsNullOrEmpty(searchString)) return true;
            int len = Length;
            int searchLen = searchString.Length;
            int start = Math.Min(Math.Max(position, 0), len);
            int resultIndex;
            for (int index = start, length = len - searchLen + 1; index < length; index++)
            {
                if (value[index] == searchString[0])
                {
                    resultIndex = index;
                    if (searchLen == 1) return true;
                    index++;
                    for (int i = 1, l = searchLen; i < l; i++)
                    {
                        if (value[resultIndex + i] == searchString[i])
                        {
                            if (i + 1 == searchLen)
                                return true;
                        }
                        else
                            break;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the position of the first occurrence of a substring.
        /// </summary>
        /// <param name="searchString">The substring to search for in the string</param>
        /// <returns></returns>
        public int IndexOf(string searchString) => IndexOf(searchString, 0);

        /// <summary>
        /// Returns the position of the first occurrence of a substring.
        /// </summary>
        /// <param name="searchString">The substring to search for in the string</param>
        /// <param name="position">The index at which to begin searching the String object. If omitted, search starts at the beginning of the string.</param>
        /// <returns></returns>
        public int IndexOf(string searchString, int position)
        {
            if (string.IsNullOrEmpty(searchString)) return 0;
            int len = Length;
            int searchLen = searchString.Length;
            int start = Math.Min(Math.Max(position, 0), len);
            int resultIndex;
            for (int index = start, length = len - searchLen + 1; index < length; index++)
            {
                if (value[index] == searchString[0])
                {
                    resultIndex = index;
                    if (searchLen == 1) return resultIndex;
                    index++;
                    for (int i = 1, l = searchLen; i < l; i++)
                    {
                        if (value[resultIndex + i] == searchString[i])
                        {
                            if (i + 1 == searchLen)
                                return resultIndex;
                        }
                        else
                            break;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Returns the last occurrence of a substring in the string.
        /// </summary>
        /// <param name="searchString">The substring to search for.</param>
        /// <returns></returns>
        public int LastIndexOf(string searchString) => LastIndexOf(searchString, Length + 1);

        /// <summary>
        /// Returns the last occurrence of a substring in the string.
        /// </summary>
        /// <param name="searchString">The substring to search for.</param>
        /// <param name="position">The index at which to begin searching. If omitted, the search begins at the end of the string.</param>
        /// <returns></returns>
        public int LastIndexOf(string searchString, int position)
        {
            int len = Length;
            if (string.IsNullOrEmpty(searchString)) return len - 1;
            int searchLen = searchString.Length;
            int start = Math.Min(Math.Max(position, 0), len);
            int resultIndex;
            for (int index = len - searchLen + 1; index >= start; index--)
            {
                if (value[index] == searchString[0])
                {
                    resultIndex = index;
                    if (searchLen == 1) return resultIndex;
                    index--;
                    for (int i = 1, l = searchLen; i < l; i++)
                    {
                        if (value[resultIndex - i] == searchString[i])
                        {
                            if (i + 1 == searchLen)
                                return resultIndex;
                        }
                        else
                            break;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Matches a string with a regular expression, and the results of that search.
        /// </summary>
        /// <param name="regexp">A variable name or string literal containing the regular expression pattern</param>
        /// <returns></returns>
        public Match Match(string regexp)
        {
            if (string.IsNullOrEmpty(regexp)) regexp = "";
            return Match(new Regex(regexp));
        }

        /// <summary>
        /// Matches a string with a regular expression, and the results of that search.
        /// </summary>
        /// <param name="regexp">A variable name or string literal containing the regular expression pattern</param>
        /// <returns></returns>
        public Match Match(Regex regexp)
        {
            if (regexp == null) return Match(new Regex(""));
            return regexp.Match(value);
        }

        /// <summary>
        /// Matches a string with a regular expression, and returns an array containing the results of that search.
        /// </summary>
        /// <param name="regexp">A variable name or string literal containing the regular expression pattern</param>
        /// <returns></returns>
        public MatchCollection Matches(string regexp)
        {
            if (string.IsNullOrEmpty(regexp)) regexp = "";
            return Matches(new Regex(regexp));
        }

        /// <summary>
        /// Matches a string with a regular expression, and returns an array containing the results of that search.
        /// </summary>
        /// <param name="regexp">A variable name or string literal containing the regular expression pattern</param>
        /// <returns></returns>
        public MatchCollection Matches(Regex regexp)
        {
            if (regexp == null) return Matches(new Regex(""));
            return regexp.Matches(value);
        }

        /// <summary>
        /// Returns the String value result of normalizing the string into the normalization form
        /// named by form as specified in Unicode Standard Annex #15, Unicode Normalization Forms.
        /// </summary>
        /// <param name="form">Applicable values: "NFC", "NFD", "NFKC", or "NFKD", If not specified default
        /// is "NFC"</param>
        /// <returns></returns>
        public String Normalize(string form)
        {
            switch (form.ToUpper())
            {
                case "NFC":
                    return Normalize(NormalizationForm.FormC);
                case "NFD":
                    return Normalize(NormalizationForm.FormD);
                case "NFKC":
                    return Normalize(NormalizationForm.FormKC);
                case "NFKD":
                    return Normalize(NormalizationForm.FormKD);
                default:
                    throw new ArgumentOutOfRangeException("form", "Expected form to be 'NFC', 'NFD', 'NFKC', or 'NFKD'.");
            }
        }

        /// <summary>
        /// Returns the String value result of normalizing the string into the normalization form
        /// named by form as specified in Unicode Standard Annex #15, Unicode Normalization Forms.
        /// </summary>
        /// <param name="form">Applicable values: "NFC", "NFD", "NFKC", or "NFKD", If not specified default
        /// is "NFC"</param>
        /// <returns></returns>
        public String Normalize(NormalizationForm form = NormalizationForm.FormC) => new String(value.Normalize(form));

        private string GetFiller(int maxLength, string fillString)
        {
            if (fillString is null) fillString = " ";
            if (fillString == "") return null;
            int fillLen = fillString.Length;
            string result = "";
            for (int index = 0, length = maxLength - Length - fillLen + 1; index < length; index += fillLen)
            {
                result += fillString;
            }
            return result;
        }

        /// <summary>
        /// Pads the current string with a given string (possibly repeated) so that the resulting string reaches a given length.
        /// The padding is applied from the end (right) of the current string.
        /// </summary>
        /// <param name="maxLength">The length of the resulting string once the current string has been padded.
        /// If this parameter is smaller than the current string's length, the current string will be returned as it is.</param>
        /// <param name="fillString">The string to pad the current string with.
        /// If this string is too long, it will be truncated and the left-most part will be applied.
        /// The default value for this parameter is " " (U+0020).</param>
        /// <returns></returns>
        public String PadEnd(int maxLength, string fillString = " ")
        {
            string filler = GetFiller(maxLength, fillString);
            if (filler is null) return this;
            return new String(value + filler);
        }

        /// <summary>
        /// Pads the current string with a given string (possibly repeated) so that the resulting string reaches a given length.
        /// The padding is applied from the start (left) of the current string.
        /// </summary>
        /// <param name="maxLength">The length of the resulting string once the current string has been padded.
        /// If this parameter is smaller than the current string's length, the current string will be returned as it is.</param>
        /// <param name="fillString">The string to pad the current string with.
        /// If this string is too long, it will be truncated and the left-most part will be applied.
        /// The default value for this parameter is " " (U+0020).</param>
        /// <returns></returns>
        public String PadStart(int maxLength, string fillString = " ")
        {
            string filler = GetFiller(maxLength, fillString);
            if (filler is null) return this;
            return new String(filler + value);
        }

        /// <summary>
        /// Returns a String value that is made from count copies appended together. If count is 0,
        /// the empty string is returned.
        /// </summary>
        /// <param name="count">number of copies to append</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if count is negative</exception>
        /// <returns></returns>
        public String Repeat(int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Cannot repeat a string a negative amount of times");
            string result = "";
            for (int index = 0; index < count; index++)
                result += value;
            return new String(result);
        }

        /// <summary>
        /// Gets the substitution for a given string from <see cref="Replace(string, string)"/> or any other method of Replace where the first parameter is not a <see cref="Regex"/>.
        /// </summary>
        /// <param name="str">The original string</param>
        /// <param name="matched">The string that was matched</param>
        /// <param name="position">The position that it was matched at</param>
        /// <param name="groups">The groups of the match</param>
        /// <param name="replacement">The replacement to replace the string that was matched</param>
        /// <returns>A new string where matched is successfully replaced with replacement</returns>
        private string GetSubstitution(string str, string matched, int position, GroupCollection groups, string replacement)
        {
            Regex numbersRegex = new Regex("[0-9]", RegexOptions.Compiled);
            Dictionary<string, string> namedCaptures = new Dictionary<string, string>();
            List<string> captures = new List<string>();
            if (groups != null)
            {
                for (int i = 0, l = groups.Count; i < l; i++)
                {
                    var g = groups[i];
                    string v = g.Value;
                    if (v == null) v = "";
                    if (g.Name != null)
                    {
                        namedCaptures.Add(g.Name, v);
                    }
                    captures.Add(v);
                }
            }
            string result = "";
            string query = "";
            int stringLength = str.Length;
            int tailPos = position + matched.Length;
            for (int index = 0, length = replacement.Length; index < length; index++)
            {
                char c = replacement[index];
                if (query.Length == 0 && c != '$') result += c;
                else if (query.Length == 1)
                {
                    if (c == '$')
                    {
                        result += "$";
                        query = "";
                    }
                    else if (c == '&')
                    {
                        result += matched;
                        query = "";
                    }
                    else if (c == '`')
                    {
                        if (position != 0)
                            result += str.Substring(0, position);
                        query = "";
                    }
                    else if (c == '\'')
                    {
                        if (tailPos < stringLength)
                            result += str.Substring(tailPos);
                        query = "";
                    }
                    else if (numbersRegex.IsMatch(c.ToString()) && c != '0')
                    {
                        int wantedGroup = int.Parse(c.ToString());
                        if (index + 1 != length)
                        {
                            char nextValue = replacement[index + 1];
                            if (numbersRegex.IsMatch(nextValue.ToString()))
                                wantedGroup = (wantedGroup * 10) + int.Parse(nextValue.ToString());
                        }
                        if (wantedGroup < captures.Count)
                            result += captures[wantedGroup];
                    }
                    else if (c == '<')
                    {
                        query += c;
                        for (int i = index, l = Length; i < l; i++)
                        {
                            c = replacement[i];
                            if (c == '>')
                            {
                                if (namedCaptures.TryGetValue(query, out string cap))
                                {
                                    result += cap;
                                    index = i;
                                    break;
                                }
                            }
                        }
                        query = "";
                    }
                    else
                    {
                        result += '$';
                        result += c;
                        query = "";
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Replaces text in a string, using a regular expression or search string.
        /// </summary>
        /// <param name="searchValue">A string to search for.</param>
        /// <param name="replaceValue">A string containing the text to replace for every successful match of searchValue in this string.</param>
        /// <returns></returns>
        public String Replace(string searchValue, string replaceValue)
        {
            int len = Length;
            int pos = IndexOf(searchValue);
            if (pos == -1) return new String(value);
            string result = "";
            int tailPos = pos + searchValue.Length;
            for (int index = 0; index < pos; index++)
                result += value[index];
            result += GetSubstitution(value, searchValue, pos, null, replaceValue);
            for (int index = tailPos; index < len; index++)
                result += value[index];
            return new String(result);
        }

        /// <summary>
        /// Replaces text in a string, using a regular expression or search string.
        /// </summary>
        /// <param name="searchValue">A string to search for.</param>
        /// <param name="replaceValue">A string containing the text to replace for every successful match of searchValue in this string.</param>
        /// <returns></returns>
        public String Replace(Regex searchValue, string replaceValue)
        {
            MatchCollection matches = searchValue.Matches(value);
            if (matches == null || matches.Count == 0) return new String(value);
            string result = "";
            int currentIndex = 0;
            for (int index = 0, length = matches.Count; index < length; index++)
            {
                Match match = matches[index];
                int i = match.Index;
                int c = i - currentIndex;
                string m = match.Value;
                result += value.Substring(currentIndex, c);
                if (m is null) m = "";
                currentIndex += c + m.Length;
                result += GetSubstitution(value, m, i, match.Groups, replaceValue);
            }
            return new String(result);
        }

        /// <summary>
        /// Replaces text in a string, using a regular expression or search string.
        /// </summary>
        /// <param name="searchValue">A string to search for.</param>
        /// <param name="replaceValue">A function that returns the replacement text.</param>
        /// <returns></returns>
        public String Replace(string searchValue, Func<string> replaceValue)
        {
            int len = Length;
            int pos = IndexOf(searchValue);
            if (pos == -1) return new String(value);
            string result = "";
            int tailPos = pos + searchValue.Length;
            for (int index = 0; index < pos; index++)
                result += value[index];
            result += replaceValue == null ? "" : replaceValue.Invoke();
            for (int index = tailPos; index < len; index++)
                result += value[index];
            return new String(result);
        }

        /// <summary>
        /// Replaces text in a string, using a regular expression or search string.
        /// </summary>
        /// <param name="searchValue">A string to search for.</param>
        /// <param name="replaceValue">A function that returns the replacement text.</param>
        /// <returns></returns>
        public String Replace(string searchValue, Func<string, string> replaceValue)
        {
            int len = Length;
            int pos = IndexOf(searchValue);
            if (pos == -1) return new String(value);
            string result = "";
            int tailPos = pos + searchValue.Length;
            for (int index = 0; index < pos; index++)
                result += value[index];
            result += replaceValue == null ? "" : replaceValue.Invoke(searchValue);
            for (int index = tailPos; index < len; index++)
                result += value[index];
            return new String(result);
        }

        /// <summary>
        /// Replaces text in a string, using a regular expression or search string.
        /// </summary>
        /// <param name="searchValue">A string to search for.</param>
        /// <param name="replaceValue">A function that returns the replacement text.</param>
        /// <returns></returns>
        public String Replace(string searchValue, Func<string, int, string> replaceValue)
        {
            int len = Length;
            int pos = IndexOf(searchValue);
            if (pos == -1) return new String(value);
            string result = "";
            int tailPos = pos + searchValue.Length;
            for (int index = 0; index < pos; index++)
                result += value[index];
            result += replaceValue == null ? "" : replaceValue.Invoke(searchValue, pos);
            for (int index = tailPos; index < len; index++)
                result += value[index];
            return new String(result);
        }

        /// <summary>
        /// Replaces text in a string, using a regular expression or search string.
        /// </summary>
        /// <param name="searchValue">A string to search for.</param>
        /// <param name="replaceValue">A function that returns the replacement text.</param>
        /// <returns></returns>
        public String Replace(string searchValue, Func<string, int, String, string> replaceValue)
        {
            int len = Length;
            int pos = IndexOf(searchValue);
            if (pos == -1) return new String(value);
            string result = "";
            int tailPos = pos + searchValue.Length;
            for (int index = 0; index < pos; index++)
                result += value[index];
            result += replaceValue == null ? "" : replaceValue.Invoke(searchValue, pos, this);
            for (int index = tailPos; index < len; index++)
                result += value[index];
            return new String(result);
        }

        /// <summary>
        /// Replaces text in a string, using a regular expression or search string.
        /// </summary>
        /// <param name="searchValue">A string to search for.</param>
        /// <param name="replaceValue">A function that returns the replacement text.</param>
        /// <returns></returns>
        public String Replace(Regex searchValue, Func<string> replaceValue)
        {
            MatchCollection matches = searchValue.Matches(value);
            if (matches == null || matches.Count == 0) return new String(value);
            string result = "";
            int currentIndex = 0;
            for (int index = 0, length = matches.Count; index < length; index++)
            {
                Match match = matches[index];
                int i = match.Index;
                int c = i - currentIndex;
                string m = match.Value;
                result += value.Substring(currentIndex, c);
                if (m is null) m = "";
                currentIndex += c + m.Length;
                result += replaceValue == null ? "" : replaceValue.Invoke();
            }
            return new String(result);
        }

        /// <summary>
        /// Replaces text in a string, using a regular expression or search string.
        /// </summary>
        /// <param name="searchValue">A string to search for.</param>
        /// <param name="replaceValue">A function that returns the replacement text.</param>
        /// <returns></returns>
        public String Replace(Regex searchValue, Func<string, string> replaceValue)
        {
            MatchCollection matches = searchValue.Matches(value);
            if (matches == null || matches.Count == 0) return new String(value);
            string result = "";
            int currentIndex = 0;
            for (int index = 0, length = matches.Count; index < length; index++)
            {
                Match match = matches[index];
                int i = match.Index;
                int c = i - currentIndex;
                string m = match.Value;
                result += value.Substring(currentIndex, c);
                if (m is null) m = "";
                currentIndex += c + m.Length;
                result += replaceValue == null ? "" : replaceValue.Invoke(match.Value);
            }
            return new String(result);
        }

        /// <summary>
        /// Replaces text in a string, using a regular expression or search string.
        /// </summary>
        /// <param name="searchValue">A string to search for.</param>
        /// <param name="replaceValue">A function that returns the replacement text.</param>
        /// <returns></returns>
        public String Replace(Regex searchValue, Func<string, GroupCollection, string> replaceValue)
        {
            MatchCollection matches = searchValue.Matches(value);
            if (matches == null || matches.Count == 0) return new String(value);
            string result = "";
            int currentIndex = 0;
            for (int index = 0, length = matches.Count; index < length; index++)
            {
                Match match = matches[index];
                int i = match.Index;
                int c = i - currentIndex;
                string m = match.Value;
                result += value.Substring(currentIndex, c);
                if (m is null) m = "";
                currentIndex += c + m.Length;
                result += replaceValue == null ? "" : replaceValue.Invoke(match.Value, match.Groups);
            }
            return new String(result);
        }

        /// <summary>
        /// Finds the first substring match in a regular expression search.
        /// </summary>
        /// <param name="regexp">The regular expression pattern and applicable flags.</param>
        /// <returns></returns>
        public int Search(string regexp) => Search(new Regex(regexp));

        /// <summary>
        /// Finds the first substring match in a regular expression search.
        /// </summary>
        /// <param name="regexp">The regular expression pattern and applicable flags.</param>
        /// <returns></returns>
        public int Search(Regex regexp)
        {
            if (regexp == null) return -1;
            Match match = regexp.Match(value);
            if (match != null) return match.Index;
            else return -1;
        }

        /// <summary>
        /// Returns a section of a string from the specified index to the end of the string.
        /// </summary>
        /// <param name="start">The index to the beginning of the specified portion of stringObj.</param>
        /// <returns></returns>
        public String Slice(int start) => Slice(start, Length);

        /// <summary>
        /// Returns a section of a string.
        /// </summary>
        /// <param name="start">The index to the beginning of the specified portion of stringObj.</param>
        /// <param name="end">The index to the end of the specified portion of stringObj. The substring includes the characters up to, but not including, the character indicated by end.
        /// If this value is not specified, the substring continues to the end of stringObj.</param>
        /// <returns></returns>
        public String Slice(int start, int end)
        {
            int len = Length;
            int from = start < 0 ? Math.Max(len + start, 0) : Math.Min(start, len);
            int to = end < 0 ? Math.Max(len + end, 0) : Math.Min(end, len);
            int span = Math.Max(to - from, 0);
            string result = "";
            for (int index = 0; index < span; index++)
                result += value[index + to];
            return new String(result);
        }

        /// <summary>
        /// Split a string into substrings using the specified separator and return them as an array.
        /// </summary>
        /// <param name="sepeprator">A string that identifies character or characters to use in separating the string. If omitted, a single-element array containing the entire string is returned.</param>
        /// <param name="limit">A value used to limit the number of elements returned in the array.</param>
        /// <returns></returns>
        public Array<String> Split(string sepeprator, int limit = int.MaxValue) => Split(new Regex(sepeprator), limit);

        /// <summary>
        /// Split a string into substrings using the specified separator and return them as an array.
        /// </summary>
        /// <param name="seperator">A string that identifies character or characters to use in separating the string. If omitted, a single-element array containing the entire string is returned.</param>
        /// <param name="limit">A value used to limit the number of elements returned in the array.</param>
        /// <returns></returns>
        public Array<String> Split(Regex seperator, int limit = int.MaxValue)
        {
            Array<String> result = new Array<String>();
            MatchCollection matches = seperator.Matches(value);
            if (matches == null || matches.Count == 0)
            {
                result.Push(this);
                return result;
            }
            int currentIndex = 0;
            for (int index = 0, length = Math.Min(matches.Count, limit); index < length; index++)
            {
                Match match = matches[index];
                int i = match.Index;
                int c = i - currentIndex;
                string m = match.Value;
                result.Push(new String(value.Substring(currentIndex, c)));
                if (m is null) m = "";
                currentIndex += c + m.Length;
            }
            return result;
        }

        /// <summary>
        /// Returns true if the sequence of elements of searchString converted to a String is the
        /// same as the corresponding elements of this object (converted to a String) starting at
        /// position. Otherwise returns false.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool StartsWith(string searchString, int position = 0)
        {
            if (string.IsNullOrEmpty(searchString)) return true;
            int len = Length;
            int start = Math.Min(Math.Max(position, 0), len);
            int searchLength = searchString.Length;
            if (searchLength + start > len) return false;
            for (int index = 0; index < searchLength; index++)
            {
                if (value[index + start] != searchString[index])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the substring at the specified location within a String object.
        /// </summary>
        /// <param name="start">The zero-based index number indicating the beginning of the substring.</param>
        /// <returns></returns>
        public String Substring(int start) => Substring(start, Length);

        /// <summary>
        /// Returns the substring at the specified location within a String object.
        /// </summary>
        /// <param name="start">The zero-based index number indicating the beginning of the substring.</param>
        /// <param name="end">Zero-based index number indicating the end of the substring. The substring includes the characters up to, but not including, the character indicated by end.
        /// If end is omitted, the characters from start through the end of the original string are returned.</param>
        /// <returns></returns>
        public String Substring(int start, int end)
        {
            int len = Length;
            int finalStart = Math.Min(Math.Max(start, 0), len);
            int finalEnd = Math.Min(Math.Max(end, 0), len);
            int from = Math.Min(finalStart, finalEnd);
            int to = Math.Max(finalStart, finalEnd);
            string result = "";
            for (int index = from; index < to; index++)
                result += value[index];
            return new String(result);
        }

        /// <summary>
        /// Converts all alphabetic characters to lowercase, taking into account the host environment's current locale.
        /// </summary>
        /// <param name="locales"></param>
        /// <returns></returns>
        public String ToLocaleLowerCase(string locales) => new String(value.ToLower());

        /// <summary>
        /// Converts all alphabetic characters to lowercase, taking into account the host environment's current locale.
        /// </summary>
        /// <param name="locales"></param>
        /// <returns></returns>
        public String ToLocaleLowerCase(string[] locales) => new String(value.ToLower());

        /// <summary>
        /// Returns a string where all alphabetic characters have been converted to uppercase, taking into account the host environment's current locale.
        /// </summary>
        /// <param name="locales"></param>
        /// <returns></returns>
        public String ToLocaleUpperCase(string locales) => new String(value.ToUpper());

        /// <summary>
        /// Returns a string where all alphabetic characters have been converted to uppercase, taking into account the host environment's current locale.
        /// </summary>
        /// <param name="locales"></param>
        /// <returns></returns>
        public String ToLocaleUpperCase(string[] locales) => new String(value.ToUpper());

        /// <summary>
        /// Converts all the alphabetic characters in a string to lowercase.
        /// </summary>
        /// <returns></returns>
        public String ToLowerCase() => new String(value.ToLower());

        /// <summary>
        /// Returns a string representation of a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => value;

        /// <summary>
        /// Converts all the alphabetic characters in a string to uppercase.
        /// </summary>
        /// <returns></returns>
        public String ToUpperCase() => new String(value.ToUpper());

        /// <summary>
        /// Removes the leading and trailing white space and line terminator characters from a string.
        /// </summary>
        /// <returns></returns>
        public String Trim() => new String(value.Trim());

        /// <summary>
        /// Removes the trailing white space and line terminator characters from a string.
        /// </summary>
        /// <returns></returns>
        public String TrimEnd() => new String(value.TrimEnd());

        /// <summary>
        /// Removes the leading white space and line terminator characters from a string.
        /// </summary>
        /// <returns></returns>
        public String TrimStart() => new String(value.TrimStart());

        /// <summary>
        /// Returns the primitive value of the specified object.
        /// </summary>
        /// <returns></returns>
        public string ValueOf() => value;

        /// <summary>
        /// Returns the length of a String object.
        /// </summary>
        public int Length => value.Length;

        /// <inheritdoc/>
        public String this[int index] => CharAt(index);

        /// <summary>
        /// Creates a string from the given character codes
        /// </summary>
        /// <param name="codeUnits">The code units</param>
        /// <returns></returns>
        public static String FromCharCode(params int[] codeUnits)
        {
            string str = "";
            for (int index = 0, length = codeUnits.Length; index < length; index++)
                str += (char)codeUnits[index];
            return new String(str);
        }

        /// <summary>
        /// Return the String value whose elements are, in order, the elements in the List elements.
        /// If length is 0, the empty string is returned.
        /// </summary>
        /// <param name="codePoints">The code points</param>
        /// <returns></returns>
        public static String FromCodePoint(params int[] codePoints)
        {
            string str = "";
            for (int index = 0, length = codePoints.Length; index < length; index++)
                str += (char)codePoints[index];
            return new String(str);
        }

        /// <inheritdoc/>
        public int CompareTo(object other) => value.CompareTo(other);

        /// <inheritdoc/>
        public int CompareTo(string other) => value.CompareTo(other);

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is String str) return str.value == value;
            else if (obj is string strValue) return strValue == value;
            else return obj.ToString() == value;
        }

        /// <inheritdoc/>
        public override int GetHashCode() => value.GetHashCode();

        /// <summary>
        /// Implicitly converts a C# String to this ECMAScript string.
        /// </summary>
        /// <param name="str">The string</param>
        public static implicit operator String(string str) => new String(str);

        /// <summary>
        /// Implicitly converts an ECMAScript string to a C# String.
        /// </summary>
        /// <param name="str">The string</param>
        public static implicit operator string(String str) => str.value;

        /// <summary>
        /// Adds two strings together
        /// </summary>
        /// <param name="str1">String A</param>
        /// <param name="str2">String B</param>
        /// <returns>A new string which holds the string value of String A combined with the string value of String B</returns>
        public static String operator +(String str1, String str2)
        {
            if (str1 is null) return null;
            else if (str2 is null) return null;
            else return new String(str1.value + str2.value);
        }

        /// <summary>
        /// (Inspired by Python) Simplification of <see cref="Repeat(int)"/>.
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="count">The amount of times to repeat</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if count is negative</exception>
        /// <returns></returns>
        public static String operator *(String str, int count)
        {
            if (str is null) return null;
            else return str.Repeat(count);
        }

        /// <summary>
        /// (Inspired by JS) Simplification of <c><see cref="String"/> is <see langword="null"/> || <see cref="Length"/> == 0</c>. This will return true if the given string is not null and it's not empty. Otherwise will return false.
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <returns></returns>
        public static bool operator !(String str) => str is null || str.Length == 0;

        /// <summary>
        /// Returns whether two ECMAScript strings are equal to each other
        /// </summary>
        /// <param name="a">String A</param>
        /// <param name="b">String B</param>
        /// <returns>Whether or not they are equal (true or false).</returns>
        public static bool operator ==(String a, String b)
        {
            bool aNull = a is null;
            bool bNull = b is null;
            if (aNull || bNull) return (aNull && bNull);
            return a.value == b.value;
        }

        /// <summary>
        /// Returns whether two ECMAScript strings are not equal to each other
        /// </summary>
        /// <param name="a">String A</param>
        /// <param name="b">String B</param>
        /// <returns>Whether or not they aren't equal (true or false).</returns>
        public static bool operator !=(String a, String b)
        {
            bool aNull = a is null;
            bool bNull = b is null;
            if (aNull || bNull) return !(aNull && bNull);
            return !(a.value == b.value);
        }
    }
}