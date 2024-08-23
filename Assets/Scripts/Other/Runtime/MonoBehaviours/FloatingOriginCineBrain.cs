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
		FloatingOriginSingleton.Instance.onAfterOriginShifted.AddListener(OnAfterOriginShifted);
	}


	// Update
	private void OnAfterOriginShifted(Vector3 shiftPosition)
	{
		CinemachineVirtualCameraBase activeCamera = (CinemachineVirtualCameraBase)selfCineBrain.ActiveVirtualCamera;
		activeCamera.OnTargetObjectWarped(activeCamera.LookAt ?? activeCamera.transform, -shiftPosition);
	}


	// Dispose
	private void OnDisable()
	{
		FloatingOriginSingleton.Instance.onAfterOriginShifted.RemoveListener(OnAfterOriginShifted);
	}
}


#if UNITY_EDITOR

public sealed partial class FloatingOriginCineBrain
{ }


#endif
