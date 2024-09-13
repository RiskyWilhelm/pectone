/// TODO: Take different state types and convert methods to abstract
public abstract partial class StateMachineDrivenPlayerBase : PlayerBase
{
	#region StateMachineDrivenPlayerBase Other

	private PlayerStateType _state = PlayerStateType.Idle;

	public PlayerStateType State
	{
		get => _state;
		protected set
		{
			if (value != _state)
			{
				_state = value;
				OnStateChangedToAny(value);
				OnStateChanged(value);
			}
		}
	}


	#endregion


	// Initialize
	protected virtual void OnEnable()
	{
		State = PlayerStateType.Idle;
	}


	// Update
	protected override void Update()
	{
		DoState();
		base.Update();
	}

	protected virtual void FixedUpdate()
	{
		DoStateFixed();
	}

	protected virtual void LateUpdate()
	{
		DoStateLate();
	}

	public void SwitchToBlockState()
	{
		State = PlayerStateType.Blocked;
	}

	public void SwitchToUnblockState()
	{
		if (_state is PlayerStateType.Blocked)
			State = PlayerStateType.Idle;
	}

	#region State in Update()

	protected void DoState()
	{
		switch (_state)
		{
			case PlayerStateType.Idle:
			DoIdle();
			break;

			case PlayerStateType.Walking:
			DoWalking();
			break;

			case PlayerStateType.Running:
			DoRunning();
			break;

			case PlayerStateType.Flying:
			DoFlying();
			break;

			case PlayerStateType.Jumping:
			DoJumping();
			break;

			case PlayerStateType.Attacking:
			DoAttacking();
			break;

			case PlayerStateType.Defending:
			DoDefending();
			break;

			case PlayerStateType.Dead:
			DoDead();
			break;

			case PlayerStateType.Blocked:
			DoBlocked();
			break;
		}

		if (IsGrounded)
			DoGrounded();
		else
			DoUnGrounded();
	}

	/// <summary> It is not a 'state' but gets called if player grounded </summary>
	protected virtual void DoGrounded()
	{ }

	/// <summary> It is not a 'state' but gets called if player not grounded </summary>
	protected virtual void DoUnGrounded()
	{ }

	protected virtual void DoIdle()
	{ }

	protected virtual void DoWalking()
	{ }

	protected virtual void DoRunning()
	{ }

	protected virtual void DoFlying()
	{ }

	protected virtual void DoJumping()
	{ }

	protected virtual void DoAttacking()
	{ }

	protected virtual void DoDefending()
	{ }

	protected virtual void DoDead()
	{ }

	protected virtual void DoBlocked()
	{ }


	#endregion

	#region State in FixedUpdate()

	protected void DoStateFixed()
	{
		switch (_state)
		{
			case PlayerStateType.Idle:
			DoIdleFixed();
			break;

			case PlayerStateType.Walking:
			DoWalkingFixed();
			break;

			case PlayerStateType.Running:
			DoRunningFixed();
			break;

			case PlayerStateType.Flying:
			DoFlyingFixed();
			break;

			case PlayerStateType.Jumping:
			DoJumpingFixed();
			break;

			case PlayerStateType.Attacking:
			DoAttackingFixed();
			break;

			case PlayerStateType.Defending:
			DoDefendingFixed();
			break;

			case PlayerStateType.Dead:
			DoDeadFixed();
			break;

			case PlayerStateType.Blocked:
			DoBlockedFixed();
			break;
		}

		if (IsGrounded)
			DoGroundedFixed();
		else
			DoUnGroundedFixed();
	}

	/// <inheritdoc cref="DoGrounded"/>
	protected virtual void DoGroundedFixed()
	{ }

	/// <inheritdoc cref="DoUnGrounded"/>
	protected virtual void DoUnGroundedFixed()
	{ }

	protected virtual void DoIdleFixed()
	{ }

	protected virtual void DoWalkingFixed()
	{ }

	protected virtual void DoRunningFixed()
	{ }

	protected virtual void DoFlyingFixed()
	{ }

	protected virtual void DoJumpingFixed()
	{ }

	protected virtual void DoAttackingFixed()
	{ }

	protected virtual void DoDefendingFixed()
	{ }

	protected virtual void DoDeadFixed()
	{ }

	protected virtual void DoBlockedFixed()
	{ }


	#endregion

	#region State in LateUpdate()

	protected void DoStateLate()
	{
		switch (_state)
		{
			case PlayerStateType.Idle:
			DoIdleLate();
			break;

			case PlayerStateType.Walking:
			DoWalkingLate();
			break;

			case PlayerStateType.Running:
			DoRunningLate();
			break;

			case PlayerStateType.Flying:
			DoFlyingLate();
			break;

			case PlayerStateType.Jumping:
			DoJumpingLate();
			break;

			case PlayerStateType.Attacking:
			DoAttackingLate();
			break;

			case PlayerStateType.Defending:
			DoDefendingLate();
			break;

			case PlayerStateType.Dead:
			DoDeadLate();
			break;

			case PlayerStateType.Blocked:
			DoBlockedLate();
			break;
		}
		if (IsGrounded)
			DoGroundedLate();
		else
			DoUnGroundedLate();
	}

	/// <inheritdoc cref="DoGrounded"/>
	protected virtual void DoGroundedLate()
	{ }

	/// <inheritdoc cref="DoUnGrounded"/>
	protected virtual void DoUnGroundedLate()
	{ }

	protected virtual void DoIdleLate()
	{ }

	protected virtual void DoWalkingLate()
	{ }

	protected virtual void DoRunningLate()
	{ }

	protected virtual void DoFlyingLate()
	{ }

	protected virtual void DoJumpingLate()
	{ }

	protected virtual void DoAttackingLate()
	{ }

	protected virtual void DoDefendingLate()
	{ }

	protected virtual void DoDeadLate()
	{ }

	protected virtual void DoBlockedLate()
	{ }


	#endregion

	private void OnStateChanged(PlayerStateType newState)
	{
		switch (newState)
		{
			case PlayerStateType.Idle:
			OnStateChangedToIdle();
			break;

			case PlayerStateType.Walking:
			OnStateChangedToWalking();
			break;

			case PlayerStateType.Running:
			OnStateChangedToRunning();
			break;

			case PlayerStateType.Flying:
			OnStateChangedToFlying();
			break;

			case PlayerStateType.Jumping:
			OnStateChangedToJumping();
			break;

			case PlayerStateType.Attacking:
			OnStateChangedToAttacking();
			break;

			case PlayerStateType.Defending:
			OnStateChangedToDefending();
			break;

			case PlayerStateType.Dead:
			OnStateChangedToDead();
			break;

			case PlayerStateType.Blocked:
			OnStateChangedToBlocked();
			break;
		}
	}

	protected virtual void OnStateChangedToIdle()
	{ }

	protected virtual void OnStateChangedToWalking()
	{ }

	protected virtual void OnStateChangedToRunning()
	{ }

	protected virtual void OnStateChangedToFlying()
	{ }

	protected virtual void OnStateChangedToJumping()
	{ }

	protected virtual void OnStateChangedToAttacking()
	{ }

	protected virtual void OnStateChangedToDefending()
	{ }

	protected virtual void OnStateChangedToDead()
	{ }

	protected virtual void OnStateChangedToBlocked()
	{ }

	protected virtual void OnStateChangedToAny(PlayerStateType newState)
	{ }
}


#if UNITY_EDITOR

public abstract partial class StateMachineDrivenPlayerBase
{ }

#endif