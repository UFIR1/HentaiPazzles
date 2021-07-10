[System.Serializable]
public class OneShotFireModule : BaseFireModeModule
{
    public override void TriggerDown(BaseHero Sander,BaseShootingModule shootingModule, BaseWeapon weapon)
    {
        shootingModule.Shot(Sander,weapon);
    }

    public override void TriggerUp(BaseHero Sander)
    {
        
    }
}
