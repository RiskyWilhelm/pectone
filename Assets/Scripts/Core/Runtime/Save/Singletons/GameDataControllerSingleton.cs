using System.IO;
using UnityEngine;
using UnityEngine.Events;

public sealed partial class GameDataControllerSingleton : MonoBehaviourSingletonBase<GameDataControllerSingleton>
{
	[Header("GameDataControllerSingleton Events")]
	#region GameDataControllerSingleton Events

	public UnityEvent<GameData> onSave = new();

	public UnityEvent<GameData> onLoad = new();


	#endregion

	#region GameDataControllerSingleton Save

	private static GameData _data;

	public static GameData Data
	{
		get
		{
			if (_data == null)
				Instance.LoadFromFile();

            return _data;
		}
	}

	public static string FullSavePath => Path.Combine(Application.persistentDataPath, "MainSave.json");


	#endregion


	// Update
	public void LoadFromFile()
    {
		try
		{
			IOUtils.Load<GameData>(FullSavePath, out _data);
			onLoad?.Invoke(_data);
		}
		catch
		{
			UseFreshSave();
		}
    }

    public void SaveToFile()
    {
        IOUtils.Save<GameData>(_data, FullSavePath);
		onSave?.Invoke(_data);
    }

	public void DeleteFile()
	{
		IOUtils.Delete(FullSavePath);
	}

	public void UseFreshSave()
	{
		_data = new();
		onLoad?.Invoke(_data);
	}
}


#if UNITY_EDITOR

public sealed partial class GameDataControllerSingleton
{ }

#endif