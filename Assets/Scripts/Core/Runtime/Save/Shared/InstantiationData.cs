using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public class InstantiationData : SaveDataBase, ICopyable<InstantiationData>
{
	[JsonProperty]
	[SerializeField]
	public AssetReference instantiationAssetReference;

	[JsonProperty]
	public (Vector3 worldPosition, Quaternion worldRotation) instantiationParams;

	[JsonProperty(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
	public Dictionary<GuidSerializable, InstantiationData> childInstantiationDatasDict = new();


	// Initialize
	public InstantiationData()
	{ }


	// Update
	public override void Copy(in object other)
	{
		if (other is InstantiationData same)
			Copy(same);
	}

	public void Copy(in InstantiationData other)
	{
		if (other.instantiationAssetReference != null)
			this.instantiationAssetReference = new (other.instantiationAssetReference.AssetGUID);

		this.instantiationParams = other.instantiationParams;
		this.childInstantiationDatasDict = new(other.childInstantiationDatasDict);
	}
}