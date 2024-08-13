using System;
using UnityEngine;

public sealed partial class OnCollisionStay2DEvent : MonoBehaviourEventBase<OnCollisionStay2DEvent.Args>
{
	public class Args : EventArgs
	{
		public Collision2D OtherCollision
		{ get; init; }
	}

	private void OnCollisionStay2D(Collision2D collision)
    {
		Raise(new Args()
		{
			OtherCollision = collision
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnCollisionStay2DEvent { }


#endif