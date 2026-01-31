public interface IHealth
{
    float Hp { get; }
    float MaxHp { get; }
    void DoDamage(IDamageSource damage);
}
