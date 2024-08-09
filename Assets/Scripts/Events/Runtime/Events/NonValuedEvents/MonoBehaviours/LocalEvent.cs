using System;

public sealed partial class LocalEvent : MonoBehaviourEventBase<EventArgs> { }


#if UNITY_EDITOR

public sealed partial class LocalEvent { }


#endif