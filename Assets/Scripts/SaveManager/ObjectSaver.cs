using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;

public class ObjectSaver : MonoBehaviour, ISaveble<ObjectSaverModel>, ISaveble<ISaveModel>
{
	private void Awake()
	{
		var contance = GameController.gameController.UnicalHashesSaver.UnicalHashController.Where(x => x.Value == PersonalHash);
		if (contance.Count() > 1)
		{
			personalHash = Guid.NewGuid().ToString();
			var id = gameObject.GetInstanceID();
			GameController.gameController.UnicalHashesSaver.UnicalHashController.Add(new ObjIndexFinger() { Key = id, Value = personalHash, Saver = this });
		}
		contance = GameController.gameController.UnicalHashesSaver.UnicalHashController.Where(x => x.Key == gameObject.GetInstanceID());
		if (contance.Count() == 0)
		{
			if (string.IsNullOrEmpty(personalHash) || GameController.gameController.UnicalHashesSaver.UnicalHashController.Where(x => x.Value == PersonalHash).Count() > 0)
			{
				personalHash = Guid.NewGuid().ToString();
			}
			var id = gameObject.GetInstanceID();
			GameController.gameController.UnicalHashesSaver.UnicalHashController.Add(new ObjIndexFinger() { Key = id, Value = personalHash, Saver = this });
		}
	}

	private void Start()
	{
		
		
	}
	private void Update()
	{
		

	}
	private void OnValidate()
	{
		/*
		if (!Application.isPlaying)
		{
			


			foreach (var item in GameController.gameController.UnicalHashesSaver.UnicalHashController)
			{
				Debug.Log($"{gameObject.name} : {item.Key} : {item.Value}");
			}
		}*/
	}
	private void OnDestroy()
	{
		GameController.gameController.UnicalHashesSaver.UnicalHashController.Remove(GameController.gameController.UnicalHashesSaver.UnicalHashController.Where(x=>x.Key==gameObject.GetInstanceID()).FirstOrDefault());
	}
	[SerializeField]
	private string personalHash = null;
	public string PersonalHash { get { return personalHash; } set { personalHash = value; } }

	public bool SaveInstant = true;

	public MonoBehaviour[] scripts;
	public List<ISaveble<ISaveModel>> OnSave;


	public Type getTT()
	{
		return typeof(ObjectSaverModel);
	}
	public void Load(ObjectSaverModel model)
	{
		var OnSave = new List<ISaveble<ISaveModel>>();
		foreach (var item in scripts)
		{
			OnSave.Add(item as ISaveble<ISaveModel>);
		}
		for (int i = 0; i < model.SaveModels.Count; i++)
		{
			var saveObj = OnSave.Where(x => x.getTT() == model.SaveModels[i].GetType()).FirstOrDefault();
			if (saveObj != null)
			{
				saveObj.Load(model.SaveModels[i]);
			}
		}
		PersonalHash = model.PersonalHash;
		
	}

	public ObjectSaverModel Save()
	{
		var result = new ObjectSaverModel();
		OnSave = new List<ISaveble<ISaveModel>>();
		foreach (var item in scripts)
		{
			OnSave.Add(item as ISaveble<ISaveModel>);
		}
		List<ISaveModel> modelsToSave = new List<ISaveModel>();
		foreach (var item in OnSave)
		{
			modelsToSave.Add(item.Save());
		}
		result.SaveModels = modelsToSave;
		result.PersonalHash = PersonalHash;
		result.InstanceId = gameObject.GetInstanceID();
		result.SaveInstant = SaveInstant;
		if (SaveInstant)
		{
			
			result.PrefabPath = GameController.gameController.RecourseManager.recourses.Where(x => gameObject.name.Contains(x.prefab.name)).FirstOrDefault()?.prefabPath;
		}
		return result;
	}

	public void Load(ISaveModel model)
	{
		Load(model as ObjectSaverModel);
	}

	ISaveModel ISaveble<ISaveModel>.Save()
	{
		return Save();
	}







	/*
    [ContextMenu("SaveFile")]
    public void SaveFile()
    {
        OnSave = new List<ISaveble<ISaveModel>>();
		foreach (var item in scripts)
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
    }
    [ContextMenu("Loaddd")]
    public void Loaddd()
    {
        OnSave = new List<ISaveble<ISaveModel>>();
        foreach (var item in scripts)
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

        var reader = new StreamReader(zxc.FullName);
        var raaa = reader.ReadToEnd();
        var ccccc = JsonConvert.DeserializeObject<List<ISaveModel>>(raaa);
		for (int i = 0; i < ccccc.Count; i++)
		{
            var saveObj= OnSave.Where(x => x.getTT() == ccccc[i].GetType()).FirstOrDefault();
			if (saveObj != null)
			{
                saveObj.Load(ccccc[i]);
			}
		}
            
   
    }*/

}
public class ObjIndexFinger
{
	public int Key { get; set; }
	public string Value { get; set; }
	public ObjectSaver Saver { get; set; }
}