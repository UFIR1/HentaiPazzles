using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaseHeroSaveModel : ISaveModel
{
	private string saveName = null;
	public override string SaveName { get => saveName; set => saveName = value; }

	public int Coins { get; set; }
	
	public List<StoredBullet> Bullets { get; set; }
	public InventoryWeapon[] Weapons { get; set; }
}
