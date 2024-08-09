using UnityEngine;

public static class TransformUtils
{
	/// <summary> Transform point Local to world space </summary>
	public static Vector3 VectorPoint(Vector3 position, Quaternion rotation, Vector3 scale, Vector3 positionToMultiply)
	{
		var localToWorldMatrix = Matrix4x4.TRS(position, rotation, scale);
		return localToWorldMatrix.MultiplyPoint3x4(positionToMultiply);
	}

	/// <summary> Transform point World to local space </summary>
	public static Vector3 InverseVectorPoint(Vector3 position, Quaternion rotation, Vector3 scale, Vector3 positionToMultiply)
	{
		var worldToLocalMatrix = Matrix4x4.TRS(position, rotation, scale).inverse;
		return worldToLocalMatrix.MultiplyPoint3x4(positionToMultiply);
	}
}
