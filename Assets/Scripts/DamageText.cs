using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] TextMeshPro _textField;
    [SerializeField] float _movementSpeed;
    [SerializeField] float _visibilityTime;

    public void OnHit(ElementType element, float damage)
    {
        _textField.text = $"{Mathf.Round(damage)}";
        _textField.color = GetElementColor(element);
    }

    private void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * _movementSpeed;
    }

    private void OnEnable()
    {
        Invoke(nameof(DisableAfterTimePassed), _visibilityTime);
    }

    void DisableAfterTimePassed() => Destroy(gameObject);

    private Color GetElementColor(ElementType element)
        => element switch
        {
            ElementType.Air => Color.grey,
            ElementType.Water => Color.blue,
            ElementType.Fire => Color.red,
            ElementType.Earth => Color.brown,
            ElementType.None => Color.white,
            _ => Color.white,
        };
}
