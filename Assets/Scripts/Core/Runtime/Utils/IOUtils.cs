using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

// Save&Load Copyright belongs to: https://github.com/shapedbyrainstudios/save-load-system - I upgraded the code
public static class IOUtils
{
	public const string encryptionWord = "Encrypted";
	public const string backupExtension = ".backup";


	/// <summary> Replaces '/' with <see cref="Path.DirectorySeparatorChar"/> </summary>
	public static string FixPathByCorrectDirectorySeperator(string path)
	{
		return path.Replace('/', Path.DirectorySeparatorChar);
	}

	/// <inheritdoc cref="FixPathByCorrectDirectorySeperator(string)"/>
	public static string FixPathByCorrectDirectorySeperator(ref string path)
		=> path = FixPathByCorrectDirectorySeperator(path);

	/// <summary> Uses Newtonsoft JSON to deserialize </summary>
	/// <remarks> Use '/' in <paramref name="fullPathWithExtension"/> for platform relative dir seperator </remarks>
	public static void Load<LoadObjectType>(string fullPathWithExtension, out LoadObjectType loadedData, bool useDecryption = false, bool allowRestoreFromBackup = true)
	{
		loadedData = default;
		FixPathByCorrectDirectorySeperator(ref fullPathWithExtension);

		// load the serialized data from the file
		try
		{
			string dataToLoad = "";
			using (var stream = new FileStream(fullPathWithExtension, FileMode.Open))
			{
				using var reader = new StreamReader(stream);
				dataToLoad = reader.ReadToEnd();
			}

			if (useDecryption)
				dataToLoad = EncryptDecrypt(dataToLoad);

			loadedData = JsonConvert.DeserializeObject<LoadObjectType>(dataToLoad);
		}
		// Try to load backup if any exists
		catch (Exception e)
		{
			if (allowRestoreFromBackup)
			{
				Debug.LogWarning($"Error occured, attempting to Rollback: {e}");

				SaveBackupAsMainFile(fullPathWithExtension);
				Load<LoadObjectType>(fullPathWithExtension, out loadedData, useDecryption, false);
			}

			// if we hit here, one possibility is that the backup file is also corrupt
			throw;
		}
	}

	/// <summary> Uses Newtonsoft JSON to deserialize </summary>
	/// <remarks> Use '/' in <paramref name="fullPathWithExtension"/> for platform relative dir seperator </remarks>
	public static void Save<SaveObjectType>(SaveObjectType data, string fullPathWithExtension, bool useEncryption = false, bool createBackup = true)
	{
		string dataToStore = JsonConvert.SerializeObject(data);
		FixPathByCorrectDirectorySeperator(ref fullPathWithExtension);
		Directory.CreateDirectory(Path.GetDirectoryName(fullPathWithExtension));

		if (useEncryption)
			dataToStore = EncryptDecrypt(dataToStore);

		// write the serialized data to the file
		using (var stream = new FileStream(fullPathWithExtension, FileMode.Create))
		{
			using var writer = new StreamWriter(stream);
			writer.Write(dataToStore);
		}

		// verify the newly saved file can be loaded successfully
		Load<SaveObjectType>(fullPathWithExtension, out _, useEncryption, false);
			
		if (createBackup)
			File.Copy(fullPathWithExtension, (fullPathWithExtension + backupExtension), true);
	}

	public static void Delete(string fullPathWithExtension, bool deleteBackupIfExists = true)
	{
		File.Delete(fullPathWithExtension);

		if (deleteBackupIfExists)
		{
			try
			{
				File.Delete(fullPathWithExtension + backupExtension);
			}
			catch { }
		}
	}

	/// <summary> Simple implementation of XOR encryption </summary>
	public static string EncryptDecrypt(string data)
	{
		var stringBuilder = new StringBuilder(data.Length);

		for (int i = 0; i < data.Length; i++)
			stringBuilder.Append((char)(data[i] ^ encryptionWord[i % encryptionWord.Length]));

		return stringBuilder.ToString();
	}

	public static void SaveBackupAsMainFile(string fullPathWithExtension)
	{
		string backupFilePath = (fullPathWithExtension + backupExtension);
		FixPathByCorrectDirectorySeperator(ref backupFilePath);

		File.Copy(backupFilePath, fullPathWithExtension, true);
	}
}