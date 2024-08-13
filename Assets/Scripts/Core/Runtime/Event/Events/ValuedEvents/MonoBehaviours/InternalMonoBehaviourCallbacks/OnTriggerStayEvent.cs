using System;
using UnityEngine;

public sealed partial class OnTriggerStayEvent : MonoBehaviourEventBase<OnTriggerStayEvent.Args>
{
	public class Args : EventArgs
	{
		public Collider OtherCollider
		{ get; init; }
	}

	private void OnTriggerStay(Collider other)
    {
		Raise(new Args()
		{
			OtherCollider = other
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnTriggerStayEvent { }


#endif