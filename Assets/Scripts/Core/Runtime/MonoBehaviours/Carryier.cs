using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

[DisallowMultipleComponent]
public sealed partial class Carryier : MonoBehaviour
{
	public enum PhysicsInteraction
	{
		OnCarryTriggerEnter,
		OnCarryTriggerExit
	}

	[Header("Carryier Carry")]
	#region Carryier Carry

	public int priority;

	public uint maxCarryableCount = 1;

	private readonly HashSet<Carryable> _carriedSet = new();

	private ReadOnlySet<Carryable> _carriedRSet;

	[field: SerializeField]
	public CarryierType CType
	{ get; private set; }

	private HashSet<Carryable> CarriedSet
	{
		get
		{
			_carriedSet.RemoveWhere((x) => !x);
			return _carriedSet;
		}
	}

	public ReadOnlySet<Carryable> CarriedRSet
		=> _carriedRSet ??= new(_carriedSet);

	public bool IsMaxCarryableCountExceeded
		=> (CarriedSet.Count >= maxCarryableCount);


	#endregion

	[Header("Carryier Events")]
	#region Carryier Events

	[SerializeField]
	internal UnityEvent<Carryable> onGrabbed = new();

	[SerializeField]
	internal UnityEvent<Carryable> onUnGrabbed = new();


	#endregion


	// Update
	public bool TryCarryCarryable(Carryable requester)
	{
		if (IsAbleToCarryCarryable(requester))
		{
			CarriedSet.Add(requester);
			requester.GetUnCarriedFromCurrent();
			requester.CurrentCarryier = this;

			onGrabbed?.Invoke(requester);
			requester.onGotGrabbed?.Invoke(this);
			return true;
		}

		return false;
	}

	public void UnCarryCarryable(Carryable requester)
	{
		if (CarriedSet.Remove(requester))
		{
			requester.CurrentCarryier = null;

			onUnGrabbed?.Invoke(requester);
			requester.onGotUnGrabbed?.Invoke(this);
		}
	}

	public void UnCarryAll()
	{
		var cachedList = ListPool<Carryable>.Get();
		cachedList.AddRange(CarriedSet);

		_carriedSet.Clear();
        for (int i = cachedList.Count - 1; i >= 0; i--)
        {
			var iteratedCarryable = cachedList[i];
			iteratedCarryable.CurrentCarryier = null;

			onUnGrabbed?.Invoke(iteratedCarryable);
			iteratedCarryable.onGotUnGrabbed?.Invoke(this);
		}

		ListPool<Carryable>.Release(cachedList);
	}

	public bool IsAbleToCarryCarryable(Carryable requester)
	{
		if (!this.isActiveAndEnabled || IsMaxCarryableCountExceeded || _carriedSet.Contains(requester))
			return false;

		if ((requester.CurrentCarryier == this) || !requester.isActiveAndEnabled || !requester.acceptedCarryiersList.Contains(CType))
			return false;

		return requester.CurrentCarryier ? (this.priority >= requester.CurrentCarryier.priority) : true;
	}


	// Dispose
	private void OnDisable()
	{
		UnCarryAll();
	}
}


#if UNITY_EDITOR

public sealed partial class Carryier
{ }

#endif