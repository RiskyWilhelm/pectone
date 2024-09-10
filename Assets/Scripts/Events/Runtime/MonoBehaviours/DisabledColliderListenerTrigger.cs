using UnityEngine;

/// <summary> Registers to <see cref="DisabledColliderNotifier"/>(s) so notifier knows which trigger listener gets interacted by which collider </summary>
[DisallowMultipleComponent]
public sealed partial class DisabledColliderListenerTrigger : MonoBehaviour
{
	// Update
	public void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<DisabledColliderNotifier>(out DisabledColliderNotifier found))
			found.OnEnterTrigger(other, this);
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.TryGetComponent<DisabledColliderNotifier>(out DisabledColliderNotifier found))
			found.OnStayTrigger(other, this);
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent<DisabledColliderNotifier>(out DisabledColliderNotifier found))
			found.OnExitTrigger(other, this);
	}
}


#if UNITY_EDITOR

public sealed partial class DisabledColliderListenerTrigger 
{ }


#endif
