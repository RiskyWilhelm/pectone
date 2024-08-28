using UnityEngine;

/// <summary> Allows you to instantiate saved instantiations at its hierarchy </summary>
public sealed partial class SavedInstantiationRoot : SavedInstantiation
{
	#region SavedInstantiationRoot Other

	protected override bool IsAbleToGetAttached
		=> false;


	#endregion


	// Initialize
	protected override void Awake()
	{
		if (UObjectUtils.IsInstantiatedInRuntime(this))
		{
			Debug.LogError("You cannot instantiate root!");
			Destroy(this.gameObject);
			return;
		}

		LoadFromGameData();
	}

	protected override void LoadFromGameData()
	{
		if (_isLoadedData)
			return;

		var isFoundLastSave = GameDataControllerSingleton.Data.rootInstantiationDatasDict.TryGetValue(_gameDataGuid, out InstantiationData found);
		if (isFoundLastSave)
			_data = found;

		UpdateGameData();
		InstantiateLastChildren();
		_isLoadedData = true;
		onLoadedLastData?.Invoke(_data);
	}


	// Update
	public override void UpdateGameData()
	{
		GameDataControllerSingleton.Data.rootInstantiationDatasDict[_gameDataGuid] = _data;
	}

	public override void RemoveFromGameData()
	{
		GameDataControllerSingleton.Data.rootInstantiationDatasDict.Remove(_gameDataGuid);
	}
}


#if UNITY_EDITOR

public sealed partial class SavedInstantiationRoot
{ }


#endif
