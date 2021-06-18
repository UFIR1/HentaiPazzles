using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoredWeapon : BaseConsumables, IInteractive
{

	[SerializeField]
	public GameObject blankUsableTextObj;
	protected GameObject usableTextObj;
	[SerializeField]
	protected Transform usedTextPlace;
	protected TextMeshPro usableTextMesh;

	public BaseWeapon weapon;
	public int weaponSlotNumber;
	public virtual void OnReadyToUse()
	{
		if (usableTextObj == null)
		{
			usableTextObj = Instantiate(blankUsableTextObj, usedTextPlace.position, blankUsableTextObj.transform.rotation, transform);
			usableTextMesh = usableTextObj.GetComponent<TextMeshPro>();
		}
		usableTextMesh.text = $"Press {HotKeysHelper.Interaction}\n to interact";
		usableTextObj.SetActive(true);
	}
	public virtual void OnNotReadyToUse()
	{
		if (usableTextObj != null)
		{
			usableTextObj.SetActive(false);
		}
	}

	public void Use(BaseHero Sender)
	{
		if (PickUp(Sender))
		{
			OnPickUp();
		}
	}

	protected override bool CanPicUp(BaseHero hero)
	{
		return false;
	}

	protected override bool PickUp(BaseHero hero)
	{
		return hero.PickUpWeapon(weapon,weaponSlotNumber);
	}

	virtual public void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.GetComponent<BaseHero>() != null)
		{
			OnReadyToUse();
		}
	}
	virtual public void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponent<BaseHero>() != null)
		{
			OnNotReadyToUse();
		}
	}
}
