using UnityEngine;
using System.Collections;

namespace TWS.Events
{
	/// <summary>
	/// Eine abstrakte Klasse für Event-Verhalten z.B. Items oder Schalter.
	/// </summary>
	public abstract class EventBehaviour : MonoBehaviour
	{
		public virtual bool CanTriggered
		{
			get { return true; }
		}

		public abstract void TriggerEvent(MonoBehaviour caller);
	}
}