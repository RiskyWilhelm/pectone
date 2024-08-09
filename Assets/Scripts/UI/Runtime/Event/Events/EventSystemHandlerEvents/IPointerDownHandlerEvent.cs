using UnityEngine.EventSystems;

public partial class IPointerDownHandlerEvent : MonoBehaviourEventBase<PointerEventDataArgs>, IPointerDownHandler
{
	public void OnPointerDown(PointerEventData eventData)
    {
		Raise(new()
		{
			EventData = eventData
		});
	}
}


#if UNITY_EDITOR

public partial class IPointerDownHandlerEvent { }


#endif