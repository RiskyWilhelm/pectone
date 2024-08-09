using UnityEngine.EventSystems;

public partial class IDragHandlerEvent : MonoBehaviourEventBase<PointerEventDataArgs>, IDragHandler
{
	public void OnDrag(PointerEventData eventData)
    {
		Raise(new()
		{
			EventData = eventData
		});
	}
}


#if UNITY_EDITOR

public partial class IDragHandlerEvent { }


#endif