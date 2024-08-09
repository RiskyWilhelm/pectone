using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody))]
public sealed partial class Movable : MonoBehaviour
{
	[Header("Movable Movement")]
	#region Movable Movement

	[SerializeField]
	[Tooltip("Stay in FixedUpdate if you dont have a reason to switch other")]
	private UpdateType updateType = UpdateType.FixedUpdate;

	[SerializeField]
	private ForceMode movementForceMode = ForceMode.Acceleration;

    public Vector3 movementForce;

	[Min(0)]
	[Tooltip("Optional. Set to zero if no velocity limitation wanted")]
	public Vector3 maxMovementVelocity;

	public bool allowVerticalMovement;

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
		set
		{
			if ((value != Vector3.zero) && !value.IsNormalized())
				throw new Exception("Value must be normalized");

			_normalizedMovingDirection = value;
		}
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
		if (updateType is UpdateType.Update)
		{
			ApplyForceToDirection();
			LimitVelocity();
		}
	}

	private void FixedUpdate()
	{
		if (updateType is UpdateType.FixedUpdate)
		{
			ApplyForceToDirection();
			LimitVelocity();
		}
	}

	private void LateUpdate()
	{
		if (updateType is UpdateType.LateUpdate)
		{
			ApplyForceToDirection();
			LimitVelocity();
		}
	}

	private void ApplyForceToDirection()
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

	public void SetMovingDirection(Vector2 normalizedWorldDir)
	{
		NormalizedMovingDirection = new Vector3(normalizedWorldDir.x, 0f, normalizedWorldDir.y);
	}

	public void SetMovingDirectionFromInput(CallbackContext normalizedContext)
		=> SetMovingDirection(normalizedContext.ReadValue<Vector2>());

	// TODO: This code repetities at Player.cs
	/// <summary> 
	/// Manipulates the <paramref name="normalizedWorldDirection"/> based on <paramref name="relativeTo"/> direction.
	/// For example, <see cref="Vector3.forward"/> can be equal to <paramref name="relativeTo"/>'s forward </summary>
	public void SetMovingDirectionRelativeToTransform(Transform relativeTo, Vector3 normalizedWorldDirection)
	{
		if ((normalizedWorldDirection != Vector3.zero) && !normalizedWorldDirection.IsNormalized())
			throw new Exception("New direction must be normalized");

		var relativeForward = relativeTo.forward;
		var relativeRight = relativeTo.right;
		var relativeUp = relativeTo.up;

		if (!allowVerticalMovement)
		{
			relativeForward.y = 0f;
			relativeRight.y = 0f;
			relativeUp.y = 0f;
		}

		var resultForward = normalizedWorldDirection.z * relativeForward;
		var resultRight = normalizedWorldDirection.x * relativeRight;
		var resultUp = normalizedWorldDirection.y * relativeUp;

		NormalizedMovingDirection = (resultForward + resultRight + resultUp).normalized;
	}
}


#if UNITY_EDITOR

public sealed partial class Movable
{ }

#endif