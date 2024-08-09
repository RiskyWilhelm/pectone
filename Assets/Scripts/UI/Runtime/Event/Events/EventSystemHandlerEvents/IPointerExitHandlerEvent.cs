using UnityEngine.EventSystems;

public partial class IPointerExitHandlerEvent : MonoBehaviourEventBase<PointerEventDataArgs>, IPointerExitHandler
{
	public void OnPointerExit(PointerEventData eventData)
    {
		Raise(new()
		{
			EventData = eventData
		});
	}
}


#if UNITY_EDITOR

public partial class IPointerExitHandlerEvent { }


#endif