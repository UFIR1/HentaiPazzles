using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoredBullet : BaseConsumables
{
	[SerializeField]
	private int maxStuckSize = 350;
	[SerializeField]
	private int currentCount;
	public BaseBullet bullet;

	public int MaxStuckSize { get => maxStuckSize; }
	public int CurrentCount
	{
		get => currentCount; 
		set
		{
			if (value > maxStuckSize)
			{
				currentCount = maxStuckSize;
			}
			else
			{
				currentCount = value;
			}
			OnCurrentCountChange?.Invoke(currentCount, bullet);

		}
	}
	public delegate void OnCurrentCountChangeHandler(int currentCount, BaseBullet currentBullet);
	public event OnCurrentCountChangeHandler OnCurrentCountChange;

	public StoredBullet Clone(StoredBullet storedBullet)
	{
		return new StoredBullet()
		{
			maxStuckSize = storedBullet.maxStuckSize,
			CurrentCount = storedBullet.CurrentCount,
			bullet = storedBullet.bullet
		};
	}

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
		return hero.PickUpBullet(this);
	}
}
