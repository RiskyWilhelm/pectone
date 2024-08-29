using System.Collections.Generic;
using UnityEngine;

public sealed partial class GravitionalPull : MonoBehaviour
{
	[Header("GravitionalPull Gravity")]
	#region GravitionalPull Gravity

	[Tooltip("Optional. If zero, a direction of registered rigibody to transform position (origin) is used")]
	public Vector3 pullDirectionWorldEuler;

	public bool isPullDirectionWorldAxis;

	public float pullGravity = 9.81f;


	#endregion

	#region GravitionalPull Other

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
		var isUsingOriginForPull = (pullDirectionWorldEuler == Vector3.zero);
		var pullDirection = default(Vector3);

		if (!isUsingOriginForPull)
		{
			if (isPullDirectionWorldAxis)
				pullDirection = Quaternion.Euler(pullDirectionWorldEuler).ForwardDirection();
			else
				pullDirection = this.transform.rotation * Quaternion.Euler(pullDirectionWorldEuler).ForwardDirection();
		}

		foreach (var iteratedRigidbody in registeredRigibodiesSet)
        {
			if (iteratedRigidbody.isKinematic || iteratedRigidbody.IsSleeping())
				return;

			if (isUsingOriginForPull)
				pullDirection = iteratedRigidbody.transform.position.GetWorldDirectionTo(this.transform.position);

			iteratedRigidbody.AddForce(pullDirection * (pullGravity * iteratedRigidbody.mass));
        }
    }

	public void RegisterChildRigidbody(Rigidbody childRigidbody)
		=> registeredRigibodiesSet.Add(childRigidbody);

	public void UnRegisterChildRigidbody(Rigidbody childRigidbody)
		=> registeredRigibodiesSet.Remove(childRigidbody);

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

public sealed partial class GravitionalPull
{
	[Header("GravitionalPull Edit")]
	[RenameLabelTo("Is Activated Interactive Editing")]
	public bool e_IsActivatedInteractiveEditing;
}


#endif
