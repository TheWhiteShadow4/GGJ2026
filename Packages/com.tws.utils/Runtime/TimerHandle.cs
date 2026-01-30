namespace TWS.Utils
{
	public class TimerHandle
	{
		internal int index;
		
		public TimerHandle(int index)
		{
			this.index = index;
		}

		public float Interval
		{
			get => GetTimer().interval;
			set => GetTimer().interval = value;
		}

		public float Delay
		{
			get => GetTimer().delay;
			set => GetTimer().delay = value;
		}

		public void Reset()
		{
			ref TimerRegistry.TimerEntry t = ref GetTimer();
			t.nextRun = TimerRegistry.Instance.currentTime + t.delay;
		}

		public void Cancel()
		{
			if (!IsValid()) return;
			GetTimer().nextRun = 0;
		}

		public bool IsValid()
		{
			return index != -1;
		}

		private ref TimerRegistry.TimerEntry GetTimer()
		{
			if (index == -1)
			{
				throw new System.Exception("Handle is not valid");
			}
			return ref TimerRegistry.Instance.activeTimers[index];
		}
	}
} 