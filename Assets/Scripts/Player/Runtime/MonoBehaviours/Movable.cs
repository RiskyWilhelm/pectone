using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody))]
public sealed partial class Movable : MonoBehaviour
{
	[Header("Movable Movement")]
	#region Movable Movement

	[SerializeField]
	private ForceMode movementForceMode = ForceMode.Acceleration;

    public Vector3 movementForce;

	[Min(0)]
	[Tooltip("Optional. Set to zero if no velocity limitation wanted")]
	public Vector3 maxMovementVelocity;

	[field: SerializeField]
	public Rigidbody SelfRigidbody
	{ get; private set; }

	public bool IsTryingToMove => (_normalizedMovingDirection != Vector3.zero);

	public bool IsMovingApproximately => SelfRigidbody.IsMovingApproximately();


	#endregion

	#region Movable Other

	private Vector3 _normalizedMovingDirection;

	public Vector3 NormalizedMovingDirection
	{
		get => _normalizedMovingDirection;
	}


	#endregion


	// Initialize
	private void OnEnable()
	{
		_normalizedMovingDirection = default;
		SelfRigidbody.linearVelocity = Vector3.zero;
	}


	// Update
	private void Update()
	{
		Debug.DrawLine(this.transform.position, this.transform.position + (NormalizedMovingDirection * 10f), Color.yellow);
	}

	private void FixedUpdate()
	{
		ApplyForceToDirectionFixed();
		LimitVelocity();
	}

	private void ApplyForceToDirectionFixed()
	{
		SelfRigidbody.AddForce(Vector3.Scale(movementForce, _normalizedMovingDirection), movementForceMode);
	}

	private void LimitVelocity()
	{
		var updatedLinearVelocity = SelfRigidbody.linearVelocity;

		if (maxMovementVelocity.x > 0f)
			updatedLinearVelocity.x = Mathf.Clamp(updatedLinearVelocity.x, -maxMovementVelocity.x, maxMovementVelocity.x);

		if (maxMovementVelocity.y > 0f)
			updatedLinearVelocity.y = Mathf.Clamp(updatedLinearVelocity.y, -maxMovementVelocity.y, maxMovementVelocity.y);

		if (maxMovementVelocity.z > 0f)
			updatedLinearVelocity.z = Mathf.Clamp(updatedLinearVelocity.z, -maxMovementVelocity.z, maxMovementVelocity.z);

		SelfRigidbody.linearVelocity = updatedLinearVelocity;
	}

	public void SetMovingDirection(Vector3 normalizedDirection)
	{
		if (!normalizedDirection.IsNormalizedOrZero())
			throw new Exception("Only normalized and zero vector is accepted");

		_normalizedMovingDirection = normalizedDirection;
	}

	public void SetMovingDirection(Vector2 normalizedDirection)
		=> SetMovingDirection(new Vector3(normalizedDirection.x, normalizedDirection.y, 0f));

	public void SetMovingDirectionFromInput(CallbackContext normalizedContext)
		=> SetMovingDirection(normalizedContext.ReadValue<Vector2>());

	/// <summary> 
	/// Manipulates the <paramref name="normalizedDirection"/> based on <paramref name="relativeTo"/> direction.
	/// For example, <see cref="Vector3.forward"/> can be equal to <paramref name="relativeTo"/>'s forward </summary>
	public void SetMovingDirectionRelativeToTransform(Transform relativeTo, Vector3 normalizedDirection)
		=> SetMovingDirection((relativeTo.rotation * normalizedDirection).normalized);

	public void UpdateRotationByDirection(Vector3 normalizedDirection, Vector3 upwards, MovableRotationAxisType acceptedRotationDirectionAxisType = MovableRotationAxisType.All)
	{
		if ((normalizedDirection == Vector3.zero) && !normalizedDirection.IsNormalized())
			return;

		var currentRotation = SelfRigidbody.rotation;
		var newRotation = Quaternion.LookRotation(normalizedDirection, upwards);

		// Allow only specific axis
		if (!acceptedRotationDirectionAxisType.HasFlag(MovableRotationAxisType.X))
			newRotation = Quaternion.FromToRotation(newRotation.GetRightDirection(), currentRotation.GetRightDirection()) * newRotation;

		if (!acceptedRotationDirectionAxisType.HasFlag(MovableRotationAxisType.Y))
			newRotation = Quaternion.FromToRotation(newRotation.GetUpDirection(), currentRotation.GetUpDirection()) * newRotation;

		if (!acceptedRotationDirectionAxisType.HasFlag(MovableRotationAxisType.Z))
			newRotation = Quaternion.FromToRotation(newRotation.GetForwardDirection(), currentRotation.GetForwardDirection()) * newRotation;

		// Set to new if any change happened
		SelfRigidbody.rotation = newRotation;
	}

	/// <summary> Defaults the upwards in <see cref="UpdateRotationByDirection(Vector3, Vector3, MovableRotationAxisType)"/> to Vector3.up </summary>
	public void UpdateRotationByDirection(Vector3 normalizedDirection, MovableRotationAxisType acceptedRotationDirectionAxisType = MovableRotationAxisType.All)
		=> UpdateRotationByDirection(normalizedDirection, Vector3.up, acceptedRotationDirectionAxisType);

	public void UpdateRotationByCurrentDirection(Vector3 upwards, MovableRotationAxisType acceptedRotationDirectionAxisType = MovableRotationAxisType.All)
		=> UpdateRotationByDirection(_normalizedMovingDirection, upwards, acceptedRotationDirectionAxisType);

	/// <inheritdoc cref="UpdateRotationByDirection(Vector3, MovableRotationAxisType)"/>
	public void UpdateRotationByCurrentDirection(MovableRotationAxisType acceptedRotationDirectionAxisType = MovableRotationAxisType.All)
		=> UpdateRotationByCurrentDirection(Vector3.up, acceptedRotationDirectionAxisType);
}


#if UNITY_EDITOR

public sealed partial class Movable
{ }

#endif