using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletConsumables: BaseConsumables
{
	public StoredBullet bullet;
	protected override bool CanPicUp(BaseHero hero)
	{
		return true;
		//var heroBullet = hero.Bullets.Where(x => x.bullet.GetType() == bullet.GetType()).FirstOrDefault();
		//if (heroBullet != null)
		//{
		//	if (heroBullet.CurrentCount < MaxStuckSize)
		//	{
		//		return true;
		//	}
		//	return false;
		//}
		//return true;
	}

	protected override bool PickUp(BaseHero hero)
	{
		return hero.PickUpBullet(new StoredBullet() {CurrentCount=bullet.CurrentCount,bullet=bullet.bullet,hideFlags=bullet.hideFlags,name=bullet.name } );
	}
}