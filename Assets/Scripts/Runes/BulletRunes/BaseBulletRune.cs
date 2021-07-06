using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace Runes
{
    abstract public class BaseBulletRune : BaseRune
    {
        public abstract void ApplyRune(BaseWeapon weapon, ref BulletPassModule bullet);
    }
	public class DamageUp : BaseBulletRune
	{
		public float _damageWeight;
		public override void ApplyRune(BaseWeapon weapon, ref BulletPassModule bulletModule)
		{
			var calculate = weapon.CurrentDamage * _damageWeight;
			bulletModule.Damage += (int)((math.abs(_damageWeight)<0.5)? math.ceil(calculate) :math.floor(calculate));
		}
	}
}
