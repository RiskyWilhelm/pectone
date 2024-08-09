using System;

public sealed partial class OnDestroyEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnDestroy()
    {
		Raise(EventArgs.Empty);
	}
}


#if UNITY_EDITOR

public sealed partial class OnDestroyEvent { }


#endif