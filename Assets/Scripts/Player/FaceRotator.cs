using UnityEngine;
using System.Collections;

/// <summary>
/// Dreht den Kopf immer mit der richtigen Seite nach Vorne, abhängig von der gewählten Maske.
/// </summary>
public class FaceRotator : MonoBehaviour
{
	public AnimationCurve faceRotationCurve;
	public float duration = 0.4f;

	public void ApplyFace(float angle)
	{
		float startAngle = transform.localRotation.eulerAngles.y;
		float endAngle = angle;
		StartCoroutine(RotateFace(startAngle, endAngle));
	}

	private IEnumerator RotateFace(float startAngle, float endAngle)
	{
		float t = 0;
		while (t < 1)
		{
			t += Time.deltaTime / duration;
			float newAngle = Mathf.LerpAngle(startAngle, endAngle, faceRotationCurve.Evaluate(t));
			transform.localRotation = Quaternion.Euler(0, newAngle, 0);
			yield return null;
		}
		transform.localRotation = Quaternion.Euler(0, endAngle, 0);
	}
}