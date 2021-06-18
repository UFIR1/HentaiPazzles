using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour, ISaveble<BaseWeaponModel>, ISaveble<ISaveModel>
{
	private float switchableTime;
	private float reloadTime;
	public float SwitchableTime { get => switchableTime; private set => switchableTime = value; }
	public float ReloadTime { get => reloadTime; private set => reloadTime = value; }
	[SerializeField]
	protected GameObject bulletSpawnerPoint;
	private StoredBullet currentBullet;
	private int magazineSize = 2;
	private int currentMagazineLoaded = 0;
	public int CurrentMagazineLoaded { get => currentMagazineLoaded; protected set { currentMagazineLoaded = value; OnMagazineLoadChange?.Invoke(value, CurrentBullet.bullet); } }
	public int MagazineSize { get => magazineSize; protected set => magazineSize = value; }
	public StoredBullet CurrentBullet { get => currentBullet; protected set => currentBullet = value; }
	public delegate void OnMagazineLoadChangeHandler(int currentMagazineLoaded, BaseBullet currentBullet);
	public event OnMagazineLoadChangeHandler OnMagazineLoadChange;


	public virtual int Reload(int downloadedBullets, BaseBullet downloadedBulletType, out BaseBullet oldBulletType)
	{
		if (currentBullet == null)
		{
			currentBullet = new StoredBullet();
		}
		oldBulletType = CurrentBullet?.bullet;
		if (downloadedBulletType.GetType() != CurrentBullet?.GetType())
		{
			var oldMagazineLoaded = CurrentMagazineLoaded;
			oldBulletType = CurrentBullet?.bullet;
			CurrentBullet.bullet = downloadedBulletType;
			CurrentMagazineLoaded = downloadedBullets;
			return oldMagazineLoaded;
		}
		// возвращать партоны при переполнении, т.к. если у игрока осталось меньше патрон чем максимум в обойме то нужно доложить а не заменить.
		var freeLoad = MagazineSize - CurrentMagazineLoaded;
		var toComeBack = downloadedBullets - freeLoad;
		if (toComeBack < 0) { toComeBack = 0; }
		CurrentMagazineLoaded += downloadedBullets - toComeBack;
		CurrentBullet.bullet = downloadedBulletType;
		return toComeBack;
	}

	public abstract void TriggerDown(BaseHero Sander);
	public abstract void TriggerUp(BaseHero Sander);

	public System.Type getTT()
	{
		return typeof(BaseWeapon);
	}

	public void Load(BaseWeaponModel model)
	{
		CurrentBullet = model.CurrentBullet;
		MagazineSize = model.MagazineSize;
		CurrentMagazineLoaded = model.CurrentMagazineLoaded;
	}

	public BaseWeaponModel Save()
	{
		var toSave = new BaseWeaponModel()
		{
			CurrentBullet = CurrentBullet,
			CurrentMagazineLoaded = CurrentMagazineLoaded,
			MagazineSize = MagazineSize,
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
}
