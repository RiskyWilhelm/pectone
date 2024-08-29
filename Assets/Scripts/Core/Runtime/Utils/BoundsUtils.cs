using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class BoundsUtils
{
	/// <returns> A random point inside or on the surface of a box with size 1.0 </returns>
	public static Vector3 InsideUnitBox()
	{
		return GetRandomPoint(Vector3.zero, Vector3.one);
	}

	/// <returns> A random point on the surface of a box with size 1.0 </returns>
	public static Vector3 OnUnitBox()
	{
		return GetRandomPointAtSurface(Vector3.zero, Vector3.one);
	}

	public static Vector3 GetRandomPoint(Vector3 center, Vector3 size)
	{
		Vector3 extents = (size * 0.5f);
		Vector3 localPoint = new(
			Random.Range(-extents.x, extents.x),
			Random.Range(-extents.y, extents.y),
			Random.Range(-extents.z, extents.z)
		);

		localPoint += center;
		return localPoint;
	}

	public static Vector3 GetRandomPointInOuter(Vector3 center, Vector3 size, Vector3 dismissBoxSize)
	{
		Vector3 outerBoxSize = (size - dismissBoxSize);

		var onUnitBoxUniformly = OnUnitBox();
		var onDismissedBox = Vector3.Scale(onUnitBoxUniformly, dismissBoxSize);
		var outerBox = new Vector3(
			MathF.Sqrt(Random.Range(0f, 1f)) * outerBoxSize.x,
			MathF.Sqrt(Random.Range(0f, 1f)) * outerBoxSize.y,
			MathF.Sqrt(Random.Range(0f, 1f)) * outerBoxSize.z
		); ;

		Vector3 localPoint = onDismissedBox + Vector3.Scale(onUnitBoxUniformly, outerBox);
		localPoint += center;
		return localPoint;
	}

	public static Vector3 GetRandomPointAtSurface(Vector3 center, Vector3 size)
	{
		Vector3 extents = (size * 0.5f);
		Vector3 localPoint = default;
		var randomFaceDirection = Random.Range(1, 7);

		switch (randomFaceDirection)
		{
			// Up
			case 1:
			localPoint = new(
				Random.Range(-extents.x, extents.x),
				extents.y,
				Random.Range(-extents.z, extents.z)
			);
			break;

			// Down
			case 2:
			localPoint = new(
				Random.Range(-extents.x, extents.x),
				-extents.y,
				Random.Range(-extents.z, extents.z)
			);
			break;

			// Right
			case 3:
			localPoint = new(
				extents.x,
				Random.Range(-extents.y, extents.y),
				Random.Range(-extents.z, extents.z)
			);
			break;

			// Left
			case 4:
			localPoint = new(
				-extents.x,
				Random.Range(-extents.y, extents.y),
				Random.Range(-extents.z, extents.z)
			);
			break;

			// Forward
			case 5:
			localPoint = new(
				Random.Range(-extents.x, extents.x),
				Random.Range(-extents.y, extents.y),
				extents.z
			);
			break;

			// Backward
			case 6:
			localPoint = new(
				Random.Range(-extents.x, extents.x),
				Random.Range(-extents.y, extents.y),
				-extents.z
			);
			break;
		}

		localPoint += center;
		return localPoint;
	}
}