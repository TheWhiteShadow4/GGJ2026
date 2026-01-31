using UnityEngine;
public interface IKnockbackTarget
{
    /// <summary>
    /// Value from 0 to 1 signifying how strongly the unit is affected by knockback effects (0 for full knockback, 1 for no knockback at all)
    /// </summary>
    float KnockbackResistance { get; }

    /// <summary>
    /// Cooldown period in seconds, where the unit cannot be knocked back again after an initial knockback effect
    /// </summary>
    float KnockbackCooldown { get; }

    void ApplyKnockback(Vector3 direction, float strength);
}
