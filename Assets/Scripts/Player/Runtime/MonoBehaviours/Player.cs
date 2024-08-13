using System;
using Unity.Cinemachine;
using UnityEngine;

public sealed partial class Player : StateMachineDrivenPlayerBase
{
	[Serializable]
	public sealed class Wing
	{
		[Header("Wing Visual")]
		#region Wing Visual

		[SerializeField]
		private Transform visualTransform;

		public PlayerWingAxisType axisType = PlayerWingAxisType.Z;

		public bool inverseRotation;

		[Tooltip("Overrides origin of the wingspan")]
		public float spanOriginAngleDegree = 0f;

		[Range(0f, 180f)]
		public float maxSpanAngleDegreeHalf = 75f;


		#endregion

		#region Wing Flapping

		private float _span01;

		/// <summary> Tells how open the wing is via progress </summary>
		public float Span01
		{
			get => _span01;
			set
			{
				LastSpan01 = _span01;
				_span01 = value;
				UpdateRotationByWingSpan01(value);
			}
		}

		public float LastSpan01
		{ get; private set; }

		public bool IsClosed => Mathf.Approximately(Span01, -1f);


		#endregion


		// Update
		/// <summary> Adds force from down to the rigidbody based on how big is new wing span value change compared to last </summary>
		public void SetWingSpan01WithRelativeForce(Rigidbody controlRigidbody, float newWingSpan01, float upForce, ForceMode forceMode = ForceMode.Force)
		{
			var wingSpanChange01 = (newWingSpan01 - LastSpan01);
			controlRigidbody.AddRelativeForce(Vector3.down * (upForce * wingSpanChange01), forceMode);
			Span01 = newWingSpan01;
		}

		private void UpdateRotationByWingSpan01(float wingSpan01)
		{
			var inverseRotation = this.inverseRotation ? -1f : 1f;
			var newRotationDegree = spanOriginAngleDegree + (inverseRotation * wingSpan01 * maxSpanAngleDegreeHalf);

			switch (axisType)
			{
				case PlayerWingAxisType.X:
				visualTransform.localRotation = Quaternion.Euler(new Vector3(newRotationDegree, 0f, 0f));
				break;

				case PlayerWingAxisType.Y:
				visualTransform.localRotation = Quaternion.Euler(new Vector3(0f, newRotationDegree, 0f));
				break;

				case PlayerWingAxisType.Z:
				visualTransform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, newRotationDegree));
				break;
			}
		}
	}

	[Header("Player Movement")]
	#region Player Movement

	[SerializeField]
	private Movable movementController;

	[SerializeField]
	[Tooltip("Manipulates the movementDirection based on this direction. For example, Vector3.forward can be equal to this forward")]
	private Transform relativeMovementTransform;

	[SerializeField]
	private float equalizeUpRotationWithSurfacePower = 45f;

	[SerializeField]
	private float equalizeUpRotationWithUnGroundedCameraPower = 45f;


	#endregion

	[Header("Player Flying")]
	#region Player Flying

	[SerializeField]
	private Wing rightWing;

	[SerializeField]
	private Wing leftWing;

	[SerializeField]
	private float wingGlideForce;

	[SerializeField]
	private float wingFlapForce;

	[SerializeField]
	private float wingClosePower;

	[SerializeField]
	private CustomizableJoystick wingControllerJoystick;


	#endregion

	[Header("Player Other")]
	#region Player Other

	[SerializeField]
	private CinemachineCamera unGroundedCamera;

	[SerializeField]
	private CinemachineCamera groundedCamera;


	#endregion


	// Initialize
	protected override void OnEnable()
	{
		CloseWings();
		base.OnEnable();
	}


	// Update
	protected override void DoGrounded()
	{
		var movementRigidbody = movementController.SelfRigidbody;
		movementRigidbody.rotation = EqualizeUpRotationWithDirection(movementRigidbody.rotation, CurrentIsGroundedHit.normal, equalizeUpRotationWithSurfacePower * Time.deltaTime);
	}

	protected override void DoUnGrounded()
	{
		var movementRigidbody = movementController.SelfRigidbody;
		movementRigidbody.rotation = EqualizeUpRotationWithDirection(movementRigidbody.rotation, relativeMovementTransform.up, equalizeUpRotationWithUnGroundedCameraPower * Time.deltaTime);
	}

	protected override void DoUnGroundedFixed()
	{
		if (wingControllerJoystick.IsGettingInteracted)
			FlapWingsToNewWingSpan(wingControllerJoystick.Input.y);
		else
			CloseWings(wingClosePower * Time.deltaTime);
	}

	protected override void DoGroundedFixed()
	{
		if (wingControllerJoystick.IsGettingInteracted)
			FlapWingsToNewWingSpan(wingControllerJoystick.Input.y);
		else
			CloseWings(wingClosePower * Time.deltaTime);
	}

	public void DoWingGlidingAtWingSpan(float newWingSpan01)
	{
		var movementRigidbody = movementController.SelfRigidbody;
		var glidingPowerFromWingSpan = Mathf.Clamp01(1f - Math.Abs(newWingSpan01));
		movementRigidbody.AddRelativeForce(Vector3.up * (glidingPowerFromWingSpan * wingGlideForce), ForceMode.Acceleration);

		rightWing.Span01 = newWingSpan01;
		leftWing.Span01 = newWingSpan01;
	}

	public void FlapWingsToNewWingSpan(float newWingSpan01)
	{
		var movementRigidbody = movementController.SelfRigidbody;
		var isNewEqualsWithCurrent = Mathf.Approximately(newWingSpan01, leftWing.Span01) || Mathf.Approximately(newWingSpan01, rightWing.Span01);
		var isWantsToGlide = !IsGrounded && isNewEqualsWithCurrent;
		var isFlappedToDownward = (newWingSpan01 < leftWing.Span01) || (newWingSpan01 < rightWing.Span01);

		if (isWantsToGlide)
			DoWingGlidingAtWingSpan(newWingSpan01);
        else if (isFlappedToDownward)
		{
			rightWing.SetWingSpan01WithRelativeForce(movementRigidbody, newWingSpan01, wingFlapForce, ForceMode.Acceleration);
			leftWing.SetWingSpan01WithRelativeForce(movementRigidbody, newWingSpan01, wingFlapForce, ForceMode.Acceleration);
		}
		else
		{
			rightWing.Span01 = newWingSpan01;
			leftWing.Span01 = newWingSpan01;
		}
	}

	public void CloseWings(float speedDelta = 1f)
	{
		var newWingSpan01 = Mathf.MoveTowards(rightWing.Span01, -1f, speedDelta);

		rightWing.Span01 = newWingSpan01;
		leftWing.Span01 = newWingSpan01;
	}

	private Quaternion EqualizeUpRotationWithDirection(Quaternion a, Vector3 normalizedDirection, float powerDelta = 360f)
	{
		var newForward = Vector3.ProjectOnPlane(a.GetForwardDirection(), normalizedDirection).normalized;
		var finalRotation = Quaternion.LookRotation(newForward, normalizedDirection);
		return Quaternion.RotateTowards(a, finalRotation, powerDelta);
	}

	protected override void OnGrounded()
	{
		groundedCamera.Priority = 1;
		unGroundedCamera.Priority = 0;
		relativeMovementTransform = groundedCamera.transform;
	}

	protected override void OnUnGrounded()
	{
		groundedCamera.Priority = 0;
		unGroundedCamera.Priority = 1;
		relativeMovementTransform = unGroundedCamera.transform;
	}

	public void OnMovementJoystickInputChanged(Vector2 input)
    {
		var normalizedInput = input.normalized;
		var inputForward = new Vector3(normalizedInput.x, 0f, normalizedInput.y);

		if (IsGrounded && (normalizedInput != Vector2.zero))
		{
			var movementRigidbody = movementController.SelfRigidbody;

			// Align the forward vector with relative transform
			var relativeAlignedInputBasedForward = (relativeMovementTransform.rotation * inputForward);
			var relativeAlignedRotation = Quaternion.LookRotation(relativeAlignedInputBasedForward);

			// Equalize up rotation to surface by power
			var surfaceAlignedRotation = EqualizeUpRotationWithDirection(relativeAlignedRotation, CurrentIsGroundedHit.normal);
			var surfaceAlignedForward = surfaceAlignedRotation.GetForwardDirection().normalized;
			var finalRotation = Quaternion.Slerp(surfaceAlignedRotation, relativeAlignedRotation, equalizeUpRotationWithSurfacePower * Time.deltaTime);

			movementController.SetMovingDirection(surfaceAlignedForward);
			movementController.UpdateRotationByDirection(finalRotation.GetForwardDirection(), movementRigidbody.rotation.GetUpDirection(), (MovableRotationAxisType.X | MovableRotationAxisType.Z));
		}
		else
		{
			movementController.SetMovingDirectionRelativeToTransform(relativeMovementTransform, inputForward);
			movementController.UpdateRotationByCurrentDirection(relativeMovementTransform.up, (MovableRotationAxisType.X | MovableRotationAxisType.Z));
		}
	}
}


#if UNITY_EDITOR

public sealed partial class Player
{ }


#endif
