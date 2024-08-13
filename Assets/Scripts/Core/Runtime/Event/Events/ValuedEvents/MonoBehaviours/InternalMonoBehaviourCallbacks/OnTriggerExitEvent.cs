using System;
using UnityEngine;

public sealed partial class OnTriggerExitEvent : MonoBehaviourEventBase<OnTriggerExitEvent.Args>
{
	public class Args : EventArgs
	{
		public Collider OtherCollider
		{ get; init; }
	}

	private void OnTriggerExit(Collider other)
    {
		Raise(new Args()
		{
			OtherCollider = other
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnTriggerExitEvent { }


#endif