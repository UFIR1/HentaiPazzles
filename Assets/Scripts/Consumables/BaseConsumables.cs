using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseConsumables : MonoBehaviour
{
	bool pickUped=false;
	protected abstract bool CanPicUp(BaseHero hero);
	protected abstract void PickUp(BaseHero hero);
	private void OnTriggerEnter2D(Collider2D collision)
	{
		var hero = collision.GetComponent<BaseHero>();
		if (hero != null)
		{
			if (CanPicUp(hero)&&!pickUped)
			{
				pickUped = true;
				PickUp(hero);
				Destroy(gameObject);
			}
		}
	}

}
