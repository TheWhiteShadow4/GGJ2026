using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Vector3 offset;


	void LateUpdate()
	{
		transform.position = PlayerController.Current.transform.position + offset;
	}
}