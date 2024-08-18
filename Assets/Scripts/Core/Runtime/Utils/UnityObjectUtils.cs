using UnityEngine;

public static class UnityObjectUtils
{
	public static bool IsInstantiatedInRuntime(this Object a)
	{
		return a.GetInstanceID() < 0;
	}
}