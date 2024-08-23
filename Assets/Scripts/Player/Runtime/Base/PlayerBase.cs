using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

public abstract partial class PlayerBase : MonoBehaviour
{
	[Header("PlayerBase Movement")]
	#region PlayerBase Movement

	[SerializeField]
	[Min(0)]
	protected Vector3 size = new(1f, 1f, 1f);

	[SerializeField]
	protected float acceptedDistanceForIsGroundedCheck = 1f;

	public LayerMask isGroundedLayerMask = Layers.Mask.Ground;

	private bool _isGrounded;

	private RaycastHit _currentGroundedHit;

	[field: SerializeField]
	public Rigidbody SelfRigidbody
	{ get; protected set; }

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
	{
		get => _currentGroundedHit;
		protected set => _currentGroundedHit = value;
	}

	#endregion


	// Initialize
	protected virtual void Awake()
	{
		OnUnGrounded();
	}


	// Update
	protected virtual void Update()
	{
		IsGrounded = IsGroundedAtVector(SelfRigidbody.position, out _currentGroundedHit, isGroundedLayerMask);
	}

	public bool IsGroundedAtVector(Vector3 worldPosition, out RaycastHit hit, int layerMask = Layers.Mask.Ground)
	{
		var castExtent = (size * 0.25f);
		var sizeExtent = (size * 0.5f);
		var currentRotation = SelfRigidbody.rotation;

		// Check the 6 main direction because the player may be floating in the air
		(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance, int layerMask) castSettings =
			(worldPosition, new Vector3(castExtent.x - 0.05f, castExtent.y - 0.05f, castExtent.z - 0.05f), currentRotation.GetDownDirection(), currentRotation, sizeExtent.y + acceptedDistanceForIsGroundedCheck, layerMask);

		if (DoCast(castSettings, out hit))
			return true;

		castSettings.direction = currentRotation.GetUpDirection();
		if (DoCast(castSettings, out hit))
			return true;

		castSettings.direction = currentRotation.GetRightDirection();
		castSettings.maxDistance = sizeExtent.x + acceptedDistanceForIsGroundedCheck;
		if (DoCast(castSettings, out hit))
			return true;

		castSettings.direction = currentRotation.GetLeftDirection();
		if (DoCast(castSettings, out hit))
			return true;

		castSettings.direction = currentRotation.GetBackDirection();
		castSettings.maxDistance = sizeExtent.z + acceptedDistanceForIsGroundedCheck;
		if (DoCast(castSettings, out hit))
			return true;

		castSettings.direction = currentRotation.GetForwardDirection();
		if (DoCast(castSettings, out hit))
			return true;

		return false;

		static bool DoCast((Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance, int layerMask) castSettings, out RaycastHit hit)
			=> Physics.BoxCast(castSettings.center, castSettings.halfExtents, castSettings.direction, out hit, castSettings.orientation, castSettings.maxDistance, castSettings.layerMask);
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
		DrawSize();
		DrawAcceptedDistanceForGroundedCheck();
	}

	private void DrawSize()
	{
		Gizmos.color = new Color(1f, 0f, 0f, 0.1f);
		Gizmos.DrawCube(this.transform.position, size);

		Handles.Label(this.transform.position + (size * 0.5f), "Size");
	}

	private void DrawAcceptedDistanceForGroundedCheck()
	{
		// BoxCast wont give good results when it's Y size is defined.You can debug that in Analysis>Physics
		var sizeExtent = (size * 0.5f);
		var currentRotation = SelfRigidbody.rotation;

		Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
		Gizmos.DrawLine(this.transform.position, this.transform.position + (currentRotation.GetDownDirection() * (sizeExtent.y + acceptedDistanceForIsGroundedCheck)));
		Gizmos.DrawLine(this.transform.position, this.transform.position + (currentRotation.GetUpDirection() * (sizeExtent.y + acceptedDistanceForIsGroundedCheck)));
		Gizmos.DrawLine(this.transform.position, this.transform.position + (currentRotation.GetRightDirection() * (sizeExtent.x + acceptedDistanceForIsGroundedCheck)));
		Gizmos.DrawLine(this.transform.position, this.transform.position + (currentRotation.GetLeftDirection() * (sizeExtent.x + acceptedDistanceForIsGroundedCheck)));
		Gizmos.DrawLine(this.transform.position, this.transform.position + (currentRotation.GetBackDirection() * (sizeExtent.z + acceptedDistanceForIsGroundedCheck)));
		Gizmos.DrawLine(this.transform.position, this.transform.position + (currentRotation.GetForwardDirection() * (sizeExtent.z + acceptedDistanceForIsGroundedCheck)));

		Handles.Label(this.transform.position + (currentRotation.GetDownDirection() * (sizeExtent.y + acceptedDistanceForIsGroundedCheck)), "Grounded Check");
	}
}

#endif