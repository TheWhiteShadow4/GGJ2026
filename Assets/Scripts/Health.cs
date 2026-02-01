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

    /// <inheritdoc cref="IHealth.Hp"/>
    public float Hp => _hp;

    public float MaxHp => _maxHp;

    public UnityEvent<ElementType, float> HitEvent => _hitEvent;

    public UnityEvent DeathEvent => _deathEvent;

    private void Awake()
    {
        _character = transform.GetComponent<ICharacter>();
    }

    public void DoDamage(IDamageSource damage)
    {
        if (!enabled) return;

        // randomize the damage with +/- 10%
        var randomDmg = Random.Range(-1f, 1f) * (damage.Damage * 0.1f) + damage.Damage;

        var multiplier = 1 / _character.Resistance.GetMultiplier(damage.Type);
        var dmg = randomDmg * multiplier;
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
                    Vector3 knockbackDirection = transform.position - other.gameObject.transform.position;
                    knockbackDirection = new Vector3(knockbackDirection.x, 0, knockbackDirection.z);
                    _character.ApplyKnockback(knockbackDirection, damageSource.KnockbackStrength);
                }
			}
        }
    }
}
