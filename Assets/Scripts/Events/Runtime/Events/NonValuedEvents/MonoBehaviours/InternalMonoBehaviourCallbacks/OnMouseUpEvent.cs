using System;

public sealed partial class OnMouseUpEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnMouseUp()
    {
		Raise(EventArgs.Empty);
	}
}


#if UNITY_EDITOR

public sealed partial class OnMouseUpEvent { }


#endif