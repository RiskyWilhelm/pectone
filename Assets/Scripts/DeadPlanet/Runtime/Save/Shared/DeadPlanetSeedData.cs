using Newtonsoft.Json;
using System;

[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public sealed class DeadPlanetSeedData : SaveData, ICopyable<DeadPlanetSeedData>
{
	[JsonProperty]
	public float currentGrowRadius;


	// Initialize
	public DeadPlanetSeedData()
	{ }


	// Update
	public void Copy(in DeadPlanetSeedData other)
	{
		currentGrowRadius = other.currentGrowRadius;
	}
}