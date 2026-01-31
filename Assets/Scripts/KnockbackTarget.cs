using UnityEngine;
using UnityEngine.Events;

public class KnockbackTarget : MonoBehaviour, IKnockbackTarget
{
    [SerializeField] float _knockbackResistance;
    private Rigidbody _rigidbody;

    /// <inheritdoc cref="IKnockbackTarget.KnockbackResistance"/>
    public float KnockbackResistance => _knockbackResistance;
    private void Awake()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    public void ApplyKnockback(Vector3 direction, float strength)
    {
        if (!enabled) return;

        Vector3 knockbackForce = direction * strength * (1 - _knockbackResistance);

        _rigidbody.AddForce(knockbackForce);
    }
}
