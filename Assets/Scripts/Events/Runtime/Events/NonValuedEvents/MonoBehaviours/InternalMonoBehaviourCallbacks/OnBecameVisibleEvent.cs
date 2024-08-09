using System;

public sealed partial class OnBecameVisibleEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnBecameVisible()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnBecameVisibleEvent { }


#endif