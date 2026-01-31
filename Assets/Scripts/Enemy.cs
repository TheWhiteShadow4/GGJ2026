using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField] ElementType _element;

    /// <inheritdoc cref="IPlayer.Health"/>
    public IHealth Health { get; private set; }

    /// <inheritdoc cref="IPlayer.Element"/>
    public ElementType Element => _element;

    /// <inheritdoc cref="IPlayer.Resistance"/>
    public IDamageResistance Resistance { get; private set; }

    [SerializeField] float _aggressionRange = 20f;
    [SerializeField] float _speed = 0.01f;
    [SerializeField] float _fireRate = 1f;

    private bool _aggro = false;
    private PlayerCharacter _player;

    private void Awake()
    {
        Health = GetComponentInChildren<IHealth>();
        Resistance = GetComponentInChildren<IDamageResistance>();
        _player = (PlayerCharacter)GameObject.FindFirstObjectByType(typeof(PlayerCharacter));
    }

    private void Update()
    {
        if (_aggro)
        {
            //TODO add more dynamic pathfinding in case we use more complex level with walls and obstacles

            transform.position += (_player.gameObject.transform.position - gameObject.transform.position).normalized * _speed;
        }
        else
        {
            if((_player.gameObject.transform.position - gameObject.transform.position).magnitude < _aggressionRange)
            {
                _aggro = true;
            }
        }
    }

    /// <summary>
    /// Called bia UnityEvent.
    /// </summary>
    public void Died() 
    {
        Debug.Log($"Enemy {name} Died.");
        GameObject.Destroy(gameObject);
    }

    /// <summary>
    /// Called bia UnityEvent.
    /// </summary>
    /// <param name="type">ElementType type.</param>
    /// <param name="damage">Damage type.</param>
    public void GotHit(ElementType type, float damage) 
    {
        Debug.Log($"Got {damage} {type} damage. {Health.Hp} HP left.");
    }
}
