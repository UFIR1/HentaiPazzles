using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseChar : MonoBehaviour
{
	[SerializeField]
	private int maxHeals;
	[SerializeField]
	private int currentHeals;


	public int CurrentHeals
	{
		get => currentHeals; 
		protected set
		{
			if (currentHeals > 0)
			{
				if (value < maxHeals)
				{
					currentHeals = value;
					if (currentHeals <= 0)
					{
						OnDeath();
					}
				}
				else
				{
					currentHeals = maxHeals;
				}
			}
		}
	}
	public int MaxHeals { get => maxHeals; protected set => maxHeals = value; }

	public void DealDamage(GameObject Projectile, int damage)
	{
		CurrentHeals -= OnDamage(damage);

	}
	protected virtual int OnDamage(int damage)
	{
		return damage;
	}
	public bool DealHeal(GameObject Projectile, int heal)
	{
		
		if (currentHeals < maxHeals && currentHeals>0)
		{
			CurrentHeals += OnHeal(heal);
			return true;
		}
		return false;

	}
	protected virtual void OnDeath()
	{

	}

	protected virtual int OnHeal(int heal)
	{
		return heal;
	}
	abstract protected void LocalStart();
	abstract protected void LocalUpdate();


}
