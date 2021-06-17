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
	private static SaveFileManager fileManager;
	public string CurrentSceneName;
	public static string CurrentSaveName;
	public static ExternalOnLoadFinished externalOnLoadFinished;
	public delegate void ExternalOnLoadFinished();

	private void Awake()
	{

		DontDestroyOnLoad(this);
		fileManager = new SaveFileManager();
		if (CurrentSceneName == null)
		{
			CurrentSceneName = SceneManager.GetActiveScene().name;
		}

	}
	private void Start()
	{
#if UNITY_EDITOR
		DeletePreSave();
		CurrentSaveName = "DebugSave";
#endif
	}

	public List<GameSaverViewModel> GetExistingSaves()
	{
		var savesPath = new DirectoryInfo(fileManager.MainDirectory.FullName);
		var result = new List<GameSaverViewModel>();
		foreach (var item in savesPath.GetDirectories().Where(x => x.Name.Contains("Save_")))
		{
			var newSaveView = new GameSaverViewModel()
			{
				Name = item.Name.Replace("Save_", ""),
				CreateTime = item.CreationTime,


			};
			var saveDirectory = item.GetDirectories().OrderBy(x => x.CreationTimeUtc).FirstOrDefault();
			if (item.GetDirectories().Any())
			{
				newSaveView.UpdateTime = saveDirectory.CreationTime;
			}
			Texture2D texture = new Texture2D(ScreenShot.Width, ScreenShot.Height);
			var saveImg = new FileInfo(saveDirectory.FullName + $"\\{FileKeyWord.GameImg}.jpg");
			if (saveImg.Exists)
			{
				byte[] bytes = File.ReadAllBytes(saveImg.FullName);
				texture.LoadImage(bytes);
				texture.Apply();
				newSaveView.Texture = texture;
			}
			result.Add(newSaveView);
		}
		return result;
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

		var sceneWriter = new StreamWriter(File.Open(sceneFile.FullName, FileMode.Create, FileAccess.ReadWrite));
		sceneWriter.Write(result);
		sceneWriter.Close();

		//Process.Start(@$"{fileManager.MainDirectory.FullName}");

		var gameSattingsModel = Save();



		var gameSattings = JsonConvert.SerializeObject(gameSattingsModel, new JsonSerializerSettings
		{
			ContractResolver = new CustomContractResolver()
		});

		var gameSettings = new FileInfo(fileManager.PreSaveDirectory + "\\GameSettings.json");
		var settingsWriter = new StreamWriter(gameSettings.FullName);
		settingsWriter.Write(gameSattings);
		settingsWriter.Close();
		SaveUniqueObjects();
		return true;
	}

	public void SaveUniqueObjects()
	{
		var saveDirectory = new DirectoryInfo(fileManager.PreSaveDirectory.FullName);
		var uniqueObjectsDirectory = new DirectoryInfo(saveDirectory + $"\\UniqueObjects");
		if (!uniqueObjectsDirectory.Exists)
		{
			uniqueObjectsDirectory.Create();
		}
		var uniqueObjects = GameObject.FindObjectsOfType<ObjectSaver>().Where(x => x.itsUniqueObject);
		var objectsModels = new List<UniqueObjectModel>();
		foreach (var item in uniqueObjects)
		{
			objectsModels.Add(new UniqueObjectModel() { SceneName = SceneManager.GetActiveScene().name, objectModel = item.Save(), Name = item.name });
		}
		foreach (var item in objectsModels)
		{
			var savebleFile = new FileInfo(uniqueObjectsDirectory + $"\\object_{item.Name}.json");
			var writerStream = File.Open(savebleFile.FullName, FileMode.Create, FileAccess.ReadWrite);
			var writer = new StreamWriter(writerStream);
			var textToWrite = JsonConvert.SerializeObject(item, new JsonSerializerSettings
			{
				ContractResolver = new CustomContractResolver()
			});
			writer.Write(textToWrite);
			writer.Close();
		}
	}

	[ContextMenu("SaveGame")]
	public void SaveGame(string saveName)
	{

		var oldMainSaveDirectory = new DirectoryInfo(fileManager.MainDirectory.FullName + $"\\Save_{CurrentSaveName}");
		var newMainSaveDirectory = new DirectoryInfo(fileManager.MainDirectory.FullName + $"\\Save_{saveName}");
		if (!newMainSaveDirectory.Exists)
		{
			newMainSaveDirectory.Create();
		}

		DirectoryInfo oldSaveDirectory = null;
		if (oldMainSaveDirectory.Exists)
		{
			if (oldMainSaveDirectory.GetDirectories().Any())
			{
				oldSaveDirectory = oldMainSaveDirectory.GetDirectories().OrderBy(x => x.CreationTimeUtc).FirstOrDefault();
				if (oldMainSaveDirectory != null)
				{
					if (oldSaveDirectory?.Exists == true)
					{
						if (oldMainSaveDirectory.Name == newMainSaveDirectory.Name)
						{
							foreach (var item in oldMainSaveDirectory.GetDirectories().Where(x => x.Name != oldSaveDirectory.Name))
							{
								item.Delete(true);
							}
						}
					}
				}
			}
		}


		/*if (!fileManager.PreSaveDirectory.Exists)
		{
			throw new System.Exception($"File integrity violated: Directory \"{fileManager.PreSaveDirectory.FullName}\" deleted");
		}*/



		var newSaveDirectory = new DirectoryInfo(newMainSaveDirectory + $"\\{DateTime.UtcNow.ToString("yyyy-MM-ddTHH-mm-ss")}");
		newSaveDirectory.Create();
		var PreSavePath = fileManager.PreSaveDirectory.FullName;
		var NewPath = newSaveDirectory.FullName;
		//Создать идентичное дерево каталогов
		foreach (string dirPath in Directory.GetDirectories(PreSavePath, "*", SearchOption.AllDirectories))
			Directory.CreateDirectory(dirPath.Replace(PreSavePath, NewPath));

		//Скопировать все файлы. И перезаписать(если такие существуют)
		foreach (string newPath in Directory.GetFiles(PreSavePath, "*.*", SearchOption.AllDirectories))
			File.Copy(newPath, newPath.Replace(PreSavePath, NewPath), true);

		#region imgGenerator
		var mainCamera = GameObject.FindGameObjectWithTag(Tags.MainCamera.ToString())?.GetComponent<Camera>();
		if (mainCamera != null)
		{
			byte[] photoBytes = CreatePhoto(mainCamera,2);
			var photoFile = new FileInfo(newSaveDirectory + $"\\{FileKeyWord.GameImg}.jpg");
			var photoStream= File.Open(photoFile.FullName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
			photoStream.Write(photoBytes, 0, photoBytes.Length);
			photoStream.Close();
		}
		#endregion

		if (oldSaveDirectory != null)
		{
			CreateDirectoryTree(oldSaveDirectory, newSaveDirectory);
		}
		if (oldMainSaveDirectory.Name == newMainSaveDirectory.Name)
		{
			oldSaveDirectory?.Delete(true);
		}
		CurrentSaveName = saveName;
		//Process.Start(@$"{fileManager.MainDirectory.FullName}");

	}

	private static byte[] CreatePhoto(Camera mainCamera, int divisor)
	{
		var height = Screen.height / divisor;
		var width = Screen.width / divisor;
		var photo = new Texture2D(width, height, TextureFormat.RGBA32, false);
		var rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
		mainCamera.targetTexture = rt;
		mainCamera.Render();
		RenderTexture.active = mainCamera.targetTexture;
		photo.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		mainCamera.targetTexture = null;
		RenderTexture.active = null;
		DestroyImmediate(rt);
		photo.Apply();
		var bytes = photo.EncodeToJPG(100);
		Destroy(photo);
		return bytes;
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
						File.Create(newWorkDirectory.FullName + $"\\{item.Name}").Close();
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
	public void LoadGame(string saveName)
	{

		CurrentSaveName = saveName;
		Action<AsyncOperation> action = loadingGameSceneWithUniqueObjectsFineshed;
		DeletePreSave();
		LoadSceneWithParam(saveName, action);

	}
	public void DeletePreSave()
	{
		if (fileManager.PreSaveDirectory.Exists)
		{
			fileManager.PreSaveDirectory?.Delete(true);
		}
		fileManager.PreSaveDirectory.Create();
	}
	public void LoadScene(string scaneName)
	{
		Action<AsyncOperation> action = loadingSceneWithUniqueObjectsFineshed;
		LoadSceneWithParam(CurrentSaveName, action, scaneName);
	}

	private void LoadSceneWithParam(string saveName, Action<AsyncOperation> action, string sceneName = null)
	{
		var saveMainDirectory = new DirectoryInfo(fileManager.MainDirectory.FullName + $"\\Save_{saveName}");
		DirectoryInfo saveDirectory = null;
		if (saveMainDirectory.Exists) {
			saveDirectory= saveMainDirectory.GetDirectories().OrderBy(x => x.CreationTimeUtc).FirstOrDefault();
		}
		else
		{
			saveDirectory = new DirectoryInfo(fileManager.PreSaveDirectory.FullName);
		}
		if (!(saveDirectory?.Exists == true))
		{
			throw new Exception("Сохранение отсутствует");
		}
		var gameConfigFile = new FileInfo(saveDirectory + "\\GameSettings.json");
		var scenesDirectory = new DirectoryInfo(saveDirectory + "\\Scenes");
		GameSaverModel gameConfig = null;
		string gameConfigText = null;
		if (gameConfigFile.Exists && scenesDirectory.Exists)
		{
			var gameConfigReader = new StreamReader(gameConfigFile.FullName);
			gameConfigText = gameConfigReader.ReadToEnd();
			gameConfigReader.Close();


		}
		if (sceneName == null)
		{
			if (gameConfigFile.Exists)
			{
				gameConfig = JsonConvert.DeserializeObject<GameSaverModel>(gameConfigText);
				loadingSceneOperation = SceneManager.LoadSceneAsync(gameConfig.CurrentSceneName);
				loadingSceneOperation.completed += action;
				return;
			}
		}
		else
		{
			loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
			loadingSceneOperation.completed += action;
			return;
		}
	}

	private void loadingGameSceneWithUniqueObjectsFineshed(AsyncOperation obj)
	{
		var saveDirectory = new DirectoryInfo(fileManager.MainDirectory.FullName + $"\\Save_{CurrentSaveName}").GetDirectories().OrderBy(x => x.CreationTimeUtc).FirstOrDefault();
		var scenesDirectory = new DirectoryInfo(saveDirectory + "\\Scenes");
		var sceneToLoadFile = new FileInfo(scenesDirectory + $"\\{SceneManager.GetActiveScene().name}.json");
		if (sceneToLoadFile.Exists)
		{
			var sceneReader = new StreamReader(sceneToLoadFile.FullName);
			string sceneText = sceneReader.ReadToEnd();
			sceneReader.Close();
			var sceneModel = JsonConvert.DeserializeObject<SceneSaverModel>(sceneText);
			var loadingScene = GameObject.FindObjectOfType<SceneSaver>();

			List<UniqueObjectModel> uniqueObjects = LoadUniqueObjects(saveDirectory, loadingScene);

			sceneModel.SaveableObjects.AddRange(uniqueObjects.Select(x => x.objectModel));



			loadingScene.Load(sceneModel);
		}
		ExternalVoidsOnLoadFinished();


	}
	private void loadingSceneWithUniqueObjectsFineshed(AsyncOperation obj)
	{




		(var sceneToLoadFile, var saveDirectory) = GetFileAndDirectoryToLoad();


		if (sceneToLoadFile.Exists)
		{
			var sceneReader = new StreamReader(sceneToLoadFile.FullName);
			string sceneText = sceneReader.ReadToEnd();
			sceneReader.Close();
			var sceneModel = JsonConvert.DeserializeObject<SceneSaverModel>(sceneText);
			var loadingScene = GameObject.FindObjectOfType<SceneSaver>();

			List<UniqueObjectModel> uniqueObjects = LoadUniqueObjects(saveDirectory, loadingScene);

			sceneModel.SaveableObjects.AddRange(uniqueObjects.Select(x => x.objectModel));



			loadingScene.Load(sceneModel);
		}
		ExternalVoidsOnLoadFinished();


	}
	private void ExternalVoidsOnLoadFinished()
	{
		externalOnLoadFinished();
	}
	public (FileInfo, DirectoryInfo) GetFileAndDirectoryToLoad()
	{
		FileInfo resultFile = null;
		DirectoryInfo resultDirectory = fileManager.PreSaveDirectory;

		var scenesPreDirectory = new DirectoryInfo(fileManager.PreSaveDirectory + "\\Scenes");
		resultFile = new FileInfo(scenesPreDirectory + $"\\{SceneManager.GetActiveScene().name}.json");
		if (!resultFile.Exists)
		{
			var saveDirectory = new DirectoryInfo(fileManager.MainDirectory.FullName + $"\\Save_{CurrentSaveName}");
			if (saveDirectory.Exists)
			{
				resultDirectory = saveDirectory.GetDirectories().OrderBy(x => x.CreationTimeUtc).FirstOrDefault();
				var scenesDirectory = new DirectoryInfo(resultDirectory + "\\Scenes");
				resultFile = new FileInfo(scenesDirectory + $"\\{SceneManager.GetActiveScene().name}.json");
			}
		}
		return (resultFile, resultDirectory);
	}



	private static List<UniqueObjectModel> LoadUniqueObjects(DirectoryInfo saveDirectory, SceneSaver ScaneSaver)
	{

		var uniqueObjects = new List<UniqueObjectModel>();
		if (fileManager.PreSaveDirectory.Exists)
		{
			var presaveUniqueObjectsDirectory = new DirectoryInfo(fileManager.PreSaveDirectory + "\\UniqueObjects");
			if (presaveUniqueObjectsDirectory.Exists)
			{
				var presaveuniqueObjectsFiles = presaveUniqueObjectsDirectory.GetFiles();

				foreach (var item in presaveuniqueObjectsFiles)
				{
					var uniqueObjectReader = new StreamReader(item.FullName);
					var uniqueObjectText = uniqueObjectReader.ReadToEnd();
					uniqueObjectReader.Close();
					var uniqueModel = JsonConvert.DeserializeObject<UniqueObjectModel>(uniqueObjectText);
					uniqueObjects.Add(uniqueModel);

				}
			}
		}

		if (saveDirectory.FullName != fileManager.PreSaveDirectory.FullName)
		{

			var uniqueObjectsDirectory = new DirectoryInfo(saveDirectory + "\\UniqueObjects");
			var uniqueObjectsFiles = uniqueObjectsDirectory.GetFiles();


			foreach (var item in uniqueObjectsFiles)
			{
				var uniqueObjectReader = new StreamReader(item.FullName);
				var uniqueObjectText = uniqueObjectReader.ReadToEnd();
				uniqueObjectReader.Close();
				var uniqueModel = JsonConvert.DeserializeObject<UniqueObjectModel>(uniqueObjectText);
				if (!uniqueObjectsFiles.Where(x => x.Name == uniqueModel.Name).Any())
				{
					uniqueObjects.Add(uniqueModel);
				}

			}
		}
		UnityEngine.Debug.Log($"loadingScene: {ScaneSaver.SaneName}");
		var uniqueObjectsToScene = uniqueObjects.Where(x => x.SceneName == ScaneSaver.SaneName);
		foreach (var item in uniqueObjectsToScene)
		{
			UnityEngine.Debug.Log($"item.Name: {item.Name}");
		}
		return uniqueObjectsToScene.ToList();
	}

	public void DeleteSave(string savename)
	{
		var saveDirectory = new DirectoryInfo(fileManager.MainDirectory + $"\\Save_{savename}");
		if (saveDirectory.Exists)
		{
			saveDirectory.Delete(true);
		}
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
public class GameSaverViewModel
{
	public string Name { get; set; }
	public Texture2D Texture { get; set; }
	public DateTime CreateTime { get; set; }
	public DateTime UpdateTime { get; set; }

}
public class GameSaverModel : ISaveModel
{
	public string CurrentSceneName { get; set; }
	public override string SaveName { get => "gamesave"; set { } }
}
public static class ScreenShot
{
	private static int width = 100;
	private static int height = 100;

	public static int Width { get => width; set => width = value; }
	public static int Height { get => height; set => height = value; }
}

public class UniqueObjectModel : ISaveModel
{
	public string SceneName { get; set; }
	[JsonIgnore]
	public string Name { get; set; }
	public ObjectSaverModel objectModel { get; set; }
	public override string SaveName { get => "UniqueModel"; set { } }
}