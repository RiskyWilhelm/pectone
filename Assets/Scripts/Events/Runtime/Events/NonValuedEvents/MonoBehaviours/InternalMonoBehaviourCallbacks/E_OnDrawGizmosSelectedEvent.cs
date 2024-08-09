#if UNITY_EDITOR

using System;

public sealed class E_OnDrawGizmosSelectedEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnDrawGizmosSelected()
    {
        Raise(EventArgs.Empty);
    }
}

#endif