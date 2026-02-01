using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IPlayer
{
	[SerializeField] PlayerMask[] _playerMasks;

    Health _health;

    [SerializeField] float _knockbackResistance = 0;
    [SerializeField] float _knockbackCooldown = 0.5f;
    private float? _lastKnockbackTime = null;

    /// <inheritdoc cref="IPlayer.Health"/>
    public IHealth Health => _health;

    /// <inheritdoc cref="IPlayer.Resistance"/>
    public IDamageResistance Resistance => _playerMasks[currentMaskIndex].maskComponent;

    /// <inheritdoc cref="IKnockbackTarget.KnockbackResistance"/>
    public float KnockbackResistance => _knockbackResistance;

    /// <inheritdoc cref="IKnockbackTarget.KnockbackCooldown"/>
    public float KnockbackCooldown => _knockbackCooldown;

    private int currentMaskIndex = -1;

    private void Awake()
    {
        _health = GetComponentInChildren<Health>();
        _health.HitEvent.AddListener(GotHit);
        _health.DeathEvent.AddListener(Died);
		OnChangeMask(0);
    }

    /// <summary>
    /// Called bia UnityEvent.
    /// </summary>
    public void Died()
    {
        Debug.Log("Player Died");
        // Deatzh animation ?
        gameObject.transform.root.GetComponentInChildren<PlayerController>().enabled = false;
        // Go back to main menu
    }

    /// <summary>
    /// Called bia UnityEvent.
    /// </summary>
    /// <param name="type">ElementType type.</param>
    /// <param name="damage">Damage type.</param>
    public void GotHit(ElementType type, float damage)
    {
        Debug.Log($"{type.GetEfficience(Resistance.Element)}! - Player got {damage} {type} damage. {Health.Hp} HP left.");
    }

    public void ApplyKnockback(Vector3 direction, float strength)
    {
        if (_lastKnockbackTime != null && (Time.time - _lastKnockbackTime) < _knockbackCooldown)
            return;

        Vector3 knockbackForce = direction * strength * (1 - _knockbackResistance);
        gameObject.transform.root.GetComponentInChildren<PlayerController>().ApplyImpactForce(knockbackForce);
        _lastKnockbackTime = Time.time;
    }

	public void OnChangeMask(int index)
	{
        // Stop current mask - needed at least for the initial one
        if (currentMaskIndex >= 0)
		{
			_playerMasks[currentMaskIndex].Deactivate();
		}
            
        currentMaskIndex = index;
		_playerMasks[currentMaskIndex].Activate();
	}

	public void Fire()
	{
		_playerMasks[currentMaskIndex].playerWeapon.Fire();
	}

	public void Stop()
	{
		_playerMasks[currentMaskIndex].playerWeapon.Stop();
	}
}
