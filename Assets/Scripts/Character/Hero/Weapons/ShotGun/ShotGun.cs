using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShotGun : BaseWeapon
{
    public OneShotFireModule startedFireModeModule;
    public MultiShootingModule startedShootingModule;

    public override BaseFireModeModule StartedFireModeModule { get => startedFireModeModule; }
    public override BaseShootingModule StartedShootingModule { get => startedShootingModule;  }

    protected override void LocalStart()
    {
        
    }

    public override void TriggerDown(BaseHero Sander)
    {
        base.TriggerDown(Sander);
    }

    public override void TriggerUp(BaseHero Sander)
    {
        base.TriggerUp(Sander);
    }

    
}
