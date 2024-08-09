using System;

public sealed partial class OnMouseUpAsButtonOnAwakeEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnMouseUpAsButton()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnMouseUpAsButtonOnAwakeEvent { }


#endif