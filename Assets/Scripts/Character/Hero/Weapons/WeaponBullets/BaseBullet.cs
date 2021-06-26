using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System;
using System.Linq;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
	public string ObjType { get { return this.GetType().Name; } set { } }
	public DamageType _damageType;
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
			var result = GameController.gameController.RecourseManager.recourses.Where(x => x.prefab.GetComponent<BaseWeapon>().GetType() == ownerWeapon.GetType()).FirstOrDefault().prefabPath;
			return result;  //"";// AssetDatabase.GetAssetPath(ownerWeapon);//PrefabStageUtility.GetPrefabStage(ownerWeapon.gameObject);
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
	protected bool DealDamageEnemy(Collider2D collision, BaseChar target)
	{
		if (target is BaseEnemy enemy)
		{
			if (_damageType.GetEnemyTargets()?.Contains(enemy.enemyType)==true)
			{
				enemy.DealDamage(gameObject,damage);
				return true;
			}
		}
		else
		{
			target.DealDamage(gameObject, damage);
			OnTriggerEnter2D(collision);
			return true;
		}
		return false;
	}

}

public class BaseBulletModel : ISaveModel
{
	public override string SaveName { get => "Bullet"; set { } }

	public string PrefabPath { get; set; }
	public static BaseBulletModel InitFromBullet(BaseBullet baseBullet)
	{

		var path = GameController.gameController.RecourseManager.recourses.Where(x => x.prefab.GetComponent<BaseBullet>()?.GetType() == baseBullet.GetType()).FirstOrDefault()?.prefabPath;
		
		return new BaseBulletModel() { PrefabPath = path ?? "" }; /*AssetDatabase.GetAssetPath(baseBullet)*/

	}
}



