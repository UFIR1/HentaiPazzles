using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryWeapon: ISaveModel, ISaveble<InventoryWeapon>,ISaveble<ISaveModel> 
{
	[JsonIgnore]
	public BaseWeapon weapon;
	public bool unlock;
	public BaseWeaponModel BaseWeaponModel;

	public override string SaveName { get { return nameof(InventoryWeapon); } set { } }

	public Type getTT()
	{
		return typeof(InventoryWeapon);
	}

	public void Load(InventoryWeapon model)
	{
		unlock = model.unlock;
		BaseWeaponModel = model.BaseWeaponModel;
		if (BaseWeaponModel != null)
		{
			weapon.Load(BaseWeaponModel);
		}
	}

	public void Load(ISaveModel model)
	{
		Load(model as InventoryWeapon);
	}

	public InventoryWeapon Save()
	{
		var toSave = new InventoryWeapon()
		{
			unlock = this.unlock,
			BaseWeaponModel = weapon?.Save()
		};
		return toSave;
	}

	ISaveModel ISaveble<ISaveModel>.Save()
	{
		return Save();
	}
}
