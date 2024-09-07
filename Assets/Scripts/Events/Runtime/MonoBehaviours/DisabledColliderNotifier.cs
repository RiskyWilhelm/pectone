using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary> Allows other collider's and attached rigidbody's <see cref="ICollisionExitListener"/> and <see cref="ITriggerExitDisabledListener"/>(s) to receive <see cref="ICollisionExitListener.OnCollisionExit(Collision)"/> and <see cref="ITriggerExitDisabledListener"/> events </summary>
[DisallowMultipleComponent]
public sealed partial class DisabledColliderNotifier : MonoBehaviour, ICollisionEnterListener, ICollisionStayListener, ICollisionExitListener
{
	private readonly Dictionary<Collider, DisabledColliderListenerTrigger> interactingTriggersDict = new(); // Dict<this, other>

	private readonly Dictionary<Collider, Collider> interactingCollisionsDict = new(); // Dict<other, this>


	// Update
	private void LateUpdate()
	{
		CheckCollisionColliderStates();
		CheckTriggerColliderStates();
    }

	private void CheckTriggerColliderStates()
	{
		var cachedInvalidList = ListPool<Collider>.Get();

		foreach (var iteratedTriggerPair in interactingTriggersDict)
		{
			var thisCollider = iteratedTriggerPair.Key;
			var otherTrigger = iteratedTriggerPair.Value;

			if (!thisCollider || !otherTrigger)
			{
				Debug.LogError("You must not destroy colliders invidually. Unable to send any events", this);
				cachedInvalidList.Add(thisCollider);
				continue;
			}

			if (!thisCollider.enabled)
			{
				cachedInvalidList.Add(thisCollider);
				NotifyDisabledTrigger(thisCollider, otherTrigger);
			}
		}

		foreach (var iteratedInvalidCollider in cachedInvalidList)
			interactingTriggersDict.Remove(iteratedInvalidCollider);

		ListPool<Collider>.Release(cachedInvalidList);
	}

	private void CheckCollisionColliderStates()
	{
		var cachedInvalidList = ListPool<Collider>.Get();

		foreach (var iteratedCollisionPair in interactingCollisionsDict)
		{
			var thisCollider = iteratedCollisionPair.Value;
			var otherCollider = iteratedCollisionPair.Key;

			if (!thisCollider || !otherCollider)
			{
				Debug.LogError("You must not destroy colliders invidually. Unable to send any events", this);
				cachedInvalidList.Add(otherCollider);
				continue;
			}

			if (!thisCollider.enabled)
			{
				cachedInvalidList.Add(otherCollider);
				NotifyDisabledCollision(otherCollider, thisCollider);
				NotifyDisabledCollision(thisCollider, otherCollider);
			}
		}

		foreach (var iteratedInvalidCollider in cachedInvalidList)
			interactingCollisionsDict.Remove(iteratedInvalidCollider);

		ListPool<Collider>.Release(cachedInvalidList);
	}

	internal void OnEnterTrigger(Collider thisCollider, DisabledColliderListenerTrigger otherTrigger)
		=> OnStayTrigger(thisCollider, otherTrigger);

	internal void OnStayTrigger(Collider thisCollider, DisabledColliderListenerTrigger otherTrigger)
	{
		interactingTriggersDict[thisCollider] = otherTrigger;
	}

	internal void OnExitTrigger(Collider thisCollider, DisabledColliderListenerTrigger otherTrigger)
	{
		interactingTriggersDict.Remove(thisCollider);
	}

	public void OnCollisionEnter(Collision collision)
		=> OnCollisionStay(collision);

	public void OnCollisionStay(Collision collision)
	{
		var initialContact = collision.GetContact(0);
		interactingCollisionsDict[initialContact.otherCollider] = initialContact.thisCollider;
	}

	public void OnCollisionExit(Collision collision)
	{
		interactingCollisionsDict.Remove(collision.collider);
	}


	// Dispose
	private void OnDisable()
	{
		NotifyAllColliders();
		interactingTriggersDict.Clear();
		interactingCollisionsDict.Clear();
	}

	private void NotifyAllColliders()
	{
		foreach (var iteratedCollisionPair in interactingCollisionsDict)
		{
			var thisCollider = iteratedCollisionPair.Value;
			var otherCollider = iteratedCollisionPair.Key;

			NotifyDisabledCollision(thisCollider, otherCollider);
			NotifyDisabledCollision(otherCollider, thisCollider);
		}

		foreach (var iteratedTriggerPair in interactingTriggersDict)
		{
			var thisCollider = iteratedTriggerPair.Key;
			var otherTrigger = iteratedTriggerPair.Value;

			NotifyDisabledTrigger(thisCollider, otherTrigger);
		}
	}

	private void NotifyDisabledTrigger(Collider thisCollider, DisabledColliderListenerTrigger otherTrigger)
	{
		var cachedList = ListPool<ITriggerExitDisabledListener>.Get();
		otherTrigger.GetComponents<ITriggerExitDisabledListener>(cachedList);

		try
		{
			foreach (var iteratedReceiver in cachedList)
				iteratedReceiver.OnTriggerExitDisabled(thisCollider);

			// By default, unity sends message to the attached body's game object too. This will notify to collided collider's body
			var otherAttachedBody = otherTrigger.GetComponent<Collider>().GetBody();
			if (!otherAttachedBody)
				return;

			var otherAttachedBodyGO = otherAttachedBody.gameObject;
			if (otherTrigger.gameObject == otherAttachedBodyGO)
				return;

			otherAttachedBodyGO.GetComponents<ITriggerExitDisabledListener>(cachedList);
			foreach (var iteratedReceiver in cachedList)
				iteratedReceiver.OnTriggerExitDisabled(thisCollider);
		}
		catch (Exception e)
		{
			Debug.LogError(e.Message);
		}
		finally
		{
			ListPool<ITriggerExitDisabledListener>.Release(cachedList);
		}
	}

	private void NotifyDisabledCollision(Collider thisCollider, Collider otherCollider)
	{
		var cachedList = ListPool<ICollisionExitDisabledListener>.Get();
		otherCollider.GetComponents<ICollisionExitDisabledListener>(cachedList);

		try
		{
            foreach (var iteratedReceiver in cachedList)
				iteratedReceiver.OnCollisionExitDisabled(otherCollider, thisCollider);

			// By default, unity sends message to the attached body's game object too. This will notify to collided collider's body
			var otherAttachedBody = otherCollider.GetBody();
			if (!otherAttachedBody)
				return;

			var otherAttachedBodyGO = otherAttachedBody.gameObject;
			if (otherCollider.gameObject == otherAttachedBodyGO)
				return;

			otherAttachedBodyGO.GetComponents<ICollisionExitDisabledListener>(cachedList);
			foreach (var iteratedReceiver in cachedList)
				iteratedReceiver.OnCollisionExitDisabled(otherCollider, thisCollider);
		}
		catch (Exception e)
		{
			Debug.LogError(e.Message);
		}
		finally
		{
			ListPool<ICollisionExitDisabledListener>.Release(cachedList);
		}
	}
}


#if UNITY_EDITOR

public sealed partial class DisabledColliderNotifier
{ }


#endif
