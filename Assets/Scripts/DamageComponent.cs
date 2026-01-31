using UnityEngine;

public class DamageComponent : MonoBehaviour, IDamageSource
{
	[SerializeField] ElementType _type;
    [SerializeField] float _damage;

	/// <inheritdoc cref="IDamageSource.Type"/>
    public ElementType Type => _type;

    /// <inheritdoc cref="IDamageSource.Damage"/>
    public float Damage => _damage;

    /// <inheritdoc cref="IDamageSource.ToString"/>
    public override string ToString() => $"{Damage} {Type} Damage";
}