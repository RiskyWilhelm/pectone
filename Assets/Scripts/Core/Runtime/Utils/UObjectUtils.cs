using UnityEditor;
using UnityEngine;

public static class UObjectUtils
{
	public static bool IsInstantiatedInRuntime(this Object a)
	{
#if UNITY_EDITOR
		return (a.GetInstanceID() < 0) && (GlobalObjectId.GetGlobalObjectIdSlow(a).identifierType == 0);
#else
		return (a.GetInstanceID() < 0);
#endif
	}
}