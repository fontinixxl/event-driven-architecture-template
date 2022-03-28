using System.Collections;
using UnityEngine;


// [CreateAssetMenu(menuName = "SaveSystem", fileName = "SaveSystem", order = 0)]
public class SaveSystem : ScriptableObject
{
	[SerializeField] private VoidEventChannelSO _saveSettingsEvent = default;
	[SerializeField] private LoadEventChannelSO _loadLocation = default;
	[SerializeField] private SettingsSO _currentSettings = default;

	public string saveFilename = "save.data";
	public string backupSaveFilename = "save.data.bak";
	public Save saveData = new Save();

	void OnEnable()
	{
		_saveSettingsEvent.OnEventRaised += SaveSettings;
		_loadLocation.OnLoadingRequested += CacheLoadLocations;
	}

	void OnDisable()
	{
		_saveSettingsEvent.OnEventRaised -= SaveSettings;
		_loadLocation.OnLoadingRequested -= CacheLoadLocations;
	}

	private void CacheLoadLocations(GameSceneSO locationToLoad, bool showLoadingScreen, bool fadeScreen)
	{
		LocationSO locationSO = locationToLoad as LocationSO;
		if (locationSO)
		{
			saveData._locationId = locationSO.Guid;
		}

		SaveDataToDisk();
	}

	public bool LoadSaveDataFromDisk()
	{
		if (FileManager.LoadFromFile(saveFilename, out var json))
		{
			saveData.LoadFromJson(json);
			return true;
		}

		return false;
	}

	/// <summary>
	/// TODO: Implement in case you have an Inventory
	/// </summary>
	/// <returns></returns>
	public IEnumerator LoadSavedInventory()
	{
		yield break;
	}
	
	public void LoadSavedQuestlineStatus()
	{
		
	}

	public void SaveDataToDisk()
	{
		saveData._itemStacks.Clear();
		
		if (FileManager.MoveFile(saveFilename, backupSaveFilename))
		{
			if (FileManager.WriteToFile(saveFilename, saveData.ToJson()))
			{
				//Debug.Log("Save successful " + saveFilename);
			}
		}
	}

	public void WriteEmptySaveFile()
	{
		FileManager.WriteToFile(saveFilename, "");

	}
	public void SetNewGameData()
	{
		FileManager.WriteToFile(saveFilename, "");
		// Init Data, like Inventory or questlines
		
		SaveDataToDisk();

	}
	void SaveSettings()
	{
		saveData.SaveSettings(_currentSettings);

	}
}
