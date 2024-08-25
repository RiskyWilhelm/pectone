using UnityEngine;

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

	public bool IsTryingToMove
		=> (_movingDirection != Vector3.zero);

	public bool IsMovingApproximately
		=> SelfRigidbody.IsMovingApproximately();


	#endregion

	#region Movable Other

	private Vector3 _movingDirection;

	public Vector3 NormalizedMovingDirection
	{
		get => _movingDirection;
		set => _movingDirection = value.normalized;
	}


	#endregion


	// Initialize
	private void OnEnable()
	{
		_movingDirection = default;
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
		SelfRigidbody.LimitLinearVelocity(maxMovementVelocity);
	}

	private void ApplyForceToDirectionFixed()
	{
		SelfRigidbody.AddForce(Vector3.Scale(movementForce, NormalizedMovingDirection), movementForceMode);
	}

	/// <summary> 
	/// Manipulates the <paramref name="normalizedDirection"/> based on <paramref name="relativeTo"/> direction.
	/// For example, <see cref="Vector3.forward"/> can be equal to <paramref name="relativeTo"/>'s forward </summary>
	public void SetMovingDirectionRelativeToTransform(Transform relativeTo, Vector3 normalizedDirection)
		=> NormalizedMovingDirection = (relativeTo.rotation * normalizedDirection);
}


#if UNITY_EDITOR

public sealed partial class Movable
{ }

#endif