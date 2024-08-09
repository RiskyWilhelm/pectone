#if UNITY_EDITOR

using System;

public sealed class E_OnValidateEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnValidate()
    {
		Raise(EventArgs.Empty);
    }
}

#endif