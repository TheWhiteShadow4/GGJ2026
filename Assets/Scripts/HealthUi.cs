using UnityEngine;

public class HealthUi : MonoBehaviour
{
    [SerializeField] RectTransform _healthBar;

    [SerializeField] float _health;
    [SerializeField] float _maxHealth;
    [SerializeField] float _width;
    [SerializeField] float _height;

    public void SetMaxHealth(float maxHealth) => _maxHealth = maxHealth;

    public void SetHealth(float health)
    {
        _health = health;
        float newWidth = (_health / _maxHealth) * _width;
        _healthBar.sizeDelta = new Vector2(newWidth, _height);
    }
}