using UnityEngine;

public sealed partial class TestLogger : MonoBehaviour
{
	public void Log(string msg)
	{
		Debug.Log(msg);
	}
}


#if UNITY_EDITOR

public sealed partial class TestLogger
{ }


#endif
