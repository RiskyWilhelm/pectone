using System;

public sealed partial class OnMouseDownEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnMouseDown()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnMouseDownEvent { }


#endif