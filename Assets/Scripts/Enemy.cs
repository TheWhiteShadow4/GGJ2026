using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField] ElementType _element;

    /// <inheritdoc cref="IPlayer.Health"/>
    public IHealth Health { get; private set; }

    /// <inheritdoc cref="IPlayer.DamageSource"/>
    public IDamageSource DamageSource { get; private set; }

    /// <inheritdoc cref="IPlayer.Element"/>
    public ElementType Element => _element;

    private void Awake()
    {
        Health = GetComponentInChildren<IHealth>();
        DamageSource = GetComponentInChildren<IDamageSource>();
    }
}

public enum ElementType
{
    None = 0,
    Fire,
    Water,
    Air,
    Earth
}
