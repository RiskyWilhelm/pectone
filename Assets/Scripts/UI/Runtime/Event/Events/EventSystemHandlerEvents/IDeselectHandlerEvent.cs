using UnityEngine.EventSystems;

public partial class IDeselectHandlerEvent : MonoBehaviourEventBase<BaseEventDataArgs>, IDeselectHandler
{
	public void OnDeselect(BaseEventData eventData)
    {
		Raise(new()
		{
			EventData = eventData
		});
	}
}


#if UNITY_EDITOR

public partial class IDeselectHandlerEvent { }


#endif