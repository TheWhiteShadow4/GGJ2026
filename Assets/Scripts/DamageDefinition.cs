using UnityEngine;

[CreateAssetMenu(fileName = "DamageDefinition", menuName = "Damage")]
public class DamageDefinition : ScriptableObject
{
    [SerializeField] ElementType _element;
    [SerializeField, Range(0, 100)] float _damage;

    public ElementType Element => _element;
    public float Damage => _damage;
}