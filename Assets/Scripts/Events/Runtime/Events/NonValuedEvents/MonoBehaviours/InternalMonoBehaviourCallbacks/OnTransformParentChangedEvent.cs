using System;

public sealed partial class OnTransformParentChangedEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnTransformParentChanged()
    {
		Raise(EventArgs.Empty);
	}
}


#if UNITY_EDITOR

public sealed partial class OnTransformParentChangedEvent { }


#endif