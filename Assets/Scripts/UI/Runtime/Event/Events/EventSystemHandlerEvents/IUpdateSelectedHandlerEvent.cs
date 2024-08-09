using UnityEngine.EventSystems;

public partial class IUpdateSelectedHandlerEvent : MonoBehaviourEventBase<BaseEventDataArgs>, IUpdateSelectedHandler
{
	public void OnUpdateSelected(BaseEventData eventData)
    {
		Raise(new()
		{
			EventData = eventData
		});
	}
}


#if UNITY_EDITOR

public partial class IUpdateSelectedHandlerEvent { }


#endif