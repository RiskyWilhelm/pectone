#if UNITY_EDITOR

using System;

public sealed class E_OnGUIEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnGUI()
    {
        Raise(EventArgs.Empty);
    }
}

#endif