using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] float _hp;
    [SerializeField] float _maxHp;
    [SerializeField] UnityEvent<ElementType, float> _hitEvent;
    [SerializeField] UnityEvent _deathEvent;
    ICharacter _character;

    [SerializeField] bool _showDamageText;
    [SerializeField] GameObject _damageText;

    [SerializeField] LayerMask _incomingDamageLayers;

    /// <inheritdoc cref="IHealth.MaxHp"/>
    public float MaxHp => _maxHp;

    /// <inheritdoc cref="IHealth.Hp"/>
    public float Hp => _hp;

    public UnityEvent<ElementType, float> HitEvent => _hitEvent;

    public UnityEvent DeathEvent => _deathEvent;

    private void Awake()
    {
        _character = transform.root.GetComponentInChildren<ICharacter>();
    }

    public void DoDamage(IDamageSource damage)
    {
        if (!enabled) return;

        var dmg = damage.Damage / _character.Resistance.GetMultiplier(damage.Type);
        _hp -= dmg;

        _hitEvent?.Invoke(damage.Type, dmg);
        if (_hp <= 0)
        {
            _deathEvent?.Invoke();
        }
        else
        {
            if (_showDamageText)
            {
                var go = GameObject.Instantiate(_damageText);
                go.transform.position = transform.position;
                go.GetComponent<DamageText>().OnHit(damage.Type, dmg);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((_incomingDamageLayers & (1 << other.gameObject.layer)) != 0)
        {
			if (other.TryGetComponent<IDamageSource>(out var damageSource))
			{
				DoDamage(damageSource);

                if (damageSource.AppliesKnockback)
                {
                    Vector3 knockbackDirection = transform.root.transform.position - other.gameObject.transform.position;
                    knockbackDirection = new Vector3(knockbackDirection.x, 0, knockbackDirection.z);
                    _character.ApplyKnockback(knockbackDirection, damageSource.KnockbackStrength);
                }
			}
        }
    }
}