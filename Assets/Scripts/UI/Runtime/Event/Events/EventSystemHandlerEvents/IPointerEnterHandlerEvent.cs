using UnityEngine.EventSystems;

public partial class IPointerEnterHandlerEvent : MonoBehaviourEventBase<PointerEventDataArgs>, IPointerEnterHandler
{
	public void OnPointerEnter(PointerEventData eventData)
    {
        Raise(new ()
		{
			EventData = eventData
		});
    }
}


#if UNITY_EDITOR

public partial class IPointerEnterHandlerEvent { }


#endif