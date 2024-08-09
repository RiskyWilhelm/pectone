using System;

public sealed partial class OnParticleUpdateJobScheduledEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnParticleUpdateJobScheduled()
    {
        Raise(EventArgs.Empty);
    }
}


#if UNITY_EDITOR

public sealed partial class OnParticleUpdateJobScheduledEvent { }


#endif