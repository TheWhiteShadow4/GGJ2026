using UnityEngine;

public enum MaskAnimationState
{
    Off = 0,
    On,
    Shoot
}

public class PlayerMask : MonoBehaviour
{
	public MaskComponent maskComponent;
	public PlayerWeapon playerWeapon;
	public float headAngle;

	[SerializeField] Animator _animator;

	public void SetAnimationState(MaskAnimationState state)
	{
		switch (state)
		{
			case MaskAnimationState.Off:
				_animator.SetInteger("state", 0);
				break;
			case MaskAnimationState.On:
                _animator.SetInteger("state", 1);
                break;
            case MaskAnimationState.Shoot:
                _animator.SetInteger("state", 2);
                break;
        }
	}
}