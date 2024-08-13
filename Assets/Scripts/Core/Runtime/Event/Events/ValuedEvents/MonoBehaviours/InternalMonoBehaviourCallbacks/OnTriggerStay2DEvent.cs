using System;
using UnityEngine;

public sealed partial class OnTriggerStay2DEvent : MonoBehaviourEventBase<OnTriggerStay2DEvent.Args>
{
	public class Args : EventArgs
	{
		public Collider2D OtherCollider2D
		{ get; init; }
	}

	private void OnTriggerStay2D(Collider2D collision)
    {
		Raise(new Args()
		{
			OtherCollider2D = collision
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnTriggerStay2DEvent { }


#endif