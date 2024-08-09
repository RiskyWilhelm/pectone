using System;
using UnityEngine;

// TODO: If you will add an editor button "Raise" then you must add "RaiseWithValue" too
public sealed partial class Vector3GameEventSO : ScriptableObjectEventBase<Vector3GameEventSO.Args>
{
	public class Args : EventArgs
	{
		public GameObject Value
		{ get; init; }
	}
}



#if UNITY_EDITOR

#pragma warning disable 0414

[CreateAssetMenu]
public sealed partial class Vector3GameEventSO
{ }

#pragma warning restore 0414

#endif
