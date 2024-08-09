using System;

public sealed partial class OnApplicationFocusEvent : MonoBehaviourEventBase<OnApplicationFocusEvent.Args>
{
	public class Args : EventArgs
	{
		public bool IsFocused
		{ get; init; }
	}

	private void OnApplicationFocus(bool isFocus)
    {
		Raise(new Args()
		{
			IsFocused = isFocus
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnApplicationFocusEvent { }


#endif