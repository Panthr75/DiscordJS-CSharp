using JavaScript;
using System;

namespace DiscordJS
{
    /// <summary>
    /// Abstract class for defining a new Collector.
    /// </summary>
    /// <typeparam name="T">The type this collector collects</typeparam>
    /// <typeparam name="O">The options for this collector</typeparam>
    public abstract class Collector<T, O> where T : class, IHasID where O : CollectorOptions
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
        /// The filter applied to this collector
        /// </summary>
        public Func<T, Collection<Snowflake, T>, bool> Filter { get; internal set; }

        /// <summary>
        /// Returns a promise that resolves with the next collected element;<br/>
        /// rejects with collected elements if the collector finishes without receiving a next element
        /// </summary>
        public IPromise<T> Next
        {
            get
            {
                return new Promise<T>((resolve, reject) =>
                {
                    CollectEvent onCollect = null;
                    EndEvent onEnd = null;

                    void Cleanup()
                    {
                        OnCollect -= onCollect;
                        OnEnd -= onEnd;
                    }

                    onCollect = (item) =>
                    {
                        Cleanup();
                        resolve(item);
                    };

                    onEnd = (collected, reason) =>
                    {
                        Cleanup();
                        reject(new CollectorCollectionException<T>(collected, reason));
                    };

                    OnCollect += onCollect;
                    OnEnd += onEnd;
                });
            }
        }

        /// <summary>
        /// The options of this collector
        /// </summary>
        public O Options { get; internal set; }


        /// <summary>
        /// Emitted whenever an element is collected.
        /// </summary>
        /// <param name="arg1">The first arg the listener emits</param>
        public delegate void CollectEvent(T arg1);

        /// <summary>
        /// Emitted whenever an element is disposed of.
        /// </summary>
        /// <param name="arg1">The first arg the listener emits</param>
        public delegate void DisposeEvent(T arg1);

        /// <summary>
        /// Emitted when the collector is finished collecting.
        /// </summary>
        /// <param name="collected">The elements collected by the collector</param>
        /// <param name="reason">The reason the collector ended</param>
        public delegate void EndEvent(Collection<Snowflake, T> collected, string reason);


        /// <summary>
        /// Emitted whenever an element is collected.
        /// </summary>
        public event CollectEvent OnCollect;

        /// <summary>
        /// Emitted whenever an element is collected.
        /// </summary>
        public event CollectEvent OnceCollect;

        /// <summary>
        /// Emitted whenever an element is disposed of.
        /// </summary>
        public event DisposeEvent OnDispose;

        /// <summary>
        /// Emitted whenever an element is disposed of.
        /// </summary>
        public event DisposeEvent OnceDispose;

        /// <summary>
        /// Emitted when the collector is finished collecting.
        /// </summary>
        public event EndEvent OnEnd;

        /// <summary>
        /// Emitted when the collector is finished collecting.
        /// </summary>
        public event EndEvent OnceEnd;

        /// <summary>
        /// Timeout for cleanup
        /// </summary>
        internal int? _timeout;

        /// <summary>
        /// Timeout for cleanup due to inactivity
        /// </summary>
        internal int? _idletimeout;

        /// <summary>
        /// Instantiates the collector
        /// </summary>
        /// <param name="client">The instantiating Client</param>
        /// <param name="options">Options for this collector</param>
        public Collector(Client client, CollectorOptions options)
        {
            if (options.Time.HasValue) _timeout = Client.SetTimeout(() => Stop("time"), options.Time.Value);
            if (options.Idle.HasValue) _idletimeout = Client.SetTimeout(() => Stop("idle"), options.Idle.Value);
        }

        /// <summary>
        /// Checks whether the collector should end, and if so, ends it.
        /// </summary>
        public void CheckEnd()
        {
            var reason = EndReason();
            if (reason != null) Stop(reason);
        }

        /// <summary>
        /// Handles incoming events from the HandleCollect function. Returns null if the event should not be collected, or returns an object describing the data that should be stored.
        /// </summary>
        /// <param name="arg1">The first arg the listener emits</param>
        /// <returns>Data to insert into collection, if any</returns>
        public abstract T Collect(T arg1);

        /// <summary>
        /// Handles incoming events from the HandleDispose. Returns null if the event should not be disposed, or returns the key that should be removed.
        /// </summary>
        /// <param name="arg1">The first arg the listener emits</param>
        /// <returns>Key to remove from the collection, if any</returns>
        public abstract Snowflake Dispose(T arg1);

        /// <summary>
        /// The reason this collector has ended or will end with.
        /// </summary>
        /// <returns>Reason to end the collector, if any</returns>
        public abstract string EndReason();

        /// <summary>
        /// Call this to handle an event as a collectable element.
        /// </summary>
        /// <param name="arg1">The first arg the listener emits</param>
        public void HandleCollect(T arg1)
        {
            T collect = Collect(arg1);
            if (collect != null && (Filter == null || Filter(arg1, Collected)))
            {
                Collected.Set(collect.ID, arg1);

                OnCollect?.Invoke(arg1);
                OnceCollect?.Invoke(arg1);

                if (_idletimeout.HasValue)
                {
                    Client.ClearTimeout(_idletimeout.Value);
                    _idletimeout = Client.SetTimeout(() => Stop("idle"), Options.Idle.Value);
                }
            }
            CheckEnd();
        }

        /// <summary>
        /// Call this to remove an element from the collection. Accepts any event data as parameters.
        /// </summary>
        /// <param name="arg1">The first arg the listener emits</param>
        public void HandleDispose(T arg1)
        {
            if (!Options.Dispose) return;

            Snowflake dispose = Dispose(arg1);
            if (dispose == null || (Filter != null && !Filter(arg1, Collected)) || !Collected.Has(dispose)) return;
            Collected.Delete(dispose);

            OnDispose?.Invoke(arg1);
            OnceDispose?.Invoke(arg1);
            OnceDispose = null;
            CheckEnd();
        }

        /// <summary>
        /// Resets the collectors timeout and idle timer.
        /// </summary>
        /// <param name="time">How long to run the collector for in milliseconds</param>
        /// <param name="idle">How long to stop the collector after inactivity in milliseconds</param>
        public void ResetTimer(long? time, long? idle)
        {
            if (_timeout.HasValue)
            {
                Client.ClearTimeout(_timeout.Value);
                _timeout = Client.SetTimeout(() => Stop("time"), time.HasValue ? time.Value : Options.Time.Value);
            }
            if (_idletimeout.HasValue)
            {
                Client.ClearTimeout(_idletimeout.Value);
                _idletimeout = Client.SetTimeout(() => Stop("idle"), idle.HasValue ? idle.Value : Options.Idle.Value);
            }
        }

        /// <summary>
        /// Stops this collector and emits the end event.
        /// </summary>
        /// <param name="reason">The reason this collector is ending</param>
        public void Stop(string reason = "user")
        {
            if (Ended) return;

            if (_timeout.HasValue)
            {
                Client.ClearTimeout(_timeout.Value);
                _timeout = null;
            }

            if (_idletimeout.HasValue)
            {
                Client.ClearTimeout(_idletimeout.Value);
                _idletimeout = null;
            }

            Ended = true;

            OnEnd?.Invoke(Collected, reason);
            OnceEnd?.Invoke(Collected, reason);

            OnceEnd = null;
        }
    }

    /// <summary>
    /// Options to be applied to the collector.
    /// </summary>
    public class CollectorOptions
    {
        /// <summary>
        /// How long to run the collector for in milliseconds
        /// </summary>
        public long? Time { get; set; } = null;

        /// <summary>
        /// How long to stop the collector after inactivity in milliseconds
        /// </summary>
        public long? Idle { get; set; } = null;

        /// <summary>
        /// Whether to dispose data when it's deleted
        /// </summary>
        public bool Dispose { get; set; } = false;
    }

    public class CollectorCollectionException<T> : Exception where T : IHasID
    {
        public CollectorCollectionException(Collection<Snowflake, T> collection, string reason)
        {

        }
    }
}