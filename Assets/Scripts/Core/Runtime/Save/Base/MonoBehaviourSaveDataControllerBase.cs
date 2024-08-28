using System;
using UnityEngine;
using UnityEngine.Events;

public abstract partial class MonoBehaviourSaveDataControllerBase<ControlledDataType> : MonoBehaviour
	where ControlledDataType : SaveDataBase, new()
{
	[Header("SaveDataController Data")]
	#region SaveDataController Data

	[SerializeField]
	protected GuidSerializable _gameDataGuid = GuidSerializable.NewGuid();

	[SerializeField]
	protected ControlledDataType _data = new();

	protected bool _isLoadedData;

	public GuidSerializable GameDataGuid
		=> _gameDataGuid;

	public ControlledDataType Data
	{
		get => _data;
		set
		{
			_data = value;
			UpdateGameData();
		}
	}

	public bool IsLoadedData
	{
		get => _isLoadedData;
		private set => _isLoadedData = value;
	}


	#endregion

	[Header("SaveDataController Events")]
	#region SaveDataController Events

	public UnityEvent<ControlledDataType> onLoadedLastData = new();


	#endregion


	// Initialize
	protected virtual void Awake()
	{
		LoadFromGameData();
	}

	protected virtual void LoadFromGameData()
	{
		if (_isLoadedData)
			return;

		_isLoadedData = true;
		var isFoundLastSave = GameDataControllerSingleton.Data.globalDatasDict.TryGetValue(_gameDataGuid, out SaveDataBase found);
		if (isFoundLastSave)
		{
			_data = (found as ControlledDataType);
			onLoadedLastData?.Invoke(_data);
		}

		UpdateGameData();
	}


	// Update
	public virtual void UpdateGameData()
	{
		GameDataControllerSingleton.Data.globalDatasDict[_gameDataGuid] = _data;
	}

	public virtual void RemoveFromGameData()
	{
		GameDataControllerSingleton.Data.globalDatasDict.Remove(_gameDataGuid);
	}

	public DataType GetDataAs<DataType>(bool updateDataType = true)
		where DataType : ControlledDataType, new()
	{
		if (_data is DataType convertedData)
			return convertedData;

		if (updateDataType)
		{
			convertedData = new DataType();
			convertedData.Copy(_data);
			return convertedData;
		}

		throw new ArgumentException($"data type '{_data.GetType()}' is not equal with '{typeof(DataType)}'");
	}
}


#if UNITY_EDITOR

public abstract partial class MonoBehaviourSaveDataControllerBase<ControlledDataType>
{ }


#endif
