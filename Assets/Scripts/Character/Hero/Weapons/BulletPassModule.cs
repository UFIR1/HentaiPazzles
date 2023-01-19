using System;

public abstract class BulletPassModule : ISaveModel
{
    private int damage;
    private float force;
    public abstract Type bulletType { get; }

    public int Damage { get => damage; set => damage = value; }
    public float Force { get => force; set => force = value; }
    public virtual void PullBullet(ref BaseBullet bullet)
    {
        bullet.damage = Damage;
        bullet.force = Force;
    }
}
public class SimpleShotGunBulletModule : BulletPassModule
{
    public override Type bulletType => typeof(SimpleShotGunBullet);
    public override string SaveName { get => throw new NotImplementedException(); set { } }

    public void Load(SimpleShotGunBulletModuleModel model)
    {
        Damage = model.Damage;
        Force = model.Force;
    }

}
public class SimpleShotGunBulletModuleModel : ISaveModel
{
    public override string SaveName { get { return nameof(SimpleShotGunBulletModuleModel); } set { } }
    public int Damage { get; set; }
    public float Force { get; set; }
}

