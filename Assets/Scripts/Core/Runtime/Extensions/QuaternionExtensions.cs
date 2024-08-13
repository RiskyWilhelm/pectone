using System;
using UnityEngine;

public static class QuaternionExtensions
{
	/// <returns> Non-normalized vector </returns>
	public static Vector3 GetForwardDirection(this Quaternion a)
	{
		return a * Vector3.forward;
	}

	/// <inheritdoc cref="GetForwardDirection(Quaternion)"/>
	public static Vector3 GetBackDirection(this Quaternion a)
	{
		return a * Vector3.back;
	}

	/// <inheritdoc cref="GetForwardDirection(Quaternion)"/>
	public static Vector3 GetUpDirection(this Quaternion a)
	{
		return a * Vector3.up;
	}

	/// <inheritdoc cref="GetForwardDirection(Quaternion)"/>
	public static Vector3 GetDownDirection(this Quaternion a)
	{
		return a * Vector3.down;
	}

	/// <inheritdoc cref="GetForwardDirection(Quaternion)"/>
	public static Vector3 GetRightDirection(this Quaternion a)
	{
		return a * Vector3.right;
	}

	/// <inheritdoc cref="GetForwardDirection(Quaternion)"/>
	public static Vector3 GetLeftDirection(this Quaternion a)
	{
		return a * Vector3.left;
	}

	public static Quaternion SubtractFrom(this Quaternion a, Quaternion b)
	{
		return b * Quaternion.Inverse(a);
	}
}