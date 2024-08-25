using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary> Unity does not allows child collider(s) to take OnCollisionXXX events. This is a solution for that issue </summary>
public sealed partial class OnCollisionEnterParentEvent : MonoBehaviourEvent<Collision>
{
	[Header("OnCollisionEnterParentEvent Chain")]
	#region OnCollisionEnterParentEvent Chain

	[SerializeField]
	private List<OnCollisionEnterEvent> childCollisionDetectorsList = new();


	#endregion


	// Update
	private void OnCollisionEnter(Collision collision)
    {
        Raise(collision);

		var cachedList = ListPool<ContactPoint>.Get();
		collision.GetContacts(cachedList);

		try
		{
			foreach (var iteratedChild in childCollisionDetectorsList)
                foreach (var iteratedContactPoint in cachedList)
					iteratedChild.OnParentCollisionEnter(collision, iteratedContactPoint.thisCollider);
		}
		finally
		{
			ListPool<ContactPoint>.Release(cachedList);
		}
	}
}


#if UNITY_EDITOR

public sealed partial class OnCollisionEnterParentEvent { }


#endif