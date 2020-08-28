using DiscordJS.Data;
using JavaScript;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Represents a user's presence.
    /// </summary>
    public class Presence : IEquatable<Presence>, IHasID
    {
        /// <summary>
        /// The activities of this presence
        /// </summary>
        public Array<Activity> Activities { get; internal set; }

        /// <summary>
        /// The client that instantiated this
        /// </summary>
        public Client Client { get; }

        /// <summary>
        /// The devices this presence is on
        /// </summary>
        public ClientStatus ClientStatus { get; internal set; }

        /// <summary>
        /// The guild of this presence
        /// </summary>
        public Guild Guild { get; internal set; }

        /// <summary>
        /// The member of this presence
        /// </summary>
        public GuildMember Member => Guild == null ? null : Guild.Members.Cache.Get(UserID);

        /// <summary>
        /// The status of this presence:
        /// <list type="table">
        /// <item>
        /// <term>online</term>
        /// <description>user is online</description>
        /// </item>
        /// <item>
        /// <term>idle</term>
        /// <description>user is AFK</description>
        /// </item>
        /// <item>
        /// <term>offline</term>
        /// <description>user is offline or invisible</description>
        /// </item>
        /// <item>
        /// <term>dnd</term>
        /// <description>user is in Do Not Disturb</description>
        /// </item>
        /// </list>
        /// </summary>
        public string Status { get; internal set; }

        /// <summary>
        /// The user of this presence
        /// </summary>
        public User User => Client.Users.Cache.Get(UserID);

        /// <summary>
        /// The user ID of this presence
        /// </summary>
        public Snowflake UserID { get; internal set; }

        /// <summary>
        /// Instantiates a new Presence
        /// </summary>
        /// <param name="client">The instantiating client</param>
        /// <param name="data">The data for the presence</param>
        public Presence(Client client, PresenceData data)
        {
            Client = client;
            UserID = data.user.id;
            Guild = data.guild;
            Patch(data);
        }

        internal Presence Patch(PresenceData data)
        {
            Status = data.status == null ? (Status == null ? "offline" : Status) : data.status;

            if (data.activities != null)
            {
                Activities = new Array<ActivityData>(data.activities).Map((activity) => new Activity(this, activity));
            }
            else if (data.activity != null)
            {
                Activities = new Array<Activity>();
                Activities.Push(new Activity(this, data.activity));
            }
            else
            {
                Activities = new Array<Activity>();
            }

            ClientStatus = new ClientStatus(data.client_status);

            return this;
        }

        internal Presence _Clone()
        {
            Presence clone = MemberwiseClone() as Presence;
            if (Activities != null) clone.Activities = Activities.Map((activity) => activity._Clone());
            return clone;
        }

        /// <summary>
        /// Whether this presence is equal to another.
        /// </summary>
        /// <param name="presence">The presence to compare with</param>
        /// <returns></returns>
        public bool Equals(Presence presence)
        {
            return this == presence ||
                (presence != null &&
                Status == presence.Status &&
                Activities.Length == presence.Activities.Length &&
                Activities.Every((activity, index) => activity.Equals(presence.Activities[index])) &&
                ClientStatus.Web == presence.ClientStatus.Web &&
                ClientStatus.Mobile == presence.ClientStatus.Mobile &&
                ClientStatus.Desktop == presence.ClientStatus.Desktop);
        }

        Snowflake IHasID.ID => UserID;
    }

    /// <summary>
    /// Represents the devices a presence is on
    /// </summary>
    public class ClientStatus
    {
        /// <summary>
        /// The current presence in the web application:
        /// <list type="table">
        /// <item>
        /// <term>online</term>
        /// <description>user is online</description>
        /// </item>
        /// <item>
        /// <term>idle</term>
        /// <description>user is AFK</description>
        /// </item>
        /// <item>
        /// <term>dnd</term>
        /// <description>user is in Do Not Disturb</description>
        /// </item>
        /// </list>
        /// </summary>
        public string Web { get; }

        /// <summary>
        /// The current presence in the mobile application:
        /// <list type="table">
        /// <item>
        /// <term>online</term>
        /// <description>user is online</description>
        /// </item>
        /// <item>
        /// <term>idle</term>
        /// <description>user is AFK</description>
        /// </item>
        /// <item>
        /// <term>dnd</term>
        /// <description>user is in Do Not Disturb</description>
        /// </item>
        /// </list>
        /// </summary>
        public string Mobile { get; }

        /// <summary>
        /// The current presence in the desktop application:
        /// <list type="table">
        /// <item>
        /// <term>online</term>
        /// <description>user is online</description>
        /// </item>
        /// <item>
        /// <term>idle</term>
        /// <description>user is AFK</description>
        /// </item>
        /// <item>
        /// <term>dnd</term>
        /// <description>user is in Do Not Disturb</description>
        /// </item>
        /// </list>
        /// </summary>
        public string Desktop { get; }

        /// <summary>
        /// Instantiates a new Client Status with the given data
        /// </summary>
        /// <param name="data">The data for this status</param>
        public ClientStatus(ClientStatusData data)
        {
            if (data != null)
            {
                Web = data.web;
                Mobile = data.mobile;
                Desktop = data.desktop;
            }
        }
    }
}