using UnityEngine;

public class ShootComponent : MonoBehaviour
{
    [SerializeField] GameObject _projectile;
    [SerializeField] Transform _gun;

    private void Start()
    {
        InvokeRepeating(nameof(Shoot), 1f, 3f);   
    }

    public void Shoot()
    {
        GameObject instance = Instantiate(_projectile, _gun.position, Quaternion.identity);
        var projectile = instance.GetComponent<Projectile>();
        projectile.Direction = _gun.transform.position - transform.position;
    }
}