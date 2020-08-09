using JavaScript;
using System;

namespace DiscordJS
{
    /// <summary>
    /// A deconstructed snowflake.
    /// </summary>
    public class DeconstructedSnowflake
    {
        /// <summary>
        /// Timestamp the snowflake was created
        /// </summary>
        public long Timestamp { get; }

        /// <summary>
        /// Date the snowflake was created
        /// </summary>
        public Date Date => new Date(Timestamp);

        /// <summary>
        /// Worker ID in the snowflake
        /// </summary>
        public int WorkerID { get; }

        /// <summary>
        /// Process ID in the snowflake
        /// </summary>
        public int ProcessID { get; }

        /// <summary>
        /// Increment in the snowflake
        /// </summary>
        public int Increment { get; }

        /// <summary>
        /// Binary representation of the snowflake
        /// </summary>
        public string Binary { get; }

        internal DeconstructedSnowflake(long timestamp, int workerID, int processID, int increment, string binary)
        {
            Timestamp = timestamp;
            WorkerID = workerID;
            ProcessID = processID;
            Increment = increment;
            Binary = binary;
        }
    }

    /// <summary>
    /// A Twitter snowflake, except the epoch is 2015-01-01T00:00:00.000Z
    /// </summary>
    public class Snowflake : JavaScript.String
    {
        /// <summary>
        /// The Discord epoch (2015-01-01T00:00:00.000Z)
        /// </summary>
        public const long EPOCH = 1420070400000;

        /// <summary>
        /// The increment used for generating snowflakes.
        /// </summary>
        public static long INCREMENT { get; private set; }

        /// <summary>
        /// Generates a Discord snowflake.
        /// <br/>
        /// <br/>
        /// <b>This hardcodes the worker ID as 1 and the process ID as 0.</b>
        /// </summary>
        /// <param name="timestamp">Timestamp or date of the snowflake to generate</param>
        public Snowflake(Date timestamp) : this(timestamp.GetTime())
        { }

        /// <summary>
        /// Generates a Discord snowflake.
        /// <br/>
        /// <br/>
        /// <b>This hardcodes the worker ID as 1 and the process ID as 0.</b>
        /// </summary>
        /// <param name="timestamp">Timestamp or date of the snowflake to generate</param>
        public Snowflake(long timestamp) : base(GetSnowflakeFromTimestamp(timestamp))
        { }

        private Snowflake(string str) : base(str)
        { }

        /// <summary>
        /// Deconstructs a Discord snowflake.
        /// </summary>
        /// <param name="snowflake">Snowflake to deconstruct</param>
        /// <returns>Deconstructed snowflake</returns>
        public static DeconstructedSnowflake Deconstruct(Snowflake snowflake)
        {
            string BINARY = Convert.ToString(long.Parse(snowflake), 2);
            while (BINARY.Length < 64)
                BINARY = "0" + BINARY;

            return new DeconstructedSnowflake
            (
                Convert.ToInt64(BINARY.Substring(0, 42), 2) + EPOCH,
                Convert.ToInt32(BINARY.Substring(42, 5), 2),
                Convert.ToInt32(BINARY.Substring(47, 5), 2),
                Convert.ToInt32(BINARY.Substring(52, 12), 2),
                BINARY
            );
        }

        private static string GetSnowflakeFromTimestamp(long timestamp)
        {
            if (INCREMENT >= 4095) INCREMENT = 0;
            string BINARY = $"{new JavaScript.String(Convert.ToString(timestamp - EPOCH, 2)).PadStart(42, "0")}0000100000{new JavaScript.String(Convert.ToString(INCREMENT++, 2)).PadStart(12, "0")}";
            return Convert.ToInt64(BINARY, 2).ToString();
        }

        public static implicit operator Snowflake(string str) => new Snowflake(str);
        public static implicit operator string(Snowflake snowflake) => snowflake.ToString();
    }
}