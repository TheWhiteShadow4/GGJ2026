using UnityEngine;

public class DamageComponent : MonoBehaviour, IDamageSource
{
    [SerializeField] bool _appliesKnockback;
    [SerializeField] float _knockbackStrength;
    [SerializeField] DamageDefinition _damageDefinition;

	/// <inheritdoc cref="IDamageSource.Type"/>
    public ElementType Type => _damageDefinition.Element;

    /// <inheritdoc cref="IDamageSource.Damage"/>
    public float Damage => _damageDefinition.Damage;

    /// <inheritdoc cref="IDamageSource.ToString"/>
    public override string ToString() => $"{Damage} {Type} Damage";

    /// <inheritdoc cref="IDamageSource.AppliesKnockback"/>
    public bool AppliesKnockback => _appliesKnockback;

    /// <inheritdoc cref="IDamageSource.KnockbackStrength"/>
    public float KnockbackStrength => _knockbackStrength;
}