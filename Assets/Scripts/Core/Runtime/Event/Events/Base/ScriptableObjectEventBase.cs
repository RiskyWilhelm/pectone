using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract partial class ScriptableObjectEventBase<EventArgsType> : ScriptableObject
	where EventArgsType : EventArgs
{
	[Header("ScriptableObjectEventBase Event")]
	#region ScriptableObjectEventBase Event

	[Space]
	[Tooltip("Raised before other listener's Response")]
	public UnityEvent<EventArgsType> onRaised = new();

	[NonSerialized]
	private readonly List<MonoBehaviourEventListenerBase<EventArgsType>> listeners = new();


	#endregion


	// Update
	public virtual bool RegisterListener(MonoBehaviourEventListenerBase<EventArgsType> listener)
	{
		if (listeners.Contains(listener))
			return false;

		listeners.Add(listener);
		return true;
	}

	public virtual bool UnRegisterListener(MonoBehaviourEventListenerBase<EventArgsType> listener)
	{
		if (!listeners.Contains(listener))
			return false;

		listeners.Remove(listener);
		return true;
	}

	public virtual void Raise(EventArgsType eventArgs)
	{
		onRaised?.Invoke(eventArgs);

		for (int i = listeners.Count - 1; i >= 0; i--)
			listeners[i].OnEventRaised(eventArgs);
	}
}



#if UNITY_EDITOR

public abstract partial class ScriptableObjectEventBase<EventArgsType>
{ }

#endif