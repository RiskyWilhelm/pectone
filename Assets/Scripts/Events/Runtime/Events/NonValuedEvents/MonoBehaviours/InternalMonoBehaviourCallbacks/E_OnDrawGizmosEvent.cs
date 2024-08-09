#if UNITY_EDITOR

using System;

public sealed class E_OnDrawGizmosEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnDrawGizmos()
    {
        Raise(EventArgs.Empty);
    }
}

#endif