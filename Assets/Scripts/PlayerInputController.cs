using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerInputController
{

	public Vector2 moveDirection;
	private Vector2 lookDirection;
	private Camera camera;

	public Action<int> OnChangeWeapon;

	public void Registrate(Controls controls)
	{
		controls.Player.Move.performed += OnMove;
		controls.Player.Look.performed += OnLook;
		controls.Player.LookAt.performed += OnLookAt;
		camera = Camera.main;

		controls.Player.Weapon1.performed += OnWeapon1;
		controls.Player.Weapon2.performed += OnWeapon2;
		controls.Player.Weapon3.performed += OnWeapon3;
		controls.Player.Weapon4.performed += OnWeapon4;
	}

	public void Unregistrate(Controls controls)
	{
		controls.Player.Move.performed -= OnMove;
		controls.Player.Look.performed -= OnLook;
		controls.Player.LookAt.performed -= OnLookAt;
		controls.Player.Weapon1.performed -= OnWeapon1;
		controls.Player.Weapon2.performed -= OnWeapon2;
		controls.Player.Weapon3.performed -= OnWeapon3;
		controls.Player.Weapon4.performed -= OnWeapon4;
		OnChangeWeapon = null;
	}

	private void OnMove(InputAction.CallbackContext context)
	{
		moveDirection = context.ReadValue<Vector2>();
	}

	private void OnLook(InputAction.CallbackContext context)
	{
		lookDirection = context.ReadValue<Vector2>();
		lookDirection += PlayerController.Current.transform.position.Vec2();
		PlayerController.Current.transform.LookAt(lookDirection);
	}

	private void OnLookAt(InputAction.CallbackContext context)
	{
		var mousePosition = context.ReadValue<Vector2>();
		Plane plane = new Plane(Vector3.up, Vector3.zero);
		Ray ray = camera.ScreenPointToRay(mousePosition);
		if (plane.Raycast(ray, out float distance))
		{
			Vector3 worldPoint = ray.GetPoint(distance);
			worldPoint.y = PlayerController.Current.transform.position.y;
			PlayerController.Current.transform.LookAt(worldPoint);
		}
	}

	private void OnWeapon1(InputAction.CallbackContext context)
	{
		OnChangeWeapon?.Invoke(0);
	}

	private void OnWeapon2(InputAction.CallbackContext context)
	{
		OnChangeWeapon?.Invoke(1);
	}
	
	
	private void OnWeapon3(InputAction.CallbackContext context)
	{
		OnChangeWeapon?.Invoke(2);
	}

	private void OnWeapon4(InputAction.CallbackContext context)
	{
		OnChangeWeapon?.Invoke(3);
	}
}