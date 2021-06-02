using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;



public class CustomContractResolver : DefaultContractResolver
{



	protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
	{
		IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
		var propsToIgnore = typeof(Rigidbody2D).GetProperties().Select(p => p.Name).ToList();



		properties =
			properties.Where(p => !propsToIgnore.Contains(p.PropertyName) && p.Writable && p.Readable).ToList();



		return properties;
	}
}
public interface ISaveble<T> where T : ISaveModel
{
	//public string PersonalHash { get; set; }
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
		if (typeof(ISaveModel).IsAssignableFrom(objectType) && !objectType.IsAbstract)
			return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
		return base.ResolveContractConverter(objectType);
	}
}

public class ISaveModelConverter : JsonConverter
{
	static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new BaseSpecifiedConcreteClassConverter() };

	public override bool CanConvert(Type objectType)
	{
		return (objectType == typeof(ISaveModel));
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		JObject jo = JObject.Load(reader);
		switch (jo["ObjType"].Value<string>())
		{
			case nameof(BaseHeroSaveModel):
				return JsonConvert.DeserializeObject<BaseHeroSaveModel>(jo.ToString(), SpecifiedSubclassConversion);
			case nameof(TransformModel):
				return JsonConvert.DeserializeObject<TransformModel>(jo.ToString(), SpecifiedSubclassConversion); 
			case nameof(ObjectSaverModel):
				return JsonConvert.DeserializeObject<ObjectSaverModel>(jo.ToString(), SpecifiedSubclassConversion);
			case nameof(GameSaverModel):
				return JsonConvert.DeserializeObject<GameSaverModel>(jo.ToString(), SpecifiedSubclassConversion);
			case nameof(SceneSaverModel):
				return JsonConvert.DeserializeObject<SceneSaverModel>(jo.ToString(), SpecifiedSubclassConversion);
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