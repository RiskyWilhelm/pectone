using System;
using UnityEngine;

public sealed partial class OnTriggerEnter2DEvent : MonoBehaviourEventBase<OnTriggerEnter2DEvent.Args>
{
	public class Args : EventArgs
	{
		public Collider2D OtherCollider2D
		{ get; init; }
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {
		Raise(new Args()
		{
			OtherCollider2D = collision
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnTriggerEnter2DEvent { }


#endif