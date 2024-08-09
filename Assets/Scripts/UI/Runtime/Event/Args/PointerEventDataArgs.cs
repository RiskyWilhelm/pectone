using System;
using UnityEngine.EventSystems;

public class PointerEventDataArgs : EventArgs
{
	public PointerEventData EventData
	{ get; init; }
}