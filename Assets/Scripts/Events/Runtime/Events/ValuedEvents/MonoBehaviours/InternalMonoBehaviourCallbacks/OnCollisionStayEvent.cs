using System;
using UnityEngine;

public sealed partial class OnCollisionStayEvent : MonoBehaviourEventBase<OnCollisionStayEvent.Args>
{
	public class Args : EventArgs
	{
		public Collision OtherCollision
		{ get; init; }
	}

	private void OnCollisionStay(Collision collision)
    {
		Raise(new Args()
		{
			OtherCollision = collision
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnCollisionStayEvent { }


#endif