using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StoredBullet", menuName = "Bullet Data", order = 51)]
public class StoredBullet : ScriptableObject
{
	[SerializeField]
	private int maxStuckSize = 350;
	[SerializeField]
	private int currentCount;
	public BaseBulletModel BaseBulletModels
	{
		get
		{
			if (bullet == null)
			{
				return null;
			}
			return	BaseBulletModel.InitFromBullet(bullet);
		}
		set
		{
			var info = new FileInfo(value.PrefabPath);
			var path = "Prefabs/"+ info.Name.Replace(".prefab", "");
			bullet = Resources.Load<BaseBullet>(path);
		}
	}
	[JsonIgnore]
	public BaseBullet bullet;
	public int MaxStuckSize { get => maxStuckSize; }
	public int CurrentCount
	{
		get => currentCount;
		set
		{
			if (value > maxStuckSize)
			{
				currentCount = maxStuckSize;
			}
			else
			{
				currentCount = value;
			}
			OnCurrentCountChange?.Invoke(currentCount, bullet);

		}
	}
	public delegate void OnCurrentCountChangeHandler(int currentCount, BaseBullet currentBullet);
	public event OnCurrentCountChangeHandler OnCurrentCountChange;
	public StoredBullet Clone(StoredBullet storedBullet)
	{
		return new StoredBullet()
		{
			maxStuckSize = storedBullet.maxStuckSize,
			CurrentCount = storedBullet.CurrentCount,
			bullet = storedBullet.bullet
		};
	}
}
