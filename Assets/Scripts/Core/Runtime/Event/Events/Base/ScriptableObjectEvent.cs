using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract partial class ScriptableObjectEvent : ScriptableObjectEventBase
{
	[Header("ScriptableObjectEvent Event")]
	#region ScriptableObjectEvent Event

	[Tooltip("Raised before other listener's Response")]
	[SerializeField]
	private UnityEvent onRaised = new();

	[SerializeField]
	private List<MonoBehaviourEventListener> listeners = new();


	#endregion


	// Update
	public virtual bool RegisterListener(MonoBehaviourEventListener listener)
	{
		if (listeners.Contains(listener))
			return false;

		listeners.Add(listener);
		return true;
	}

	public virtual bool UnRegisterListener(MonoBehaviourEventListener listener)
	{
		if (!listeners.Contains(listener))
			return false;

		listeners.Remove(listener);
		return true;
	}

	public void Raise()
	{
		onRaised?.Invoke();

		for (int i = listeners.Count - 1; i >= 0; i--)
			listeners[i].OnEventRaised();
	}
}


#if UNITY_EDITOR

public abstract partial class ScriptableObjectEvent
{ }


#endif