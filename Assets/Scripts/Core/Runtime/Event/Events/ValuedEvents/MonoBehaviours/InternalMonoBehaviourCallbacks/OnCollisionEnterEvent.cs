using System;
using UnityEngine;

public sealed partial class OnCollisionEnterEvent : MonoBehaviourEventBase<OnCollisionEnterEvent.Args>
{
	public class Args : EventArgs
	{
		public Collision OtherCollision
		{ get; init; }
	}

	private void OnCollisionEnter(Collision collision)
    {
		Raise(new Args()
		{
			OtherCollision = collision
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnCollisionEnterEvent { }


#endif