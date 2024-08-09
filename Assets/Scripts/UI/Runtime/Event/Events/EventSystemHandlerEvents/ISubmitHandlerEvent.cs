using UnityEngine.EventSystems;

public partial class ISubmitHandlerEvent : MonoBehaviourEventBase<BaseEventDataArgs>, ISubmitHandler
{
	public void OnSubmit(BaseEventData eventData)
    {
		Raise(new()
		{
			EventData = eventData
		});
	}
}


#if UNITY_EDITOR

public partial class ISubmitHandlerEvent { }


#endif