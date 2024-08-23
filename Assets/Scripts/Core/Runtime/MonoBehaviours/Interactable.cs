using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

[DisallowMultipleComponent]
public sealed partial class Interactable : MonoBehaviour
{
	[Header("Interactable Interaction")]
	#region Interactable Interaction

	public List<InteractorType> acceptedInteractorsList = new();

	public uint maxInteractorCount = 1;

	private readonly HashSet<Interactor> _interactorsSet = new();

	private ReadOnlySet<Interactor> _interactorsRSet;

	private HashSet<Interactor> InteractorsSet
	{
		get
		{
			_interactorsSet.RemoveWhere((x) => !x);
			return _interactorsSet;
		}
	}

	public ReadOnlySet<Interactor> InteractorsRSet
		=> _interactorsRSet ??= new(_interactorsSet);

	public bool IsMaxInteractorsCountExceeded
		=> (InteractorsSet.Count >= maxInteractorCount);


	#endregion

	[Header("Interactable Events")]
	#region Interactable Events

	[SerializeField]
	internal UnityEvent<Interactor> onGotInteracted = new();

	[SerializeField]
	internal UnityEvent<Interactor> onGotUnInteracted = new();


	#endregion


	// Update
	public bool TryGetInteractedBy(Interactor requester)
	{
		if (IsAbleToGetInteractedBy(requester))
		{
			InteractorsSet.Add(requester);
			requester.UnInteractWithCurrent();
			requester.CurrentInteractable = this;

			requester.onInteracted?.Invoke(this);
			onGotInteracted?.Invoke(requester);
			return true;
		}

		return false;
	}

	public void UnInteractInteractor(Interactor requester)
	{
		if (InteractorsSet.Remove(requester))
		{
			requester.CurrentInteractable = null;

			requester.onUnInteracted?.Invoke(this);
			onGotUnInteracted?.Invoke(requester);
		}
	}

	public void UnInteractAll()
	{
		var cachedList = ListPool<Interactor>.Get();
		cachedList.AddRange(InteractorsSet);

		_interactorsSet.Clear();
		for (int i = cachedList.Count - 1; i >= 0; i--)
		{
			var iteratedInteractor = cachedList[i];
			iteratedInteractor.CurrentInteractable = null;

			iteratedInteractor.onUnInteracted?.Invoke(this);
			onGotUnInteracted?.Invoke(iteratedInteractor);
		}

		ListPool<Interactor>.Release(cachedList);
	}

	public bool IsAbleToGetInteractedBy(Interactor requester)
	{
		if (!this.isActiveAndEnabled || IsMaxInteractorsCountExceeded || _interactorsSet.Contains(requester))
			return false;

		if (requester.CurrentInteractable || !requester.isActiveAndEnabled || !acceptedInteractorsList.Contains(requester.IType))
			return false;

		return true;
	}


	// Dispose
	private void OnDisable()
	{
		UnInteractAll();
	}
}


#if UNITY_EDITOR

public sealed partial class Carryable
{ }

#endif