using UnityEngine;

public class ShootComponent : MonoBehaviour
{
    [SerializeField] GameObject _projectile;
    [SerializeField] Transform _aim;
    [SerializeField] float _shootingSpeed = 3f;
    Pool _pool;

    private void Start()
    {
        InvokeRepeating(nameof(Shoot), 1f, _shootingSpeed);

        _pool = Pool.GetSharedPool(_projectile);
    }

    public void Shoot()
    {
        var instance = _pool.GetInstance<Projectile>();
        instance.transform.position = _aim.position;
        instance.transform.rotation = Quaternion.identity;
        instance.Direction = _aim.transform.position - transform.position;
    }
}