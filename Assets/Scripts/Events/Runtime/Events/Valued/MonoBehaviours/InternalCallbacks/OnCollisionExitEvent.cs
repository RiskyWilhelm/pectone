using UnityEngine;

public sealed partial class OnCollisionExitEvent : MonoBehaviourEvent<Collision>
{
	// Update
	private void OnCollisionExit(Collision collision)
    {
        Raise(collision);
    }
}


#if UNITY_EDITOR

public sealed partial class OnCollisionExitEvent { }


#endif