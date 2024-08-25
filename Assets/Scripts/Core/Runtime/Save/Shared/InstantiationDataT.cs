using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class InstantiationDataT<DataType> : InstantiationData, ICopyable<InstantiationDataT<DataType>>
	where DataType : SaveData, ICopyable<DataType>, new()
{
	[JsonProperty]
	public DataType innerData = new();


	// Initialize
	public InstantiationDataT()
	{ }


	// Update
	public void Copy(in InstantiationDataT<DataType> other)
	{
		innerData.Copy(other.innerData);
		base.Copy(other);
	}
}