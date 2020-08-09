using JavaScript;

namespace DiscordJS
{
    /// <summary>
    /// Represents a Party for an Activity
    /// </summary>
    public class ActivityParty
    {
        public ActivityParty(Snowflake id, int[] size)
        {
            ID = id;
            Size = new int[2] { size[0], size[1] };
        }

        /// <summary>
        /// ID of the party
        /// </summary>
        public Snowflake ID { get; internal set; }

        /// <summary>
        /// The max size this party can have
        /// </summary>
        public int MaxSize => Size[1];

        /// <summary>
        /// The current size of this party
        /// </summary>
        public int CurrentSize => Size[0];

        /// <summary>
        /// Size of the party as [current, max]
        /// </summary>
        public int[] Size { get; internal set; }
    }
}