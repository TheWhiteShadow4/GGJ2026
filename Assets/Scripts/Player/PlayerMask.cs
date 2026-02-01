using UnityEngine;

public class PlayerMask : MonoBehaviour
{
	public MaskComponent maskComponent;
	public PlayerWeapon playerWeapon;
	public GameObject activatedEffect;
	public SpriteRenderer faceRenderer;
	public Sprite deactivatedFace;
	public Sprite activatedFace;
	public float headAngle;

	void Awake()
	{
		if (activatedEffect != null)
		{
			activatedEffect.SetActive(false);
		}
		faceRenderer.sprite = deactivatedFace;
	}

	public void Activate(FaceRotator faceRotator)
	{
		if (activatedEffect != null)
		{
			activatedEffect.SetActive(true);
		}
		faceRotator.ApplyFace(headAngle);
		faceRenderer.sprite = activatedFace;
	}

	public void Deactivate()
	{
		if (activatedEffect != null)
		{
			activatedEffect.SetActive(false);
		}
		playerWeapon.Stop();
		faceRenderer.sprite = deactivatedFace;
	}
}