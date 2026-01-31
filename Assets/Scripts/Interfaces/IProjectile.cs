public interface IProjectile
{
    /// <summary>
    /// Gets the Damage Source of this projectile.
    /// </summary>
    IDamageSource DamageSource { get; }

    /// <summary>
    /// Gets the velocity of this projectile.
    /// </summary>
    float Velocity { get; }
}
