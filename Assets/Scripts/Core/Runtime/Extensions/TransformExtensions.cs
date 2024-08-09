using System;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
	public static Vector3 TransformPointUnscaled(this Transform transform, Vector3 positionToMultiply)
	{
		return TransformUtils.VectorPoint(transform.position, transform.rotation, Vector3.one, positionToMultiply);
	}

	public static Vector3 RigidbodyPoint(this Rigidbody rigidbody, Vector3 positionToMultiply)
	{
		return TransformUtils.VectorPoint(rigidbody.position, rigidbody.rotation, rigidbody.transform.lossyScale, positionToMultiply);
	}

	public static Vector3 RigidbodyPointUnscaled(this Rigidbody rigidbody, Vector3 positionToMultiply)
	{
		return TransformUtils.VectorPoint(rigidbody.position, rigidbody.rotation, Vector3.one, positionToMultiply);
	}

	public static Vector3 InverseTransformPointUnscaled(this Transform transform, Vector3 positionToMultiply)
	{
		return TransformUtils.InverseVectorPoint(transform.position, transform.rotation, Vector3.one, positionToMultiply);
	}

	public static Vector3 InverseRigidbodyPoint(this Rigidbody rigidbody, Vector3 positionToMultiply)
	{
		return TransformUtils.InverseVectorPoint(rigidbody.position, rigidbody.rotation, rigidbody.transform.lossyScale, positionToMultiply);
	}

	public static Vector3 InverseRigidbodyPointUnscaled(this Rigidbody rigidbody, Vector3 positionToMultiply)
	{
		return TransformUtils.InverseVectorPoint(rigidbody.position, rigidbody.rotation, Vector3.one, positionToMultiply);
	}
	
	public static bool TryGetNearestTransform<TransformEnumeratorType>(this Transform relativeTo, TransformEnumeratorType transformEnumerator, out Transform nearestTransform, Predicate<Transform> predicateNearest = null)
		where TransformEnumeratorType : IEnumerator<Transform>
	{
		nearestTransform = default;

		var isFoundNearest = false;
		float nearestHorizontalDistance = float.MaxValue;
		float iteratedDistance;

		// Check sqr distances and select nearest
		foreach (var iteratedTransform in transformEnumerator)
		{
			iteratedDistance = (iteratedTransform.position - relativeTo.position).sqrMagnitude;

			if ((iteratedDistance < nearestHorizontalDistance) && (predicateNearest == null || predicateNearest.Invoke(iteratedTransform)))
			{
				nearestTransform = iteratedTransform;
				nearestHorizontalDistance = iteratedDistance;
				isFoundNearest = true;
			}
		}

		return isFoundNearest;
	}
}
