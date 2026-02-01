using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
	public GameObject objectToActivate;
	public GameObject objectToDeactivate;

	public void OnSwitchToggled(bool on)
	{
		if (objectToActivate != null)
		{
			objectToActivate.SetActive(on);
		}
		if (objectToDeactivate != null)
		{
			objectToDeactivate.SetActive(!on);
		}
	}
}
