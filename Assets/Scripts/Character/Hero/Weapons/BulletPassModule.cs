using System;

public abstract  class BulletPassModule
{
	private int damage;
	private float force;
	public abstract Type bulletType { get; }

	public int Damage { get => damage; set => damage = value; }
	public float Force { get => force; set => force = value; }

	public virtual void PullBullet(ref BaseBullet bullet )
	{
		bullet.damage = Damage;
		bullet.force = Force;
	}
}
public class SimpleShotGunBulletModule : BulletPassModule
{
	public override Type bulletType => typeof(SimpleShotGunBullet);
}

