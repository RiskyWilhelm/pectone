using UnityEngine.EventSystems;

public partial class IScrollHandlerEvent : MonoBehaviourEventBase<PointerEventDataArgs>, IScrollHandler
{
	public void OnScroll(PointerEventData eventData)
    {
		Raise(new()
		{
			EventData = eventData
		});
	}
}


#if UNITY_EDITOR

public partial class IScrollHandlerEvent { }


#endif