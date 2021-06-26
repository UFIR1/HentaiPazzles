using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShotGunBullet : BaseBullet
{
	private void FixedUpdate()
	{
		transform.Translate(Vector2.up * speed * Time.deltaTime);
	}


	public override void OnTriggerEnter2D(Collider2D collision)
	{
		BaseChar target = null;
		if (collision.transform.tag == Tags.Enemy.ToString() && collision.transform.TryGetComponent(out target))
		{
			if (DealDamageEnemy(collision, target))
			{
				base.OnTriggerEnter2D(collision);
			}
		}
		if (collision.tag == Tags.LevelBorder.ToString())
		{
			Destroy(gameObject);
		}
	}
	

}
