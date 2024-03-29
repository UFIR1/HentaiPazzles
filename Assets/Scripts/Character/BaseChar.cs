using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseChar : MonoBehaviour
{
	[SerializeField]
	private int maxHeals;
	[SerializeField]
	private int currentHeals;
	[SerializeField]
	private int damage;

	public int CurrentHeals { get => currentHeals; protected set => currentHeals = value; }
	public int Damage { get => damage; protected set => damage = value; }
	public int MaxHeals { get => maxHeals; protected set => maxHeals = value; }

	public void DealDamage(GameObject Projectile, int damage)
	{
		CurrentHeals -= OnDamage(damage);

	}
    protected virtual int OnDamage(int damage)
	{
		return damage;
	}
	public void DealHeal(GameObject Projectile, int heal)
	{
		if (currentHeals < maxHeals)
		{
			CurrentHeals += OnHeal(heal);
		}

	}
	protected virtual int OnHeal(int heal)
	{
		return heal;
	}
	abstract protected void LocalStart();
	abstract protected void LocalUpdate();


}
