using System;

public sealed partial class OnAwakeEvent : MonoBehaviourEventBase<EventArgs>
{
    private void Awake()
    {
		Raise(EventArgs.Empty);
	}
}


#if UNITY_EDITOR

public sealed partial class OnAwakeEvent { }


#endif