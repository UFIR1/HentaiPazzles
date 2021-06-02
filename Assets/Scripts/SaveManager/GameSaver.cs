using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class GameSaver : MonoBehaviour, ISaveble<GameSaverModel>, ISaveble<ISaveModel>
{
	//private static scenetransition
	private SaveFileManager fileManager;
	public string CurrentSceneName;
	private void Awake()
	{

		fileManager = new SaveFileManager();
		if (CurrentSceneName == null)
		{
			CurrentSceneName = SceneManager.GetActiveScene().name;
		}
	}

	[ContextMenu("SaveCurrentScene")]
	public bool SaveCurrentScene()
	{




		var sceneSaveModel = GameObject.FindObjectOfType<SceneSaver>().Save();



		var result = JsonConvert.SerializeObject(sceneSaveModel, new JsonSerializerSettings
		{
			ContractResolver = new CustomContractResolver()
		});

		var sceneSaveDirectory = new DirectoryInfo(fileManager.PreSaveDirectory + "\\Scenes");
		if (!sceneSaveDirectory.Exists)
		{
			sceneSaveDirectory.Create();
		}
		var sceneFile = new FileInfo(sceneSaveDirectory + $"\\{SceneManager.GetActiveScene().name}.json");
		var sceneWriter = new StreamWriter(sceneFile.FullName);
		sceneWriter.Write(result);
		sceneWriter.Close();
		Process.Start(@$"{fileManager.MainDirectory.FullName}");

		var gameSattingsModel = Save();



		var gameSattings = JsonConvert.SerializeObject(gameSattingsModel, new JsonSerializerSettings
		{
			ContractResolver = new CustomContractResolver()
		});

		var gameSettings = new FileInfo(fileManager.PreSaveDirectory + "\\GameSettings.json");
		var settingsWriter = new StreamWriter(gameSettings.FullName);
		settingsWriter.Write(gameSattings);
		settingsWriter.Close();

		return true;
	}
	private void OnLevelWasLoaded(int level)
	{
		
	}

	[ContextMenu("SaveGame")]
	public void SaveGame()
	{
		string saveName = "testSave";

		var saveDirectory = new DirectoryInfo(fileManager.MainDirectory.FullName + $"\\Save_{saveName}");
		if (!saveDirectory.Exists)
		{
			saveDirectory.Create();
		}
		var oldSaveDirectory = saveDirectory.GetDirectories().OrderBy(x => x.CreationTimeUtc).FirstOrDefault();
		if (oldSaveDirectory?.Exists == true)
		{
			foreach (var item in saveDirectory.GetDirectories().Where(x => x.Name != oldSaveDirectory.Name))
			{
				item.Delete(true);
			}
		}

		if (!fileManager.PreSaveDirectory.Exists)
		{
			throw new System.Exception($"File integrity violated: Directory \"{fileManager.PreSaveDirectory.FullName}\" deleted");
		}



		var newSaveDirectory = new DirectoryInfo(saveDirectory + $"\\{DateTime.UtcNow.ToString("yyyy-MM-ddTHH-mm-ss")}");
		newSaveDirectory.Create();
		var PreSavePath = fileManager.PreSaveDirectory.FullName;
		var NewPath = newSaveDirectory.FullName;
		//Создать идентичное дерево каталогов
		foreach (string dirPath in Directory.GetDirectories(PreSavePath, "*", SearchOption.AllDirectories))
			Directory.CreateDirectory(dirPath.Replace(PreSavePath, NewPath));

		//Скопировать все файлы. И перезаписать(если такие существуют)
		foreach (string newPath in Directory.GetFiles(PreSavePath, "*.*", SearchOption.AllDirectories))
			File.Copy(newPath, newPath.Replace(PreSavePath, NewPath), true);
		if (oldSaveDirectory != null)
		{
			CreateDirectoryTree(oldSaveDirectory, newSaveDirectory);
		}

		oldSaveDirectory?.Delete(true);
		Process.Start(@$"{fileManager.MainDirectory.FullName}");

	}

	private void CreateDirectoryTree(DirectoryInfo oldWorkDirectory, DirectoryInfo newWorkDirectory)
	{
		if (oldWorkDirectory.Exists)
		{
			var newFiles = newWorkDirectory.GetFiles();
			var newDirectories = newWorkDirectory.GetDirectories();
			foreach (var item in oldWorkDirectory.GetFiles())
			{
				if (newFiles != null)
				{
					if (!newFiles.Where(x => x.Name == item.Name).Any())
					{
						File.Create(newWorkDirectory.FullName + $"\\{item.Name}");
					}
				}
			}
			foreach (var item in oldWorkDirectory.GetDirectories())
			{
				if (newDirectories != null)
				{
					if (!newDirectories.Where(x => x.Name == item.Name).Any())
					{
						var lockalDirectory = newWorkDirectory.CreateSubdirectory($"{item.Name}");
						CreateDirectoryTree(item, lockalDirectory);
					}
				}
			}
		}
	}
	private AsyncOperation loadingSceneOperation;
	[ContextMenu("LoadGame")]
	public void LoadGame()
	{
		string saveName = "testSave";
		var saveDirectory = new DirectoryInfo(fileManager.MainDirectory.FullName + $"\\Save_{saveName}").GetDirectories().OrderBy(x => x.CreationTimeUtc).FirstOrDefault();
		if (!(saveDirectory?.Exists==true))
		{
			throw new Exception("Сохранение отсутствует");
		}
		var gameConfigFile = new FileInfo(saveDirectory + "\\GameSettings.json");
		var scenesDirectory = new DirectoryInfo(saveDirectory + "\\Scenes");
		if (gameConfigFile.Exists && scenesDirectory.Exists)
		{
			var gameConfigReader = new StreamReader(gameConfigFile.OpenRead());
			var gameConfigText = gameConfigReader.ReadToEnd();
			gameConfigReader.Close();
			var gameConfig = JsonConvert.DeserializeObject<GameSaverModel>(gameConfigText);
			loadingSceneOperation = SceneManager.LoadSceneAsync(gameConfig.CurrentSceneName);
			loadingSceneOperation.completed += loadingSceneFineshed;
			Loadingsc();

		}

	}

	private void loadingSceneFineshed(AsyncOperation obj)
	{
		string saveName = "testSave";
		var saveDirectory = new DirectoryInfo(fileManager.MainDirectory.FullName + $"\\Save_{saveName}").GetDirectories().OrderBy(x => x.CreationTimeUtc).FirstOrDefault();
		var scenesDirectory = new DirectoryInfo(saveDirectory + "\\Scenes");
		var sceneToLoadFile = new FileInfo(scenesDirectory + $"\\{SceneManager.GetActiveScene().name}.json");
		if (sceneToLoadFile.Exists)
		{
			string sceneText = new StreamReader(sceneToLoadFile.FullName).ReadToEnd();
			var sceneModel = JsonConvert.DeserializeObject<SceneSaverModel>(sceneText);
			var loadingScene = GameObject.FindObjectOfType<SceneSaver>();
			loadingScene.Load(sceneModel);
		}
	}

	public void Loadingsc()
	{

		
	}

	public System.Type getTT()
	{
		return typeof(GameSaverModel);
	}

	public void Load(ISaveModel model)
	{
		Load(model);
	}

	ISaveModel ISaveble<ISaveModel>.Save()
	{
		return Save();
	}

	public void Load(GameSaverModel model)
	{
		CurrentSceneName = model.CurrentSceneName;
	}

	public GameSaverModel Save()
	{
		return new GameSaverModel()
		{
			CurrentSceneName = this.CurrentSceneName
		};
	}
}
public class GameSaverModel : ISaveModel
{
	public string CurrentSceneName { get; set; }
	public override string SaveName { get => "gamesave"; set  { } }
}
