using UnityEngine;
using TWS.Events;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class PlayerController : MonoBehaviour
{
	public static PlayerController Current;

	public IceCannon iceCannon;

	public float moveSpeed = 5f;

	private PlayerInputController playerInputController = new PlayerInputController();

	private bool isAttacking = false;

	private PlayerCharacter playerCharacter;

	void Awake()
	{
		if (Current != null)
		{
			Destroy(Current.gameObject);
		}
		Current = this;
		playerCharacter = GetComponent<PlayerCharacter>();

        if (GameManager.Instance.wasLoaded)
        {
            GameManager.Instance.wasLoaded = false;
            GameProgress.Current.RestorePlayerPosition(transform);
        }
    }

    void OnEnable()
	{
		var controls = GameManager.Instance.controls;
		controls.Player.Enable();
		playerInputController.Registrate(controls);
		controls.Player.Attack.performed += OnAttack;
		controls.Player.Block.performed += OnBlock;
		controls.Player.Interact.performed += OnInteract;
		controls.Player.Dash.performed += OnDash;
		playerInputController.OnChangeWeapon += OnChangeMask;
	}

	void OnDisable()
	{
		var controls = GameManager.Instance.controls;
		playerInputController.Unregistrate(controls);
		controls.Player.Attack.performed -= OnAttack;
		controls.Player.Block.performed -= OnBlock;
		controls.Player.Interact.performed -= OnInteract;
		controls.Player.Dash.performed -= OnDash;
		controls.Player.Disable();
	}

	void Update()
	{
		Vector3 velocity = new Vector3(playerInputController.moveDirection.x, 0, playerInputController.moveDirection.y) * moveSpeed;
		Vector3 movement = velocity * Time.deltaTime;
		transform.Translate(movement, Space.World);

		if (isAttacking)
		{
			playerCharacter.Fire();
		}
	}

	private void OnChangeMask(int index)
	{
		playerCharacter.OnChangeMask(index);
	}

	private void OnAttack(InputAction.CallbackContext context)
	{
		isAttacking = context.ReadValueAsButton();
		if (!isAttacking) playerCharacter.Stop();
	}

	private void OnBlock(InputAction.CallbackContext context)
	{
		Debug.Log("Block");
	}

	private void OnInteract(InputAction.CallbackContext context)
	{
		Debug.Log("Interact");
	}

	private void OnDash(InputAction.CallbackContext context)
	{
		Debug.Log("Dash");
	}
}