using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IPlayer
{
    /// <inheritdoc cref="IPlayer.Health"/>
    public IHealth Health { get; private set; }

    /// <inheritdoc cref="IPlayer.DamageSource"/>
    public IDamageSource DamageSource { get; private set; }

    private void Awake()
    {
        Health = GetComponentInChildren<IHealth>();
        DamageSource = GetComponentInChildren<IDamageSource>();
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
    /// <param name="damage">Damage source.</param>
    public void GotHit(IDamageSource damage)
    {
        Debug.Log($"Got {damage} damage. {Health.Hp} HP left.");
    }
}
