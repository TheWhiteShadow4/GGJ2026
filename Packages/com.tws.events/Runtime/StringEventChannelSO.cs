using UnityEngine;
using UnityEngine.Events;

namespace TWS.Events
{
	/// <summary>
	/// This class is used for Events that have one string argument.
	/// Example: A UI element that needs to be updated with a string value.
	/// </summary>
	[CreateAssetMenu(menuName = "Events/String Event Channel")]
	public class StringEventChannelSO : ScriptableObject
	{
		public UnityAction<string> OnEventRaised;

		public void RaiseEvent(string value)
		{
			OnEventRaised?.Invoke(value);
		}
	}
}