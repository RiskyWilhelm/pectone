using System;
using UnityEngine;

// TODO: Do gliding when wing snap is in certain angle
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
		/// <summary> Adds force to down vector of rigidbody based on new wing span value </summary>
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


	#endregion

	[Header("Player Flying")]
	#region Player Flying

	[SerializeField]
	private Wing rightWing;

	[SerializeField]
	private Wing leftWing;

	[SerializeField]
	private float wingGlidingForce;

	[SerializeField]
	private float wingFlappingForce;

	[SerializeField]
	private float wingClosingSpeed;

	[SerializeField]
	private CustomizableJoystick wingControllerJoystick;


	#endregion


	// Initialize
	protected override void OnEnable()
	{
		CloseWings();
		base.OnEnable();
	}


	// Update
	protected override void Update()
	{
		if (!wingControllerJoystick.IsGettingInteracted)
			CloseWingsBySpeed();

		base.Update();
	}

	protected override void FixedUpdate()
	{
		if (wingControllerJoystick.IsGettingInteracted)
			OnWingControllerJoystickInteractedFixed();

		base.FixedUpdate();
	}

	public void OnMovementJoystickInputChanged(Vector2 input)
    {
		var normalizedInput = input.normalized;

		movementController.SetMovingDirectionRelativeToTransform(relativeMovementTransform, normalizedInput);
		UpdateRotationBasedOnInput(normalizedInput);
	}

	public void OnWingControllerJoystickInteractedFixed()
	{
		var input = wingControllerJoystick.Input;
		if ((input == wingControllerJoystick.LastInput) && !IsGrounded())
		{
			var glidingPowerFromWingSpan = Mathf.Clamp01(1f - Math.Abs(input.y));

			SelfRigidbody.AddRelativeForce(Vector3.up * (glidingPowerFromWingSpan * wingGlidingForce), ForceMode.Acceleration);
			rightWing.Span01 = input.y;
			leftWing.Span01 = input.y;
			return;
		}

		var isInputMovedDownwards = (input.y < rightWing.Span01) || (input.y < rightWing.Span01);
		if (isInputMovedDownwards)
		{
			rightWing.SetWingSpan01WithRelativeForce(SelfRigidbody, input.y, wingFlappingForce, ForceMode.Acceleration);
			leftWing.SetWingSpan01WithRelativeForce(SelfRigidbody, input.y, wingFlappingForce, ForceMode.Acceleration);
			return;
		}

		rightWing.Span01 = input.y;
		leftWing.Span01 = input.y;
	}

	public void CloseWings()
	{
		rightWing.Span01 = -1f;
		leftWing.Span01 = -1f;
	}

	public void CloseWingsBySpeed()
	{
		var newWingSpan01 = Mathf.MoveTowards(rightWing.Span01, -1f, wingClosingSpeed * Time.deltaTime);

		rightWing.Span01 = newWingSpan01;
		leftWing.Span01 = newWingSpan01;
	}

	// TODO: This code repetities at Movable.cs
	private void UpdateRotationBasedOnInput(Vector2 normalizedInput)
	{
		// Also prevents the zero
		if (!normalizedInput.IsNormalized())
			return;

		var relativeForward = relativeMovementTransform.forward;
		var relativeRight = relativeMovementTransform.right;

		relativeForward.y = 0f;
		relativeRight.y = 0f;

		var resultRight = normalizedInput.x * relativeRight;
		var resultForward = normalizedInput.y * relativeForward;

		var normalizedResult = (resultForward + resultRight).normalized;
		SelfRigidbody.rotation = Quaternion.LookRotation(normalizedResult);
	}
}


#if UNITY_EDITOR

public sealed partial class Player
{ }


#endif
