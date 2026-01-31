using UnityEngine;
using TWS.Utils;

public class Projectile : MonoBehaviour, IProjectile
{
	[SerializeField] float _velocity;
	[SerializeField] float _lifetime;
	IDamageSource _damageSource;

	/// <inheritdoc cref="IProjectile.DamageSource"/>
	public IDamageSource DamageSource => _damageSource;

    /// <inheritdoc cref="IProjectile.Velocity"/>
    public float Velocity => _velocity;

    public Vector3 Direction { get; set; }

	private float lifetimer;

    private void Awake()
    {
        _damageSource = GetComponent<IDamageSource>();
    }

    private void Update()
    {
        transform.position += Direction * Time.deltaTime * _velocity;
		lifetimer += Time.deltaTime;
		if (lifetimer >= _lifetime)
		{
			DestroySelf();
		}
    }

    private void OnTriggerEnter(Collider other)
    {
        DestroySelf();
    }

	void DestroySelf()
	{
		lifetimer = 0;
		Pool.SafeDestroy(gameObject);
	}
}
