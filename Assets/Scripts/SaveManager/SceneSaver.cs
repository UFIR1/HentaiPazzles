using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class SceneSaver : MonoBehaviour, ISaveble<SceneSaverModel>, ISaveble<ISaveModel>
{
    public string SaneName;
	private void Awake()
	{
        SaneName = gameObject.scene.name;
    }

    public System.Type getTT()
	{
        return typeof(SceneSaverModel);
	}

	public void Load(ISaveModel model)
	{
        Load(model);
	}

	ISaveModel ISaveble<ISaveModel>.Save()
	{
        return Save();
	}

	public void Load(SceneSaverModel model)
	{

        var ccccc = model.SaveableObjects;
        var onDestroy = ObjectSaver.UniqueHashController.Where(x => !ccccc.Where(y => y.InstanceId == x.Key).Any() && !ccccc.Where(y => y.PersonalHash == x.Value).Any()).ToList();
        var destroed = new List<ObjIndexFinger>();
        foreach (var item in onDestroy)
        {
            if (item.Saver != null && item.Saver.dontDestroyMe==false)
            {
                Destroy(item.Saver.gameObject);
                destroed.Add(item);
            }
        }
        foreach (var item in destroed)
        {
            ObjectSaver.UniqueHashController.Remove(item);
        }
        for (int i = 0; i < ccccc.Count; i++)
        {
            var asd = ObjectSaver.UniqueHashController.ToList();
            var sameObj = ObjectSaver.UniqueHashController.Where(x => x.Key == ccccc[i].InstanceId || x.Value == ccccc[i].PersonalHash).ToList();
            if (!sameObj.Any())
            {
                if (ccccc[i].SaveInstant)
                {
                    try
                    {
                        var pref = Resources.Load<GameObject>(ccccc[i].PrefabPath.Replace("Assets/Resources/", "").Replace(".prefab", ""));
                        var obj = GameObject.Instantiate(pref);
                        obj.GetComponent<ObjectSaver>().Load(ccccc[i]);
                    }
					catch
					{
                        UnityEngine.Debug.LogError($"Type: {ccccc[i].ObjType} SaveName: {ccccc[i].SaveName} Path: {ccccc[i].PrefabPath}");
					}
                  
                }

            }
            else
            {
                var obj = sameObj.FirstOrDefault();
                obj.Saver.Load(ccccc[i]);
            }
        }

    }

    public SceneSaverModel Save()
	{
        var objectSavers = GameObject.FindObjectsOfType<ObjectSaver>().Where(x=>x.itsUniqueObject!=true);
        var toSave = new List<ISaveble<ISaveModel>>();
        foreach (var item in objectSavers)
        {
            toSave.Add(item as ISaveble<ISaveModel>);
        }
        var modelsToSave = new List<ObjectSaverModel>();
		foreach (var item in toSave)
		{
            modelsToSave.Add(item.Save() as ObjectSaverModel);
		}
        var Result = new SceneSaverModel()
        {
            SaveableObjects = modelsToSave
        };
        return Result;
    }
}
