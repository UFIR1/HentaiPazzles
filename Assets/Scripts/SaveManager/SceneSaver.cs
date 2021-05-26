using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

public class SceneSaver : MonoBehaviour
{

	
	[ContextMenu("SaveFile")]
    public void SaveFile()
    {
        var objectSavers = GameObject.FindObjectsOfType<ObjectSaver>();
        var OnSave =  new List<ISaveble<ISaveModel>>();
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

    }
    [ContextMenu("Loaddd")]
    public void Loaddd()
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
            if (item.Saver!=null)
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
            var sameObj= ObjectSaver.UnicalHashController.Where(x => x.Key == ccccc[i].InstanceId || x.Value == ccccc[i].PersonalHash).ToList();
			if (!sameObj.Any())
			{
				if (ccccc[i].SaveInstant)
				{
                    var pref = Resources.Load<GameObject>(ccccc[i].PrefabPath.Replace("Assets/Resources/", "").Replace(".prefab", ""));
                    var obj= GameObject.Instantiate(pref);
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
