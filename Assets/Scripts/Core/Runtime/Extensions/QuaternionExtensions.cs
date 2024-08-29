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
	/// <returns> Non-normalized local direction </returns>
	public static Vector3 ForwardDirection(this Quaternion a)
	{
		return a * Vector3.forward;
	}

	/// <inheritdoc cref="ForwardDirection(Quaternion)"/>
	public static Vector3 BackDirection(this Quaternion a)
	{
		return a * Vector3.back;
	}

	/// <inheritdoc cref="ForwardDirection(Quaternion)"/>
	public static Vector3 UpDirection(this Quaternion a)
	{
		return a * Vector3.up;
	}

	/// <inheritdoc cref="ForwardDirection(Quaternion)"/>
	public static Vector3 DownDirection(this Quaternion a)
	{
		return a * Vector3.down;
	}

	/// <inheritdoc cref="ForwardDirection(Quaternion)"/>
	public static Vector3 RightDirection(this Quaternion a)
	{
		return a * Vector3.right;
	}

	/// <inheritdoc cref="ForwardDirection(Quaternion)"/>
	public static Vector3 LeftDirection(this Quaternion a)
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

		var newForward = a.ForwardDirection();

		if (preventForwardInvert)
			newForward = Vector3.ProjectOnPlane(newForward, direction).normalized;

		var finalRotation = Quaternion.LookRotation(newForward, direction);
		return Quaternion.RotateTowards(a, finalRotation, powerDelta);
	}

	public static Quaternion RotateTowardsDirection(this Quaternion current, Vector3 direction, Vector3 upwards, AcceptedRotationDirectionAxisType acceptedRotationDirectionAxisType = AcceptedRotationDirectionAxisType.All, float powerDelta = 360f)
	{
		var newRotation = Quaternion.LookRotation(direction, upwards);

		// Allow only specific axis
		if (!acceptedRotationDirectionAxisType.HasFlag(AcceptedRotationDirectionAxisType.X))
			newRotation = Quaternion.FromToRotation(newRotation.RightDirection(), current.RightDirection()) * newRotation;

		if (!acceptedRotationDirectionAxisType.HasFlag(AcceptedRotationDirectionAxisType.Y))
			newRotation = Quaternion.FromToRotation(newRotation.UpDirection(), current.UpDirection()) * newRotation;

		if (!acceptedRotationDirectionAxisType.HasFlag(AcceptedRotationDirectionAxisType.Z))
			newRotation = Quaternion.FromToRotation(newRotation.ForwardDirection(), current.ForwardDirection()) * newRotation;

		return Quaternion.RotateTowards(current, newRotation, powerDelta);
	}

	public static Quaternion RotateTowardsDirection(this Quaternion current, Vector3 normalizedDirection, AcceptedRotationDirectionAxisType acceptedRotationDirectionAxisType = AcceptedRotationDirectionAxisType.All, float powerDelta = 360f)
		=> current.RotateTowardsDirection(normalizedDirection, Vector3.up, acceptedRotationDirectionAxisType, powerDelta);
}