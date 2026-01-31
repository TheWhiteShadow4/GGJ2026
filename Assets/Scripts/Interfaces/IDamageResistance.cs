public interface IDamageResistance
{
    /// <summary>
    /// Get the resistance of the incoming type.
    /// </summary>
    /// <param name="type">Element type.</param>
    /// <returns>Returns the damage multiplier.</returns>
    float GetMultiplier(ElementType type);
}