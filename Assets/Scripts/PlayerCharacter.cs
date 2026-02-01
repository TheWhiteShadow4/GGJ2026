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
    private Transform _modelTransform;
    private Quaternion _targetRotation;

    [SerializeField] float inactiveMaskSize = 1.0f;
    [SerializeField] float activeMaskSize = 1.2f;

    private void Awake()
    {
        _health = GetComponentInChildren<Health>();
        _health.HitEvent.AddListener(GotHit);
        _health.DeathEvent.AddListener(Died);
        _modelTransform = transform.Find("Model");
		OnChangeMask(0);
    }

    private void Update()
    {
        _modelTransform.localRotation = Quaternion.Lerp(_modelTransform.localRotation, _targetRotation, Time.deltaTime * 10.0f);
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
            Stop();
            _playerMasks[currentMaskIndex].SetAnimationState(MaskAnimationState.Off);
            _playerMasks[currentMaskIndex].SetMaskSize(inactiveMaskSize);
        }

        currentMaskIndex = index;
        _playerMasks[currentMaskIndex].SetAnimationState(MaskAnimationState.On);
        _playerMasks[currentMaskIndex].SetMaskSize(activeMaskSize);

        _targetRotation = Quaternion.AngleAxis(_playerMasks[currentMaskIndex].headAngle, _modelTransform.up);
    }

	public void Fire()
	{
		_playerMasks[currentMaskIndex].playerWeapon.Fire();
        _playerMasks[currentMaskIndex].SetAnimationState(MaskAnimationState.Shoot);
	}

	public void Stop()
	{
		_playerMasks[currentMaskIndex].playerWeapon.Stop();
        _playerMasks[currentMaskIndex].SetAnimationState(MaskAnimationState.On);
    }
}
