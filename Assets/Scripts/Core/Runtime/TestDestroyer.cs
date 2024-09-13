using System.Collections;
using UnityEngine;
using UnityEngine.LowLevel;

public sealed partial class TestDestroyer : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		Destroy(this);
		Debug.Log("Destroyed");
	}
}


#if UNITY_EDITOR

public sealed partial class TestDestroyer
{ }


#endif
