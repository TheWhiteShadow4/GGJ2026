using UnityEngine;
public interface IKnockbackTarget
{
    //Value from 0 to 1 signifying how strongly the unit is affected by knockback effects (0 for full knockback, 1 for no knockback at all)
    float KnockbackResistance { get; }
    void ApplyKnockback(Vector3 direction, float strength);
}
