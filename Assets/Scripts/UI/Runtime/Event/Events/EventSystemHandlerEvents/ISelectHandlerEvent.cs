using UnityEngine.EventSystems;

public partial class ISelectHandlerEvent : MonoBehaviourEventBase<BaseEventDataArgs>, ISelectHandler
{
	public void OnSelect(BaseEventData eventData)
    {
		Raise(new()
		{
			EventData = eventData
		});
	}
}


#if UNITY_EDITOR

public partial class ISelectHandlerEvent { }


#endif