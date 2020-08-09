using System;
using System.Collections.Generic;
using DiscordJS.Rest;
using JavaScript;

namespace DiscordJS
{
    /// <summary>
    /// The base class for all clients.
    /// </summary>
    public class BaseClient
    {
        /// <summary>
        /// Timeouts set by <seealso cref="SetTimeout(Action, long)"/> that are still active
        /// </summary>
        internal List<Timeout> timeouts;

        /// <summary>
        /// Intervals set by <seealso cref="SetInterval(Action, long)"/> that are still active
        /// </summary>
        internal List<Timeout> intervals;

        /// <summary>
        /// The REST manager of the client
        /// </summary>
        internal RESTManager rest;

        /// <summary>
        /// API shortcut
        /// </summary>
        public APIRouter API => rest.API;

        public BaseClient() : this(new ClientOptions())
        { }

		public BaseClient(ClientOptions options)
		{
			timeouts = new List<Timeout>();
			intervals = new List<Timeout>();
		}

        /// <summary>
        /// Destroys all assets used by the base client.
        /// </summary>
        public void Destroy()
        {
			timeouts.ForEach((timeout) =>
			{
				timeout.Stop();
			});
			intervals.ForEach((interval) =>
			{
				interval.Stop();
			});
        }

        /// <summary>
        /// Sets a timeout that will be automatically cancelled if the client is destroyed.
        /// </summary>
        /// <param name="fn">Function to execute</param>
        /// <param name="delay">Time to wait before executing (in milliseconds)</param>
        /// <returns></returns>
        public int SetTimeout(Action fn, long delay)
        {
            Timeout timeout = null;
            timeout = new Timeout(delay, () =>
            {
                fn();
                timeouts.Remove(timeout);
            }, false, false);
            timeouts.Add(timeout);
            return timeout.Start();
        }

        /// <summary>
        /// Clears a timeout.
        /// </summary>
        /// <param name="id">Timeout to cancel</param>
        public void ClearTimeout(int id)
        {
			Timeout timeout = null;
			for (int index = 0, length = timeouts.Count; index < length; index++)
			{
				var t = timeouts[index];
				if (t.ID == id)
				{
					timeout = t;
                    timeouts.RemoveAt(index);
					break;
				}
			}
			
			if (timeout == null) return;
			
			timeout.Stop();
        }

        /// <summary>
        /// Sets an interval that will be automatically cancelled if the client is destroyed.
        /// </summary>
        /// <param name="fn">Function to execute</param>
        /// <param name="delay">Time to wait between executions (in milliseconds)</param>
        /// <returns></returns>
        public int SetInterval(Action fn, long delay)
        {
            Timeout timeout = null;
            timeout = new Timeout(delay, () =>
            {
                fn();
                timeouts.Remove(timeout);
            }, true, false);
            intervals.Add(timeout);
            return timeout.Start();
        }

        /// <summary>
        /// Clears an interval.
        /// </summary>
        /// <param name="id">Interval to cancel</param>
        public void ClearInterval(int id)
        {
            Timeout interval = null;
            for (int index = 0, length = intervals.Count; index < length; index++)
            {
                var i = intervals[index];
                if (i.ID == id)
                {
                    interval = i;
                    intervals.RemoveAt(index);
                    break;
                }
            }

            if (interval == null) return;

            interval.Stop();
        }
    }
}