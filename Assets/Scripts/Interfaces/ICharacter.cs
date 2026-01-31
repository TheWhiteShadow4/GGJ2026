public interface ICharacter : IKnockbackTarget
{
    IHealth Health { get; }
    IDamageResistance Resistance { get; }
}
