using UnityEngine;

public sealed partial class OnCollisionStayEvent : MonoBehaviourEvent<Collision>
{
	// Update
	private void OnCollisionStay(Collision collision)
    {
		Raise(collision);
	}
}


#if UNITY_EDITOR

public sealed partial class OnCollisionStayEvent { }


#endif