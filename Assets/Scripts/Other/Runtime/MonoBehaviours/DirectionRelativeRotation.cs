using UnityEngine;

public sealed partial class DirectionRelativeRotation : MonoBehaviour
{
	[Header("DirectionRelativeRotation Movement")]
	#region DirectionRelativeRotation Movement

	[SerializeField]
	private Transform relativeTo;

	[SerializeField]
	private Rigidbody controlled;

	[SerializeField]
	private bool isReversed;

	[SerializeField]
	private UpdateType updateType = UpdateType.FixedUpdate;

	[SerializeField]
	private DirectionRelativeRotationAxisType controlledAxisTypes = DirectionRelativeRotationAxisType.Y;

    private Vector3 lastPosition;


	#endregion


	// Initialize
	private void Start()
	{
		lastPosition = controlled.position;
	}


	// Update
	private void Update()
	{
		if (updateType is UpdateType.Update)
			UpdateRotation();
	}

	private void FixedUpdate()
	{
		if (updateType is UpdateType.FixedUpdate)
			UpdateRotation();
	}

	private void LateUpdate()
	{
		if (updateType is UpdateType.LateUpdate)
			UpdateRotation();
	}

	private void UpdateRotation()
	{
		var newLookDirection = lastPosition.GetDirectionWithMagnitudeTo(relativeTo.position);

		if (newLookDirection.Approximately(Vector3.zero))
		{
			lastPosition = relativeTo.position;
			return;
		}

		var newRotationEulerAngles = Quaternion.LookRotation(newLookDirection).eulerAngles;

		if (!controlledAxisTypes.HasFlag(DirectionRelativeRotationAxisType.X))
			newRotationEulerAngles.x = relativeTo.rotation.eulerAngles.x;

		if (!controlledAxisTypes.HasFlag(DirectionRelativeRotationAxisType.Y))
			newRotationEulerAngles.y = relativeTo.rotation.eulerAngles.y;

		if (!controlledAxisTypes.HasFlag(DirectionRelativeRotationAxisType.Z))
			newRotationEulerAngles.z = relativeTo.rotation.eulerAngles.z;

		controlled.rotation = Quaternion.Euler(newRotationEulerAngles);
		lastPosition = relativeTo.position;
	}
}


#if UNITY_EDITOR

public sealed partial class DirectionRelativeRotation
{ }

#endif