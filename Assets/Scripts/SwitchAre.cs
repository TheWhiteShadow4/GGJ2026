using UnityEngine;
using UnityEngine.Events;

public class SwitchAre : MonoBehaviour
{
    [SerializeField] bool _on;
    [SerializeField] UnityEvent<bool> _onSwitchChanged;
    [SerializeField] Transform _switchPivot;
    [SerializeField] Vector3 _rotationAxis;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _on = !_on;
            _switchPivot.Rotate(_rotationAxis, _on ? -45 : 45);
            _onSwitchChanged?.Invoke(_on);
        }
    }
}