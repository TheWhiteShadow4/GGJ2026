using UnityEngine;
using UnityEngine.Events;

namespace TWS.Events
{
	[CreateAssetMenu(menuName = "Events/Bool Event Channel")]
	public class BoolEventChannelSO : ScriptableObject
	{
		public UnityAction<bool> OnEventRaised;

		public void RaiseEvent(bool value)
		{
			OnEventRaised?.Invoke(value);
		}
	}
}