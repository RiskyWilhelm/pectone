using UnityEngine;

public sealed partial class SavedInstantiationRoot : SavedInstantiation
{
	#region SavedInstantiationRoot Other

	protected override bool CanGetAttachedToHandler
		=> false;


	#endregion


	// Initialize
	private void Awake()
	{
		if (UObjectUtils.IsInstantiatedInRuntime(this))
		{
			Debug.LogError("You cannot instantiate root!");
			Destroy(this.gameObject);
			return;
		}

		Initialize();
	}

	public void Initialize()
	{
		var isFoundLastSave = GameDataControllerSingleton.Data.rootInstantiationDatasDict.TryGetValue(_guid, out InstantiationData found);
		if (isFoundLastSave)
			_data = found;
		else
			GameDataControllerSingleton.Data.rootInstantiationDatasDict.Add(_guid, _data);

		InstantiateLastChildren();
	}


	// Dispose
	public override void DestroyWithSave()
	{
		GameDataControllerSingleton.Data.rootInstantiationDatasDict.Remove(_guid);
		base.DestroyWithSave();
	}
}


#if UNITY_EDITOR

public sealed partial class SavedInstantiationRoot
{ }


#endif
