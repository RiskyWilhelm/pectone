using System;
using UnityEngine;

public sealed partial class OnParticleCollisionEvent : MonoBehaviourEventBase<OnParticleCollisionEvent.Args>
{
	public class Args : EventArgs
	{
		public GameObject OtherGameObject
		{ get; init; }
	}

	private void OnParticleCollision(GameObject other)
    {
		Raise(new Args()
		{
			OtherGameObject = other
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnParticleCollisionEvent { }


#endif