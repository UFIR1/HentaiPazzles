using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
	private float switchableTime;
	private float reloadTime;
	public float SwitchableTime { get => switchableTime; private set => switchableTime = value; }
	public float ReloadTime { get => reloadTime; private set => reloadTime = value; }
	[SerializeField]
	protected GameObject bulletSpawnerPoint;
	private BaseBullet currentBullet;
	private int magazineSize = 2;
	private int currentMagazineLoaded=0;
	public int CurrentMagazineLoaded { get => currentMagazineLoaded; protected set => currentMagazineLoaded = value; }
	public int MagazineSize { get => magazineSize; protected set => magazineSize = value; }
	public BaseBullet CurrentBullet { get => currentBullet;protected set => currentBullet = value; }

	public virtual int Reload(int downloadedBullets, BaseBullet downloadedBulletType,out BaseBullet oldBulletType)
	{
		oldBulletType = CurrentBullet;
		if (downloadedBulletType.GetType() != CurrentBullet?.GetType())
		{
			var oldMagazineLoaded = currentMagazineLoaded;
			oldBulletType = CurrentBullet;
			CurrentBullet = downloadedBulletType;
			CurrentMagazineLoaded = downloadedBullets;
			return oldMagazineLoaded;
		}
		// возвращать партоны при переполнении, т.к. если у игрока осталось меньше патрон чем максимум в обойме то нужно доложить а не заменить.
		var freeLoad=  MagazineSize - CurrentMagazineLoaded;
		var toComeBack = downloadedBullets - freeLoad;
		if (toComeBack < 0) { toComeBack = 0; }
		currentMagazineLoaded += downloadedBullets-toComeBack;
		CurrentBullet = downloadedBulletType;
		return toComeBack;
	}

	public abstract void TriggerDown(BaseHero Sander);
	public abstract void TriggerUp(BaseHero Sander);

}
