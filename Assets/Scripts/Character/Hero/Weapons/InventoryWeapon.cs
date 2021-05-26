using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryWeapon 
{
	[JsonIgnore]
	public BaseWeapon weapon;
	public bool unlock;
 
}
