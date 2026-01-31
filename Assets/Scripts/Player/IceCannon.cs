using UnityEngine;
using TWS.Utils;

public class IceCannon : MonoBehaviour
{
	public Projectile projectilePrefab;

	public float interval = 0.2f;
	public float dispersion = 0.5f; // in degrees

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

	public void Fire()
	{
		if (cooldown > 0) return;
		cooldown = interval;
		
		Projectile projectile = pool.GetInstance<Projectile>();
		projectile.transform.position = transform.position;
		projectile.transform.rotation = transform.rotation * Quaternion.Euler(Random.Range(-dispersion, dispersion), Random.Range(-dispersion, dispersion), 0);
		projectile.Direction = projectile.transform.forward;
		projectile.gameObject.SetActive(true);
	}
}