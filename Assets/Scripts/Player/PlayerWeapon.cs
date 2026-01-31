using UnityEngine;

public abstract class PlayerWeapon : MonoBehaviour
{
	public abstract void Fire();
	public virtual void Stop() {}
}