using System;
using UnityEngine;

public sealed partial class OnJointBreak2DEvent : MonoBehaviourEventBase<OnJointBreak2DEvent.Args>
{
	public class Args : EventArgs
	{
		public Joint2D Joint
		{ get; init; }
	}

	private void OnJointBreak2D(Joint2D joint)
    {
		Raise(new Args()
		{
			Joint = joint
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnJointBreak2DEvent { }


#endif