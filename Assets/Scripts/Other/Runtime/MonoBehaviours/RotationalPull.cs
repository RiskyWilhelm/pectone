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

	public void OnRigidbodyTriggerEnter(OnTriggerEnterEvent.Args args)
	{
		if (EventReflectorUtils.TryGetComponentByEventReflector<Rigidbody>(args.OtherCollider.gameObject, out Rigidbody found))
			registeredRigibodiesSet.Add(found);
	}

	public void OnRigidbodyTriggerExit(OnTriggerExitEvent.Args args)
	{
		if (EventReflectorUtils.TryGetComponentByEventReflector<Rigidbody>(args.OtherCollider.gameObject, out Rigidbody found))
			registeredRigibodiesSet.Remove(found);
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
	[RenameLabelTo("Is Activated Interactive Editing")]
	public bool e_IsActivatedInteractiveEditing;
}


#endif
