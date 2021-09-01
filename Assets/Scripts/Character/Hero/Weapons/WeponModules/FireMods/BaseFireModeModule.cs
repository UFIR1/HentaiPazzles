public abstract class BaseFireModeModule
{
    public float delayBetweenShots;
    public abstract void TriggerDown(BaseHero Sander, BaseShootingModule shootingModule, BaseWeapon weapon);
    public abstract void TriggerUp(BaseHero Sander);

}
