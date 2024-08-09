using System;

public sealed partial class OnMouseExitEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnMouseExit()
    {
		Raise(EventArgs.Empty);
	}
}


#if UNITY_EDITOR

public sealed partial class OnMouseExitEvent { }


#endif