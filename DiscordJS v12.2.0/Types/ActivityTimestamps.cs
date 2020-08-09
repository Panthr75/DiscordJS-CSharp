using JavaScript;

namespace DiscordJS
{
    /// <summary>
    /// Represents Timestamps of an Activity
    /// </summary>
    public class ActivityTimestamps
    {
        public ActivityTimestamps(Date start, Date end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// When the activity started
        /// </summary>
        public Date Start { get; }

        /// <summary>
        /// When the activity will end
        /// </summary>
        public Date End { get;  }
    }
}