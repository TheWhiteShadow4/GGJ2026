using UnityEngine;

[CreateAssetMenu(fileName = "ElenentType", menuName = "Element")]
public class ElementTypeDefinition : ScriptableObject
{
    [SerializeField] ElementType _element;

    [Header("Fire - Weakness: Water, Strength: Air")]
    [Header("Water - Weakness: Earth, Strength: Fire")]
    [Header("Air - Weakness: Fire, Strength: Earth")]
    [Header("Earth - Weakness: Air, Strength: Water")]
    [Space]
    [SerializeField, Range(0.1f, 100f)] float _fireResistance = 1f;
    [SerializeField, Range(0.1f, 100f)] float _waterResistance = 1f;
    [SerializeField, Range(0.1f, 100f)] float _airResistance = 1f;
    [SerializeField, Range(0.1f, 100f)] float _earthResistance = 1f;

    public ElementType Element => _element;
    public float FireResistance => _fireResistance;
    public float WaterResistance => _waterResistance;
    public float AirResistance => _airResistance;
    public float EarthResistance => _earthResistance;
}