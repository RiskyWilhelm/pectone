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
				pullDirection = Quaternion.Euler(pullDirectionWorldEuler).GetForwardDirection();
			else
				pullDirection = this.transform.rotation * Quaternion.Euler(pullDirectionWorldEuler).GetForwardDirection();
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

public sealed partial class GravitionalPull
{
	[Header("GravitionalPull Edit")]
	[RenameLabelTo("Is Activated Interactive Editing")]
	public bool e_IsActivatedInteractiveEditing;
}


#endif
