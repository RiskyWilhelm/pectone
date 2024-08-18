using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public partial class SavedInstantiation : MonoBehaviour
{
	#region SavedInstantiation Data

	[HideInInspector]
	[SerializeField]
	protected GuidSerializable _guid = GuidSerializable.NewGuid();

	protected SavedInstantiation _handler;

	protected InstantiationData _data;

	public GuidSerializable Guid
		=> _guid;

	public SavedInstantiation Handler
		=> _handler;

	public InstantiationData Data
		=> _data;


	#endregion


	// Initialize
	public void InstantiateLastChildren()
	{
		foreach (var iteratedChildData in _data.childInstantiationDatasDict)
			InstantiateSavedAsync(iteratedChildData.Value, guid: iteratedChildData.Key);
	}

	public void Initialize(SavedInstantiation handler, GuidSerializable guid, InstantiationData data)
	{
		if (_handler && (_data != null))
			return;

		if (!handler || (data == null))
			throw new ArgumentNullException("Cannot initialize with null values");

		if (guid == GuidSerializable.Empty)
			guid = GuidSerializable.NewGuid();

		_guid = guid;
		_handler = handler;
		_data = data;

		// WARNING: TryAdd is giving error due to foreach loop when instantiating last children
		if (!handler.Data.childInstantiationDatasDict.ContainsKey(guid))
			handler.Data.childInstantiationDatasDict.Add(guid, _data);

		InstantiateLastChildren();
	}


	// Update
	public AsyncOperationHandle<GameObject> InstantiateSavedAsync(InstantiationData data, GuidSerializable guid = default, bool trackHandle = true)
	{
		var handle = Addressables.InstantiateAsync(data.instantiationAssetReference, data.instantiationParameters, trackHandle);
		
		handle.Completed +=
			(handle) => InitializeInstantiated(handle, data, guid);

		return handle;
	}

	private void InitializeInstantiated(AsyncOperationHandle<GameObject> handle, InstantiationData data, GuidSerializable guid)
	{
		var isSucceeded = (handle.Status == AsyncOperationStatus.Succeeded);
		var isInstantiatedCorrect = handle.Result.TryGetComponent<SavedInstantiation>(out SavedInstantiation instantiated);

		if (!isSucceeded || !isInstantiatedCorrect)
		{
			Debug.LogErrorFormat("AssetReference not contains {0} or somehow handle is not succeeded", nameof(SavedInstantiation));
			handle.Release();
			return;
		}

		instantiated.Initialize(this, guid, data);
	}
}


#if UNITY_EDITOR

public partial class SavedInstantiation
{ }


#endif
