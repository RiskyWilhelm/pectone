using System.Collections.Generic;
using UnityEngine;

public sealed partial class OnCollisionEnterEvent : MonoBehaviourEvent<Collision>
{
	[Header("OnCollisionEnterEvent Chain")]
	#region OnCollisionEnterEvent Chain

	[SerializeField]
	private List<Collider> selfCollidersList = new();


	#endregion


	// Update
	private void OnCollisionEnter(Collision collision)
    {
        Raise(collision);
	}

	internal void OnParentCollisionEnter(Collision collision, Collider contactThisCollider)
	{
		if (selfCollidersList.Contains(contactThisCollider))
			OnCollisionEnter(collision);
	}
}


#if UNITY_EDITOR

public sealed partial class OnCollisionEnterEvent { }


#endif