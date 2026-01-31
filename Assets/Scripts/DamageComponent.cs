using UnityEngine;

public class DamageComponent : MonoBehaviour, IDamageSource
{
	[SerializeField] ElementType _type;
    [SerializeField] float _damage;
    [SerializeField] bool _appliesKnockback;
    [SerializeField] float _knockbackStrength;

	/// <inheritdoc cref="IDamageSource.Type"/>
    public ElementType Type => _type;

    /// <inheritdoc cref="IDamageSource.Damage"/>
    public float Damage => _damage;

    /// <inheritdoc cref="IDamageSource.ToString"/>
    public override string ToString() => $"{Damage} {Type} Damage";

    /// <inheritdoc cref="IDamageSource.AppliesKnockback"/>
    public bool AppliesKnockback => _appliesKnockback;

    /// <inheritdoc cref="IDamageSource.KnockbackStrength"/>
    public float KnockbackStrength => _knockbackStrength;
}