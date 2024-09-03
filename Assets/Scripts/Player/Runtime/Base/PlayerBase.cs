using UnityEditor;
using UnityEngine;

public abstract partial class PlayerBase : MonoBehaviour
{
	[Header("PlayerBase Movement")]
	#region PlayerBase Movement

	[SerializeField]
	private Rigidbody _selfRigidbody;

	public LayerMask isGroundedLayerMask = Layers.Mask.Ground;

	public float isGroundedCheckRadius = 1f;

	public float isGroundedCheckDistance = 1f;

	private bool _isGrounded;

	private RaycastHit _currentGroundedHit;

	public Rigidbody SelfRigidbody
		=> _selfRigidbody;

	public bool IsGrounded
	{
		get => _isGrounded;
		private set
		{
			if (_isGrounded != value)
			{
				_isGrounded = value;
				if (_isGrounded)
					OnGrounded();
				else
					OnUnGrounded();
			}
		}
	}

	public RaycastHit CurrentIsGroundedHit
		=> _currentGroundedHit;


	#endregion


	// Initialize
	protected virtual void Awake()
	{
		OnUnGrounded();
	}


	// Update
	protected virtual void Update()
	{
		IsGrounded = IsGroundedAtVector(_selfRigidbody.position, out _currentGroundedHit, isGroundedLayerMask);
	}

	public bool IsGroundedAtVector(Vector3 worldPosition, out RaycastHit hit, int layerMask = Layers.Mask.Ground)
	{
		return Physics.SphereCast(worldPosition, isGroundedCheckRadius, _selfRigidbody.rotation.DownDirection(), out hit, isGroundedCheckDistance, layerMask);
	}

	protected virtual void OnGrounded()
	{ }

	protected virtual void OnUnGrounded()
	{ }
}


#if UNITY_EDITOR

public abstract partial class PlayerBase
{
	protected virtual void OnDrawGizmosSelected()
	{
		DrawAcceptedDistanceForGroundedCheck();
	}

	private void DrawAcceptedDistanceForGroundedCheck()
	{
		var currentRotation = _selfRigidbody.rotation;
		Gizmos.color = new Color(1f, 0f, 0f, 0.25f);

		Gizmos.DrawSphere(this.transform.position, isGroundedCheckRadius);
		Gizmos.DrawSphere(this.transform.position + (currentRotation.DownDirection() * isGroundedCheckDistance), isGroundedCheckRadius);
		Handles.Label(this.transform.position + (currentRotation.DownDirection() * isGroundedCheckDistance), "Grounded Check");
	}
}

#endif