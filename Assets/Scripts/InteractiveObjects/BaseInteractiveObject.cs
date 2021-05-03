using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class BaseInteractiveObject : MonoBehaviour, IInteractive
{
	[SerializeField]
	public GameObject blankUsableTextObj;
	protected GameObject usableTextObj;
	[SerializeField]
	protected Transform usedTextPlace;
	protected TextMeshPro usableTextMesh;

	

	public virtual void ReadyToUse()
	{
		if (usableTextObj == null)
		{
			usableTextObj = Instantiate(blankUsableTextObj, usedTextPlace.position, blankUsableTextObj.transform.rotation, transform);
			usableTextMesh = usableTextObj.GetComponent<TextMeshPro>();
		}
		usableTextMesh.text = $"Press {HotKeysHelper.Interaction}\n to interaction";
		usableTextObj.SetActive(true);
	}
	public virtual void NotReadyToUse()
	{
		if (usableTextObj!=null)
		{
			usableTextObj.SetActive(false);
		}
	}
	public virtual void Use(BaseHero Sender)
	{

	}

	virtual public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<BaseHero>() != null)
		{
			ReadyToUse();
		}
	}
	virtual public void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponent<BaseHero>() != null)
		{
			NotReadyToUse();
		}
	}
	[System.Serializable]
	protected  class RandomDropUpSpawnerEvent
	{
		public GameObject SpawnConsumable;
		public int count;
		public float randomWeight;
	}


}
