using DiscordJS.Resolvables;
using DiscordJS.Data;
using JavaScript;
using JavaScript.Web;
using System;
using System.Text.RegularExpressions;
using NodeJS.Modules;

namespace DiscordJS
{
    /// <summary>
    /// Contains various general-purpose utility methods. These functions are also available on the base <c>Discord</c> object.
    /// </summary>
    public static class DiscordUtil
    {
        public sealed class ParsedEmoji
        {
            /// <inheritdoc cref="Emoji.Animated"/>
            public bool Animated { get; }

            /// <inheritdoc cref="Emoji.Name"/>
            public string Name { get; }

            /// <inheritdoc cref="Emoji.ID"/>
            public Snowflake ID { get; }

            internal ParsedEmoji(bool animated, string name, Snowflake id)
            {
                Animated = animated;
                Name = name;
                ID = id;
            }
        }

        /// <summary>
        /// What types of markdown to escape
        /// </summary>
        public sealed class EscapeMarkdownOptions
        {
            /// <summary>
            /// Whether to escape code blocks or not
            /// </summary>
            public bool CodeBlock { get; set; } = true;

            /// <summary>
            /// Whether to escape inline code or not
            /// </summary>
            public bool InlineCode { get; set; } = true;

            /// <summary>
            /// Whether to escape bolds or not
            /// </summary>
            public bool Bold { get; set; } = true;

            /// <summary>
            /// Whether to escape italics or not
            /// </summary>
            public bool Italic { get; set; } = true;

            /// <summary>
            /// Whether to escape underlines or not
            /// </summary>
            public bool Underline { get; set; } = true;

            /// <summary>
            /// Whether to escape strikethroughs or not
            /// </summary>
            public bool Strikethrough { get; set; } = true;

            /// <summary>
            /// Whether to escape spoilers or not
            /// </summary>
            public bool Spoiler { get; set; } = true;

            /// <summary>
            /// Whether to escape text inside code blocks or not
            /// </summary>
            public bool CodeBlockContent { get; set; } = true;

            /// <summary>
            /// Whether to escape text inside inline code or not
            /// </summary>
            public bool InlineCodeContent { get; set; } = true;
        }

        /// <summary>
        /// Splits a string into multiple chunks at a designated character that do not exceed a specific length.
        /// </summary>
        /// <param name="content">Content to split</param>
        /// <param name="options">Options controlling the behavior of the split</param>
        /// <returns></returns>
        public static Array<string> SplitMessage(StringResolvable content, SplitOptions options = null)
        {
            if (options == null) options = new SplitOptions();
            string character = options.Char, prepend = options.Prepend, append = options.Append;
            int maxLength = options.MaxLength;
            JavaScript.String text = ResolveString(content);
            if (text.Length <= maxLength) return new Array<string>(text);
            Array<JavaScript.String> splitText = text.Split(character);
            if (splitText.Some((chunk) => chunk.Length > maxLength)) throw new DJSError.Error("SPLIT_MAX_LEN");
            Array<string> messages = new Array<string>();
            string msg = "";
            for (int index = 0, length = splitText.Length; index < length; index++)
            {
                string chunk = splitText[index];
                if (msg.Length > 0 && (msg + character + chunk + append).Length > options.MaxLength)
                {
                    messages.Push(msg + append);
                    msg = prepend;
                }
                msg += (msg.Length > 0 && msg != prepend ? character : "") + chunk;
            }
            messages.Push(msg);
            return messages.Filter((m) => m.Length > 0);
        }

        /// <summary>
        /// Escapes any Discord-flavour markdown in a string.
        /// </summary>
        /// <param name="content">Content to escape</param>
        /// <param name="options">What types of markdown to escape</param>
        /// <returns></returns>
        public static string EscapeMarkdown(string content, EscapeMarkdownOptions options)
        {
            JavaScript.String text = content;
            if (options == null) options = new EscapeMarkdownOptions();
            bool codeBlock = options.CodeBlock, 
                inlineCode = options.InlineCode, 
                bold = options.Bold, 
                italic = options.Italic, 
                underline = options.Underline, 
                strikethrough = options.Strikethrough, 
                spoiler = options.Spoiler, 
                codeBlockContent = options.CodeBlockContent, 
                inlineCodeContent = options.InlineCodeContent;
            if (!codeBlockContent)
            {
                return text.Split("```").Map((substring, index, array) =>
                {
                    if (index % 2 == 1 && index != array.Length - 1) return substring;
                    return EscapeMarkdown(substring, new EscapeMarkdownOptions()
                    {
                        InlineCode = inlineCode,
                        Bold = bold,
                        Italic = italic,
                        Underline = underline,
                        Strikethrough = strikethrough,
                        Spoiler = spoiler,
                        InlineCodeContent = inlineCodeContent
                    });
                }).Join(codeBlock ? "\\`\\`\\`" : "```");
            }
            if (!inlineCodeContent)
            {
                return text.Split(EscapeInlineCodeRegex).Map((substring, index, array) =>
                {
                    if (index % 2 == 1 && index != array.Length - 1) return substring;
                    return EscapeMarkdown(substring, new EscapeMarkdownOptions()
                    {
                        CodeBlock = codeBlock,
                        Bold = bold,
                        Italic = italic,
                        Underline = underline,
                        Strikethrough = strikethrough,
                        Spoiler = spoiler
                    });
                }).Join(inlineCode ? "\\`" : "`");
            }
            if (inlineCode) text = EscapeInlineCode(text);
            if (codeBlock) text = EscapeCodeBlock(text);
            if (italic) text = EscapeItalic(text);
            if (bold) text = EscapeBold(text);
            if (underline) text = EscapeUnderline(text);
            if (strikethrough) text = EscapeStrikethrough(text);
            if (spoiler) text = EscapeSpoiler(text);
            return text;
        }

        /// <summary>
        /// Escapes code block markdown in a string.
        /// </summary>
        /// <param name="content">Content to escape</param>
        /// <returns></returns>
        public static string EscapeCodeBlock(string content)
        {
            JavaScript.String text = content;
            return text.Replace(EscapeCodeBlockRegex, "\\`\\`\\`");
        }

        /// <summary>
        /// Escapes inline code markdown in a string.
        /// </summary>
        /// <param name="content">Content to escape</param>
        /// <returns></returns>
        public static string EscapeInlineCode(string content)
        {
            JavaScript.String text = content;
            return text.Replace(EscapeInlineCodeRegex, "\\`");
        }

        /// <summary>
        /// Escapes italic markdown in a string.
        /// </summary>
        /// <param name="content">Content to escape</param>
        /// <returns></returns>
        public static string EscapeItalic(string content)
        {
            JavaScript.String text = content;
            int i = 0;
            text = text.Replace(EscapeItalicRegexA, (string _, GroupCollection groups) =>
            {
                string match = groups[0].Value;
                if (match == "**") return ++i % 2 != 0 ? $"\\*{match}" : $"{match}\\*";
                return $"\\*{match}";
            });
            i = 0;
            return text.Replace(EscapeItalicRegexB, (string _, GroupCollection groups) =>
            {
                string match = groups[0].Value;
                if (match == "__") return ++i % 2 != 0 ? $"\\_{match}" : $"{match}\\_";
                return $"\\_{match}";
            });
        }

        /// <summary>
        /// Escapes bold markdown in a string.
        /// </summary>
        /// <param name="content">Content to escape</param>
        /// <returns></returns>
        public static string EscapeBold(string content)
        {
            JavaScript.String text = content;
            int i = 0;
            return text.Replace(EscapeBoldRegex, (string _, GroupCollection groups) =>
            {
                Group group = groups[0];
                if (group.Success)
                {
                    string match = group.Value;
                    return ++i % 2 != 0 ? $"{match}\\*\\*" : $"\\*\\*{match}";
                }
                return "\\*\\*";
            });
        }

        /// <summary>
        /// Escapes underline markdown in a string.
        /// </summary>
        /// <param name="content">Content to escape</param>
        /// <returns></returns>
        public static string EscapeUnderline(string content)
        {
            JavaScript.String text = content;
            int i = 0;
            return text.Replace(EscapeUnderlineRegex, (string _, GroupCollection groups) =>
            {
                Group group = groups[0];
                if (group.Success)
                {
                    string match = group.Value;
                    return ++i % 2 != 0 ? $"{match}\\_\\_" : $"\\_\\_{match}";
                }
                return "\\_\\_";
            });
        }

        /// <summary>
        /// Escapes strikethrough markdown in a string.
        /// </summary>
        /// <param name="content">Content to escape</param>
        /// <returns></returns>
        public static string EscapeStrikethrough(string content)
        {
            JavaScript.String text = content;
            return text.Replace(EscapeStrikethroughRegex, "\\~\\~");
        }

        /// <summary>
        /// Escapes spoiler markdown in a string.
        /// </summary>
        /// <param name="content">Content to escape</param>
        /// <returns></returns>
        public static string EscapeSpoiler(string content)
        {
            JavaScript.String text = content;
            return text.Replace(EscapeSpoilerRegex, "\\|\\|");
        }

        /// <summary>
        /// Gets the recommended shard count from Discord.
        /// </summary>
        /// <param name="token">Discord auth token</param>
        /// <param name="guildsPerShard">Number of guilds per shard</param>
        /// <returns>The recommended number of shards</returns>
        public static IPromise<int> FetchRecommendedShards(string token, int guildsPerShard = 1000)
        {
            if (token == null) throw new DJSError.Error("TOKEN_MISSING");
            ClientOptions DefaultOptions = new ClientOptions();
            return FetchAPI.Fetch($"{DefaultOptions.http.api}/v{DefaultOptions.http.version}{Endpoints.BotGateway}", new RequestInit()
            {
                Method = "GET",
                Headers = new HeadersInit()
                {
                    ["Authorization"] = $"Bot {FetchRecommendedShardsRegex.Replace(token, "")}"
                }
            }).Then((res) =>
            {
                if (res.Ok) return res.JSON<BotGatewayData>();
                throw new Exception();
            }).Then((data) => data.shards * (1000 / guildsPerShard));
        }

        /// <summary>
        /// Parses emoji info out of a string. The string must be one of:
        /// <list type="bullet">
        /// <item>A UTF-8 emoji (no ID)</item>
        /// <item>A URL-encoded UTF-8 emoji (no ID)</item>
        /// <item>A Discord custom emoji (`<:name:id>` or `<a:name:id>`)</item>
        /// </list>
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static ParsedEmoji ParseEmoji(string text)
        {
            JavaScript.String Text = text;
            if (Text.Includes("%"))
            {
                text = Uri.EscapeDataString(text);
                Text = text;
            }
            if (!Text.Includes(":")) return new ParsedEmoji(false, text, null);
            Match m = Text.Match(ParseEmojiRegex);
            if (m == null || !m.Success) return null;
            return new ParsedEmoji(bool.Parse(m.Groups[1].Value), m.Groups[2].Value, m.Groups[3].Value);
        }

        /// <summary>
        /// Shallow-copies an object
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="obj">Object to clone</param>
        /// <returns></returns>
        public static T CloneObject<T>(T obj) where T : class
        {
            if (obj is Base b) return b._Clone() as T;
            else if (obj is ICloneable cloneable) return cloneable.Clone() as T;
            else throw new Exception("Object is not able to be cloned");
        }

        /// <summary>
        /// Sets default properties on an object that aren't already specified.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="def">Default properties</param>
        /// <param name="given">Object to assign defaults to</param>
        /// <returns></returns>
        public static T MergeDefault<T>(T def, T given) where T : IDefaultableObject
        {
            given.FromDefault(def);
            return given;
        }

        /// <summary>
        /// Sets default properties on an object that aren't already specified.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="given">Object to assign defaults to</param>
        /// <returns></returns>
        public static T MergeDefault<T>(T given) where T : IDefaultableObject
        {
            given.ImplementDefault();
            return given;
        }

        /// <summary>
        /// Converts an ArrayBuffer or string to a Buffer.
        /// </summary>
        /// <param name="ab">ArrayBuffer to convert</param>
        // public static Buffer ConvertToBuffer(ArrayBuffer ab) => Buffer.From(buffer);

        /// <summary>
        /// Converts an ArrayBuffer or string to a Buffer.
        /// </summary>
        /// <param name="ab">String to convert</param>
        // public static Buffer ConvertToBuffer(string ab) => Buffer.From(Str2AB(ab));

        /// <summary>
        /// Converts a string to an ArrayBuffer.
        /// </summary>
        /// <param name="str">String to convert</param>
        // public static ArrayBuffer Str2AB(str)
        // {
        //     var buffer = new ArrayBuffer(str.Length * 2);
        //     var view = new UInt16Array(buffer)
        //     for (int i = 0, strLength = str.Length; i < strLen; i++) view[i] = ((JavaScript.String)str).CharCodeAt(i);
        //     return buffer;
        // }

        /// <summary>
        /// Moves an element in an array <i>in place</i>.
        /// </summary>
        /// <typeparam name="T">The type of items in the array</typeparam>
        /// <param name="array">Array to modify</param>
        /// <param name="element">Element to move</param>
        /// <param name="newIndex">Index or offset to move the element to</param>
        /// <param name="offset">Move the element by an offset amount rather than to a set index</param>
        /// <returns></returns>
        public static int MoveElementInArray<T>(Array<T> array, T element, int newIndex, bool offset = false)
        {
            int index = array.IndexOf(element);
            newIndex = (offset ? index : 0) + newIndex;
            if (newIndex > -1 && newIndex < array.Length)
            {
                T removedItem = array.Splice(index, 1)[0];
                array.Splice(newIndex, 0, removedItem);
            }
            return array.IndexOf(element);
        }

        /// <summary>
        /// Resolves a StringResolvable to a string.
        /// </summary>
        /// <param name="data">The string resolvable to resolve</param>
        /// <returns></returns>
        public static string ResolveString(StringResolvable data)
        {
            if (data == null) return "null";
            string result = data.Resolve();
            if (result == null) return "null";
            return result;
        }

        /// <summary>
        /// Resolves a ColorResolvable into a color number.
        /// </summary>
        /// <param name="color">Color to resolve</param>
        /// <returns>A color</returns>
        public static int ResolveColor(ColorResolvable color)
        {
            if (color == null) throw new DJSError.Error("COLOR_CONVERT");
            else return color.Resolve();
        }

        /// <summary>
        /// Sorts by Discord's position and ID.
        /// </summary>
        /// <param name="collection">Collection of objects to sort</param>
        /// <returns></returns>
        public static ICollection<Snowflake, Role> DiscordSort(ICollection<Snowflake, Role> collection)
        {
            return collection.Sorted((a, b) =>
            {
                int result = a.RawPosition - b.RawPosition;
                if (result != 0) return result;
                result = int.Parse(b.ID.Slice(0, -10)) - int.Parse(a.ID.Slice(0, -10));
                if (result != 0) return result;
                return int.Parse(b.ID.Slice(10)) - int.Parse(a.ID.Slice(10));
            });
        }

        /// <summary>
        /// Sorts by Discord's position and ID.
        /// </summary>
        /// <param name="collection">Collection of objects to sort</param>
        /// <returns></returns>
        public static Collection<Snowflake, Role> DiscordSort(Collection<Snowflake, Role> collection)
        {
            return collection.Sorted((a, b) =>
            {
                int result = a.RawPosition - b.RawPosition;
                if (result != 0) return result;
                result = int.Parse(b.ID.Slice(0, -10)) - int.Parse(a.ID.Slice(0, -10));
                if (result != 0) return result;
                return int.Parse(b.ID.Slice(10)) - int.Parse(a.ID.Slice(10));
            });
        }

        /// <summary>
        /// Sorts by Discord's position and ID.
        /// </summary>
        /// <param name="collection">Collection of objects to sort</param>
        /// <returns></returns>
        public static ICollection<Snowflake, GuildChannel> DiscordSort(ICollection<Snowflake, GuildChannel> collection) 
        {
            return collection.Sorted((a, b) =>
            {
                int result = a.RawPosition - b.RawPosition;
                if (result != 0) return result;
                result = int.Parse(b.ID.Slice(0, -10)) - int.Parse(a.ID.Slice(0, -10));
                if (result != 0) return result;
                return int.Parse(b.ID.Slice(10)) - int.Parse(a.ID.Slice(10));
            });
        }

        /// <summary>
        /// Sorts by Discord's position and ID.
        /// </summary>
        /// <param name="collection">Collection of objects to sort</param>
        /// <returns></returns>
        public static Collection<Snowflake, GuildChannel> DiscordSort(Collection<Snowflake, GuildChannel> collection)
        {
            return collection.Sorted((a, b) =>
            {
                int result = a.RawPosition - b.RawPosition;
                if (result != 0) return result;
                result = int.Parse(b.ID.Slice(0, -10)) - int.Parse(a.ID.Slice(0, -10));
                if (result != 0) return result;
                return int.Parse(b.ID.Slice(10)) - int.Parse(a.ID.Slice(10));
            });
        }

        /// <summary>
        /// Sets the position of a Channel or Role.
        /// </summary>
        /// <typeparam name="T">The type of item</typeparam>
        /// <param name="item">Object to set the position of</param>
        /// <param name="position">New position for the object</param>
        /// <param name="relative">Whether <paramref name="position"/> is relative to its current position</param>
        /// <param name="sorted">A collection of the objects sorted properly</param>
        /// <param name="route">Route to call PATCH on</param>
        /// <param name="reason">Reason for the change</param>
        /// <returns>Updated item list, with <c>id</c> and <c>position</c> properties</returns>
        public static IPromise<Array<ItemWithPositionAndID>> SetPosition<T>(T item, int position, bool relative, Collection<Snowflake, T> sorted, dynamic route, string reason) where T : IHasID
        {
            Array<T> initialItems = sorted.Array();
            MoveElementInArray(initialItems, item, position, relative);
            Array<ItemWithPositionAndID> updatedItems = initialItems.Map((r, i) => new ItemWithPositionAndID(r.ID, i));
            IPromise promise = route.Patch(new { data = updatedItems.ToArray(), reason });
            return promise.Then(() => Promise<Array<ItemWithPositionAndID>>.Resolved(updatedItems));
        }

        /// <summary>
        /// Alternative to Node's <c>path.basename</c>, removing query string after the extension if it exists.
        /// </summary>
        /// <param name="path">Path to get the basename of</param>
        /// <param name="ext">File extension to remove</param>
        /// <returns>Basename of the path</returns>
        public static string Basename(string path, string ext = null)
        {
            var res = Path.Parse(path);
            return ext != null && res.Ext.StartsWith(ext) ? res.Name : new JavaScript.String(res.Base).Split("?")[0].ToString();
        }

        /// <summary>
        /// Transforms a snowflake from a decimal string to a bit string.
        /// </summary>
        /// <param name="num">Snowflake to be transformed</param>
        /// <returns></returns>
        public static string IDToBinary(Snowflake num)
        {
            return Convert.ToString(Convert.ToInt32(num), 2);
        }

        /// <summary>
        /// Transforms a snowflake from a bit string to a decimal string.
        /// </summary>
        /// <param name="num">Bit string to be transformed</param>
        /// <returns></returns>
        public static Snowflake BinaryToID(int num)
        {
            return Convert.ToInt32(num.ToString(), 2).ToString();
        }

        /// <summary>
        /// Transforms a snowflake from a bit string to a decimal string.
        /// </summary>
        /// <param name="num">Bit string to be transformed</param>
        /// <returns></returns>
        public static Snowflake BinaryToID(string num)
        {
            return Convert.ToInt32(num, 2).ToString();
        }

        /// <summary>
        /// Breaks user, role and everyone/here mentions by adding a zero width space after every @ character
        /// </summary>
        /// <param name="str">The string to sanitize</param>
        /// <returns></returns>
        public static string RemoveMentions(string str)
        {
            return RemoveMentionsRegex.Replace(str, "@\u200b");
        }

        /// <summary>
        /// The content to have all mentions replaced by the equivalent text.
        /// </summary>
        /// <param name="str">The string to be converted</param>
        /// <param name="message">The message object to reference</param>
        /// <returns></returns>
        public static string CleanContent(string str, Message message)
        {
            str = UserMentionRegex.Replace(str, (match) =>
            {
                string input = match.Value;
                string id = UserMentionReplaceRegex.Replace(input, "");
                if (message.Channel.Type == "dm")
                {
                    User user = message.Client.Users.Cache.Get(id);
                    return user == null ? input : $"@{user.Username}";
                }

                GuildMember member = message.Channel.Guild.Members.Cache.Get(id);
                if (member == null)
                {
                    User user = message.Client.Users.Cache.Get(id);
                    return user == null ? input : $"@{user.Username}";
                }
                else
                    return $"@{member.DisplayName}";
            });
            str = ChannelMentionRegex.Replace(str, (match) =>
            {
                string input = match.Value;
                Channel channel = message.Client.Channels.Cache.Get(ChannelMentionReplaceRegex.Replace(input, ""));
                return channel == null || channel.Type == "dm" || channel.Type == "group" ? input : $"#{((GuildChannel)channel).Name}";
            });
            str = RoleMentionRegex.Replace(str, (match) =>
            {
                string input = match.Value;
                if (message.Channel.Type == "dm") return input;
                Role role = message.Guild.Roles.Cache.Get(RoleMentionReplaceRegex.Replace(input, ""));
                return role == null ? input : $"@{role.Name}";
            });
            if (message.Client.Options.disableMentions == DisableMentionType.Everyone)
            {
                return EveryoneReplaceRegex.Replace(str, (match) =>
                {
                    string target = match.Groups[0].Value;
                    if (EveryoneTargetRegex.IsMatch(target))
                    {
                        return $"@{target}";
                    }
                    else
                    {
                        return $"@\u200b{target}";
                    }
                });
            }
            else if (message.Client.Options.disableMentions == DisableMentionType.All)
            {
                return RemoveMentions(str);
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// The content to put in a codeblock with all codeblock fences replaced by the equivalent backticks.
        /// </summary>
        /// <param name="text">The string to be converted</param>
        /// <returns></returns>
        public static string CleanCodeBlockContent(string text)
        {
            return CleanCodeBlockContentRegex.Replace(text, "`\u200b``");
        }

        /// <summary>
        /// Creates a Promise that resolves after a specified duration.
        /// </summary>
        /// <param name="ms">How long to wait before resolving (in milliseconds)</param>
        /// <returns></returns>
        public static IPromise DelayFor(long ms)
        {
            return new Promise((resolve, reject) =>
            {
                new Timeout(ms, resolve, false);
            });
        }

        internal static T FromDynamic<T>(dynamic value) where T : new()
        {
            if (value is null) return default;
            else if (value is T already) return already;
            Type tType = typeof(T);
            Type dataAttrType = typeof(DataAttribute);
            Type notDataAttrType = typeof(NotDataAttribute);
            bool assumeData = Attribute.IsDefined(tType, dataAttrType);
            return CreateDataFromDynamic<T>(tType, dataAttrType, notDataAttrType, assumeData, value);
        }

        private static T CreateDataFromDynamic<T>(Type tType, Type dataAttrType, Type notDataAttrType, bool assumeData, dynamic value) where T : new()
        {
            if (Attribute.IsDefined(tType, notDataAttrType)) return default;
            else if (value is null) return default;
            else if (value is T already) return already;
            else
            {
                try
                {
                    T result = new T();
                    Type valueType = value.GetType();
                    var properties = tType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    for (int index = 0, length = properties.Length; index < length; index++)
                    {
                        var prop = properties[index];
                        bool valid = assumeData ?
                            !Attribute.IsDefined(prop, notDataAttrType) :
                            Attribute.IsDefined(prop, dataAttrType);

                        if (valid)
                        {
                            prop.SetValue(result, valueType.GetProperty(prop.Name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(value));
                        }
                    }
                    return result;
                }
                catch (Exception)
                {
                    return default;
                }
            }
        }

        internal static T[] ArrayFromDynamic<T>(dynamic value) where T : new()
        {
            if (value is null) return null;
            else if (value is T[] alreadyArray) return alreadyArray;
            Type tType = typeof(T);
            Type dataAttrType = typeof(DataAttribute);
            Type notDataAttrType = typeof(NotDataAttribute);
            bool assumeData = Attribute.IsDefined(tType, dataAttrType);
            try
            {
                int length = value.Length;
                T[] result = new T[length];
                for (int index = 0; index < length; index++)
                {
                    dynamic val = value[index];
                    result[index] = CreateDataFromDynamic<T>(tType, dataAttrType, notDataAttrType, assumeData, val);
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static Regex EscapeCodeBlockRegex { get; }
        private static Regex EscapeInlineCodeRegex { get; }
        private static Regex EscapeItalicRegexA { get; }
        private static Regex EscapeItalicRegexB { get; }
        private static Regex EscapeBoldRegex { get; }
        private static Regex EscapeUnderlineRegex { get; }
        private static Regex EscapeStrikethroughRegex { get; }
        private static Regex EscapeSpoilerRegex { get; }

        private static Regex FetchRecommendedShardsRegex { get; }

        private static Regex ParseEmojiRegex { get; }

        private static Regex RemoveMentionsRegex { get; }

        private static Regex UserMentionRegex { get; }
        private static Regex UserMentionReplaceRegex { get; }
        private static Regex ChannelMentionRegex { get; }
        private static Regex ChannelMentionReplaceRegex { get; }
        private static Regex RoleMentionRegex { get; }
        private static Regex RoleMentionReplaceRegex { get; }
        private static Regex EveryoneReplaceRegex { get; }
        private static Regex EveryoneTargetRegex { get; }

        private static Regex CleanCodeBlockContentRegex { get; }

        static DiscordUtil()
        {
            EscapeCodeBlockRegex = new Regex("```");
            EscapeInlineCodeRegex = new Regex(@"(?<=^|[^`])`(?=[^`]|$)");
            EscapeItalicRegexA = new Regex(@"(?<=^|[^*])\*([^*]|\*\*|$)");
            EscapeItalicRegexB = new Regex(@"(?<=^|[^_])_([^_]|__|$)");
            EscapeBoldRegex = new Regex(@"\*\*(\*)?");
            EscapeUnderlineRegex = new Regex("__(_)?");
            EscapeStrikethroughRegex = new Regex("~~");
            EscapeSpoilerRegex = new Regex(@"\|\|");

            FetchRecommendedShardsRegex = new Regex(@"^Bot\s*", RegexOptions.IgnoreCase);

            ParseEmojiRegex = new Regex(@"<?(?:(a):)?(\w{2,32}):(\d{17,19})?>?");

            RemoveMentionsRegex = new Regex("@");

            UserMentionRegex = new Regex("<@!?[0-9]+>");
            UserMentionReplaceRegex = new Regex("<|!|>|@");
            ChannelMentionRegex = new Regex("<#[0-9]+>");
            ChannelMentionReplaceRegex = new Regex("<|#|>");
            RoleMentionRegex = new Regex("<@&[0-9]+>");
            RoleMentionReplaceRegex = new Regex("<|@|>|&");
            EveryoneReplaceRegex = new Regex("@([^<>@ ]*)", RegexOptions.Multiline | RegexOptions.ECMAScript); // multi-line, . match newline, unicode points
            EveryoneTargetRegex = new Regex(@"^[&!]?\d+$");

            CleanCodeBlockContentRegex = new Regex("```");
        }
    }

    public class ItemWithPositionAndID
    {
        public readonly string id;
        public readonly int position;

        public ItemWithPositionAndID(Snowflake id, int position)
        {
            this.id = id;
            this.position = position;
        }

        public static implicit operator ChannelData(ItemWithPositionAndID item) => item == null ? null : new ChannelData()
        {
            id = item.id,
            position = item.position
        };
    }
}