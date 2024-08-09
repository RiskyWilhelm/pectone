using System;

public sealed partial class OnRenderObjectEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnRenderObject()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnRenderObjectEvent { }


#endif