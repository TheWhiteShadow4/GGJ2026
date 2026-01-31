public enum ElementType
{
    None = 0,
    Fire,
    Water,
    Air,
    Earth
}

public enum Effectiveness
{
    Normal,
    NotEffective,
    Weak,
    Strong,
}

public static class ElementTypeExtensions
{
    public static Effectiveness GetEfficience(this ElementType element, ElementType other)
        => element switch
        {
            ElementType.None => Effectiveness.Normal,
            ElementType.Fire => other switch 
            {
                ElementType.Fire => Effectiveness.NotEffective,
                ElementType.Water => Effectiveness.Weak,
                ElementType.Air => Effectiveness.Strong,
                ElementType.Earth => Effectiveness.Normal,
                _ => Effectiveness.Normal,
            },
            ElementType.Water => other switch
            {
                ElementType.Fire => Effectiveness.Strong,
                ElementType.Water => Effectiveness.NotEffective,
                ElementType.Air => Effectiveness.Normal,
                ElementType.Earth => Effectiveness.Weak,
                _ => Effectiveness.Normal,
            },
            ElementType.Air => other switch
            {
                ElementType.Fire => Effectiveness.Weak,
                ElementType.Water => Effectiveness.Normal,
                ElementType.Air => Effectiveness.NotEffective,
                ElementType.Earth => Effectiveness.Strong,
                _ => Effectiveness.Normal,
            },
            ElementType.Earth => other switch
            {
                ElementType.Fire => Effectiveness.Normal,
                ElementType.Water => Effectiveness.Strong,
                ElementType.Air => Effectiveness.Weak,
                ElementType.Earth => Effectiveness.NotEffective,
                _ => Effectiveness.Normal,
            },
            _ => Effectiveness.Normal,
        };
}
