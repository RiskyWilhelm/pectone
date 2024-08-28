using UnityEngine;

public static class SphereColliderExtensions
{
	public static Vector3 GetRandomPoint(this SphereCollider a)
	{
		var insideUnitSphereUniformly = Random.insideUnitSphere * Mathf.Sqrt(Random.Range(0.0f, 1.0f));
		Vector3 localPoint = insideUnitSphereUniformly * a.radius;
		localPoint += a.center;
		return a.transform.TransformPoint(localPoint);
	}

	public static Vector3 GetRandomPointAtSurface(this SphereCollider a)
	{
		Vector3 localPoint = Random.onUnitSphere * a.radius;
		localPoint += a.center;
		return a.transform.TransformPoint(localPoint);
	}
}