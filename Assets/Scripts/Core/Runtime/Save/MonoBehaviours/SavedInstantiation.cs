using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[DisallowMultipleComponent]
public partial class SavedInstantiation : MonoBehaviourSaveDataControllerBase<InstantiationData>
{
	[Header("SavedInstantiation Data")]
	#region SavedInstantiation Data

	protected SavedInstantiation _parentHandler;

	protected AsyncOperationHandle<GameObject> _addressableHandle;

	public SavedInstantiation ParentHandler
		=> _parentHandler;

	public AsyncOperationHandle<GameObject> AddressableHandle
		=> _addressableHandle;


	#endregion

	#region SavedInstantiation Other

	protected virtual bool IsAbleToGetAttached
		=> true;


	#endregion


	// Initialize
	protected override void Awake()
	{ }

	protected override void LoadFromGameData()
	{
		if (_isLoadedData)
			return;

		InstantiateLastChildren();
		_isLoadedData = true;
		onLoadedLastData?.Invoke(_data);
	}

	protected void InstantiateLastChildren()
	{
		foreach (var iteratedChildData in _data.childInstantiationDatasDict)
		{
			var childGuid = iteratedChildData.Key;
			var childData = iteratedChildData.Value;

			var handle = Addressables.InstantiateAsync(childData.instantiationAssetReference, childData.instantiationParams.worldPosition, childData.instantiationParams.worldRotation, this.transform, true);
			handle.Completed +=
				(handle) => InitializeInstantiated(handle, childData, childGuid);
		}
	}

	private void InitializeInstantiated(AsyncOperationHandle<GameObject> handle, InstantiationData data, GuidSerializable gameDataGuid)
	{
		var isSucceeded = (handle.Status == AsyncOperationStatus.Succeeded);
		var isInstantiatedCorrect = handle.Result.TryGetComponent<SavedInstantiation>(out SavedInstantiation instantiated);

		if (!isSucceeded || !isInstantiatedCorrect)
		{
			Debug.LogErrorFormat("AssetReference not contains {0} or somehow handle is not succeeded", nameof(SavedInstantiation));
			handle.Release();
			return;
		}

		instantiated._gameDataGuid = gameDataGuid;
		instantiated._data = data;
		instantiated._addressableHandle = handle;
		instantiated.AttachToHandler(this);
		instantiated.LoadFromGameData();
	}


	// Update
	public void AttachToHandler(SavedInstantiation newParentHandler)
	{
		if (!IsAbleToGetAttached)
		{
			Debug.LogWarningFormat("You cannot attach {0} to any handler", this.GetType());
			return;
		}

		RemoveFromGameData();
		_parentHandler = newParentHandler;

		// WARNING: TryAdd() giving error due to foreach loop when instantiating last children
		if (newParentHandler)
		{
			UpdateGameData();
			this.transform.SetParent(newParentHandler.transform, true);
		}
		else
			this.transform.SetParent(null);
	}

	public override void UpdateGameData()
	{
		if (_parentHandler)
			_parentHandler._data.childInstantiationDatasDict[_gameDataGuid] = _data;
	}

	public override void RemoveFromGameData()
	{
		if (_parentHandler)
			_parentHandler._data.childInstantiationDatasDict.Remove(_gameDataGuid);
	}
}


#if UNITY_EDITOR

public partial class SavedInstantiation
{ }


#endif
