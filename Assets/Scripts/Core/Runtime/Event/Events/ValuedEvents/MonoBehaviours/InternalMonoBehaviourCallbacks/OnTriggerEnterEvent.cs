using System;
using UnityEngine;

public sealed partial class OnTriggerEnterEvent : MonoBehaviourEventBase<OnTriggerEnterEvent.Args>
{
	public class Args : EventArgs
	{
		public Collider OtherCollider
		{ get; init; }
	}

	private void OnTriggerEnter(Collider other)
    {
		Raise(new Args()
		{
			OtherCollider = other
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnTriggerEnterEvent { }


#endif