using Newtonsoft.Json;
using System;

[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public class InstantiationDataT<DataType> : InstantiationData, ICopyable<InstantiationDataT<DataType>>
	where DataType : SaveDataBase, new()
{
	[JsonProperty]
	public DataType innerData = new();


	// Initialize
	public InstantiationDataT()
	{ }


	// Update
	public override void Copy(in object other)
	{
		if (other is InstantiationDataT<DataType> same)
			Copy(same);
		else
			base.Copy(other);
	}

	public void Copy(in InstantiationDataT<DataType> other)
	{
		innerData.Copy(other.innerData);
		base.Copy(other);
	}
}