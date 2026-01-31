using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] float _hp;
    [SerializeField] float _maxHp;
    [SerializeField] UnityEvent<IDamageSource> _hitEvent;
    [SerializeField] UnityEvent _deathEvent;

    /// <inheritdoc cref="IHealth.MaxHp"/>
    public float MaxHp => _maxHp;

    /// <inheritdoc cref="IHealth.Hp"/>
    public float Hp => _hp;

    public void DoDamage(IDamageSource damage)
    {
        _hp -= damage.Damage;

        _hitEvent?.Invoke(damage);
        if (_hp <= 0)
        {
            _deathEvent?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DamageSource"))
        {
            var damageSource = other.transform.root.GetComponentInChildren<IDamageSource>();
            DoDamage(damageSource);
        }
    }
}