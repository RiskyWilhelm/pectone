using UnityEngine;

public sealed partial class OnPlayerTriggerExitEvent : MonoBehaviourEvent<Collider, Player>
{
	// Update
	private void OnTriggerExit(Collider other)
	{
		if (EventReflectorUtils.TryGetComponentByEventReflector<Player>(other.gameObject, out Player found))
			Raise(other, found);
	}
}


#if UNITY_EDITOR

public sealed partial class OnPlayerTriggerExitEvent
{ }


#endif
