using System;
using UnityEngine;

public sealed partial class OnCollisionEnter2DEvent : MonoBehaviourEventBase<OnCollisionEnter2DEvent.Args>
{
	public class Args : EventArgs
	{
		public Collision2D OtherCollision
		{ get; init; }
	}

	private void OnCollisionEnter2D(Collision2D collision)
    {
		Raise(new Args()
		{
			OtherCollision = collision
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnCollisionEnter2DEvent { }


#endif