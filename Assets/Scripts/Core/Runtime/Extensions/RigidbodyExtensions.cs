using UnityEngine;

public static partial class RigidbodyExtensions
{
	/// <summary> Checks if rigidbody <b>exactly</b> moving </summary>
	public static bool IsMoving(this Rigidbody rigidbody)
	{
		return rigidbody.linearVelocity.sqrMagnitude != 0;
	}

	/// <summary> Checks if rigidbody <b>exactly</b> rotating </summary>
	public static bool IsRotating(this Rigidbody rigidbody)
	{
		return rigidbody.angularVelocity.sqrMagnitude != 0;
	}

	/// <summary> Checks if rigidbody <b>exactly</b> moving or rotating </summary>
	public static bool IsMovingOrRotating(this Rigidbody rigidbody)
	{
		return IsMoving(rigidbody) || IsRotating(rigidbody);
	}

	/// <summary> Checks if rigidbody moving more than a <b>threshold</b> </summary>
	/// <param name="allowedDifference"> Default value is <see cref="Vector3.kEpsilon"/> </param>
	public static bool IsMovingApproximately(this Rigidbody rigidbody, float allowedDifference = Vector3.kEpsilon)
	{
		return !VectorExtensions.Approximately(rigidbody.linearVelocity, Vector3.zero, allowedDifference);
	}

	/// <summary> Checks if rigidbody rotating more than a <b>threshold</b> </summary>
	/// <param name="allowedDifference"> Default value is <see cref="Vector3.kEpsilon"/> </param>
	public static bool IsRotatingApproximately(this Rigidbody rigidbody, float allowedDifference = Vector3.kEpsilon)
	{
		return !VectorExtensions.Approximately(rigidbody.angularVelocity, Vector3.zero, allowedDifference);
	}

	/// <summary> Checks if rigidbody moving or rotating more than a <b>threshold</b> </summary>
	/// <param name="allowedDifference"> Default value is <see cref="Vector3.kEpsilon"/> </param>
	public static bool IsMovingOrRotatingApproximately(this Rigidbody rigidbody, float allowedDifference = Vector3.kEpsilon)
	{
		return IsMovingApproximately(rigidbody, allowedDifference) || IsRotatingApproximately(rigidbody, allowedDifference);
	}
}