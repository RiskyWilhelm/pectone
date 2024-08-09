using UnityEngine.EventSystems;

public partial class IPointerClickHandlerEvent : MonoBehaviourEventBase<PointerEventDataArgs>, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
    {
		Raise(new()
		{
			EventData = eventData
		});
	}
}


#if UNITY_EDITOR

public partial class IPointerClickHandlerEvent { }


#endif