using System;

public sealed partial class OnBecameInvisibleEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnBecameInvisible()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnBecameInvisibleEvent { }


#endif