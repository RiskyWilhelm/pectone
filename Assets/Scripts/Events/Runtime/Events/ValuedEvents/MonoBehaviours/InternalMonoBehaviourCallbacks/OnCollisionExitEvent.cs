using System;
using UnityEngine;

public sealed partial class OnCollisionExitEvent : MonoBehaviourEventBase<OnCollisionExitEvent.Args>
{
    public class Args : EventArgs
    {
        public Collision OtherCollision
        { get; init; }
	}

    private void OnCollisionExit(Collision collision)
    {
        Raise(new Args()
        {
			OtherCollision = collision
        });
    }
}


#if UNITY_EDITOR

public sealed partial class OnCollisionExitEvent { }


#endif