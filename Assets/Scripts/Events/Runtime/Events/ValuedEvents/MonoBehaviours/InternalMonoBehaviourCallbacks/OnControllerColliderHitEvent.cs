using System;
using UnityEngine;

public sealed partial class OnControllerColliderHitEvent : MonoBehaviourEventBase<OnControllerColliderHitEvent.Args>
{
	public class Args : EventArgs
	{
		public ControllerColliderHit Hit
		{ get; init; }
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
    {
		Raise(new Args()
		{
			Hit = hit
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnControllerColliderHitEvent { }


#endif