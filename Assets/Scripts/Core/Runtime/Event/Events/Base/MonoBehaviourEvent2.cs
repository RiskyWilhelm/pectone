using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract partial class MonoBehaviourEvent<T0, T1> : MonoBehaviourEventBase
{
	[Header("MonoBehaviourEvent<T0, T1> Event")]
	#region MonoBehaviourEvent<T0, T1> Event

	[Tooltip("Raised before other listener's Response")]
	[SerializeField]
	private UnityEvent<T0, T1> onRaised = new();

	[SerializeField]
	private List<MonoBehaviourEventListener<T0, T1>> listeners = new();


	#endregion


	// Update
	public virtual bool RegisterListener(MonoBehaviourEventListener<T0, T1> listener)
	{
		if (listeners.Contains(listener))
			return false;

		listeners.Add(listener);
		return true;
	}

	public virtual bool UnRegisterListener(MonoBehaviourEventListener<T0, T1> listener)
	{
		if (!listeners.Contains(listener))
			return false;

		listeners.Remove(listener);
		return true;
	}

	public void Raise(T0 arg0, T1 arg1)
	{
		onRaised?.Invoke(arg0, arg1);

		for (int i = listeners.Count - 1; i >= 0; i--)
			listeners[i].OnEventRaised(arg0, arg1);
	}
}


#if UNITY_EDITOR

public abstract partial class MonoBehaviourEvent<T0, T1>
{ }


#endif