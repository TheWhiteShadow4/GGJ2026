using UnityEngine;

public class DestructionExplode : MonoBehaviour
{
	public IceCannon cannon;

	void OnDisable()
	{
		cannon.Fire();
	}
}