using Newtonsoft.Json;
using System.Collections.Generic;

public sealed class GameData : SaveData
{
	[JsonProperty(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
	public Dictionary<GuidSerializable, InstantiationData> rootInstantiationDatasDict = new();
}