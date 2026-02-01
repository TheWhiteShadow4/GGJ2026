using UnityEngine;

public class PlayerMask : MonoBehaviour
{
	public MaskComponent maskComponent;
	public PlayerWeapon playerWeapon;
	public GameObject activatedEffect;
	public float headAngle;

	void Awake()
	{
		if (activatedEffect != null)
		{
			activatedEffect.SetActive(false);
		}
	}

	public void Activate()
	{
		if (activatedEffect != null)
		{
			activatedEffect.SetActive(true);
		}
	}

	public void Deactivate()
	{
		if (activatedEffect != null)
		{
			activatedEffect.SetActive(false);
		}
		playerWeapon.Stop();
	}
}