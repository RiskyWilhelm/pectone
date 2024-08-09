using UnityEngine.EventSystems;

public partial class IPointerUpHandlerEvent : MonoBehaviourEventBase<PointerEventDataArgs>, IPointerUpHandler, IPointerDownHandler
{
	public void OnPointerDown(PointerEventData eventData)
	{ }

	public void OnPointerUp(PointerEventData eventData)
    {
		Raise(new()
		{
			EventData = eventData
		});
	}
}


#if UNITY_EDITOR

public partial class IPointerUpHandlerEvent { }


#endif