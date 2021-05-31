using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System;
using System.Linq;
using UnityEngine;

public class GameSaver : MonoBehaviour
{
    private SaveFileManager fileManager = new SaveFileManager();




    [ContextMenu("SaveGame")]
    public void SaveGame(string saveName)
	{
		var saveDirectory = new DirectoryInfo(fileManager.MainDirectory.FullName + $"\\Save_{saveName}");
		if (!saveDirectory.Exists)
		{
			saveDirectory.Create();
		}
		var oldSaveDirectory = saveDirectory.GetDirectories().OrderBy(x => x.CreationTimeUtc).FirstOrDefault();
		if (oldSaveDirectory.Exists)
		{
			foreach (var item in saveDirectory.GetDirectories().Where(x => x.Name != oldSaveDirectory.Name))
			{
				item.Delete();
			}
		}

		if (!fileManager.PreSaveDirectory.Exists)
		{
			throw new System.Exception($"File integrity violated: Directory \"{fileManager.PreSaveDirectory.FullName}\" deleted");
		}



		var newSaveDirectory = new DirectoryInfo(fileManager.MainDirectory.FullName + $"\\{DateTime.UtcNow.ToString("yyyy-MM-ddTHH-mm-ss")}");
        newSaveDirectory.Create();
        var PreSavePath = fileManager.PreSaveDirectory.FullName;
        var NewPath = newSaveDirectory.FullName;
        //Создать идентичное дерево каталогов
        foreach (string dirPath in Directory.GetDirectories(PreSavePath, "*", SearchOption.AllDirectories))
            Directory.CreateDirectory(dirPath.Replace(PreSavePath, NewPath));

        //Скопировать все файлы. И перезаписать(если такие существуют)
        foreach (string newPath in Directory.GetFiles(PreSavePath, "*.*", SearchOption.AllDirectories))
            File.Copy(newPath, newPath.Replace(PreSavePath, NewPath), true);

        CreateDirectoryTree(oldSaveDirectory, newSaveDirectory);

        oldSaveDirectory.Delete();

		/*
        var objectSavers = GameObject.FindObjectsOfType<ObjectSaver>();
        var OnSave = new List<ISaveble<ISaveModel>>();
        foreach (var item in objectSavers)
        {
            OnSave.Add(item as ISaveble<ISaveModel>);
        }
        var qwe = new DirectoryInfo(Application.persistentDataPath + "\\Saves");
        if (!qwe.Exists)
        {
            qwe.Create();
        }
        var zxc = new FileInfo(qwe + "\\PlayerSave.json");
        if (!zxc.Exists)
        {
            zxc.Create();
        }
        List<ISaveModel> modelsToSave = new List<ISaveModel>();
        foreach (var item in OnSave)
        {
            modelsToSave.Add(item.Save());
        }
        var asd = JsonConvert.SerializeObject(modelsToSave, new JsonSerializerSettings
        {
            ContractResolver = new CustomContractResolver()
        });

        var writer = new StreamWriter(zxc.FullName);
        writer.Write(asd);
        writer.Close();
        Process.Start(@$"{qwe.FullName}");
        */
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
					  var lockalDirectory=	newWorkDirectory.CreateSubdirectory($"{item.Name}");
                      CreateDirectoryTree(item, lockalDirectory);
					}
				}
			}
		}
	}

	[ContextMenu("LoadGame")]
    public void LoadGame()
    {
        var qwe = new DirectoryInfo(Application.persistentDataPath + "\\Saves");
        if (!qwe.Exists)
        {
            qwe.Create();
        }
        var zxc = new FileInfo(qwe + "\\PlayerSave.json");
        if (!zxc.Exists)
        {
            zxc.Create();
        }

        var reader = new StreamReader(zxc.FullName);
        var raaa = reader.ReadToEnd();
        var ccccc = JsonConvert.DeserializeObject<List<ObjectSaverModel>>(raaa);
        var onDestroy = ObjectSaver.UnicalHashController.Where(x => !ccccc.Where(y => y.InstanceId == x.Key).Any() && !ccccc.Where(y => y.PersonalHash == x.Value).Any()).ToList();
        var destroed = new List<ObjIndexFinger>();
        foreach (var item in onDestroy)
        {
            if (item.Saver != null)
            {
                Destroy(item.Saver.gameObject);
                destroed.Add(item);
            }
        }
        foreach (var item in destroed)
        {
            ObjectSaver.UnicalHashController.Remove(item);
        }
        for (int i = 0; i < ccccc.Count; i++)
        {
            var asd = ObjectSaver.UnicalHashController.ToList();
            var sameObj = ObjectSaver.UnicalHashController.Where(x => x.Key == ccccc[i].InstanceId || x.Value == ccccc[i].PersonalHash).ToList();
            if (!sameObj.Any())
            {
                if (ccccc[i].SaveInstant)
                {
                    var pref = Resources.Load<GameObject>(ccccc[i].PrefabPath.Replace("Assets/Resources/", "").Replace(".prefab", ""));
                    var obj = GameObject.Instantiate(pref);
                    obj.GetComponent<ObjectSaver>().Load(ccccc[i]);
                }

            }
            else
            {
                var obj = sameObj.FirstOrDefault();
                obj.Saver.Load(ccccc[i]);
            }
        }

    }
}
