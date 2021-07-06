using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShotGun : BaseWeapon
{
	[SerializeField]
	private int bulletCountPerShot =5;
	[SerializeField]
	private float spread = 0;


	public override void TriggerDown(BaseHero Sander)
	{
		if (CurrentMagazineLoaded > 0)
		{
			CurrentMagazineLoaded -= 1;
			var newBullets = new List<GameObject>();
			for (int i = 0; i < bulletCountPerShot; i++)
			{
				var bulletRotation = new Quaternion(
					bulletSpawnerPoint.transform.rotation.x,
					bulletSpawnerPoint.transform.rotation.y,
					bulletSpawnerPoint.transform.rotation.z + Random.Range(-spread, spread),
					bulletSpawnerPoint.transform.rotation.w
					);
				var newBulletObj = Instantiate(CurrentBullet.bullet.gameObject, bulletSpawnerPoint.transform.position, bulletRotation);
				newBulletObj.SetActive(false);
				var newBullet = newBulletObj.GetComponent<BaseBullet>();
				newBullet.Shooter = Sander;
				var module = passModules.Where(x => x.bulletType == newBullet.GetType()).FirstOrDefault();
				module.PullBullet(ref newBullet);
				newBullets.Add(newBulletObj);
			}
			foreach (var item in newBullets)
			{
				item.SetActive(true);
			}
		}
			
		
	
	}

	public override void TriggerUp(BaseHero Sander)
	{
	}
}
