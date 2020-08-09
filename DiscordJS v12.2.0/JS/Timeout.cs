using System;
using System.Timers;
using System.Collections.Generic;
using System.Threading;

namespace JavaScript
{
	/// <summary>
	/// Represents a Timeout
	/// </summary>
    public class Timeout
    {
		private static Dictionary<int, Timeout> timeouts = new Dictionary<int, Timeout>();
		private static List<int> timeoutsForKeepAlive = new List<int>();
		private static Thread keepAliveThread;

		public static void Unref(int id)
        {
			if (timeouts.TryGetValue(id, out Timeout timeout))
			{
				if (timeout.keepAlive)
				{
					int index = timeoutsForKeepAlive.IndexOf(id);
					timeout.keepAlive = false;
					if (index > -1)
					{
						timeoutsForKeepAlive.RemoveAt(index);
						if (timeoutsForKeepAlive.Count == 0 && keepAliveThread != null)
						{
							keepAliveThread.Abort();
						}
					}
				}
			}
		}

		public static void Ref(int id)
		{
			if (timeouts.TryGetValue(id, out Timeout timeout))
			{
				if (!timeout.keepAlive)
				{
					timeout.keepAlive = true;
					if (timeoutsForKeepAlive.Count == 0)
						keepAliveThread = new Thread(() => Console.ReadKey(true));
					timeoutsForKeepAlive.Add(timeout.ID);
				}
			}
		}

		private static int AddTimeout(Timeout timeout)
		{
			int id = timeouts.Count;
            while (!timeouts.ContainsKey(id))
                id++;
            timeouts[id] = timeout;
            return id;
		}
		
		public static int Refresh(int id)
        {
			if (timeouts.TryGetValue(id, out Timeout timeout))
            {
				timeout.timer.Stop();
				timeout.timer.Start();
            }
			return id;
        }

		private System.Timers.Timer timer;
		private readonly bool repeat;
		private readonly Action onTick;
		private bool keepAlive = false;
		public int ID { get; }
		
		private void OnTimerTick(object sender, ElapsedEventArgs e)
		{
			onTick.Invoke();
			if (!repeat)
				Stop();
		}
		
		internal void Stop()
		{
			if (timer != null)
			{
				timer.Stop();
				timer.Dispose();
				timer = null;
				if (keepAlive) Unref(ID);
				
				timeouts.Remove(ID);
			}
		}
		
		internal int Start()
		{
			timer.Start();
			return ID;
		}
		
		internal Timeout(long ms, Action callback, bool repeat, bool autoStart = true)
		{
			this.repeat = repeat;
			onTick = callback;
			
			timer = new System.Timers.Timer()
			{
				Interval = ms
			};
			
			ID = AddTimeout(this);
			
			timer.Elapsed += OnTimerTick;
			if (autoStart) Start();
		}
    }
}