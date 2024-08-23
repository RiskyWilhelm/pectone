using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public sealed partial class Interactor : MonoBehaviour
{
	[Header("Interactor Interaction")]
	#region Interactor Interaction

	[field: SerializeField]
	public InteractorType IType
	{ get; private set; }

	public Interactable CurrentInteractable
	{ get; internal set; }


	#endregion

	[Header("Interactor Events")]
	#region Interactor Events

	[SerializeField]
	internal UnityEvent<Interactable> onInteracted = new();

	[SerializeField]
	internal UnityEvent<Interactable> onUnInteracted = new();


	#endregion


	// Update
	public bool TryInteractWith(Interactable requester)
	{
		return requester.TryGetInteractedBy(this);
	}

	public void UnInteractWithCurrent()
	{
		if (CurrentInteractable)
			CurrentInteractable.UnInteractInteractor(this);
	}

	public bool IsAbleToGetGrabbedBy(Interactable requester)
	{
		return requester.IsAbleToGetInteractedBy(this);
	}


	// Dispose
	private void OnDisable()
	{
		UnInteractWithCurrent();
	}
}


#if UNITY_EDITOR

public sealed partial class Interactor
{ }

#endif