using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
	public int damage;
	private BaseHero shooter;
	public float speed;
	public float force;
	public float lifeTime;
	public BaseWeapon ownerWeapon;
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
