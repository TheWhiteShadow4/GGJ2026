using UnityEngine;
using UnityEngine.Events;

namespace TWS.Events
{
	/// <summary>
	/// This class is used for Events that have no arguments (Example: Exit game event)
	/// </summary>
	[CreateAssetMenu(menuName = "Events/Object Event Channel")]
	public class ObjectEventChannelSO : ScriptableObject
	{
		public UnityAction<GameObject> OnEventRaised;

		public void RaiseEvent(GameObject value)
		{
			if (OnEventRaised != null)
			{
				OnEventRaised.Invoke(value);
			}
		}
	}
}