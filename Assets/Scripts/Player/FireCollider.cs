using UnityEngine;
using System.Collections.Generic;
using TWS.Utils;

public class FireCollider : MonoBehaviour
{
	public SphereCollider[] colliders;
	
	public AnimationCurve radius;

	public int usedColliders = 0;
	public float baseLifetime = 0.75f;
	public float speed = 10f;
	public float frequency = 0.25f;

	private List<Vector3> velocities;
	private List<float> lifetimes;

	private float timer;

    void Start()
    {
        usedColliders = 0;
		velocities = new List<Vector3>();
		lifetimes = new List<float>();
		for (int i = 0; i < colliders.Length; i++)
		{
			velocities.Add(Vector3.zero);
			lifetimes.Add(0);
			colliders[i].enabled = false;
		}
		timer = frequency;
    }

    // Update is called once per frame
    void Update()
    {
		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			timer = frequency;
			if (usedColliders < colliders.Length)
			{
				usedColliders++;
				CreateCollider(usedColliders-1);
			}
		}
        for (int i = 0; i < usedColliders; i++)
		{
			lifetimes[i] += Time.deltaTime;
			if (lifetimes[i] >= baseLifetime)
			{
				CreateCollider(i);
			}
			/*if (lifetimes[i] <= 0)
			{
				usedColliders--;
				lifetimes.SwapRemove(i);
				velocities.SwapRemove(i);
				colliders[i].enabled = false;
				continue;
			}*/
			colliders[i].transform.position += Time.deltaTime * velocities[i];
			colliders[i].radius = radius.Evaluate(lifetimes[i] / baseLifetime);
		}
    }

	void CreateCollider(int index)
	{
		SphereCollider collider = colliders[index];
		
		velocities[index] = PlayerController.Current.transform.forward * speed;
		lifetimes[index] = 0;
		collider.transform.position = PlayerController.Current.transform.position;
		collider.radius = radius.Evaluate(0);
		collider.enabled = true;
	}
}
