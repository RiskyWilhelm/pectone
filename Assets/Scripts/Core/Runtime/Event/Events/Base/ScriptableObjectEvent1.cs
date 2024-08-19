using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract partial class ScriptableObjectEvent<T0> : ScriptableObjectEventBase
{
	[Header("ScriptableObjectEvent<T0> Event")]
	#region ScriptableObjectEvent<T0> Event

	[Tooltip("Raised before other listener's Response")]
	[SerializeField]
	private UnityEvent<T0> onRaised = new();

	[SerializeField]
	private List<MonoBehaviourEventListener<T0>> listeners = new();


	#endregion


	// Update
	public virtual bool RegisterListener(MonoBehaviourEventListener<T0> listener)
	{
		if (listeners.Contains(listener))
			return false;

		listeners.Add(listener);
		return true;
	}

	public virtual bool UnRegisterListener(MonoBehaviourEventListener<T0> listener)
	{
		if (!listeners.Contains(listener))
			return false;

		listeners.Remove(listener);
		return true;
	}

	public void Raise(T0 arg0)
	{
		onRaised?.Invoke(arg0);

		for (int i = listeners.Count - 1; i >= 0; i--)
			listeners[i].OnEventRaised(arg0);
	}
}


#if UNITY_EDITOR

public abstract partial class ScriptableObjectEvent<T0>
{ }

#endif