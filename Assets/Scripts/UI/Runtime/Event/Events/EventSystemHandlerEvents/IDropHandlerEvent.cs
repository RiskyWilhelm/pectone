using UnityEngine.EventSystems;

public partial class IDropHandlerEvent : MonoBehaviourEventBase<PointerEventDataArgs>, IDropHandler
{
	public void OnDrop(PointerEventData eventData)
    {
		Raise(new()
		{
			EventData = eventData
		});
	}
}


#if UNITY_EDITOR

public partial class IDropHandlerEvent { }


#endif