using JavaScript;
using System;
using System.Collections.Generic;

namespace DiscordJS
{
    public abstract class DiscordJSException : Exception
    {
        public string Name { get; }
        public string Code { get; }

        internal DiscordJSException(string name, string key, object[] args) : base(DJSError.Message(key, args))
        {
            Code = key;
            Name = $"{name} [{key}]";
        }
    }

    internal class DJSError
    {
        public class Error : DiscordJSException
        {
            internal Error(string key, params object[] args) : base("Error", key, args)
            { }
        }

        private abstract class DJSErrorMessage
        {
            public abstract string Get(params object[] args);
        }

        private sealed class DJSErrorMessageString : DJSErrorMessage
        {
            private readonly string msg;

            public DJSErrorMessageString(string message)
            {
                msg = message;
            }

            public override string Get(params object[] args) => msg;
        }

        private sealed class DJSErrorMessageFunction : DJSErrorMessage
        {
            private readonly Func<string> fn;

            public DJSErrorMessageFunction(Func<string> function)
            {
                fn = function;
            }

            public override string Get(params object[] args) => Invoke();

            public string Invoke() => fn();
        }

        private sealed class DJSErrorMessageFunction<P1> : DJSErrorMessage
        {
            private readonly Func<P1, string> fn;
            private readonly bool p1Optional;
            private readonly P1 p1Default;

            public DJSErrorMessageFunction(Func<P1, string> function)
            {
                fn = function;
                p1Optional = false;
            }

            public DJSErrorMessageFunction(Func<P1, string> function, P1 p1Default)
            {
                fn = function;
                p1Optional = true;
                this.p1Default = p1Default;
            }

            public override string Get(params object[] args)
            {
                if (args.Length == 0) return Invoke();
                else return Invoke((P1)args[0]);
            }

            public string Invoke()
            {
                if (p1Optional) return Invoke(p1Default);
                throw new InvalidOperationException();
            }

            public string Invoke(P1 arg1) => fn(arg1);
        }

        private sealed class DJSErrorMessageFunction<P1, P2> : DJSErrorMessage
        {
            private readonly Func<P1, P2, string> fn;
            private readonly bool p1Optional;
            private readonly bool p2Optional;
            private readonly P1 p1Default;
            private readonly P2 p2Default;

            public DJSErrorMessageFunction(Func<P1, P2, string> function)
            {
                fn = function;
                p1Optional = false;
                p2Optional = false;
            }

            public DJSErrorMessageFunction(Func<P1, P2, string> function, P2 p2Default)
            {
                fn = function;
                p1Optional = false;
                p2Optional = true;
                this.p2Default = p2Default;
            }

            public DJSErrorMessageFunction(Func<P1, P2, string> function, P1 p1Default, P2 p2Default)
            {
                fn = function;
                p1Optional = true;
                p2Optional = true;
                this.p1Default = p1Default;
                this.p2Default = p2Default;
            }

            public override string Get(params object[] args)
            {
                if (args.Length == 0) return Invoke();
                else if (args.Length == 1) return Invoke((P1)args[0]);
                else return Invoke((P1)args[0], (P2)args[1]);
            }

            public string Invoke()
            {
                if (p1Optional && p2Optional) return Invoke(p1Default, p2Default);
                throw new InvalidOperationException();
            }

            public string Invoke(P1 arg1)
            {
                if (p2Optional) return Invoke(arg1, p2Default);
                throw new InvalidOperationException();
            }

            public string Invoke(P1 arg1, P2 arg2) => fn(arg1, arg2);
        }

        private sealed class DJSErrorMessageFunction<P1, P2, P3> : DJSErrorMessage
        {
            private readonly Func<P1, P2, P3, string> fn;
            private readonly bool p1Optional;
            private readonly bool p2Optional;
            private readonly bool p3Optional;
            private readonly P1 p1Default;
            private readonly P2 p2Default;
            private readonly P3 p3Default;

            public DJSErrorMessageFunction(Func<P1, P2, P3, string> function)
            {
                fn = function;
                p1Optional = false;
                p2Optional = false;
                p3Optional = false;
            }

            public DJSErrorMessageFunction(Func<P1, P2, P3, string> function, P3 p3Default)
            {
                fn = function;
                p1Optional = false;
                p2Optional = false;
                p3Optional = true;
                this.p3Default = p3Default;
            }

            public DJSErrorMessageFunction(Func<P1, P2, P3, string> function, P2 p2Default, P3 p3Default)
            {
                fn = function;
                p1Optional = false;
                p2Optional = true;
                p3Optional = true;
                this.p2Default = p2Default;
                this.p3Default = p3Default;
            }

            public DJSErrorMessageFunction(Func<P1, P2, P3, string> function, P1 p1Default, P2 p2Default, P3 p3Default)
            {
                fn = function;
                p1Optional = true;
                p2Optional = true;
                p3Optional = true;
                this.p1Default = p1Default;
                this.p2Default = p2Default;
                this.p3Default = p3Default;
            }

            public override string Get(params object[] args)
            {
                if (args.Length == 0) return Invoke();
                else if (args.Length == 1) return Invoke((P1)args[0]);
                else if (args.Length == 2) return Invoke((P1)args[0], (P2)args[1]);
                else return Invoke((P1)args[0], (P2)args[1], (P3)args[2]);
            }

            public string Invoke()
            {
                if (p1Optional && p2Optional && p3Optional) return Invoke(p1Default, p2Default, p3Default);
                throw new InvalidOperationException();
            }

            public string Invoke(P1 arg1)
            {
                if (p2Optional && p3Optional) return Invoke(arg1, p2Default, p3Default);
                throw new InvalidOperationException();
            }

            public string Invoke(P1 arg1, P2 arg2)
            {
                if (p3Optional) return Invoke(arg1, arg2, p3Default);
                throw new InvalidOperationException();
            }

            public string Invoke(P1 arg1, P2 arg2, P3 arg3) => fn(arg1, arg2, arg3);
        }

        private static Dictionary<string, DJSErrorMessage> messages = new Dictionary<string, DJSErrorMessage>();

        /// <summary>
        /// Register an error code and message.
        /// </summary>
        /// <param name="code">Unique name for the error</param>
        /// <param name="message">Value of the error</param>
        public static void Register(string code, string message)
        {
            code = code.ToUpper();
            var msg = new DJSErrorMessageString(message);
            if (messages.ContainsKey(code)) messages[code] = msg;
            else messages.Add(code, msg);
        }

        public static void Register(string code, Func<string> fn)
        {
            code = code.ToUpper();
            var msg = new DJSErrorMessageFunction(fn);
            if (messages.ContainsKey(code)) messages[code] = msg;
            else messages.Add(code, msg);
        }

        public static void Register<P1>(string code, Func<P1, string> fn)
        {
            code = code.ToUpper();
            var msg = new DJSErrorMessageFunction<P1>(fn);
            if (messages.ContainsKey(code)) messages[code] = msg;
            else messages.Add(code, msg);
        }

        public static void Register<P1>(string code, Func<P1, string> fn, P1 arg1Default)
        {
            code = code.ToUpper();
            var msg = new DJSErrorMessageFunction<P1>(fn, arg1Default);
            if (messages.ContainsKey(code)) messages[code] = msg;
            else messages.Add(code, msg);
        }

        public static void Register<P1, P2>(string code, Func<P1, P2, string> fn)
        {
            code = code.ToUpper();
            var msg = new DJSErrorMessageFunction<P1, P2>(fn);
            if (messages.ContainsKey(code)) messages[code] = msg;
            else messages.Add(code, msg);
        }

        public static void Register<P1, P2>(string code, Func<P1, P2, string> fn, P2 arg2Default)
        {
            code = code.ToUpper();
            var msg = new DJSErrorMessageFunction<P1, P2>(fn, arg2Default);
            if (messages.ContainsKey(code)) messages[code] = msg;
            else messages.Add(code, msg);
        }

        public static void Register<P1, P2>(string code, Func<P1, P2, string> fn, P1 arg1Default, P2 arg2Default)
        {
            code = code.ToUpper();
            var msg = new DJSErrorMessageFunction<P1, P2>(fn, arg1Default, arg2Default);
            if (messages.ContainsKey(code)) messages[code] = msg;
            else messages.Add(code, msg);
        }

        public static void Register<P1, P2, P3>(string code, Func<P1, P2, P3, string> fn)
        {
            code = code.ToUpper();
            var msg = new DJSErrorMessageFunction<P1, P2, P3>(fn);
            if (messages.ContainsKey(code)) messages[code] = msg;
            else messages.Add(code, msg);
        }

        public static void Register<P1, P2, P3>(string code, Func<P1, P2, P3, string> fn, P3 arg3Default)
        {
            code = code.ToUpper();
            var msg = new DJSErrorMessageFunction<P1, P2, P3>(fn, arg3Default);
            if (messages.ContainsKey(code)) messages[code] = msg;
            else messages.Add(code, msg);
        }

        public static void Register<P1, P2, P3>(string code, Func<P1, P2, P3, string> fn, P2 arg2Default, P3 arg3Default)
        {
            code = code.ToUpper();
            var msg = new DJSErrorMessageFunction<P1, P2, P3>(fn, arg2Default, arg3Default);
            if (messages.ContainsKey(code)) messages[code] = msg;
            else messages.Add(code, msg);
        }

        public static void Register<P1, P2, P3>(string code, Func<P1, P2, P3, string> fn, P1 arg1Default, P2 arg2Default, P3 arg3Default)
        {
            code = code.ToUpper();
            var msg = new DJSErrorMessageFunction<P1, P2, P3>(fn, arg1Default, arg2Default, arg3Default);
            if (messages.ContainsKey(code)) messages[code] = msg;
            else messages.Add(code, msg);
        }

        /// <summary>
        /// Format the message for an error.
        /// </summary>
        /// <param name="key">Error key</param>
        /// <param name="args">Arguments to pass for util format or as function args</param>
        /// <returns>Formatted string</returns>
        public static string Message(string key, params object[] args)
        {
            if (messages.TryGetValue(key, out DJSErrorMessage msg)) return msg.Get(args);
            else throw new Exception($"An invalid error message key was used: {key}");
        }

        static DJSError()
        {
            Register("CLIENT_INVALID_OPTION", (string prop, string must) => $"The {prop} option must be {must}");
            Register("CLIENT_INVALID_PROVIDED_SHARDS", "None of the provided shards were valid.");

            Register("TOKEN_INVALID", "An invalid token was provided.");
            Register("TOKEN_MISSING", "Request to use token, but token was unavailable to the client.");

            Register("WS_CLOSE_REQUESTED", "WebSocket closed due to user request.");
            Register("WS_CONNECTION_EXISTS", "There is already an existing WebSocket connection.");
            Register("WS_NOT_OPEN", (object data) => $"Websocket not open to send {data}", "data");

            Register("BITFIELD_INVALID", "Invalid bitfield flag or number.");
            Register("SHARDING_INVALID", "Invalid shard settings were provided.");
            Register("SHARDING_REQUIRED", "This session would have handled too many guilds - Sharding is required.");
            Register("INVALID_INTENTS", "Invalid intent provided for WebSocket intents.");
            Register("DISALLOWED_INTENTS", "Privileged intent provided is not enabled or whitelisted.");
            Register("SHARDING_NO_SHARDS", "No shards have been spawned.");
            Register("SHARDING_IN_PROCESS", "Shards are still being spawned.");
            Register("SHARDING_ALREADY_SPAWNED", (int count) => $"Already spawned {count} shards.");
            Register("SHARDING_PROCESS_EXISTS", (int id) => $"Shard {id} already has an active process.");
            Register("SHARDING_READY_TIMEOUT", (int id) => $"Shard {id}'s Client took too long to become ready.");
            Register("SHARDING_READY_DISCONNECTED", (int id) => $"Shard {id}'s Client disconnected before becoming ready.");
            Register("SHARDING_READY_DIED", (int id) => $"Shard {id}'s process exited before its Client became ready.");

            Register("COLOR_RANGE", "Color must be within the range 0 - 16777215 (0xFFFFFF).");
            Register("COLOR_CONVERT", "Unable to convert color to a number.");

            Register("EMBED_FIELD_NAME", "MessageEmbed field names may not be empty.");
            Register("EMBED_FIELD_VALUE", "MessageEmbed field values may not be empty.");

            Register("FILE_NOT_FOUND", (string file) => $"File could not be found: {file}");

            Register("USER_NO_DMCHANNEL", "No DM Channel exists!");

            Register("VOICE_INVALID_HEARTBEAT", "Tried to set voice heartbeat but no valid interval was specified.");
            Register("VOICE_USER_MISSING", "Couldn't resolve the user to create stream.");
            Register("VOICE_JOIN_CHANNEL", (bool full) => $"You do not have permission to join this voice channel{(full ? "; it is full." : ".")}");
            Register("VOICE_CONNECTION_TIMEOUT", "Connection not established within 15 seconds.");
            Register("VOICE_TOKEN_ABSENT", "Token not provided from voice server packet.");
            Register("VOICE_SESSION_ABSENT", "Session ID not supplied.");
            Register("VOICE_INVALID_ENDPOINT", "Invalid endpoint received.");
            Register("VOICE_NO_BROWSER", "Voice connections are not available in browsers.");
            Register("VOICE_CONNECTION_ATTEMPTS_EXCEEDED", (int attempts) => $"Too many connection attempts ({attempts}).");
            Register("VOICE_JOIN_SOCKET_CLOSED", "Tried to send join packet, but the WebSocket is not open.");
            Register("VOICE_PLAY_INTERFACE_NO_BROADCAST", "A broadcast cannot be played in this context.");
            Register("VOICE_PLAY_INTERFACE_BAD_TYPE", "Unknown stream type");
            Register("VOICE_PRISM_DEMUXERS_NEED_STREAM", "To play a webm/ogg stream, you need to pass a ReadableStream.");

            Register("UDP_SEND_FAIL", "Tried to send a UDP packet, but there is no socket available.");
            Register("UDP_ADDRESS_MALFORMED", "Malformed UDP address or port.");
            Register("UDP_CONNECTION_EXISTS", "There is already an existing UDP connection.");

            Register("REQ_RESOURCE_TYPE", "The resource must be a string, Buffer or a valid file stream.");

            Register("IMAGE_FORMAT", (object format) => $"Invalid image format: {format}");
            Register("IMAGE_SIZE", (object size) => $"Invalid image size: {size}");

            Register("MESSAGE_BULK_DELETE_TYPE", "The messages must be an Array, Collection, or number.");
            Register("MESSAGE_NONCE_TYPE", "Message nonce must fit in an unsigned 64-bit integer.");

            Register("TYPING_COUNT", "Count must be at least 1");

            Register("SPLIT_MAX_LEN", "Chunk exceeds the max length and contains no split characters.");

            Register("BAN_RESOLVE_ID", (bool ban) => $"Couldn't resolve the user ID to {(ban ? "ban" : "unban")}.", false);
            Register("FETCH_BAN_RESOLVE_ID", "Couldn't resolve the user ID to fetch the ban.");

            Register("PRUNE_DAYS_TYPE", "Days must be a number");

            Register("GUILD_CHANNEL_RESOLVE", "Could not resolve channel to a guild channel.");
            Register("GUILD_VOICE_CHANNEL_RESOLVE", "Could not resolve channel to a guild voice channel.");
            Register("GUILD_CHANNEL_ORPHAN", "Could not find a parent to this guild channel.");
            Register("GUILD_OWNED", "Guild is owned by the client.");
            Register("GUILD_MEMBERS_TIMEOUT", "Members didn't arrive in time.");
            Register("GUILD_UNCACHED_ME", "The client user as a member of this guild is uncached.");

            Register("INVALID_TYPE", (string name, string expected, bool an) => $"Supplied {name} is not a{(an ? "n" : "")} {expected}.", false);

            Register("WEBHOOK_MESSAGE", "The message was not sent by a webhook.");

            Register("EMOJI_TYPE", "Emoji must be a string or GuildEmoji/ReactionEmoji");
            Register("EMOJI_MANAGED", "Emoji is managed and has no Author.");
            Register("MISSING_MANAGE_EMOJIS_PERMISSION", (Guild guild) => $"Client must have Manage Emoji permission in guild {guild} to see emoji authors.");

            Register("REACTION_RESOLVE_USER", "Couldn't resolve the user ID to remove from the reaction.");

            Register("VANITY_URL", "This guild does not have the VANITY_URL feature enabled.");

            Register("DELETE_GROUP_DM_CHANNEL", "Bots don't have access to Group DM Channels and cannot delete them");
            Register("FETCH_GROUP_DM_CHANNEL", "Bots don't have access to Group DM Channels and cannot fetch them");
        }
    }
}