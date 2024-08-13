using System;
using UnityEngine;

public sealed partial class OnCollisionExit2DEvent : MonoBehaviourEventBase<OnCollisionExit2DEvent.Args>
{
	public class Args : EventArgs
	{
		public Collision2D OtherCollision
		{ get; init; }
	}

	private void OnCollisionExit2D(Collision2D collision)
    {
		Raise(new Args()
		{
			OtherCollision = collision
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnCollisionExit2DEvent { }


#endif