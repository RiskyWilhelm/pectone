using System;

public sealed partial class OnEnableEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnEnable()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnEnableEvent { }


#endif