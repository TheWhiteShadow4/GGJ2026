public interface IHealth
{
    float Hp { get; }
    void DoDamage(IDamageSource damage);
}
