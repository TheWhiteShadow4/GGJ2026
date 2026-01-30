using UnityEngine;
using System.Reflection;

public static class LayerMaskExtensions
{
    public static bool Include(this LayerMask layerMask, int layer)
    {
        return (layerMask == (layerMask | (1 << layer)));
    }
}

public static class TransformExtensions
{
    public static void Lerp(this Transform transfom, Transform t1, Transform t2, float t)
    {
        Vector3 p1;
        Vector3 p2;
        Quaternion r1;
        Quaternion r2;
        t1.GetPositionAndRotation(out p1, out r1);
        t2.GetPositionAndRotation(out p2, out r2);
        transfom.Lerp(p1, r1, p2, r2, t);
    }

    public static void Lerp(this Transform transfom, Vector3 pos1, Quaternion rot1, Vector3 pos2, Quaternion rot2, float t)
    {
        Quaternion rot = Quaternion.Slerp(rot1, rot2, t);
        transfom.SetPositionAndRotation(Vector3.Lerp(pos1, pos2, t), rot);
    }
}

public static class Vector3Extensions
{
    public static Vector3 Round(this Vector3 vector)
    {
        vector.x = Mathf.Round(vector.x);
        vector.y = Mathf.Round(vector.y);
        vector.z = Mathf.Round(vector.z);
        return vector;
    }

    public static void ToFloatVec(this Vector3 vector, ref float[] floats)
    {
        floats[0] = vector.x;
        floats[1] = vector.y;
        floats[2] = vector.z;
    }

    public static float Angle180(Vector3 from, Vector3 to, Vector3 axis)
    {
        float angle = Vector3.SignedAngle(from, to, axis);
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }

	public static Vector2 Vec2(this Vector3 v)
	{
		return new Vector2(v.x, v.z);
	}
}

public static class Vector2Extension
{
    public static Vector2 Floor(this Vector2 v)
    {
        return new Vector2(Mathf.Floor(v.x), Mathf.Floor(v.y));
    }

    public static Vector2 Abs(this Vector2 v)
    {
        return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
    }

    public static Vector2 MinComponents(this Vector2 v, Vector2 other)
    {
        return new Vector2(Mathf.Min(v.x, other.x), Mathf.Min(v.y, other.y));
    }

    public static Vector2 MaxComponents(this Vector2 v, Vector2 other)
    {
        return new Vector2(Mathf.Max(v.x, other.x), Mathf.Max(v.y, other.y));
    }

	public static Quaternion ToRotation(this Vector2 v)
	{
		//v = v.normalized;
		float rotZ = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
		return Quaternion.Euler(0f, 0f, rotZ - 90);
	}
}

// Reflektion Optimierung beim Aufrufen von Methoden ist eine Array erforderlich.
// Diese Extension Methoden vermeiden die Erstellung eines neuen Arrays für jedes Invoke.
// Hab ich aus dem Visual Scripting Package geklaut.
public static class MethodInfoExtensions
{
    private static object[] oneArgArray = new object[1];
	private static object[] twoArgArray = new object[2];

    public static object InvokeOptimized(this MethodInfo method, object obj, object arg0)
    {
        oneArgArray[0] = arg0;
        return method.Invoke(obj, oneArgArray);
    }

	public static object InvokeOptimized(this MethodInfo method, object obj, object arg0, object arg1)
	{
		twoArgArray[0] = arg0;
		twoArgArray[1] = arg1;
		return method.Invoke(obj, twoArgArray);
	}
}