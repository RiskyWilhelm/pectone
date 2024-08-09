using System;

public sealed partial class OnPostRenderEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnPostRender()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnPostRenderEvent { }


#endif