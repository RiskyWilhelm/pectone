using System;

public sealed partial class OnApplicationPauseEvent : MonoBehaviourEventBase<OnApplicationPauseEvent.Args>
{
	public class Args : EventArgs
	{
		public bool IsPaused
		{ get; init; }
	}

	private void OnApplicationPause(bool isPaused)
    {
		Raise(new Args()
		{
			IsPaused = isPaused
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnApplicationPauseEvent { }


#endif