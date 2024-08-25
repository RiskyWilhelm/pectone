using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

[DisallowMultipleComponent]
public partial class SavedInstantiation : MonoBehaviour
{
	[Header("SavedInstantiation Events")]
	#region SavedInstantiation Events

	public UnityEvent onInitializedWithLastSave = new();


	#endregion

	#region SavedInstantiation Data

	[HideInInspector]
	[SerializeField]
	protected GuidSerializable _guid = GuidSerializable.NewGuid();

	protected SavedInstantiation _parentHandler;

	[SerializeField]
	protected InstantiationData _data = new();

	protected AsyncOperationHandle<GameObject> asyncHandle;

	public GuidSerializable Guid
		=> _guid;

	public SavedInstantiation ParentHandler
		=> _parentHandler;

	public InstantiationData Data
	{
		get => _data;
		set
		{
			_data = value;

			// Update data globally
			if (_parentHandler && _parentHandler._data.childInstantiationDatasDict.ContainsKey(_guid))
				_parentHandler._data.childInstantiationDatasDict[_guid] = _data;
		}
	}


	#endregion

	#region SavedInstantiation Other

	protected virtual bool CanGetAttachedToHandler
		=> true;


	#endregion

	// Initialize
	public void InstantiateLastChildren()
	{
		foreach (var iteratedChildData in _data.childInstantiationDatasDict)
			InstantiateSavedAsync(iteratedChildData.Value, iteratedChildData.Key);
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

		if (guid == GuidSerializable.Empty)
			guid = GuidSerializable.NewGuid();

		instantiated._guid = guid;
		instantiated._data = data;
		instantiated.asyncHandle = handle;
		instantiated.AttachToHandler(this);
		instantiated.InstantiateLastChildren();
		instantiated.onInitializedWithLastSave?.Invoke();
	}


	// Update
	public void AttachToHandler(SavedInstantiation newParentHandler)
	{
		if (!CanGetAttachedToHandler)
		{
			Debug.LogWarningFormat("You cannot attach {0} to any handler", this.GetType());
			return;
		}

		if (_parentHandler)
			_parentHandler._data.childInstantiationDatasDict.Remove(_guid);

		// WARNING: TryAdd() giving error due to foreach loop when instantiating last children
		if (newParentHandler)
		{
			if (!newParentHandler._data.childInstantiationDatasDict.ContainsKey(_guid))
				newParentHandler._data.childInstantiationDatasDict.Add(_guid, _data);

			this.transform.SetParent(newParentHandler.transform, true);
		}
		else
			this.transform.SetParent(null);

		_parentHandler = newParentHandler;
	}

	private AsyncOperationHandle<GameObject> InstantiateSavedAsync(InstantiationData data, GuidSerializable guid)
	{
		var handle = Addressables.InstantiateAsync(data.instantiationAssetReference, data.instantiationParams.worldPosition, data.instantiationParams.worldRotation, this.transform, true);
		handle.Completed +=
			(handle) => InitializeInstantiated(handle, data, guid);

		return handle;
	}

	/// <param name="updateDataType"> Last <see cref="InstantiationDataT{DataType}.innerData"/> will be gone forever if this method decides to convert </param>
	public DataType GetDataAs<DataType>(bool updateDataType = true)
		where DataType : InstantiationData, new()
	{
		if (_data is DataType convertedData)
			return convertedData;

		if (updateDataType)
			return OverrideDataTypeAs<DataType>();

		throw new ArgumentException($"data type '{_data.GetType()}' is not equal with '{typeof(DataType)}'");
	}

	/// <inheritdoc cref="GetDataAs{DataType}(bool)"/>
	public InnerDataType GetInnerDataAs<InnerDataType>(bool updateDataType = true)
		where InnerDataType : SaveData, ICopyable<InnerDataType>, new()
	{
		return GetDataAs<InstantiationDataT<InnerDataType>>(updateDataType).innerData;
	}

	/// <remarks> If last data type was <see cref="InstantiationDataT{DataType}"/>, last <see cref="InstantiationDataT{DataType}.innerData"/> will be gone forever if this method decides to convert </remarks>
	public NewDataType OverrideDataTypeAs<NewDataType>()
		where NewDataType : InstantiationData, new()
	{
		if (_data.GetType() == typeof(NewDataType))
			return _data as NewDataType;

		var lastInstantiationData = _data;
		var newInstantiationData = new NewDataType();
		newInstantiationData.Copy(lastInstantiationData);
		Data = newInstantiationData;

		return newInstantiationData;
	}

	/// <remarks>
	/// If current data is just <see cref="InstantiationData"/>, updates the whole type with <see cref="InstantiationDataT{DataType}"/> <br/>
	/// If last data type was <see cref="InstantiationDataT{DataType}"/>, last <see cref="InstantiationDataT{DataType}.innerData"/> will be gone forever if this method decides to convert
	/// </remarks>
	public NewInnerDataType OverrideInnerDataTypeAs<NewInnerDataType>()
		where NewInnerDataType : SaveData, ICopyable<NewInnerDataType>, new()
	{
		return OverrideDataTypeAs<InstantiationDataT<NewInnerDataType>>().innerData;
	}


	// Dispose
	public virtual void DestroyWithSave()
	{
		if (_parentHandler)
			_parentHandler._data.childInstantiationDatasDict.Remove(_guid);

		if (asyncHandle.IsValid())
			asyncHandle.Release();
		else
			Destroy(this.gameObject);
	}
}


#if UNITY_EDITOR

public partial class SavedInstantiation
{ }


#endif
