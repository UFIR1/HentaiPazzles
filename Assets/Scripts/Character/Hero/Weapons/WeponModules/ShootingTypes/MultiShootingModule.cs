using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class MultiShootingModule : BaseShootingModule
{
    [SerializeField]
    int bulletCountPerShot=5;
    [SerializeField]
    float spread;
    public int BulletCountPerShot { get => bulletCountPerShot; set => bulletCountPerShot = value; }

    public override void Shot(BaseHero Sander, BaseWeapon weapon)
    {
        if (weapon.CurrentMagazineLoaded > 0)
        {
            weapon.CurrentMagazineLoaded -= 1;
            var newBullets = new List<GameObject>();
            for (int i = 0; i < BulletCountPerShot; i++)
            {
                var bulletRotation = new Quaternion(
                    weapon.bulletSpawnerPoint.transform.rotation.x,
                    weapon.bulletSpawnerPoint.transform.rotation.y,
                    weapon.bulletSpawnerPoint.transform.rotation.z + UnityEngine.Random.Range(-spread, spread),
                    weapon.bulletSpawnerPoint.transform.rotation.w
                    );
                var newBulletObj = UnityEngine.Object.Instantiate(weapon.CurrentBullet.bullet.gameObject, weapon.bulletSpawnerPoint.transform.position, bulletRotation);
                newBulletObj.SetActive(false);
                var newBullet = newBulletObj.GetComponent<BaseBullet>();
                newBullet.Shooter = Sander;
                var module = weapon.passModules.Where(x => x.bulletType == newBullet.GetType()).FirstOrDefault();
                module.PullBullet(ref newBullet);
                newBullets.Add(newBulletObj);
            }
            foreach (var item in newBullets)
            {
                item.SetActive(true);
            }
        }
    }
}
