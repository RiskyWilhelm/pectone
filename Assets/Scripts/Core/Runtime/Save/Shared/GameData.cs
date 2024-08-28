using Newtonsoft.Json;
using System.Collections.Generic;

public sealed class GameData : SaveDataBase
{
	[JsonProperty(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
	public Dictionary<GuidSerializable, InstantiationData> rootInstantiationDatasDict = new();

	[JsonProperty(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
	public Dictionary<GuidSerializable, SaveDataBase> globalDatasDict = new();


	// Update
	public override void Copy(in object other)
	{
		if (other is GameData same)
		{
			this.rootInstantiationDatasDict = new (same.rootInstantiationDatasDict);
			this.globalDatasDict = new (same.globalDatasDict);
		}
	}
}