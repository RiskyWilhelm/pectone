using UnityEngine;
using UnityEngine.EventSystems;

public partial class IPointerMoveHandlerEvent : MonoBehaviourEventBase<PointerEventDataArgs>, IPointerMoveHandler
{
	public void OnPointerMove(PointerEventData eventData)
    {
		Raise(new()
		{
			EventData = eventData
		});
	}
}


#if UNITY_EDITOR

public partial class IPointerMoveHandlerEvent { }


#endif