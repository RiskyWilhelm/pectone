using System;

public sealed partial class OnAnimatorIKEvent : MonoBehaviourEventBase<OnAnimatorIKEvent.Args>
{
	public class Args : EventArgs
	{
		public int LayerIndex
		{ get; init; }
	}

	private void OnAnimatorIK(int layerIndex)
    {
		Raise(new Args()
		{
			LayerIndex = layerIndex
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnAnimatorIKEvent { }


#endif