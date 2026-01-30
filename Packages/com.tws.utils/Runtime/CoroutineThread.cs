using UnityEngine;
using System;
using System.Threading;

public class CoroutineThread : CustomYieldInstruction
{
	Thread thread;

	public CoroutineThread(Action action)
	{
		thread = new Thread(new ThreadStart(action));
		thread.Start();
	}

	public override bool keepWaiting
	{
		get { return thread.IsAlive; }
	}
}