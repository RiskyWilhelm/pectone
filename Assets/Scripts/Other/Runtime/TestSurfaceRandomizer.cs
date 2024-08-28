using UnityEngine;

public sealed partial class TestSurfaceRandomizer : MonoBehaviour
{
    public bool generate;

    public Vector3 currentRandomPoint;

	public Collider colliderToCheck;


	// Initialize


	// Update
	private void Update()
	{
		if (generate)
		{
			currentRandomPoint = colliderToCheck.GetRandomPointAtSurface();
			generate = false;
		}

		Debug.DrawLine(this.transform.position, currentRandomPoint, Color.red);
	}


	// Dispose
}


#if UNITY_EDITOR

public sealed partial class TestSurfaceRandomizer
{ }


#endif
