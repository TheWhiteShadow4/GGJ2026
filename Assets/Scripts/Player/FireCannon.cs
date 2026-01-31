using UnityEngine;
using UnityEngine.VFX;

public class FireCannon : PlayerWeapon
{
	public VisualEffect fireBeam;
	public FireCollider fireCollider;
	public int fireRate = 96;

	private bool isFiring = false;

	void Start()
	{
		fireCollider.enabled = false;
	}

	public override void Fire()
	{
		if (isFiring) return;
		isFiring = true;
		fireBeam.SetInt("Rate", fireRate);
		fireCollider.transform.parent = null;
		fireCollider.transform.position = new Vector3(0, 0, 0);
		fireCollider.enabled = true;
	}

	public override void Stop()
	{
		if (!isFiring) return;
		isFiring = false;
		fireBeam.SetInt("Rate", 0);
		fireCollider.enabled = false;
	}
}