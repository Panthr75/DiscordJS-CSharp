using JavaScript;
using System;
using NodeJS;
using System.IO;

namespace DiscordJS
{
    /// <summary>
    /// A voice broadcast can be played across multiple voice connections for improved shared-stream efficiency.
    /// </summary>
    public class VoiceBroadcast : EventEmitter, IPlay
    {
        /// <summary>
        /// Emitted whenever a stream dispatcher subscribes to the broadcast.
        /// </summary>
        /// <param name="subscriber">The subscribed dispatcher</param>
        public delegate void SubscribeEvent(StreamDispatcher subscriber);

        /// <summary>
        /// Emitted whenever a stream dispatcher unsubscribes to the broadcast.
        /// </summary>
        /// <param name="dispatcher">The unsubscribed dispatcher</param>
        public delegate void UnsubscribeEvent(StreamDispatcher dispatcher);


        /// <summary>
        /// Emitted whenever a stream dispatcher subscribes to the broadcast.
        /// </summary>
        [Listener(EventName = "subscribe", InvokesOnce = false)]
        public event SubscribeEvent OnSubscribe;

        /// <summary>
        /// Emitted whenever a stream dispatcher subscribes to the broadcast.
        /// </summary>
        [Listener(EventName = "subscribe", InvokesOnce = true)]
        public event SubscribeEvent OnceSubscribe;

        internal bool EmitSubscribe(StreamDispatcher subscriber)
        {
            bool result = OnSubscribe == null && OnceSubscribe == null;
            OnSubscribe?.Invoke(subscriber);
            OnceUnsubscribe?.Invoke(subscriber);
            OnceSubscribe = null;
            return result;
        }


        /// <summary>
        /// Emitted whenever a stream dispatcher unsubscribes to the broadcast.
        /// </summary>
        [Listener(EventName = "unsubscribe", InvokesOnce = false)]
        public event UnsubscribeEvent OnUnsubscribe;

        /// <summary>
        /// Emitted whenever a stream dispatcher unsubscribes to the broadcast.
        /// </summary>
        [Listener(EventName = "unsubscribe", InvokesOnce = true)]
        public event UnsubscribeEvent OnceUnsubscribe;

        internal bool EmitUnsubscribe(StreamDispatcher dispatcher)
        {
            bool result = OnUnsubscribe == null && OnceUnsubscribe == null;
            OnUnsubscribe?.Invoke(dispatcher);
            OnceUnsubscribe?.Invoke(dispatcher);
            OnceUnsubscribe = null;
            return result;
        }


        /// <summary>
        /// The client that created the broadcast
        /// </summary>
        public Client Client { get; }

        /// <summary>
        /// The current master dispatcher, if any. This dispatcher controls all that is played by subscribed dispatchers.
        /// </summary>
        public StreamDispatcher Dispatcher => Player.Dispatcher;

        /// <summary>
        /// The subscribed StreamDispatchers of this broadcast
        /// </summary>
        public Array<StreamDispatcher> Subscribers { get; }

        /// <inheritdoc cref="IPlay.Player"/>
        public BroadcastAudioPlayer Player { get; }

        BasePlayer IPlay.Player => Player;

        public VoiceBroadcast(Client client)
        {
            Client = client;
            Subscribers = new Array<StreamDispatcher>();
            Player = new BroadcastAudioPlayer(this);
        }

        /// <summary>
        /// Ends the broadcast, unsubscribing all subscribed channels and deleting the broadcast
        /// </summary>
        public void End()
        {
            for (int i = Subscribers.Length - 1; i > -1; i--)
                Delete(Subscribers[i]);
            int index = Client.Voice.Broadcasts.IndexOf(this);
            if (index != -1) Client.Voice.Broadcasts.Splice(index, 1);
        }

        /// <inheritdoc cref="IPlay.Play(string, StreamOptions)"/>
        public BroadcastDispatcher Play(string resource, StreamOptions options = null)
        {
            return null;
        }

        StreamDispatcher IPlay.Play(string resource, StreamOptions options) => Play(resource, options);

        /// <inheritdoc cref="IPlay.Play(Stream, StreamOptions)"/>
        public BroadcastDispatcher Play(Stream resource, StreamOptions options = null)
        {
            return null;
        }

        StreamDispatcher IPlay.Play(Stream resource, StreamOptions options) => Play(resource, options);

        StreamDispatcher IPlay.Play(VoiceBroadcast resource, StreamOptions options) => null;

        public void On(string eventName, Action fn) => On(eventName, (Delegate)fn);
        public void On<P1>(string eventName, Action<P1> fn) => On(eventName, (Delegate)fn);
        internal void Emit(string eventName)
        {
            switch (eventName)
            {
                case "subscribe":
                    EmitSubscribe(default);
                    break;
                case "unsubscribe":
                    EmitUnsubscribe(default);
                    break;
            }
        }
        internal void Emit<P1>(string eventName, P1 arg1)
        {
            switch (eventName)
            {
                case "subscribe":
                    EmitSubscribe(arg1 as StreamDispatcher);
                    break;
                case "unsubscribe":
                    EmitUnsubscribe(arg1 as StreamDispatcher);
                    break;
            }
        }

        internal bool Add(StreamDispatcher dispatcher)
        {
            int index = Subscribers.IndexOf(dispatcher);
            if (index == -1)
            {
                Subscribers.Push(dispatcher);
                Emit("subscribe", dispatcher);
                return true;
            }
            return false;
        }

        internal bool Delete(StreamDispatcher dispatcher)
        {
            int index = Subscribers.IndexOf(dispatcher);
            if (index != -1)
            {
                Subscribers.Splice(index, 1);
                Dispatcher.Destroy();
                Emit("unsubscribe", dispatcher);
                return true;
            }
            return false;
        }
    }
}