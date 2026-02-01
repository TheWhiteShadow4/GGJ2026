using UnityEngine;

public class PlayerMask : MonoBehaviour
{
	public enum MaskAnimationState
	{
		Off = 0,
		On,
		Shoot
	}

	public MaskComponent maskComponent;
	public PlayerWeapon playerWeapon;
	public GameObject activatedEffect;
	public SpriteRenderer faceRenderer;
	public float headAngle;

	void Awake()
	{
		if (activatedEffect != null)
		{
			activatedEffect.SetActive(false);
		}
	}

	public void Activate(FaceRotator faceRotator)
	{
		if (activatedEffect != null)
		{
			activatedEffect.SetActive(true);
		}
		faceRotator.ApplyFace(headAngle);
		SetAnimationState(MaskAnimationState.On);
	}

	public void Deactivate()
	{
		if (activatedEffect != null)
		{
			activatedEffect.SetActive(false);
		}
		playerWeapon.Stop();
		SetAnimationState(MaskAnimationState.Off);
	}

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