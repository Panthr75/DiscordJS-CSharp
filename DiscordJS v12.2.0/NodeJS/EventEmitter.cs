using System;
using System.Collections.Generic;
using System.Reflection;

namespace NodeJS
{
    public class EventEmitter
    {
        /// <summary>
        /// The attribute added to events in an event emitter
        /// to specify a specific on event
        /// </summary>
        [AttributeUsage(AttributeTargets.Event, AllowMultiple = false, Inherited = true)]
        public class ListenerAttribute : Attribute
        {
            /// <summary>
            /// The name of the event
            /// </summary>
            public string EventName { get; set; }

            /// <summary>
            /// Whether or not this is a once event, meaning once it's invoked,
            /// all it's listeners are removed
            /// </summary>
            public bool InvokesOnce { get; set; }
        }

        private void __AddListener(bool listenOnce, string eventName, Delegate listener)
        {
            if (listener == null) return;

            Type type = GetType();
            Type listenerAttribute = typeof(ListenerAttribute);
            Type paramsType = typeof(ParamArrayAttribute);

            EventInfo[] events = type.GetEvents();
            EventInfo ev = null;

            for (int index = 0, length = events.Length; index < length; index++)
            {
                EventInfo e = events[index];
                if (Attribute.IsDefined(e, listenerAttribute))
                {
                    ListenerAttribute attr = Attribute.GetCustomAttribute(e, listenerAttribute) as ListenerAttribute;
                    if (attr.EventName == eventName && attr.InvokesOnce == listenOnce)
                    {
                        ev = e;
                        break;
                    }
                }
            }

            if (ev == null) return;

            Type handlerType = ev.EventHandlerType;
            Type delType = listener.GetType();

            MethodInfo evInvokeMethod = handlerType.GetMethod("Invoke");
            MethodInfo delInvokeMethod = delType.GetMethod("Invoke");

            ParameterInfo[] evParams = evInvokeMethod.GetParameters();
            ParameterInfo[] delParams = delInvokeMethod.GetParameters();

            if (evParams.Length != delParams.Length) 
                throw new ArgumentException("Listener's arguments doesn't match the same arguments as the event", "listener");

            for (int index = 0, length = evParams.Length; index < length; index++)
            {
                ParameterInfo evParam = evParams[index],
                    delParam = delParams[index];

                if (!(evParam.ParameterType.Equals(delParam.ParameterType) &&
                    evParam.IsOptional == delParam.IsOptional &&
                    evParam.IsOut == delParam.IsOut &&
                    evParam.IsIn == delParam.IsIn &&
                    Attribute.IsDefined(evParam, paramsType) == Attribute.IsDefined(delParam, paramsType)))
                {
                    throw new ArgumentException("Listener's arguments doesn't match the same arguments as the event", "listener");
                }
            }

            ev.AddEventHandler(this, listener);
        }

        public void On(string eventName, Delegate listener) => __AddListener(false, eventName, listener);
        public void Once(string eventName, Delegate listener) => __AddListener(true, eventName, listener);
    }
}