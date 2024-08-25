using UnityEngine;

/// <summary> Used with <see cref="EventReflectorUtils"/> </summary>
public sealed partial class EventReflector : MonoBehaviour
{
	public GameObject reflected;
}


#if UNITY_EDITOR

public sealed partial class EventReflector
{ }


#endif
