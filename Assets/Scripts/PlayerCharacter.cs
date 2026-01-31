using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IPlayer
{
    /// <inheritdoc cref="IPlayer.Health"/>
    public IHealth Health { get; private set; }

    /// <inheritdoc cref="IPlayer.Resistance"/>
    public IDamageResistance Resistance { get; private set; }

    private void Awake()
    {
        Health = GetComponentInChildren<IHealth>();
        Resistance = GetComponentInChildren<IDamageResistance>();
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
}
