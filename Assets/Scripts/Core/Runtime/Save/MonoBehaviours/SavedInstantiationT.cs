using UnityEngine;
using UnityEngine.Events;

/// <summary> Linking two data(s) in a single instantiation is a headache. This class loads two data at the single time to ensure your data is loaded correctly with the instantiation </summary>
public partial class SavedInstantiationT<InnerDataType> : SavedInstantiation
	where InnerDataType : SaveDataBase, new()
{
	[Header("SavedInstantiationT<InnerDataType> Data")]
	#region SavedInstantiationT<InnerDataType> Data

	[SerializeField]
	protected InnerDataType _innerData = new();

	public InnerDataType InnerData
	{
		get => _innerData;
		set
		{
			_innerData = value;
			var convertedData = GetDataAs<InstantiationDataT<InnerDataType>>();
			convertedData.innerData = _innerData;
			_data = convertedData;
		}
	}


	#endregion

	[Header("SavedInstantiationT<InnerDataType> Events")]
	#region SavedInstantiationT<InnerDataType> Events

	public UnityEvent<InnerDataType> onLoadedLastInnerData = new();


	#endregion


	// Update
	protected override void LoadFromGameData()
	{
		base.LoadFromGameData();
		onLoadedLastInnerData?.Invoke(_innerData);
	}

	public override void UpdateGameData()
	{
		if (_data is InstantiationDataT<InnerDataType> same)
			_innerData = same.innerData;
		else
		{
			var convertedData = GetDataAs<InstantiationDataT<InnerDataType>>();
			convertedData.innerData = _innerData;
			_data = convertedData;
		}

		base.UpdateGameData();
	}
}


#if UNITY_EDITOR

public partial class SavedInstantiationT<InnerDataType>
{ }


#endif
