using UnityEngine.EventSystems;

public partial class IInitializePotentialDragHandlerEvent : MonoBehaviourEventBase<PointerEventDataArgs>, IInitializePotentialDragHandler
{
	public void OnInitializePotentialDrag(PointerEventData eventData)
    {
		Raise(new()
		{
			EventData = eventData
		});
	}
}


#if UNITY_EDITOR

public partial class IInitializePotentialDragHandlerEvent { }


#endif