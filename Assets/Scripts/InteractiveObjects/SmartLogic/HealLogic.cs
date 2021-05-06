using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class HealLogic : BaseSmartLogic
{
	public override float GetWeight(BaseHero sender)
	{
		return sender.CurrentHeals / sender.MaxHeals;
	}
}
