using System;
using UnityEngine;

public sealed partial class OnRenderImageEvent : MonoBehaviourEventBase<OnRenderImageEvent.Args>
{
	public class Args : EventArgs
	{
		public RenderTexture Source
		{ get; init; }

		public RenderTexture Destination
		{ get; init; }
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
		Raise(new Args()
		{
			Source = source,
			Destination = destination
		});
	}
}


#if UNITY_EDITOR

public sealed partial class OnRenderImageEvent { }


#endif