using System;

public sealed partial class OnDisableEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnDisable()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnDisableEvent { }


#endif