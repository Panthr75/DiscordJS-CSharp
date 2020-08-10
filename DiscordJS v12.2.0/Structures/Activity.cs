using DiscordJS.Data;
using JavaScript;

namespace DiscordJS
{
    /// <summary>
    /// Represents an activity that is part of a user's presence.
    /// </summary>
    public class Activity
    {
        /// <summary>
        /// Application ID associated with this activity
        /// </summary>
        public Snowflake ApplicationID { get; internal set; }

        /// <summary>
        /// Assets for rich presence
        /// </summary>
        public RichPresenceAssets Assets { get; internal set; }

        /// <summary>
        /// The time the activity was created at
        /// </summary>
        public Date CreatedAt => new Date(CreatedTimestamp);

        /// <summary>
        /// Creation date of the activity
        /// </summary>
        public long CreatedTimestamp { get; internal set; }

        /// <summary>
        /// Details about the activity
        /// </summary>
        public string Details { get; internal set; }

        /// <summary>
        /// Emoji for a custom activity
        /// </summary>
        public Emoji Emoji { get; internal set; }

        /// <summary>
        /// Flags that describe the activity
        /// </summary>
        public ActivityFlags Flags { get; internal set; }

        /// <summary>
        /// The name of the activity being played
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Party of the activity
        /// </summary>
        public ActivityParty Party { get; internal set; }

        /// <summary>
        /// State of the activity
        /// </summary>
        public string State { get; internal set; }

        /// <summary>
        /// Timestamps for the activity
        /// </summary>
        public ActivityTimestamps Timestamps { get; internal set; }

        /// <summary>
        /// The type of the activity status
        /// </summary>
        public ActivityType Type { get; internal set; }

        /// <summary>
        /// If the activity is being streamed, a link to the stream
        /// </summary>
        public string URL { get; internal set; }

        /// <inheritdoc/>
        public Presence Presence { get; }

        /// <summary>
        /// Instantiates a new Activity
        /// </summary>
        /// <param name="presence">The presence that instantiated this</param>
        /// <param name="data">The data for the activity</param>
        public Activity(Presence presence, ActivityData data)
        {
            Presence = presence;
            Name = data.name;
            Type = (ActivityType)data.type;
            URL = data.url;
            State = data.state;
            Details = data.details;
            State = data.state;
            if (data.timestamps != null)
            {
                Timestamps = new ActivityTimestamps(data.timestamps.start.HasValue ? new Date(data.timestamps.start.Value) : null,
                    data.timestamps.end.HasValue ? new Date(data.timestamps.end.Value) : null);
            }
            if (data.party != null) Party = new ActivityParty(data.party.id, data.party.size);
            if (data.assets != null) Assets = new RichPresenceAssets(this, data.assets);
            Flags = new ActivityFlags(data.flags);
            Flags.Freeze();
            if (data.emoji != null) Emoji = new Emoji(presence.Client, data.emoji);
            CreatedTimestamp = new Date(data.created_at).GetTime();
        }

        internal Activity _Clone() => MemberwiseClone() as Activity;

        /// <summary>
        /// Whether this activity is equal to another activity.
        /// </summary>
        /// <param name="activity">The activity to compare with</param>
        /// <returns></returns>
        public bool Equals(Activity activity)
        {
            return activity != null && (
                this == activity ||
                Name == activity.Name ||
                Type == activity.Type ||
                URL == activity.URL
                );
        }

        /// <summary>
        /// When concatenated with a string, this automatically returns the activities' name instead of the Activity object.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;
    }
}