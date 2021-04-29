using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseChar : MonoBehaviour
{
	[SerializeField]
    protected int heals;
	[SerializeField]
	protected int damage;
    
    public void DealDamage(GameObject Projectile, int damage)
	{
		heals -= OnDamage(damage);

	}
    protected virtual int OnDamage(int damage)
	{
		return damage;
	}
	abstract protected void LocalStart();
	abstract protected void LocalUpdate();


}
