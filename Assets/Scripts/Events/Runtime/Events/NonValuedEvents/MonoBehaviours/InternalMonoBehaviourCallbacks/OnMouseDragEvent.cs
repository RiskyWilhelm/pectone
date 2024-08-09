using System;

public sealed partial class OnMouseDragEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnMouseDrag()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnMouseDragEvent { }


#endif