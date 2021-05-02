using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Swordsman : BaseHero
{
	public GameObject swordSplesh;
	protected override void HitStart()
	{
		animator.Play("MiliHit");
		
		
		
		var projRot = swordSplesh.transform.rotation;
		var projectile = Instantiate(swordSplesh, this.transform.position, new Quaternion( projRot.x,projRot.y, (spriteRenderer.flipX == true)?180:0,projRot.w));
		var weapon = projectile.GetComponent<BaseProjectile>();
		weapon.Damage = damage;
		weapon.Shooter = gameObject;
	}
	

	protected override void HitFinish()
	{
		
	}

	protected override void LocalFixedUpdate()
	{
	}

	protected override void LocalStart()
	{
	}

	protected override void LocalUpdate()
	{
	}


	
}
