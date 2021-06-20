using System.Collections;
using System.Collections.Generic;
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
				var newBullet = Instantiate(CurrentBullet.bullet.gameObject, bulletSpawnerPoint.transform.position, bulletRotation);
				newBullet.SetActive(false);
				newBullet.GetComponent<BaseBullet>().Shooter = Sander;
				newBullets.Add(newBullet);
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
