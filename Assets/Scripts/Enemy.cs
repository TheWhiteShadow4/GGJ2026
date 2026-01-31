using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField] ElementType _element;
    Health _health;
    ShootComponent _shootComponent;

    /// <inheritdoc cref="IPlayer.Health"/>
    public IHealth Health => _health;

    /// <inheritdoc cref="IPlayer.Element"/>
    public ElementType Element => _element;

    /// <inheritdoc cref="IPlayer.Resistance"/>
    public IDamageResistance Resistance { get; private set; }

    [SerializeField] bool _drawGizmos = false;
    [SerializeField] float _aggressionRange = 20f;
    [SerializeField] float _speed = 0.01f;
    [SerializeField] float _fireRate = 1f;

    private bool _aggro = false;
    private PlayerCharacter _player;

    public bool HasShootComponent => _shootComponent != null;

    private void Awake()
    {
        _health = GetComponentInChildren<Health>();
        _health.HitEvent.AddListener(GotHit);
        _health.DeathEvent.AddListener(Died);
        Resistance = GetComponentInChildren<IDamageResistance>();
        _player = (PlayerCharacter)GameObject.FindFirstObjectByType(typeof(PlayerCharacter));

        _shootComponent = GetComponentInChildren<ShootComponent>();
        if (HasShootComponent)
        {
            InvokeRepeating(nameof(ShootAtThePlayer), 1f, 3f);
        }
    }

    private void Update()
    {
        var playerDir = _player.gameObject.transform.position - gameObject.transform.position;

        if (_aggro)
        {
            //TODO add more dynamic pathfinding in case we use more complex level with walls and obstacles
            transform.LookAt(_player.gameObject.transform.position);

            if (HasShootComponent)
            {
                // come closer
                if (playerDir.magnitude < _aggressionRange)
                {
                    transform.position += playerDir.normalized * _speed;
                }
            }
            else
            {
                // try to deathly touch the player
                transform.position += playerDir.normalized * _speed;
            }
        }
        else
        {
            if(playerDir.magnitude < _aggressionRange)
            {
                _aggro = true;
            }
        }
    }

    private void ShootAtThePlayer() 
    {
        var playerDir = _player.gameObject.transform.position - gameObject.transform.position;
        if (playerDir.magnitude < _aggressionRange)
        {
            _shootComponent.Shoot();
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

    private void OnDrawGizmos()
    {
        if (!_drawGizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _aggressionRange);
    }
}
