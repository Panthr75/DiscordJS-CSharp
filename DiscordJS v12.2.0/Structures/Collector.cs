using System;

namespace DiscordJS
{
    public abstract class Collector<T> where T : IHasID
    {
        /// <summary>
        /// The client that instantiated this Collector
        /// </summary>
        public Client Client { get; }

        /// <summary>
        /// The items collected by this collector
        /// </summary>
        public Collection<Snowflake, T> Collected { get; internal set; }

        /// <summary>
        /// Whether this collector has finished collecting
        /// </summary>
        public bool Ended { get; internal set; }

        /// <summary>
        /// Emitted when the collector is finished collecting.
        /// </summary>
        /// <param name="collected">The elements collected by the collector</param>
        /// <param name="reason">The reason the collector ended</param>
        public delegate void EndEvent(Collection<Snowflake, T> collected, string reason);

        public EndEvent OnEnd;
        public EndEvent OnceEnd;

        public Collector(Client client, CollectorOptions options)
        {
            //
        }
    }

    public class CollectorOptions
    {
        public long time;
        public long idle;
        public bool dispose = false;
    }

    public class CollectorCollectionException<T> : Exception where T : IHasID
    {
        public CollectorCollectionException(Collection<Snowflake, T> collection, string reason)
        {

        }
    }
}