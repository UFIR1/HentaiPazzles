using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Runes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System;

public abstract class BaseWeapon : MonoBehaviour, ISaveble<BaseWeaponModel>, ISaveble<ISaveModel>
{
    private float switchableTime;
    private float reloadTime;
    [SerializeField]
    private int _startDamage;
    [SerializeField]
    private int _currentDamage;
    [SerializeField]
    private float _force;
    public float SwitchableTime { get => switchableTime; private set => switchableTime = value; }
    public float ReloadTime { get => reloadTime; private set => reloadTime = value; }
    [SerializeField]
    public GameObject bulletSpawnerPoint;
    private StoredBullet currentBullet;
    private int magazineSize = 2;
    private int currentMagazineLoaded = 0;
    public int CurrentMagazineLoaded
    {
        get => currentMagazineLoaded;
        set
        {
            currentMagazineLoaded = value;
            OnMagazineLoadChange?.Invoke(value, CurrentBullet.bullet);
        }
    }
    public int MagazineSize { get => magazineSize; protected set => magazineSize = value; }
    public StoredBullet CurrentBullet { get => currentBullet; protected set => currentBullet = value; }
    public int CurrentDamage { get => _currentDamage; set => _currentDamage = value; }
    public float Force { get => _force; set => _force = value; }
    public BaseFireModeModule _fireModeModule;
    public BaseShootingModule _shootingModule;

    public abstract BaseFireModeModule StartedFireModeModule { get;}
    public abstract BaseShootingModule StartedShootingModule { get;}


    public delegate void OnMagazineLoadChangeHandler(int currentMagazineLoaded, BaseBullet currentBullet);
    public event OnMagazineLoadChangeHandler OnMagazineLoadChange;
    public List<BulletPassModule> passModules = new List<BulletPassModule>();
    public List<BaseRune> runes = new List<BaseRune>();

    private void Start()
    {
        runes.Add(new DamageUp() { _grade = RuneGrade.Common, _damageWeight = 0.2f });
        passModules.Add(new SimpleShotGunBulletModule() { Damage = CurrentDamage, Force = Force });
        RecalculateAllBulletModules();
        LocalStart();
        _fireModeModule = StartedFireModeModule;
        _shootingModule = StartedShootingModule;
    }
    protected abstract void LocalStart();
    public virtual int Reload(int downloadedBullets, BaseBullet downloadedBullet, out BaseBullet oldBulletType)
    {
        //неуспешная перезарядка 

        if (!passModules.Where(x => x.bulletType == downloadedBullet.GetType()).Any())
        {
            oldBulletType = downloadedBullet;
            return downloadedBullets;
        }


        if (currentBullet == null)
        {
            currentBullet = new StoredBullet();
        }
        oldBulletType = CurrentBullet?.bullet;
        if (downloadedBullet.GetType() != CurrentBullet?.GetType())
        {
            var oldMagazineLoaded = CurrentMagazineLoaded;
            oldBulletType = CurrentBullet?.bullet;
            CurrentBullet.bullet = downloadedBullet;
            CurrentMagazineLoaded = downloadedBullets;
            return oldMagazineLoaded;
        }
        // возвращать партоны при переполнении, т.к. если у игрока осталось меньше патрон чем максимум в обойме то нужно доложить а не заменить.
        var freeLoad = MagazineSize - CurrentMagazineLoaded;
        var toComeBack = downloadedBullets - freeLoad;
        if (toComeBack < 0) { toComeBack = 0; }
        CurrentMagazineLoaded += downloadedBullets - toComeBack;
        CurrentBullet.bullet = downloadedBullet;
        return toComeBack;
    }


    public void RecalculateAllBulletModules()
    {
        foreach (var item in passModules)
        {
            RecalculateBulletModule(item);
        }
    }

    public void RecalculateBulletModule(BulletPassModule bulletModule)
    {
        foreach (BaseBulletRune item in runes.Where(x => x is BaseBulletRune))
        {
            item.ApplyRune(this, ref bulletModule);
        }
    }

    public virtual void TriggerDown(BaseHero Sander)
    {
        _fireModeModule.TriggerDown(Sander,_shootingModule,this);
    }
    public virtual void TriggerUp(BaseHero Sander)
    {
        _fireModeModule.TriggerUp(Sander);
    }







    public System.Type getTT()
    {
        return typeof(BaseWeapon);
    }

    public void Load(BaseWeaponModel model)
    {
        CurrentBullet = model.CurrentBullet;
        MagazineSize = model.MagazineSize;
        CurrentMagazineLoaded = model.CurrentMagazineLoaded;
        passModules = model.BulletPassModules;
    }

    public BaseWeaponModel Save()
    {
        var toSave = new BaseWeaponModel()
        {
            CurrentBullet = CurrentBullet,
            CurrentMagazineLoaded = CurrentMagazineLoaded,
            MagazineSize = MagazineSize,
            BulletPassModules = passModules,
        };


        return toSave;

    }

    public void Load(ISaveModel model)
    {
        Load(model as BaseWeaponModel);
    }

    ISaveModel ISaveble<ISaveModel>.Save()
    {
        return Save();
    }
}


public class BaseWeaponModel : ISaveModel
{
    public override string SaveName { get { return nameof(BaseWeaponModel); } set { } }
    public int CurrentMagazineLoaded { get; set; }
    public int MagazineSize { get; set; }
    public StoredBullet CurrentBullet { get; set; }
    public List<BulletPassModule> BulletPassModules { get; set; }
}


public enum DamageType
{
    [EnemyTypeTargets(EnemyType.Physical)]
    Physical,
    [EnemyTypeTargets(EnemyType.Ghost)]
    Ghost,
    [EnemyTypeTargets(EnemyType.Physical, EnemyType.Ghost)]
    Clear
}

public class EnemyTypeTargets : Attribute
{
    public EnemyType[] types;

    public EnemyTypeTargets(params EnemyType[] types)
    {
        this.types = types;
    }
}

public static class EnumExtensions
{
    public static EnemyType[] GetEnemyTargets(this Enum value)
    {
        var attr = value.GetType().GetMember(value.ToString()).FirstOrDefault(m => m.DeclaringType == value.GetType()).GetCustomAttributes(typeof(EnemyTypeTargets), false);
        if (attr != null)
        {
            if (attr.Any())
            {
                return (attr[0] as EnemyTypeTargets).types;
            }
        }
        return null;
    }
}