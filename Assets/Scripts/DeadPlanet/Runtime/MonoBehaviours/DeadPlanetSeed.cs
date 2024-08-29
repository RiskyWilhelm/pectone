using UnityEngine;

public sealed partial class DeadPlanetSeed : MonoBehaviour, ISaveDataRequester<DeadPlanetSeedData>
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
    private float growSpeed;

    [SerializeField]
    private float desiredGrowRadius;

	[SerializeField]
	private float currentGrowRadius;

	private bool isPlanted;


	#endregion

	[Header("DeadPlanetSeed Data")]
	#region DeadPlanetSeed Data

	[SerializeField]
	private SavedInstantiation selfSave;


	#endregion


	// Update
	private void Update()
	{
		if (isPlanted)
			SetCurrentGrowRadius(Mathf.MoveTowards(currentGrowRadius, desiredGrowRadius, growSpeed * Time.deltaTime));

		OverrideCurrentData();
	}

	private void SetCurrentGrowRadius(float value)
	{
		currentGrowRadius = value;
		selfGrowObject.localScale = (Vector3.one * currentGrowRadius);
	}

	private void SaveToCollidedGround(SavedInstantiation saveRoot)
	{
		selfRigidbody.isKinematic = true;
		isPlanted = true;

		if (!selfSave || selfSave.ParentHandler)
			return;

		selfSave.AttachToHandler(saveRoot);
		SaveDataFileControllerSingleton.Instance.SaveToFile();
	}

	public void OnGroundCollisionEnter(Collision other)
	{
		if ((other.collider.gameObject.layer is Layers.Ground) && EventReflectorUtils.TryGetComponentByEventReflector<SavedInstantiation>(other.collider.gameObject, out SavedInstantiation found))
			SaveToCollidedGround(found);
	}

	public void OverrideCurrentData()
	{
		if (!selfSave)
			return;

		selfSave.Data.instantiationParams = new(this.transform.position, this.transform.rotation);

		//selfSave.InnerData.currentGrowRadius = 5f;
	}

	// WARNING: Support implementation for custom Events
	public void OnLastDataLoaded(DeadPlanetSeedData loadedData)
	{
		SetCurrentGrowRadius(loadedData.currentGrowRadius);
		selfRigidbody.isKinematic = true;
		isPlanted = true;
	}
}


#if UNITY_EDITOR

public sealed partial class DeadPlanetSeed
{ }


#endif
