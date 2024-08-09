using System;

public sealed partial class OnAudioFilterReadEvent : MonoBehaviourEventBase<OnAudioFilterReadEvent.Args>
{
	public class Args : EventArgs
	{
		public float[] Data
		{ get; init; }

		public int Channels
		{ get; init; }
	}

	private void OnAudioFilterRead(float[] data, int channels)
    {
		Raise(new Args()
		{
			Data = data,
			Channels = channels
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnAudioFilterReadEvent { }


#endif