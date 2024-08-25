using System.Collections.Generic;
using UnityEngine;

public sealed partial class RotationalPull : MonoBehaviour
{
	[Header("RotationalPull Rotation")]
	#region RotationalPull Rotation

	[Tooltip("Optional. If zero, a direction of transform position (origin) to registered rigibody is used")]
	public Vector3 upDirectionWorldEuler;

	public bool isUpDirectionWorldAxis;

	public float equalizeUpRotationsPower = 90f;


	#endregion

	#region RotationalPull Other

	private readonly HashSet<Rigidbody> registeredRigibodiesSet = new();


	#endregion


	// Update
	private void Update()
	{
		registeredRigibodiesSet.RemoveWhere(x => !x);
	}

	private void FixedUpdate()
	{
		PullControlledRigidbodiesFixed();
	}

	private void PullControlledRigidbodiesFixed()
	{
		var isUsingOriginForUp = (upDirectionWorldEuler == Vector3.zero);
		var upDirection = default(Vector3);

		if (!isUsingOriginForUp)
		{
			if (isUpDirectionWorldAxis)
				upDirection = Quaternion.Euler(upDirectionWorldEuler).GetForwardDirection();
			else
				upDirection = this.transform.rotation * Quaternion.Euler(upDirectionWorldEuler).GetForwardDirection();
		}

		foreach (var iteratedRigidbody in registeredRigibodiesSet)
        {
			if (iteratedRigidbody.isKinematic || iteratedRigidbody.IsSleeping())
				return;

			if (isUsingOriginForUp)
				upDirection = this.transform.position.GetWorldDirectionTo(iteratedRigidbody.transform.position);

			iteratedRigidbody.rotation = iteratedRigidbody.rotation.EqualizeUpRotationWithDirection(upDirection, powerDelta: equalizeUpRotationsPower * Time.deltaTime);
        }
    }

	// WARNING: Support implementation for custom Events
	public void OnRigidbodyTriggerEnter(Collider other)
	{
		var attachedRigidbody = other.attachedRigidbody;
		if (attachedRigidbody && !attachedRigidbody.isKinematic)
			registeredRigibodiesSet.Add(other.attachedRigidbody);
	}

	public void OnRigidbodyTriggerExit(Collider other)
	{
		if (other.attachedRigidbody)
			registeredRigibodiesSet.Remove(other.attachedRigidbody);
	}


	// Dispose
	private void OnDisable()
	{
		registeredRigibodiesSet.Clear();
	}
}


#if UNITY_EDITOR

public sealed partial class RotationalPull
{
	[Header("RotationalPull Edit")]
	[RenameLabelTo("Activate Interactive Editing")]
	public bool e_IsActivatedInteractiveEditing;
}


#endif