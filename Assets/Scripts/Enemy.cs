using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    Health _health;
    [SerializeField] float _knockbackResistance;
    ShootComponent _shootComponent;

    /// <inheritdoc cref="IPlayer.Health"/>
    public IHealth Health => _health;

    /// <inheritdoc cref="IPlayer.Resistance"/>
    public IDamageResistance Resistance { get; private set; }

    /// <inheritdoc cref="IKnockbackTarget.KnockbackResistance"/>
    public float KnockbackResistance => _knockbackResistance;

    [SerializeField] bool _drawGizmos = false;
    [SerializeField] float _aggressionRange = 20f;
    [SerializeField] float _speed = 1f;
    [SerializeField] float _fireRate = 1f;

    private bool _aggro = false;
    private PlayerCharacter _player;
    private Vector3 impactForce = Vector3.zero;

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

        Vector3 impactMovement = Vector3.zero;
        Vector3 movement = Vector3.zero;

        if (impactForce.magnitude > 0.2f)
        {
            impactMovement = impactForce * Time.deltaTime;
        }
        impactForce = Vector3.Lerp(impactForce, Vector3.zero, 5 * Time.deltaTime);

        if (_aggro)
        {
            //TODO add more dynamic pathfinding in case we use more complex level with walls and obstacles
            transform.LookAt(_player.gameObject.transform.position);

            if (HasShootComponent)
            {
                // come closer
                if (playerDir.magnitude < _aggressionRange)
                {
                    movement = playerDir.normalized * _speed * Time.deltaTime;
                }
            }
            else
            {
                // try to deathly touch the player
                movement = playerDir.normalized * _speed * Time.deltaTime;
            }
        }
        else
        {
            if(playerDir.magnitude < _aggressionRange)
            {
                _aggro = true;
            }
        }

        transform.position += movement + impactMovement;
    }

    public void ApplyKnockback(Vector3 direction, float strength)
    {
        Vector3 knockbackForce = direction * strength * (1 - _knockbackResistance);

        impactForce += knockbackForce;
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
        Debug.Log($"{type.GetEfficience(Resistance.Element)}! - Enemy got {damage} {type} damage. {Health.Hp} HP left.");
    }

    private void OnDrawGizmos()
    {
        if (!_drawGizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _aggressionRange);
    }
}
