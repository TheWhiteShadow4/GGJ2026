using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] float _hp;
    [SerializeField] float _maxHp;
    [SerializeField] UnityEvent<ElementType, float> _hitEvent;
    [SerializeField] UnityEvent _deathEvent;
    IDamageResistance _resistance;

    /// <inheritdoc cref="IHealth.MaxHp"/>
    public float MaxHp => _maxHp;

    /// <inheritdoc cref="IHealth.Hp"/>
    public float Hp => _hp;

    private void Awake()
    {
        _resistance = transform.root.GetComponentInChildren<IDamageResistance>();
    }

    public void DoDamage(IDamageSource damage)
    {
		if (!enabled) return;
        var dmg = damage.Damage * _resistance.GetMultiplier(damage.Type);
        _hp -= dmg;

        _hitEvent?.Invoke(damage.Type, dmg);
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