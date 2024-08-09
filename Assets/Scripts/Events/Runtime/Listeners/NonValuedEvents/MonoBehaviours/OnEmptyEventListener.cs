using System;

public sealed partial class OnEmptyEventListener : MonoBehaviourEventListenerBase<EventArgs>
{ }


#if UNITY_EDITOR

public sealed partial class OnEmptyEventListener { }


#endif