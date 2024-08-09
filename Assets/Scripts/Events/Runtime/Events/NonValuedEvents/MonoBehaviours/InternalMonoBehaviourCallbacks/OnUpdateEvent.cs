using System;

public sealed partial class OnUpdateEvent : MonoBehaviourEventBase<EventArgs>
{
	private void Update()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnUpdateEvent { }


#endif