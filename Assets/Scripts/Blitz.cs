using UnityEngine;

[ExecuteInEditMode]
public class Blitz : MonoBehaviour
{
	public LineRenderer lineRenderer;
	public Transform target;
	public int segments;
	public float amplitude = 1f;

	public float width = 0.2f;

	public float interval = 0.1f;
	private float timer;

	void OnEnable()
	{
		timer = 0;
		lineRenderer.enabled = true;
		UpdateLineRenderer();
	}

	void OnDisable()
	{
		lineRenderer.enabled = false;
	}

    // Update is called once per frame
    void LateUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer -= interval;
            UpdateLineRenderer();
        }
    }

	private void UpdateLineRenderer()
	{
		lineRenderer.positionCount = segments + 1;
		lineRenderer.SetPosition(0, transform.position);
		for (int i = 1; i < segments; i++)
		{
			float t = i / (float)segments;
			Vector3 position = Vector3.Lerp(transform.position, target.position, t);
			position += Random.onUnitSphere * amplitude;
			//position.y += Mathf.Sin(t * Mathf.PI * 2) * amplitude;
			lineRenderer.SetPosition(i, position);
		}
		lineRenderer.SetPosition(segments, target.position);
	}

	void OnValidate()
	{
		if (lineRenderer == null)
		{
			lineRenderer = GetComponent<LineRenderer>();
		}
		lineRenderer.widthMultiplier = width;
	}
}
