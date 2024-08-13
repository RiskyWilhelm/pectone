using System;

public sealed partial class OnJointBreakEvent : MonoBehaviourEventBase<OnJointBreakEvent.Args>
{
	public class Args : EventArgs
	{
		public float BreakForce
		{ get; init; }
	}

	private void OnJointBreak(float breakForce)
    {
		Raise(new Args()
		{
			BreakForce = breakForce
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnJointBreakEvent { }


#endif