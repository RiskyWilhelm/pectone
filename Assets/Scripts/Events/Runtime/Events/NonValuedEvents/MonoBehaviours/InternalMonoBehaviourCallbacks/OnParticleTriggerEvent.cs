using System;

public sealed partial class OnParticleTriggerEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnParticleTrigger()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnParticleTriggerEvent { }


#endif