using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public sealed partial class FloatingOriginSingleton : MonoBehaviourSingletonBase<FloatingOriginSingleton>
{
	[Header("FloatingOriginSingleton Floating Origin")]
	#region FloatingOriginSingleton Floating Origin

	[SerializeField]
	private Rigidbody worldOriginRigidbody;

	[SerializeField]
	private Rigidbody alignRigidbody;

	public bool doShiftingEveryAllowedDistance = true;

	public uint allowedDistance = 3000;

	private Vector3 syncedShiftPosition;

	private bool isRequestedSyncedTransformShift;

	private readonly HashSet<Rigidbody> rigidbodiesSet = new();

	private readonly HashSet<Transform> transformsSet = new();


	#endregion

	[Header("FloatingOriginSingleton Events")]
	#region FloatingOriginSingleton Events

	public UnityEvent onBeforeOriginShifted = new();

	public UnityEvent<Vector3> onAfterOriginShiftedFixed = new();


	#endregion


	// Update
	private void Update()
	{
		TryAutoShift();
	}

	private void FixedUpdate()
	{
		if (syncedShiftPosition != Vector3.zero)
		{
			if (isRequestedSyncedTransformShift)
			{
				ShiftTransforms(syncedShiftPosition);
				isRequestedSyncedTransformShift = false;
			}

			ShiftCineCameras(syncedShiftPosition);
			onAfterOriginShiftedFixed?.Invoke(syncedShiftPosition);
			syncedShiftPosition = Vector3.zero;
		}
	}

	public void Shift()
	{
		onBeforeOriginShifted?.Invoke();
		var shiftPosition = alignRigidbody.position;
		syncedShiftPosition += shiftPosition;

		ShiftTransformsInSync();
		ShiftRigidbodies(shiftPosition);
		alignRigidbody.position = Vector3.zero;
	}

	private void ShiftRigidbodies(Vector3 shiftPosition)
	{
		worldOriginRigidbody.position -= shiftPosition;

		rigidbodiesSet.RemoveWhere((x) => !x);
		foreach (var iteratedRigidbody in rigidbodiesSet)
		{
			if (!iteratedRigidbody.IsMovingApproximately())
				iteratedRigidbody.Sleep();

			iteratedRigidbody.position -= shiftPosition;
		}
	}

	private void ShiftTransforms(Vector3 shiftPosition)
	{
		transformsSet.RemoveWhere((x) => !x);
		foreach (var iteratedTransform in transformsSet)
			iteratedTransform.position -= shiftPosition;
	}

	private void ShiftCineCameras(Vector3 shiftPosition)
	{
		for (int i = 0; i < CinemachineBrain.ActiveBrainCount; i++)
		{
			var iteratedVCamera = (CinemachineVirtualCameraBase)CinemachineBrain.GetActiveBrain(i).ActiveVirtualCamera;
			iteratedVCamera.OnTargetObjectWarped(iteratedVCamera.LookAt ?? iteratedVCamera.transform, -shiftPosition);
		}
	}

	/// <summary> Synces with the Rigidbody position changes </summary>
	private void ShiftTransformsInSync()
	{
		isRequestedSyncedTransformShift = true;
	}

	private void TryAutoShift()
	{
		if (doShiftingEveryAllowedDistance)
		{
			var currentVelocity = alignRigidbody.linearVelocity;
			if ((currentVelocity.x >= allowedDistance) || (currentVelocity.y >= allowedDistance) || (currentVelocity.z >= allowedDistance))
			{
				Debug.LogWarningFormat("{0}'s speed more than floating origin's allowed distance! Limiting its speed to allowed distance", alignRigidbody.gameObject.name);
				alignRigidbody.LimitLinearVelocity(new Vector3(allowedDistance, allowedDistance, allowedDistance));
			}

			var currentPosition = alignRigidbody.position.Abs();
			if ((currentPosition.x >= allowedDistance) || (currentPosition.y >= allowedDistance) || (currentPosition.z >= allowedDistance))
				Shift();
		}
	}

	public void RegisterChildRigidbody(Rigidbody requester)
		=> rigidbodiesSet.Add(requester);

	public void UnRegisterChildRigidbody(Rigidbody requester)
		=> rigidbodiesSet.Remove(requester);

	/// <remarks> Not recommended to use </remarks>
	public void RegisterTransform(Transform requester)
		=> transformsSet.Add(requester);

	public void UnRegisterTransform(Transform requester)
		=> transformsSet.Remove(requester);
}


#if UNITY_EDITOR

public partial class FloatingOriginSingleton
{ }

#endif