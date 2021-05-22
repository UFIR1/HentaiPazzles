using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;

public class Save : MonoBehaviour
{
    public ScriptableObject BaseHero;

    public MonoBehaviour[] scripts;
    public List<ISaveble<ISaveModel>> OnSave;
    // Start is called before the first frame update

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
        var asd = /*JsonUtility.ToJson*/JsonConvert.SerializeObject(modelsToSave, new JsonSerializerSettings
        {
            ContractResolver = new CustomContractResolver()
        });
        /*JsonConvert.SerializeObject(BaseHero, new JsonSerializerSettings
    {
        ContractResolver = new CustomContractResolver()
    });*/
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
       // var ttttt = new Swordsman() { Coins = 100 };
        var ccccc = JsonConvert.DeserializeObject<List<ISaveModel>>(raaa);
		for (int i = 0; i < ccccc.Count; i++)
		{
            var saveObj= OnSave.Where(x => x.getTT() == ccccc[i].GetType()).FirstOrDefault();
			if (saveObj != null)
			{
                saveObj.Load(ccccc[i]);
			}
            //var model = ccccc.Where(x => x.GetType() == OnSave[i].getTT()).FirstOrDefault();
		}
            
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public class CustomContractResolver : DefaultContractResolver
{



    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
        var propsToIgnore = typeof(Rigidbody2D).GetProperties().Select(p => p.Name).ToList();



        properties =
            properties.Where(p => !propsToIgnore.Contains(p.PropertyName)&&p.Writable&&p.Readable).ToList();



        return properties;
    }
}
public interface ISaveble<T> where T: ISaveModel 
{
    public string PersonalHash { get; set; }
    public Type getTT();
    public void Load(T model);
    public T Save();
}
[JsonConverter(typeof(ISaveModelConverter))]
public abstract class ISaveModel
{
    public string ObjType { get { return this.GetType().Name; } set { } }
    public abstract string SaveName { get; set; }
}

public class BaseSpecifiedConcreteClassConverter : DefaultContractResolver
{
    protected override JsonConverter ResolveContractConverter(Type objectType)
    {
        if (typeof(BaseBullet).IsAssignableFrom(objectType) && !objectType.IsAbstract)
            return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
        return base.ResolveContractConverter(objectType);
    }
}

public class ISaveModelConverter : JsonConverter
{
    static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new BaseSpecifiedConcreteClassConverter() };

    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(BaseBullet));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        switch (jo["ObjType"].Value<string>())
        {
            case nameof(BaseHeroSaveModel):
                return JsonConvert.DeserializeObject<BaseHeroSaveModel>(jo.ToString(), SpecifiedSubclassConversion);
            default:
                throw new Exception();
        }
        throw new NotImplementedException();
    }

    public override bool CanWrite
    {
        get { return false; }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException(); // won't be called because CanWrite returns false
    }


}