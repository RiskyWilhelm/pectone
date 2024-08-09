using System;

public sealed partial class OnAnimatorMoveEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnAnimatorMove()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnAnimatorMoveEvent { }


#endif