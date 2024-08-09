using System;
using UnityEngine;

public sealed partial class OnTriggerExit2DEvent : MonoBehaviourEventBase<OnTriggerExit2DEvent.Args>
{
	public class Args : EventArgs
	{
		public Collider2D OtherCollider
		{ get; init; }
	}

	private void OnTriggerExit2D(Collider2D collision)
    {
		Raise(new Args()
		{
			OtherCollider = collision
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnTriggerExit2DEvent { }


#endif