using UnityEngine;

public sealed partial class SavedInstantiationRoot : SavedInstantiation
{
	// Initialize
	private void OnEnable()
	{
		if (UnityObjectUtils.IsInstantiatedInRuntime(this))
		{
			Debug.LogError("You cannot instantiate root!");
			Destroy(this.gameObject);
			return;
		}

		Initialize();
	}

	public void Initialize()
	{
		var isFoundLastSave = GameDataControllerSingleton.Data.instantiationDatasDict.TryGetValue(_guid, out _data);
		if (!isFoundLastSave)
		{
			_data = new ();
			GameDataControllerSingleton.Data.instantiationDatasDict.Add(_guid, _data);
		}

		InstantiateLastChildren();
	}
}


#if UNITY_EDITOR

public sealed partial class SavedInstantiationRoot
{ }


#endif
