using System.Collections.Generic;
using UnityEngine;

public class LightningCannon : PlayerWeapon
{
	public float scanRadius = 2f;
	public float mainRange = 5f;
	public float dispersionDistance = 0.5f;
	public float secondaryRange = 3f;
	public int secondaryHits = 2;
	public float rescanInterval = 0.20f;
	public LayerMask scanLayerMask;
	public bool slitOnFirstHit = true;

	private Blitz[] blitze;
	private float reScanTimer;
	private List<TargetObject> hitObjects = new List<TargetObject>();
	private IDamageSource damageSource;

	void Awake()
	{
		blitze = GetComponentsInChildren<Blitz>();
		damageSource = GetComponent<IDamageSource>();
		foreach (Blitz blitz in blitze)
		{
			blitz.enabled = false;
		}
	}

	public override void Fire()
	{
		reScanTimer = rescanInterval;
		ScanForTargets();
	}

	void Update()
	{
		if (blitze[0].enabled)
		{
			reScanTimer -= Time.deltaTime;
			if (reScanTimer <= 0)
			{
				reScanTimer += rescanInterval;
				ScanForTargets();
			}
		}
	}

	private void ScanForTargets()
	{
		hitObjects.Clear();
		Transform tr = transform;
		Vector3 position = PlayerController.Current.playerInputController.mouseTarget;
		// Limit Range
		Vector3 direction = (position - tr.position).normalized;
		float distance = (position - tr.position).magnitude;
		position = tr.position + direction * Mathf.Min(distance, mainRange);
		if (ScanPosition(position, out TargetObject target))
		{
			// Für den Ersten Blitz wird nur das Ziel gesetzt, Ausgangspunkt bleibt der Spieler.
			ActivaterBlitz(tr.position, target);
			int iterationDeep = 0;
			position = target.hitPosition;
			while (iterationDeep < secondaryHits && ScanPosition(position, out TargetObject subTarget))
			{
				ActivaterBlitz(position, subTarget);
				if (!slitOnFirstHit) position = subTarget.hitPosition;
				iterationDeep++;
			}
		}
		else
		{
			// Erstelle ein Dummy Target
			ActivaterBlitz(tr.position, new TargetObject()
			{
				hitPosition = position,// + GetRandomDispersion(),
				collider = null,
			});
		}
		if (hitObjects.Count > 0 && hitObjects[0].collider != null)
		{
			Debug.Log("Blitz Scan Result: " + hitObjects.Count);
		}
		else
		{
			Debug.Log("No Target Found");
		}
	
		// Deaktiviere alle Blitze, die nicht getroffen wurden.
		for(int i = hitObjects.Count; i < blitze.Length; i++)
		{
			blitze[i].enabled = false;
		}
	}

	private static Collider[] HITS = new Collider[8];

	private bool ScanPosition(Vector3 aimPosition, out TargetObject targetObject)
	{
		int hits = Physics.OverlapSphereNonAlloc(aimPosition, scanRadius, HITS, scanLayerMask);
		Collider nearestHit = null;
		float nearestDistance = float.MaxValue;
		if (hits > 0)
		{
			for (int i = 0; i < hits; i++)
			{
				if (IsValidTarget(HITS[i]))
				{
					float dist = Vector3.Distance(aimPosition, HITS[i].transform.position);
					if (dist < nearestDistance)
					{
						nearestHit = HITS[i];
						nearestDistance = dist;
					}
				}
			}
			if (nearestDistance < float.MaxValue && nearestHit != null)
			{
				targetObject = new TargetObject()
				{
					hitPosition = nearestHit.transform.position,
					collider = nearestHit,
				};
				return true;
			}
		}
		targetObject = default;
		return false;
	}

	private bool IsValidTarget(Collider collider)
	{
		for (int i = 0; i < hitObjects.Count; i++)
		{
			if (collider == hitObjects[i].collider)
			{	// Bereits getroffen
				return false;
			}
		}
		return true;
	}

	private void ActivaterBlitz(Vector3 origin, TargetObject targetObject)
	{
		hitObjects.Add(targetObject);
		int idx = hitObjects.Count - 1;
		blitze[idx].enabled = true;
		blitze[idx].transform.position = origin;
		blitze[idx].target.position = targetObject.hitPosition;

		if (targetObject.collider != null && targetObject.collider.transform.parent.TryGetComponent(out IHealth health))
		{
			health.DoDamage(damageSource);
		}
	}

	private Vector3 GetRandomDispersion()
	{
		return new Vector3(Random.Range(-dispersionDistance, dispersionDistance), 0, Random.Range(-dispersionDistance, dispersionDistance));
	}

	public override void Stop()
	{
		foreach (Blitz blitz in blitze)
		{
			blitz.enabled = false;
		}
		hitObjects.Clear();
	}

	private struct TargetObject
	{
		public Vector3 hitPosition;
		public Collider collider;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		foreach (TargetObject target in hitObjects)
		{
			Gizmos.DrawWireSphere(target.hitPosition, scanRadius);
		}
	}
}