using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealedConsumables : BaseConsumables
{
	public int healedHealthCount=0;
	protected override bool CanPicUp(BaseHero hero)
	{
		return hero.CurrentHeals < hero.MaxHeals;
	}

	protected override void PickUp(BaseHero hero)
	{
		hero.DealHeal(gameObject,healedHealthCount);
	}
}
