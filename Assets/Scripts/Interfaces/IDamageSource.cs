public interface IDamageSource
{
    /// <summary>
    /// Get Damage Type.
    /// </summary>
    ElementType Type { get; }

    /// <summary>
    /// Get Damage Value.
    /// </summary>
    float Damage { get; }

    /// <summary>
    /// Signifies if the damage source applies a knockback effect to the target on hit.
    /// </summary>
    bool AppliesKnockback { get; }

    /// <summary>
    /// Get strength of the knockback effect.
    /// </summary>
    float KnockbackStrength { get; }

    /// <summary>
    /// Convert Damage Source as string.
    /// </summary>
    /// <returns>Returns DamageSource as string.</returns>
    string ToString();
}
