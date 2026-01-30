using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class is used to pool game objects.<br/>
/// - <see cref="GetSharedPool"/> is used to get a pool for a specific object type.<br/>
/// - <see cref="GetInstance"/> is used to get an instance of a pooled object.<br/>
/// - <see cref="Return"/> is used to return an object back to the pool.<br/>
/// </summary>
public partial class Pool
{
	private static Dictionary<Object, Pool> pools = new Dictionary<Object, Pool>();

	/// <summary>
	/// Get a shared pool for a specific object type.
	/// </summary>
	/// <param name="prefab">A MonoBehaviour or GameObject prefab.</param>
	/// <returns>The pool for the instances of the prefab.</returns>
	public static Pool GetSharedPool(Object prefab)
	{
		if (prefab == null) throw new System.ArgumentNullException("Prefab can not be null");

		if (!pools.TryGetValue(prefab, out Pool p))
		{
			p = new Pool(prefab);
			pools.Add(prefab, p);
		}
		return p;
	}

	/// <summary>
	/// Get all pools. Costly operation, use with caution.
	/// </summary>
	public static Pool[] GetPools()
	{
		return pools.Values.ToArray();
	}

	private readonly Object prefab;
	private readonly string cloneName;
	private readonly List<GameObject> poolItems = new List<GameObject>();

	public int Count { get { return poolItems.Count; } }

	public Object Prefab { get { return prefab; } }

	private Pool(Object prefab)
	{
		this.prefab = prefab;
		cloneName = $"{prefab.name} (Pooled)";
	}

	public GameObject GetInstance()
	{
		return GetInstance<GameObject>();
	}

	public T GetInstance<T>() where T : Object
	{
		GameObject obj;
		if (poolItems.Count == 0)
		{
			if (prefab is GameObject)
				obj = Object.Instantiate(prefab as GameObject);
			else if (prefab is Component)
				obj = Object.Instantiate(prefab as Component).gameObject;
			else
			{
				Debug.LogError("Unsupporteter Typ " + prefab.GetType());
				return null;
			}

			obj.name = cloneName;
			obj.AddComponent<PoolTracker>().pool = this;
		}
		else
		{
			obj = poolItems[^1];
			poolItems.RemoveAt(poolItems.Count - 1);
			if (obj == null)
			{
				return GetInstance<T>();
			}
		}
		obj.SetActive(true);

		if (typeof(T) == typeof(GameObject))
			return obj as T;
		else
			return obj.GetComponent<T>() as T;
	}

	/// <summary>
	/// Clear all pools.
	/// For example when you switch to a new scene.
	/// </summary>
	public static void ClearAll()
	{
		pools.Clear();
	}

	/// <summary>
	/// Returns an pooled object back to the pool.
	/// </summary>
	public static void Return(GameObject obj)
	{
		obj.SetActive(false);
	}

	/// <summary>
	/// Returns an pooled object back to the pool.
	/// </summary>
	public void Return<T>(T obj) where T : Object
	{
		if (obj is GameObject)
			(obj as GameObject).SetActive(false);
		else if (obj is Component)
			(obj as Component).gameObject.SetActive(false);
	}

	/// <summary>
	/// Use if you dont know if the object is pooled or not.
	/// </summary>
	public static void SafeDestroy(GameObject obj)
	{
		if (obj.GetComponent<PoolTracker>() != null)
			Return(obj);
		else
			Object.Destroy(obj);
	}

	[ExcludeFromObjectFactory]
	private sealed class PoolTracker : MonoBehaviour
	{
		internal Pool pool;

		void OnDisable()
		{
			if (gameObject == null)
			{
				Debug.LogWarning("Pool-Objekt wurde zerstört.");
				return;
			}
			pool.poolItems.Add(gameObject);
		}
	}

}

