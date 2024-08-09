using System;

public sealed partial class OnPreCullEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnPreCull()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnPreCullEvent { }


#endif