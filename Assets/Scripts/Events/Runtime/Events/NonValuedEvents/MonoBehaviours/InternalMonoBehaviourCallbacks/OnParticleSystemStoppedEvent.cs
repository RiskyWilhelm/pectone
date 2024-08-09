using System;

public sealed partial class OnParticleSystemStoppedEvent : MonoBehaviourEventBase<EventArgs>
{
    private void OnParticleSystemStopped()
    {
		Raise(EventArgs.Empty);
	}
}


#if UNITY_EDITOR

public sealed partial class OnParticleSystemStoppedEvent { }


#endif