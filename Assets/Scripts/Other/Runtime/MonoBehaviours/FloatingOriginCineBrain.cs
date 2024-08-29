using Unity.Cinemachine;
using UnityEngine;

public sealed partial class FloatingOriginCineBrain : MonoBehaviour
{
	#region FloatingOriginCineCam Floating Origin

	[SerializeField]
	private CinemachineBrain selfCineBrain;


	#endregion


	// Initialize
	private void OnEnable()
	{
		FloatingOriginSingleton.Instance.onAfterOriginShiftedFixed.AddListener(OnAfterOriginShiftedFixed);
	}


	// Update
	private void OnAfterOriginShiftedFixed(Vector3 shiftPosition)
	{
		CinemachineVirtualCameraBase activeCamera = (CinemachineVirtualCameraBase)selfCineBrain.ActiveVirtualCamera;
		activeCamera.OnTargetObjectWarped(activeCamera.LookAt ?? activeCamera.transform, -shiftPosition);
	}


	// Dispose
	private void OnDisable()
	{
		FloatingOriginSingleton.Instance.onAfterOriginShiftedFixed.RemoveListener(OnAfterOriginShiftedFixed);
	}
}


#if UNITY_EDITOR

public sealed partial class FloatingOriginCineBrain
{ }


#endif
