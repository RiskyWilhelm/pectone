using Newtonsoft.Json;
using System;

[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public sealed class DeadPlanetSeedData : SaveDataBase, ICopyable<DeadPlanetSeedData>
{
	[JsonProperty]
	public float currentGrowRadius;


	// Initialize
	public DeadPlanetSeedData()
	{ }



	// Update
	public override void Copy(in object other)
	{
		if (other is DeadPlanetSeedData same)
			Copy(same);
	}

	public void Copy(in DeadPlanetSeedData other)
	{
		currentGrowRadius = other.currentGrowRadius;
	}
}