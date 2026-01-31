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
    /// Convert Damage Source as string.
    /// </summary>
    /// <returns>Returns DamageSource as string.</returns>
    string ToString();
}
