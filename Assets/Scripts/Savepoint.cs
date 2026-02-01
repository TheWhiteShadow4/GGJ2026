using UnityEngine;

public class Savepoint : MonoBehaviour
{
	public bool activated = false;
	public GameObject activatedEffect;
	public AudioClip saveSound;

	public void OnTriggerEnter(Collider other)
	{
		if (activated) return;
		if (other.gameObject.layer == Layers.Player)
		{
			GameManager.Instance.SaveGame();
			activated = true;
			if (activatedEffect != null)
			{
				activatedEffect.SetActive(true);
			}
			GameManager.Instance.PlaySound(saveSound);
		}
	}
}