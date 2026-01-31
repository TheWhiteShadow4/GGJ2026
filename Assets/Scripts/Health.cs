using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] float _hp;
    [SerializeField] float _maxHp;
    [SerializeField] UnityEvent<ElementType, float> _hitEvent;
    [SerializeField] UnityEvent _deathEvent;
    IDamageResistance _resistance;

    [SerializeField] LayerMask _incomingDamageLayers;

    /// <inheritdoc cref="IHealth.MaxHp"/>
    public float MaxHp => _maxHp;

    /// <inheritdoc cref="IHealth.Hp"/>
    public float Hp => _hp;

    public UnityEvent<ElementType, float> HitEvent => _hitEvent;

    public UnityEvent DeathEvent => _deathEvent;

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
        if ((_incomingDamageLayers & (1 << other.gameObject.layer)) != 0)
        {
			if (other.TryGetComponent<IDamageSource>(out var damageSource))
			{
				DoDamage(damageSource);
			}
        }
    }
}