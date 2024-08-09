using System;

public sealed partial class OnFixedUpdateEvent : MonoBehaviourEventBase<EventArgs>
{
    private void FixedUpdate()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnFixedUpdateEvent { }


#endif