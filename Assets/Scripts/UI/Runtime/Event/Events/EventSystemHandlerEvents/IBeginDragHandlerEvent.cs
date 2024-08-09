using UnityEngine.EventSystems;

public partial class IBeginDragHandlerEvent : MonoBehaviourEventBase<PointerEventDataArgs>, IBeginDragHandler, IDragHandler
{
	public void OnBeginDrag(PointerEventData eventData)
    {
		Raise(new()
		{
			EventData = eventData
		});
	}

	public void OnDrag(PointerEventData eventData)
	{ }
}


#if UNITY_EDITOR

public partial class IBeginDragHandlerEvent { }


#endif