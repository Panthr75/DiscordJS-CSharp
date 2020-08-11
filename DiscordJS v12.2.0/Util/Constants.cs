using System.Collections.Generic;

namespace DiscordJS
{
    /// <summary>
    /// The class containing a list of constants for things that need to be instaniated, like WSCodes for the indexer
    /// </summary>
    public static class Constants
    {
        public static ConstantsWSCodes WSCodes { get; } = new ConstantsWSCodes();
    }

    /// <summary>
    /// A class containing a list of WS Codes
    /// </summary>
    public class ConstantsWSCodes
    {
        private readonly Dictionary<int, string> codes;

        public ConstantsWSCodes()
        {
            codes = new Dictionary<int, string>()
            {
                [1000] = "WS_CLOSE_REQUESTED",
                [4004] = "TOKEN_INVALID",
                [4010] = "SHARDING_INVALID",
                [4011] = "SHARDING_REQUIRED",
                [4013] = "INVALID_INTENTS",
                [4014] = "DISALLOWED_INTENTS"
            };
        }

        /// <summary>
        /// Returns whether the given code is not resumable
        /// </summary>
        /// <param name="code">The code to check</param>
        /// <returns></returns>
        public bool IsUnresumable(int code) => code == 1000 || code == 4006 || code == 4007;

        /// <summary>
        /// Returns whether the given code is an unrecoverable code
        /// </summary>
        /// <param name="code">The code to check</param>
        /// <returns></returns>
        public bool IsUnrecoverable(int code) => codes.ContainsKey(code) && code != 1000;

        /// <summary>
        /// Gets a specific error id for the given code. If not found, returns <see langword="null"/>.
        /// </summary>
        /// <param name="code">The code to look for</param>
        /// <returns></returns>
        public string this[int code]
        {
            get
            {
                if (codes.TryGetValue(code, out string result))
                    return result;
                else
                    return null;
            }
        }
    }
}