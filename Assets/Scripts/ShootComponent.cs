using UnityEngine;
using TWS.Utils;

public class ShootComponent : MonoBehaviour
{
    [SerializeField] GameObject _projectile;
    [SerializeField] Transform _aim;
    Pool _pool;

    private void Start()
    {
        InvokeRepeating(nameof(Shoot), 1f, 3f);

        _pool = Pool.GetSharedPool(_projectile);
    }

    public void Shoot()
    {
        var instance = _pool.GetInstance<Projectile>();
        instance.transform.position = _aim.position;
        instance.transform.rotation = Quaternion.identity;
        var projectile = instance.GetComponent<Projectile>();
        projectile.Direction = _aim.transform.position - transform.position;
    }
}