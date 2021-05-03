using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class BaseConsumables : MonoBehaviour
{
	bool pickUped=false;
	protected abstract bool CanPicUp(BaseHero hero);
	protected abstract void PickUp(BaseHero hero);
	private Collider2D MyCollider;
	private Rigidbody2D rigidbody;
	private bool OnGround=false;

	private void Start()
	{
		MyCollider = transform.GetComponents<Collider2D>().Where(x => !x.isTrigger).FirstOrDefault();
		rigidbody = transform.GetComponent<Rigidbody2D>();
		MyCollider.enabled = false;
		if (GameController.gameController.Player != null)
		{
			var playerColliders = GameController.gameController.Player.GetComponents<Collider2D>().Where(x=>!x.isTrigger);
			foreach (var item in playerColliders)
			{
				Physics2D.IgnoreCollision(item, MyCollider);
			}
		}
	}
	private void Update()
	{
		if (!MyCollider.enabled)
		{
			if (rigidbody.velocity.y <= 0)
			{
				MyCollider.enabled = true;
			}
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (OnGround)
		{
			var hero = collision.GetComponent<BaseHero>();
			if (hero != null)
			{
				if (CanPicUp(hero) && !pickUped)
				{
					pickUped = true;
					PickUp(hero);
					Destroy(gameObject);
				}
			}
		}
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.tag == Tags.Consumables.ToString() || collision.transform.tag == Tags.Player.ToString())
		{
			Physics2D.IgnoreCollision(collision.collider, MyCollider);
		}
		if(collision.transform.tag == Tags.LevelBorder.ToString()
			|| collision.transform.tag == Tags.LevelPlatform.ToString())
		{
			OnGround = true;
		}
	}
	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.transform.tag == Tags.Consumables.ToString() || collision.transform.tag == Tags.Player.ToString())
		{
			Physics2D.IgnoreCollision(collision.collider, MyCollider);
		}
	}

}
