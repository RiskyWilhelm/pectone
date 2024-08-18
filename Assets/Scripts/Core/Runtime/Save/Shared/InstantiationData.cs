using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

public class InstantiationData : SaveData
{
	[JsonProperty]
	public AssetReference instantiationAssetReference;

	[JsonProperty]
	public InstantiationParametersSerializable instantiationParameters;

	[JsonProperty(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
	public Dictionary<GuidSerializable, InstantiationData> childInstantiationDatasDict = new();


	// Initialize
	public InstantiationData()
	{ }

	public InstantiationData(AssetReferenceGameObject instantiationAssetReference, InstantiationParametersSerializable instantiationParameters)
	{
		this.instantiationAssetReference = instantiationAssetReference;
		this.instantiationParameters = instantiationParameters;
	}
}