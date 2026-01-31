using UnityEngine;

public class FireCannon : PlayerWeapon
{
	public GameObject fireBeam;
	public GameObject fireCollider;

	private bool isFiring = false;

	void Start()
	{
		fireBeam.SetActive(false);
		fireCollider.SetActive(false);
	}

	public override void Fire()
	{
		if (isFiring) return;
		isFiring = true;
		fireBeam.SetActive(true);
		fireCollider.transform.parent = null;
		fireCollider.transform.position = new Vector3(0, 0, 0);
		fireCollider.SetActive(true);
	}

	public override void Stop()
	{
		if (!isFiring) return;
		isFiring = false;
		fireBeam.SetActive(false);
		fireCollider.SetActive(false);
	}
}