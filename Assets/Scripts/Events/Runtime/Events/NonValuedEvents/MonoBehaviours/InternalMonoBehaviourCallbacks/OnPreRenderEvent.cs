using System;

public sealed partial class OnPreRenderEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnPreRender()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnPreRenderEvent { }


#endif