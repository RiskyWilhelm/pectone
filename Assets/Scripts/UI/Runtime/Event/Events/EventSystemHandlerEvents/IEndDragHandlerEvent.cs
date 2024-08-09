using UnityEngine.EventSystems;

public partial class IEndDragHandlerEvent : MonoBehaviourEventBase<PointerEventDataArgs>, IEndDragHandler, IDragHandler
{
	public void OnDrag(PointerEventData eventData)
	{ }

	public void OnEndDrag(PointerEventData eventData)
    {
		Raise(new()
		{
			EventData = eventData
		});
	}
}

#if UNITY_EDITOR

public partial class IEndDragHandlerEvent { }


#endif