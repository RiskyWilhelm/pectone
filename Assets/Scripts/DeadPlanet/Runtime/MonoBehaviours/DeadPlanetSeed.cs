using UnityEngine;

public sealed partial class DeadPlanetSeed : MonoBehaviour
{
	[Header("DeadPlanetSeed Movement")]
	#region DeadPlanetSeed Movement

	[SerializeField]
	private Rigidbody _selfRigidbody;

	public Rigidbody SelfRigidbody
		=> _selfRigidbody;


	#endregion

	[Header("DeadPlanetSeed Grow")]
	#region DeadPlanetSeed Grow

	[SerializeField]
	private Transform selfGrowObject;

	[SerializeField]
    private float growSpeed;

    [SerializeField]
    private float desiredGrowRadius;

	[SerializeField]
	private float currentGrowRadius;

	[SerializeField]
	private bool isPlanted;


	#endregion


	// Update
	private void Update()
	{
		if (isPlanted)
			SetCurrentGrowRadius(Mathf.MoveTowards(currentGrowRadius, desiredGrowRadius, growSpeed * Time.deltaTime));
	}

	private void SetCurrentGrowRadius(float value)
	{
		currentGrowRadius = value;
		selfGrowObject.localScale = (Vector3.one * currentGrowRadius);
	}
}


#if UNITY_EDITOR

public sealed partial class DeadPlanetSeed
{ }


#endif
