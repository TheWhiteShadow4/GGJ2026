using UnityEngine;

public static class Layers
{
	public static int Default = 0;
	public static int TransparentFX = 1;
	public static int IgnoreRaycast = 2;
	public static int Water = 4;
	public static int UI = 5;
	public static int Interactable = 6;
	public static int Enemy = 7;
	public static int Player = 8;
	public static int EnemyWeapon = 9;
	public static int PlayerWeapon = 10;

	public static int GetMask(int layer)
	{
		return 1 << layer;
	}
}