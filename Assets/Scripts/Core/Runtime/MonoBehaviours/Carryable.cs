using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public sealed partial class Carryable : MonoBehaviour
{
	[Header("Carryable Carry")]
	#region Carryable Carry

	public List<CarryierType> acceptedCarryiersList = new();

	[field: SerializeField]
	public CarryableType CType
	{ get; private set; }

	public Carryier CurrentCarryier
	{ get; internal set; }


	#endregion

	[Header("Carryable Events")]
	#region Carryable Events

	[SerializeField]
	internal UnityEvent<Carryier> onGotGrabbed = new();

	[SerializeField]
	internal UnityEvent<Carryier> onGotUnGrabbed = new();


	#endregion


	// Update
	public bool TryGetCarriedBy(Carryier requester)
	{
		return requester.TryCarryCarryable(this);
	}

	public void GetUnCarriedFromCurrent()
	{
		if (CurrentCarryier)
			CurrentCarryier.UnCarryCarryable(this);
	}

	public bool IsAbleToGetCarriedBy(Carryier requester)
	{
		return requester.IsAbleToCarryCarryable(this);
	}


	// Dispose
	private void OnDisable()
	{
		GetUnCarriedFromCurrent();
	}
}


#if UNITY_EDITOR

public sealed partial class Carryable
{ }

#endif