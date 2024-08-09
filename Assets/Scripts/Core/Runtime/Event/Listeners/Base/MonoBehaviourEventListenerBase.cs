using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class MonoBehaviourEventListenerBase<EventArgsType> : MonoBehaviour
	where EventArgsType : EventArgs
{
	[Header("MonoBehaviourEventListenerBase Event")]
	#region MonoBehaviourEventListenerBase Event

	[Tooltip("Registers and unregisters at OnEnable() and OnDisable()")]
	public List<MonoBehaviourEventBase<EventArgsType>> primaryEventsList = new();

	[Space]
	public UnityEvent<EventArgsType> response = new();


	#endregion


	// Initialize
	private void OnEnable()
	{
        foreach (var iteratedEvent in primaryEventsList)
            RegisterToEvent(iteratedEvent);
    }


	// Update
	public void RegisterToEvent(MonoBehaviourEventBase<EventArgsType> @event)
	{
		if (@event)
			@event.RegisterListener(this);
	}

	public void UnRegisterFromEvent(MonoBehaviourEventBase<EventArgsType> @event)
	{
		if (@event)
			@event.UnRegisterListener(this);
	}

	public void OnEventRaised(EventArgsType eventArgs)
	{
		response?.Invoke(eventArgs);
	}


	// Dispose
	private void OnDisable()
	{
		foreach (var iteratedEvent in primaryEventsList)
			UnRegisterFromEvent(iteratedEvent);
	}
}