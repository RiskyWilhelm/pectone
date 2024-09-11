using System.Collections;
using UnityEngine;

public sealed partial class TestCollision : MonoBehaviour
{
	// Initialize
	IEnumerator Start()
	{
		yield return null;

		this.destroyCancellationToken.Register((obj) => Debug.Log(obj), null, true);
		Destroy(this);
	}



	// Update


	// Dispose
	private void OnDestroy()
	{
		Debug.LogFormat("OnDestroy called {0}", this.GetType());
	}
}


#if UNITY_EDITOR

public sealed partial class TestCollision
{ }


#endif
