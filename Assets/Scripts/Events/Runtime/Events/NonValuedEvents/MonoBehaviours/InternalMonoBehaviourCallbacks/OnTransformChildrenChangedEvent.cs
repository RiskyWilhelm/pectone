using System;

public sealed partial class OnTransformChildrenChangedEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnTransformChildrenChanged()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnTransformChildrenChangedEvent { }


#endif