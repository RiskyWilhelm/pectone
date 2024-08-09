using System;

public sealed partial class OnMouseEnterEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnMouseEnter()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnMouseEnterEvent { }


#endif