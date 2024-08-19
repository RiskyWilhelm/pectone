using UnityEngine;

public sealed partial class OnPlayerTriggerEnterEvent : MonoBehaviourEvent<Collider, Player>
{
	// Update
	private void OnTriggerEnter(Collider other)
	{
		if (EventReflectorUtils.TryGetComponentByEventReflector<Player>(other.gameObject, out Player found))
			Raise(other, found);
	}
}


#if UNITY_EDITOR

public sealed partial class OnPlayerTriggerEnterEvent
{ }


#endif
