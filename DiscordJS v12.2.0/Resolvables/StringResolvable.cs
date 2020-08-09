using System.Collections;

namespace DiscordJS.Resolvables
{
    /// <summary>
    /// Data that can be resolved to give a string. This can be:
    /// <list type="bullet">
    /// <item>A string</item>
    /// <item>An array (joined with a new line delimiter to give a string)</item>
    /// <item>Any value</item>
    /// </list>
    /// </summary>
    public class StringResolvable
    {
        internal enum Type
        {
            String,
            Array,
            Object
        }

        internal string str;
        internal IEnumerable array;
        internal object obj;
        internal Type type;

        internal string resolvedValue;
        internal bool isResolved;

        internal string Resolve()
        {
            if (isResolved)
                return resolvedValue;
            else
            {
                if (type == Type.Array)
                {
                    resolvedValue = "";
                    int index = 0;
                    foreach (object val in array)
                    {
                        if (index > 0) resolvedValue += "\n";
                        resolvedValue += val.ToString();
                        index++;
                    }
                    isResolved = true;
                }
                else if (type == Type.Object)
                {
                    resolvedValue = obj.ToString();
                    isResolved = true;
                }
                else
                {
                    resolvedValue = str;
                    isResolved = true;
                }
                return resolvedValue;
            }
        }

        /// <summary>
        /// Instantiates a string resolvable with a string value
        /// </summary>
        /// <param name="str">The string</param>
        public StringResolvable(string str)
        {
            this.str = str;
            type = Type.String;
        }

        /// <summary>
        /// Instantiates a string resolvable with a javascript string
        /// </summary>
        /// <param name="str">The string</param>
        public StringResolvable(JavaScript.String str)
        {
            this.str = str;
            type = Type.String;
        }

        /// <summary>
        /// Instantiates a string resolvable with a snowflake
        /// </summary>
        /// <param name="snowflake">The snowflake</param>
        public StringResolvable(Snowflake snowflake)
        {
            str = snowflake;
            type = Type.String;
        }

        /// <summary>
        /// Instantiates a string resolvable with an IEnumerable
        /// </summary>
        /// <param name="array">The array</param>
        public StringResolvable(IEnumerable array)
        {
            this.array = array;
            type = Type.Array;
        }

        /// <summary>
        /// Instantiates a string resolvable with an object
        /// </summary>
        /// <param name="obj">The object</param>
        public StringResolvable(object obj)
        {
            if (obj is IEnumerable array)
            {
                this.array = array;
                type = Type.Array;
            }
            else 
            {
                this.obj = obj;
                type = Type.Object;
            }
        }

        /// <inheritdoc/>
        public static implicit operator StringResolvable(string str) => new StringResolvable(str);

        /// <inheritdoc/>
        public static implicit operator StringResolvable(JavaScript.String str) => new StringResolvable(str);

        /// <inheritdoc/>
        public static implicit operator StringResolvable(Snowflake snowflake) => new StringResolvable(snowflake);
    }
}