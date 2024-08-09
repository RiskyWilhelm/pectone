using System;
using UnityEngine;

public sealed partial class OnWillRenderObjectEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnWillRenderObject()
    {
		Raise(EventArgs.Empty);
	}
}


#if UNITY_EDITOR

#pragma warning disable 0414

public sealed partial class OnWillRenderObjectEvent { }

#pragma warning restore 0414

#endif