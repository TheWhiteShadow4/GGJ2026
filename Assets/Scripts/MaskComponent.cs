using UnityEngine;

public class MaskComponent : MonoBehaviour, IDamageResistance
{
    [Header("Damage Resistance - Multiplier, (0 - 100% resistance)")]
    [SerializeField] float _fire = 1f;
    [SerializeField] float _water = 1f;
    [SerializeField] float _air = 1f;
    [SerializeField] float _earth = 1f;

    public float GetMultiplier(ElementType type)
        => type switch 
        {
            ElementType.Fire => _air,
            ElementType.Water => _water,
            ElementType.Air => _air,
            ElementType.Earth => _earth,
            _ => 1f,
        };
}
