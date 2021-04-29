using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSword : BaseProjectile
{
	// Start is called before the first frame update
	private void Start()
	{
		transform.parent = Shooter.transform;
		Destroy(gameObject, LifeTime);
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		transform.Translate(Vector2.right * Speed * Time.deltaTime);
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log(collision.transform.name);
		if (collision.transform.tag == Tags.Enemy.ToString())
		{
			Debug.Log(collision.transform.tag);
			var target = collision.transform.GetComponent<BaseChar>();
			target.DealDamage(gameObject, Damage);
			var body = collision.transform.GetComponent<Rigidbody2D>();
			var vector = (Vector2)body.transform.position - (Vector2)collision.ClosestPoint(transform.position);
			body.AddForce( vector.normalized * Forse, ForceMode2D.Impulse);

		}
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		
	}
}
