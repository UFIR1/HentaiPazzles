using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[JsonConverter(typeof(BaseConverter))]
public abstract class BaseBullet : MonoBehaviour
{
	public string ObjType { get {return this.GetType().Name; } set { } }
	public int damage;
	private BaseHero shooter;
	public float speed;
	public float force;
	public float lifeTime;
	[JsonIgnore]
	public BaseWeapon ownerWeapon;
	public string saveOwnerWeapon
	{
		get
		{
			return AssetDatabase.GetAssetPath(ownerWeapon);//PrefabStageUtility.GetPrefabStage(ownerWeapon.gameObject);
			//return pref.assetPath ?? "";
		}
		set
		{
			try
			{
				ownerWeapon = Resources.Load<BaseWeapon>(value);
			}
			catch
			{
			}
		}
	}
	//public Type ownerType;

	public BaseHero Shooter
	{
		get => shooter;
		set
		{
			shooter = value;
			/*foreach (var item in value.GetComponents<Collider2D>().Where(x=>!x.isTrigger))
			{
				Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), item);
			}*/
			damage = value.Damage;

		}
	}
	private void Start()
	{
		Destroy(gameObject, lifeTime);
	}
	public virtual void OnTriggerEnter2D(Collider2D collision)
	{
		var body = collision.transform.GetComponent<Rigidbody2D>();
		var vector = (Vector2)body.transform.position - (Vector2)collision.ClosestPoint(transform.position);
		body.AddForce(vector.normalized * force, ForceMode2D.Impulse);
		Destroy(gameObject);
	}


}

public class BaseBulletModel : ISaveModel
{
	public override string SaveName { get => "Bullet"; set { } }

	public string PrefabPath { get; set; }
	public static BaseBulletModel InitFromBullet(BaseBullet baseBullet )
	{
		return new BaseBulletModel() { PrefabPath = AssetDatabase.GetAssetPath(baseBullet) };
	}
}

public class BaseConverter : JsonConverter
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
			case nameof(FireShotGunBullet):
				return JsonConvert.DeserializeObject<FireShotGunBullet>(jo.ToString(), SpecifiedSubclassConversion);
			case nameof(SimpleShotGunBullet):
				return JsonConvert.DeserializeObject<SimpleShotGunBullet>(jo.ToString(), SpecifiedSubclassConversion);
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

