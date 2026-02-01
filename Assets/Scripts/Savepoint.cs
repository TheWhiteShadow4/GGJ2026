using UnityEngine;

public class Savepoint : MonoBehaviour
{
	public bool activated = false;
	public GameObject activatedEffect;

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			GameManager.Instance.SaveGame();
			activated = true;
			if (activatedEffect != null)
			{
				activatedEffect.SetActive(true);
			}
		}
	}
}