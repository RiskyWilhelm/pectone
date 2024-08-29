using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json.Converters;

public static partial class NewtonsoftJSONInitializer
{
	// Initialize
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void OnBeforeSceneLoad()
	{
		ConfigureSettings();
	}

	private static void ConfigureSettings()
	{
		JsonConvert.DefaultSettings = () => new JsonSerializerSettings
		{
			Formatting = Formatting.Indented,
			TypeNameHandling = TypeNameHandling.All,
			Converters = new JsonConverter[]
			{
				new VersionConverter(),
				new NewtonsoftColorConverter(),
				new NewtonsoftVector2Converter(),
				new NewtonsoftVector3Converter(),
				new NewtonsoftVector3IntConverter(),
				new NewtonsoftVector4Converter(),
				new NewtonsoftQuaternionConverter(),
				new NewtonsoftAssetReferenceConverter()
			}
		};
	}
}

#if UNITY_EDITOR

public static partial class NewtonsoftJSONInitializer
{
	[InitializeOnLoadMethod]
	private static void OnEditorInitializeOnLoad()
	{
		ConfigureSettings();
	}
}

#endif