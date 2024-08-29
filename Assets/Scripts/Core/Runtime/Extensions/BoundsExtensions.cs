using UnityEngine;

public static class BoundsExtensions
{
	/// <returns> A random point inside or on the surface of a box with size 1.0 </returns>
	public static Vector3 InsideUnitBox()
	{
		return BoundsUtils.InsideUnitBox();
	}

	/// <returns> A random point on the surface of a box with size 1.0 </returns>
	public static Vector3 OnUnitBox()
	{
		return BoundsUtils.OnUnitBox();
	}

	public static Vector3 GetRandomPoint(this Bounds a)
	{
		return BoundsUtils.GetRandomPoint(a.center, a.size);
	}

	public static Vector3 GetRandomPointInOuter(this Bounds a, Vector3 dismissBoxSize)
	{
		return BoundsUtils.GetRandomPointInOuter(a.center, a.size, dismissBoxSize);
	}

	public static Vector3 GetRandomPointAtSurface(this Bounds a)
	{
		return BoundsUtils.GetRandomPointAtSurface(a.center, a.size);
	}
}