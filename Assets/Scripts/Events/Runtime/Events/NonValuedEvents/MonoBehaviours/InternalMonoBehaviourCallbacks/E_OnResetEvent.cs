#if UNITY_EDITOR

using System;

public sealed class E_OnResetEvent : MonoBehaviourEventBase<EventArgs>
{
    private void Reset()
    {
        Raise(EventArgs.Empty);
    }
}

#endif