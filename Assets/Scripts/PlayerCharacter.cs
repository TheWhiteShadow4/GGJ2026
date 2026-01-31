using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IPlayer
{
	[SerializeField] PlayerMask[] _playerMasks;

    Health _health;

    /// <inheritdoc cref="IPlayer.Health"/>
    public IHealth Health => _health;

    /// <inheritdoc cref="IPlayer.Resistance"/>
    public IDamageResistance Resistance => _playerMasks[currentMaskIndex].maskComponent;

	private int currentMaskIndex = -1;

    private void Awake()
    {
        _health = GetComponentInChildren<Health>();
        _health.HitEvent.AddListener(GotHit);
        _health.DeathEvent.AddListener(Died);
		foreach (var mask in _playerMasks)
		{
			mask.gameObject.SetActive(false);
		}
		OnChangeMask(0);
    }

    /// <summary>
    /// Called bia UnityEvent.
    /// </summary>
    public void Died()
    {
        Debug.Log("Player Died");
        GameObject.Destroy(gameObject);
    }

    /// <summary>
    /// Called bia UnityEvent.
    /// </summary>
    /// <param name="type">ElementType type.</param>
    /// <param name="damage">Damage type.</param>
    public void GotHit(ElementType type, float damage)
    {
        Debug.Log($"Got {damage} {type} damage. {Health.Hp} HP left.");
    }

	public void OnChangeMask(int index)
	{
		if (currentMaskIndex != -1)
		{
			_playerMasks[currentMaskIndex].gameObject.SetActive(false);
		}
		currentMaskIndex = index;
		_playerMasks[index].gameObject.SetActive(true);
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
