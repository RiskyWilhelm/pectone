using System;

public sealed partial class OnApplicationQuitEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnApplicationQuit()
    {
		Raise(EventArgs.Empty);
	}
}


#if UNITY_EDITOR

public sealed partial class OnApplicationQuitEvent { }


#endif