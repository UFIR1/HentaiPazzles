using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoredBullet : BaseConsumables
{
	public int maxStuckSize;
	public int currentCount;
	public BaseBullet bullet;
	public StoredBullet Clone(StoredBullet storedBullet)
	{
		return new StoredBullet()
		{
			maxStuckSize = storedBullet.maxStuckSize,
			currentCount = storedBullet.currentCount,
			bullet = storedBullet.bullet
		};
	}

	protected override bool CanPicUp(BaseHero hero)
	{
		var heroBullet = hero.Bullets.Where(x => x.bullet.GetType() == bullet.GetType()).FirstOrDefault();
		if (heroBullet != null)
		{
			if (heroBullet.currentCount < heroBullet.maxStuckSize)
			{
				return true;
			}
			return false;
		}
		return true;
	}

	protected override void PickUp(BaseHero hero)
	{
		hero.PickUpBullet(this);
	}
}
