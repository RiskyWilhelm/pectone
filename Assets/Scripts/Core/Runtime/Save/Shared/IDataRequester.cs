/// <summary> Does nothing in special. Just a contract for the methods </summary>
public interface IDataRequester
{
	public void OverrideCurrentData();
}

/// <inheritdoc cref="IDataRequester"/>
public interface IDataRequester<DataType> : IDataRequester
	where DataType : SaveDataBase
{
	// WARNING: Support implementation for custom events
	public void OnLastDataLoaded(DataType loadedData);
}