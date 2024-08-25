using UnityEngine;

public sealed partial class DeadPlanetSeed : MonoBehaviour
{
	[Header("DeadPlanetSeed Movement")]
	#region DeadPlanetSeed Movement

	[SerializeField]
	private Rigidbody selfRigidbody;


	#endregion

	[Header("DeadPlanetSeed Grow")]
	#region DeadPlanetSeed Grow

	[SerializeField]
	private Transform selfGrowObject;

	[SerializeField]
	private SavedInstantiation selfSavedInstantiation;

	[SerializeField]
    private float growSpeed;

    [SerializeField]
    private float desiredGrowRadius;

	[SerializeField]
	private float currentGrowRadius;

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

	private void SaveToCollidedGround(SavedInstantiation saveRoot)
	{
		if (selfSavedInstantiation.ParentHandler)
			return;

		selfSavedInstantiation.AttachToHandler(saveRoot);
		selfSavedInstantiation.OverrideInnerDataTypeAs<DeadPlanetSeedData>();
		selfSavedInstantiation.Data.instantiationParams = new(this.transform.position, this.transform.rotation);

		selfRigidbody.isKinematic = true;
		isPlanted = true;

		// DEBUG
		GameDataControllerSingleton.Instance.SaveToFile();
	}

	// WARNING: Support implementation for custom Events
	public void OnInstantiatedWithLastSave()
	{
		var seedData = selfSavedInstantiation.GetInnerDataAs<DeadPlanetSeedData>();
		SetCurrentGrowRadius(seedData.currentGrowRadius);

		selfRigidbody.isKinematic = true;
		isPlanted = true;
	}

	public void OnGroundCollisionEnter(Collision other)
	{
		if ((other.collider.gameObject.layer is Layers.Ground) && EventReflectorUtils.TryGetComponentByEventReflector<SavedInstantiation>(other.collider.gameObject, out SavedInstantiation found))
			SaveToCollidedGround(found);
	}
}


#if UNITY_EDITOR

public sealed partial class DeadPlanetSeed
{ }


#endif
