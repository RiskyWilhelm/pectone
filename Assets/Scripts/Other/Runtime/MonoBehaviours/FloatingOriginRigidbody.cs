using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed partial class FloatingOriginRigidbody : MonoBehaviour
{
	#region FloatingOriginSingleton Floating Origin

	[SerializeField]
	private Rigidbody selfRigidbody;


	#endregion


	// Initialize
	private void OnEnable()
	{
		FloatingOriginSingleton.Instance.RegisterChildRigidbody(selfRigidbody);
	}


	// Dispose
	private void OnDisable()
	{
		FloatingOriginSingleton.Instance.UnRegisterChildRigidbody(selfRigidbody);
	}
}


#if UNITY_EDITOR

public sealed partial class FloatingOriginRigidbody
{ }


#endif
