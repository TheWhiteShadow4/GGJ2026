using UnityEngine;

public class MaskComponent : MonoBehaviour, IDamageResistance
{
    [SerializeField] ElementTypeDefinition _resistances;

    public ElementType Element => _resistances.Element;

    public float GetMultiplier(ElementType type)
        => type switch 
        {
            ElementType.Fire => _resistances.FireResistance,
            ElementType.Water => _resistances.WaterResistance,
            ElementType.Air => _resistances.AirResistance,
            ElementType.Earth => _resistances.EarthResistance,
            _ => 1f,
        };
}
