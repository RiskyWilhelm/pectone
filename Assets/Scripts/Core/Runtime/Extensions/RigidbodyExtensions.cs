using UnityEngine;

public static class RigidbodyExtensions
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

	/// <summary> Use when temporary limitation wanted </summary>
	/// <param name="maxVelocity"> If you dont want velocity limit on specific axises, set them to zero </param>
	public static void LimitLinearVelocity(this Rigidbody rigidbody, Vector3 maxVelocity)
	{
		var updatedLinearVelocity = rigidbody.linearVelocity;

		if (maxVelocity.x > 0f)
			updatedLinearVelocity.x = Mathf.Clamp(updatedLinearVelocity.x, -maxVelocity.x, maxVelocity.x);

		if (maxVelocity.y > 0f)
			updatedLinearVelocity.y = Mathf.Clamp(updatedLinearVelocity.y, -maxVelocity.y, maxVelocity.y);

		if (maxVelocity.z > 0f)
			updatedLinearVelocity.z = Mathf.Clamp(updatedLinearVelocity.z, -maxVelocity.z, maxVelocity.z);

		rigidbody.linearVelocity = updatedLinearVelocity;
	}
}