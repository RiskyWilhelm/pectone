using System;

public sealed partial class OnLateUpdateEvent : MonoBehaviourEventBase<EventArgs>
{
    private void LateUpdate()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnLateUpdateEvent { }


#endif