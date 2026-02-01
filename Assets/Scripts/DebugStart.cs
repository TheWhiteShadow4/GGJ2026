using UnityEngine;

public class DebugStart : MonoBehaviour
{
	void Start()
	{
		#if UNITY_EDITOR
			PlayerController.Current.transform.position = transform.position;
		#endif
	}
}