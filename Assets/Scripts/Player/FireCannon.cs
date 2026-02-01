using UnityEngine;
using UnityEngine.VFX;

public class FireCannon : PlayerWeapon
{
	public VisualEffect fireBeam;
	public FireCollider fireCollider;
	public Light fireLight;
	public int fireRate = 96;

	private bool isFiring = false;
	private AudioSource audioSource;

	void Start()
	{
		fireCollider.enabled = false;
		fireLight.enabled = false;
		audioSource = GetComponent<AudioSource>();
	}

	public override void Fire()
	{
		if (isFiring) return;
		isFiring = true;
		fireBeam.SetInt("Rate", fireRate);
		fireCollider.transform.parent = null;
		fireCollider.transform.position = new Vector3(0, 0, 0);
		fireCollider.enabled = true;
		fireLight.enabled = true;
		audioSource.Play();
	}

	public override void Stop()
	{
		if (!isFiring) return;
		isFiring = false;
		fireBeam.SetInt("Rate", 0);
		fireCollider.enabled = false;
		fireLight.enabled = false;
		audioSource.Stop();
	}
}