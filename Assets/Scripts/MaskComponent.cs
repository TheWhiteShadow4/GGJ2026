using UnityEngine;

public class MaskComponent : MonoBehaviour, IDamageResistance
{
    [Header("HealthDamage = IncomingDamage / Resistance -> Resistance: (0, 100] -> HealthDamage [hoch, niedrig]")]
    [SerializeField, Range(0.1f, 100f)] float _fireResistance = 1f;
    [SerializeField, Range(0.1f, 100f)] float _waterResistance = 1f;
    [SerializeField, Range(0.1f, 100f)] float _airResistance = 1f;
    [SerializeField, Range(0.1f, 100f)] float _earthResistance = 1f;

    public float GetMultiplier(ElementType type)
        => type switch 
        {
            ElementType.Fire => _fireResistance,
            ElementType.Water => _waterResistance,
            ElementType.Air => _airResistance,
            ElementType.Earth => _earthResistance,
            _ => 1f,
        };
}