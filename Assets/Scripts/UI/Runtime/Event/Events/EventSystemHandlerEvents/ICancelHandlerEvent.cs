using UnityEngine.EventSystems;

public partial class ICancelHandlerEvent : MonoBehaviourEventBase<BaseEventDataArgs>, ICancelHandler
{
	public void OnCancel(BaseEventData eventData)
    {
		Raise(new()
		{
			EventData = eventData
		});
	}
}


#if UNITY_EDITOR

public partial class ICancelHandlerEvent { }


#endif