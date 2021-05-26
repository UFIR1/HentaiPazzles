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

	protected bool canUse = true;



	public virtual void OnReadyToUse()
	{
		if (usableTextObj == null)
		{
			usableTextObj = Instantiate(blankUsableTextObj, usedTextPlace.position, blankUsableTextObj.transform.rotation, transform);
			usableTextMesh = usableTextObj.GetComponent<TextMeshPro>();
		}
		usableTextMesh.text = $"Press {HotKeysHelper.Interaction}\n to interaction";
		usableTextObj.SetActive(true);
	}
	public virtual void OnNotReadyToUse()
	{
		if (usableTextObj != null)
		{
			usableTextObj.SetActive(false);
		}
	}
	public virtual void Use(BaseHero Sender)
	{

	}

	//virtual public void OnTriggerEnter2D(Collider2D collision)
	//{
	//	if (collision.GetComponent<BaseHero>() != null)
	//	{
	//		ReadyToUse();
	//	}
	//}
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
	protected void Unlock(float timeToUnlock)
	{
		Invoke(nameof(_Unlock), timeToUnlock);
	}
	private void _Unlock()
	{
		canUse = true;
	}
	[System.Serializable]
	protected class RandomDropUpSpawnerEvent
	{
		public int count;
		public BaseSmartLogic smartLogic;
		public AnimationCurve randomWeight;
		public RandomDropUpSpawnerItem[] randomDropUpSpawnerItems;
	}
	[System.Serializable]
	protected class RandomDropUpSpawnerItem
	{
		public GameObject spawnConsumable;
		public int count;
		public BaseSmartLogic smartLogic;
		public AnimationCurve randomWeight;
	}
	

}
