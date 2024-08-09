using System;

public sealed partial class GameEventSO : MonoBehaviourEventBase<EventArgs> { }


#if UNITY_EDITOR

public sealed partial class GameEventSO { }


#endif