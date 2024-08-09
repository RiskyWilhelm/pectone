using UnityEngine.EventSystems;

public partial class IMoveHandlerEvent : MonoBehaviourEventBase<AxisEventDataArgs>, IMoveHandler
{
	public void OnMove(AxisEventData eventData)
    {
		Raise(new()
		{
			EventData = eventData
		});
	}
}


#if UNITY_EDITOR

public partial class IMoveHandlerEvent { }


#endif