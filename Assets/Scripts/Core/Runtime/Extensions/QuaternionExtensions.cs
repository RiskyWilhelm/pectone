using System;
using UnityEngine;

[Flags]
public enum AcceptedRotationDirectionAxisType
{
	None = 0,

	X = 1 << 0,

	Y = 1 << 1,

	Z = 1 << 2,

	All = ~(-1 << 3),
}

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

	/// <summary> Subtracts A using B’s local coords (a - b) </summary>
	public static Quaternion Subtract(this Quaternion a, Quaternion b)
	{
		return a * Quaternion.Inverse(b);
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

	public static Quaternion RotateToDirection(this Quaternion current, Vector3 direction, Vector3 upwards, AcceptedRotationDirectionAxisType acceptedRotationDirectionAxisType = AcceptedRotationDirectionAxisType.All, float powerDelta = 360f)
	{
		var newRotation = Quaternion.LookRotation(direction, upwards);

		// Allow only specific axis
		if (!acceptedRotationDirectionAxisType.HasFlag(AcceptedRotationDirectionAxisType.X))
			newRotation = Quaternion.FromToRotation(newRotation.GetRightDirection(), current.GetRightDirection()) * newRotation;

		if (!acceptedRotationDirectionAxisType.HasFlag(AcceptedRotationDirectionAxisType.Y))
			newRotation = Quaternion.FromToRotation(newRotation.GetUpDirection(), current.GetUpDirection()) * newRotation;

		if (!acceptedRotationDirectionAxisType.HasFlag(AcceptedRotationDirectionAxisType.Z))
			newRotation = Quaternion.FromToRotation(newRotation.GetForwardDirection(), current.GetForwardDirection()) * newRotation;

		return Quaternion.RotateTowards(current, newRotation, powerDelta);
	}

	public static Quaternion RotateToDirection(this Quaternion current, Vector3 normalizedDirection, AcceptedRotationDirectionAxisType acceptedRotationDirectionAxisType = AcceptedRotationDirectionAxisType.All, float powerDelta = 360f)
		=> current.RotateToDirection(normalizedDirection, Vector3.up, acceptedRotationDirectionAxisType, powerDelta);
}