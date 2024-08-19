using UnityEngine;

public sealed partial class OnCollisionEnterEvent : MonoBehaviourEvent<Collision>
{
	// Update
	private void OnCollisionEnter(Collision collision)
    {
		Raise(collision);
	}
}


#if UNITY_EDITOR

public sealed partial class OnCollisionEnterEvent { }


#endif