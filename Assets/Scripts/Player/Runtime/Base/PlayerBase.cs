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
	protected float acceptedDistanceForGroundedCheck = 1f;

	[field: SerializeField]
	public Rigidbody SelfRigidbody
	{ get; protected set; }


	#endregion


	// Update
	public bool IsGrounded()
		=> IsGroundedAtVector(SelfRigidbody.position);

	public bool IsGroundedAtVector(Vector3 worldPosition)
	{
		// BoxCast wont give good results when it's Y size is defined.You can debug that in Analysis>Physics
		var manipulatedSize = (size * 0.5f);
		manipulatedSize.y = 0f;

		return Physics.BoxCast(worldPosition, manipulatedSize, Vector3.down, SelfRigidbody.rotation, acceptedDistanceForGroundedCheck, Layers.Mask.Ground);
	}
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
		Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
		Gizmos.DrawLine(this.transform.position, this.transform.position + (Vector3.down * acceptedDistanceForGroundedCheck));

		Handles.Label(this.transform.position + (Vector3.down * acceptedDistanceForGroundedCheck), "Grounded Check");
	}
}

#endif