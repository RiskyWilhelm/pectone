using System;
using UnityEngine;

public static class QuaternionExtensions
{
	/// <returns> Non-normalized direction vector in world up </returns>
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

	/// <summary> Subtracts B using A’s local coords </summary>
	public static Quaternion SubtractFrom(this Quaternion a, Quaternion b)
	{
		return b * Quaternion.Inverse(a);
	}

	/// <param name="preventForwardInvert"> Sometimes the forward is inverted due to quaternions slerping to nearest behaviour. Prevents that and keeps the forward exactly as before </param>
	public static Quaternion EqualizeUpRotationWithDirection(this Quaternion a, Vector3 direction, bool preventForwardInvert = true, float powerDelta = 360f)
	{
		if (powerDelta == 0f)
			return a;

		var newForward = a.GetForwardDirection();

		if (preventForwardInvert)
			newForward = Vector3.ProjectOnPlane(newForward, direction).normalized;

		var finalRotation = Quaternion.LookRotation(newForward, direction);
		return Quaternion.RotateTowards(a, finalRotation, powerDelta);
	}
}