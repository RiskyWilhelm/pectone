using System.Collections.Generic;
using UnityEngine;

public sealed partial class GravitionalPull : MonoBehaviour
{
	[Header("GravitionalCollider Gravity")]
	#region GravitionalCollider Gravity

	[SerializeField]
	private Collider selfCollider;

	public float pullGravity = 9.81f;


	#endregion

	[Header("GravitionalCollider Rotation")]
	#region GravitionalCollider Rotation

	public float equalizeRigidbodyRotationPower = 90f;


	#endregion

	#region GravitionalCollider Other

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
        foreach (var iteratedRigidbody in registeredRigibodiesSet)
        {
			if (iteratedRigidbody.isKinematic || iteratedRigidbody.IsSleeping())
				return;

			var normalizedDirTargetToSelf = iteratedRigidbody.transform.position.GetWorldDirectionTo(this.transform.position);
			iteratedRigidbody.AddForce(normalizedDirTargetToSelf * (pullGravity * iteratedRigidbody.mass));
			EqualizeRigidbodyUpRotation(iteratedRigidbody, equalizeRigidbodyRotationPower * Time.deltaTime);
        }
    }

	public void EqualizeRigidbodyUpRotation(Rigidbody targetRigidbody, float powerDelta = 360f)
	{
		if (powerDelta == 0f)
			return;

		var targetRigidbodyTransform = targetRigidbody.transform;
		var normalizedDirTargetToSelf = targetRigidbodyTransform.position.GetWorldDirectionTo(this.transform.position);
		var newRotationDir = Quaternion.FromToRotation(-targetRigidbodyTransform.up, normalizedDirTargetToSelf) * targetRigidbodyTransform.rotation;
		targetRigidbody.rotation = Quaternion.RotateTowards(targetRigidbody.rotation, newRotationDir, powerDelta);
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
{ }


#endif
