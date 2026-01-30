using UnityEngine;

public class Player : MonoBehaviour, IPlayer
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
}
