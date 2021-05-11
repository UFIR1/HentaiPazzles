using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleShotGunBulletLogic : BaseSmartLogic
{
	public override float GetWeight(BaseHero sender)
	{
		var bullet = sender.Bullets.Where(x => x.bullet.GetType() == typeof(SimpleShotGunBullet)).FirstOrDefault();
		if (bullet?.bullet == null)
		{
			return 0;
		}
		return bullet.CurrentCount / bullet.MaxStuckSize;
	}
}
