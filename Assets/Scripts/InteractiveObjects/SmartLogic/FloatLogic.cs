using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatLogic : BaseSmartLogic
{
	[SerializeField]
	[Range(0,1)]
	float asd;
	public override float GetWeight(BaseHero sender)
	{
		return asd;
	}
}
