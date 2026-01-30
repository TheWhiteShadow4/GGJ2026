using UnityEngine;
using TWS.Events;

public class PlayerController : MonoBehaviour
{
	public static PlayerController Current;

	void Awake()
	{
		if (Current != null)
		{
			Destroy(Current.gameObject);
		}
		Current = this;

		if (GameManager.Instance.wasLoaded)
		{
			GameManager.Instance.wasLoaded = false;
			GameProgress.Current.RestorePlayerPosition(transform);
		}
	}
}