using UnityEngine;

namespace TWS.Utils
{
	[DefaultExecutionOrder(-200)]
	public class TimerRegistry : MonoBehaviour
	{
		public static TimerRegistry Instance;

		internal float currentTime = 0;

		void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(Instance);
			}
			Instance = this;
		}

		public delegate void OnFire(float verzug);

		internal TimerEntry[] activeTimers = new TimerEntry[1024];
		private int count = 0;

		public static TimerHandle Register(float delay, float interval, OnFire callback)
		{
			TimerRegistry instance = Instance;
			if (instance == null)
			{
				Debug.LogError("TimerRegistry not found");
				return null;
			}
			return instance.RegisterImpl(delay, interval, callback);
		}

		private TimerHandle RegisterImpl(float delay, float interval, OnFire callback)
		{
			if (count >= activeTimers.Length)
			{
				Debug.LogError("Too many timers registered");
				return null;
			}
			var handle = new TimerHandle(count);
			activeTimers[count++] = new TimerEntry
			{
				nextRun = currentTime + delay,
				delay = delay,
				interval = interval,
				callback = callback,
				handle = handle
			};
			return handle;
		}
		
		void Update()
		{
			currentTime += Time.deltaTime;
			for (int i = count - 1; i >= 0; i--)
			{
				ref TimerEntry timer = ref activeTimers[i];
				if (timer.nextRun <= currentTime)
				{
					if (timer.nextRun != 0)
					{
						try
						{
							timer.callback(currentTime - timer.nextRun);
						}
						catch (System.Exception e)
						{
							Debug.LogError("Timer callback failed: " + e.Message);
						}

						if (timer.interval > 0 && timer.nextRun != 0)
						{
							timer.nextRun = currentTime + timer.interval;
							continue;
						}
					}
					activeTimers[i].handle.index = -1;
					if (count > i + 1)
					{
						activeTimers[i] = activeTimers[count - 1];
						activeTimers[i].handle.index = i;
					}
					count--;
				}
			}
		}

		internal struct TimerEntry
		{
			internal float nextRun;
			internal float delay;
			internal float interval;
			internal OnFire callback;
			internal TimerHandle handle;
		}
	}
} 