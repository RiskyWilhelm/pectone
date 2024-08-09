using System;

public sealed partial class OnMouseOverEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnMouseOver()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnMouseOverEvent { }


#endif