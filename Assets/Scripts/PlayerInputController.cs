using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerInputController
{

	public Vector2 moveDirection;
	private Vector2 lookDirection;
	private Camera camera;

	public void Registrate(Controls controls)
	{
		controls.Player.Move.performed += OnMove;
		controls.Player.Look.performed += OnLook;
		controls.Player.LookAt.performed += OnLookAt;
		camera = Camera.main;
	}

	public void Unregistrate(Controls controls)
	{
		controls.Player.Move.performed -= OnMove;
		controls.Player.Look.performed -= OnLook;
		controls.Player.LookAt.performed -= OnLookAt;
	}

	private void OnMove(InputAction.CallbackContext context)
	{
		moveDirection = context.ReadValue<Vector2>();
	}

	private void OnLook(InputAction.CallbackContext context)
	{
		lookDirection = context.ReadValue<Vector2>();
		//lookDirection += PlayerController.Current.transform.position.Vec2();
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
}