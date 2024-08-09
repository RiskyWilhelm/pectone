using System;

public sealed partial class LocalEventListener : MonoBehaviourEventListenerBase<EventArgs> { }


#if UNITY_EDITOR

public sealed partial class LocalEventListener { }


#endif