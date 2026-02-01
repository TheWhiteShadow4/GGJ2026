using UnityEngine;
using TWS.Utils;

public class IceCannon : PlayerWeapon
{
	public Projectile projectilePrefab;

	public float interval = 0.2f;
	public float dispersion = 5f; // Abweichung eines Projektils
	public float spread = 60; // Winkelverteilung aller Projektile
	public int projectileCount = 12;
	public AudioClip fireSound;

	private Pool pool;

	private float cooldown;

	void Start()
	{
		pool = Pool.GetSharedPool(projectilePrefab.gameObject);
	}

	void Update()
	{
		if (cooldown > 0) cooldown -= Time.deltaTime;
	}

	public override void Fire()
	{
		if (cooldown > 0) return;
		cooldown = interval;
		
		GameManager.Instance.PlaySound(fireSound);
		
		for (int i = 0; i < projectileCount; i++)
		{
			float angle = Mathf.Lerp(-spread / 2, spread / 2, (float)i / projectileCount);
			angle += Random.Range(-dispersion, dispersion);
			Quaternion rotation = transform.rotation * Quaternion.Euler(0, angle, 0);
			Projectile projectile = pool.GetInstance<Projectile>();
			projectile.transform.position = transform.position;
			projectile.transform.rotation = rotation;
			projectile.Direction = projectile.transform.forward;
			projectile.gameObject.SetActive(true);
		}
	}
}