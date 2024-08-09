using System;

public sealed partial class OnStartEvent : MonoBehaviourEventBase<EventArgs>
{
    private void Start()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnStartEvent { }


#endif