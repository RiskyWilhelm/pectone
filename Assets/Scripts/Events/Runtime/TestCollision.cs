using UnityEngine;

public sealed partial class TestCollision : MonoBehaviour
{
	// Initialize


	// Update
	private void OnCollisionEnter(Collision collision)
	{
		Debug.LogWarning($"Enter Collision {collision.collider.name}", collision.collider);
	}

	private void OnCollisionExit(Collision collision)
	{
		Debug.LogWarning($"Exit Collision {collision.collider.name}", collision.collider);
	}


	// Dispose
}


#if UNITY_EDITOR

public sealed partial class TestCollision
{ }


#endif
