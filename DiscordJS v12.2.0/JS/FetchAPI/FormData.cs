using System;
using System.Collections.Generic;

namespace JavaScript.Web
{
    public class FormData
    {
        internal class Entry
        {
            public string Name { get; }
            public FormDataEntryValue Value { get; }

            public Entry(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public Entry(string name, Blob value, string fileName = "blob")
            {
                Name = name;
                Value = new File(value, fileName);
            }

            public Entry(string name, File value)
            {
                Name = name;
                Value = value;
            }
        }

        internal FormData(IDictionary<string, string> dict)
        {
            foreach (string key in dict.Keys)
            {
                entries.Add(CreateEntry(key, dict[key]));
            }
        }

        internal List<Entry> entries = new List<Entry>();

        private Entry CreateEntry(string name, string value) => new Entry(name, value);
        private Entry CreateEntry(string name, Blob value, string filename = null) 
            => value is File file ? CreateEntry(name, file, filename) : new Entry(name, value);
        private Entry CreateEntry(string name, File value, string filename = null)
            => filename == null ? new Entry(name, value) : new Entry(name, value, filename);

        public void Append(string name, string value)
        {
            entries.Add(CreateEntry(name, value));
        }

        public void Append(string name, Blob blobValue, string filename = null)
        {
            entries.Add(CreateEntry(name, blobValue, filename));
        }

        public void Delete(string name)
        {
            entries.RemoveAll((entry) => entry.Name == name);
        }
        public FormDataEntryValue Get(string name)
        {
            for (int index = 0, length = entries.Count; index < length; index++)
            {
                Entry entry = entries[index];
                if (entry.Name == name) return entry.Value;
            }
            return null;
        }
        public Array<FormDataEntryValue> GetAll(string name)
        {
            Array<FormDataEntryValue> results = new Array<FormDataEntryValue>();
            for (int index = 0, length = entries.Count; index < length; index++)
            {
                Entry entry = entries[index];
                if (entry.Name == name)
                    results.Push(entry.Value);
            }
            return results;
        }
        public bool Has(string name)
        {
            for (int index = 0, length = entries.Count; index < length; index++)
            {
                if (entries[index].Name == name)
                    return true;
            }
            return false;
        }
        public void Set(string name, string value)
        {
            List<Entry> entriesToRemove = new List<Entry>();

            int indexToInsert = entries.Count;
            bool updatedIndex = false;

            for (int index = 0, length = entries.Count; index < length; index++)
            {
                Entry entry = entries[index];
                if (entry.Name == name)
                {
                    if (!updatedIndex)
                    {
                        indexToInsert = index;
                        updatedIndex = true;
                    }
                    else
                    {
                        entriesToRemove.Add(entry);
                    }
                }
            }
            Entry newEntry = CreateEntry(name, value);
            if (updatedIndex)
            {
                entries[indexToInsert] = newEntry;
                entriesToRemove.ForEach((entry) => entries.Remove(entry));
            }
            else
                entries.Add(newEntry);
        }
        public void Set(string name, Blob blobValue, string filename)
        {
            List<Entry> entriesToRemove = new List<Entry>();

            int indexToInsert = entries.Count;
            bool updatedIndex = false;

            for (int index = 0, length = entries.Count; index < length; index++)
            {
                Entry entry = entries[index];
                if (entry.Name == name)
                {
                    if (!updatedIndex)
                    {
                        indexToInsert = index;
                        updatedIndex = true;
                    }
                    else
                    {
                        entriesToRemove.Add(entry);
                    }
                }
            }
            Entry newEntry = CreateEntry(name, blobValue, filename);
            if (updatedIndex)
            {
                entries[indexToInsert] = newEntry;
                entriesToRemove.ForEach((entry) => entries.Remove(entry));
            }
            else
                entries.Add(newEntry);
        }
    }

    public class FormDataEntryValue
    {
        internal enum Type
        {
            File,
            String
        }

        internal Type type;

        internal File file;
        internal string str;

        private FormDataEntryValue(File file)
        {
            this.file = file;
            type = Type.File;
        }

        private FormDataEntryValue(string str)
        {
            this.str = str;
            type = Type.String;
        }

        public bool IsFile() => type == Type.File;
        public bool IsString() => type == Type.String;

        public override int GetHashCode()
        {
            object original;
            if (type == Type.File)
                original = file;
            else if (type == Type.String)
                original = str;
            else
                throw new ArgumentException("The given type cannot be cast to a 'FormDataEntryValue'");
            if (original is null)
                return -1;
            else
                return original.GetHashCode();
        }

        public override string ToString()
        {
            object original;
            if (type == Type.File)
                original = file;
            else if (type == Type.String)
                original = str;
            else
                throw new ArgumentException("The given type cannot be cast to a 'FormDataEntryValue'");
            if (original is null)
                return null;
            else
                return original.ToString();
        }

        public override bool Equals(object obj)
        {
            object original;
            if (type == Type.File)
                original = file;
            else if (type == Type.String)
                original = str;
            else
                throw new ArgumentException("The given type cannot be cast to a 'FormDataEntryValue'");
            if (original is null)
                return obj is null;
            else
                return original.Equals(obj);
        }

        public static implicit operator FormDataEntryValue(string str) => new FormDataEntryValue(str);
        public static implicit operator FormDataEntryValue(String str) => new FormDataEntryValue(str);
        public static implicit operator FormDataEntryValue(DiscordJS.Snowflake str) => new FormDataEntryValue(str);
        public static implicit operator FormDataEntryValue(File file) => new FormDataEntryValue(file);

        public static explicit operator File(FormDataEntryValue value)
        {
            if (value == null) return null;
            else if (value.IsFile()) return value.file;
            else throw new InvalidCastException("The given form data entry value is not a file");
        }

        public static explicit operator string(FormDataEntryValue value)
        {
            if (value == null) return null;
            else if (value.IsString()) return value.str;
            else throw new InvalidCastException("The given form data entry value is not a string");
        }

        public static explicit operator String(FormDataEntryValue value)
        {
            if (value == null) return null;
            else if (value.IsString()) return value.str;
            else throw new InvalidCastException("The given form data entry value is not a string");
        }

        public static explicit operator DiscordJS.Snowflake(FormDataEntryValue value)
        {
            if (value == null) return null;
            else if (value.IsString()) return value.str;
            else throw new InvalidCastException("The given form data entry value is not a string");
        }
    }
}