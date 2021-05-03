using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;

public class CoinDropUpSpawner : BaseInteractiveObject
{
	public int coinCost = 1;
	public float xCorMaxRnd = 5;
	public float xCorMinRnd = -5;
	public float yCorMaxRnd = 5;
	public float yCorMinRnd = 3;
	[SerializeField]
	private float coolDown = 0;
	private bool readyToUse = true;

	[SerializeField]
	private RandomDropUpSpawnerEvent[] spawnerEvents;
	[SerializeField]
	private bool singlton = false;
	[SerializeField]
	private float waitBetweenSpawns = 0.5f;

	private bool spawning = false;


	public override void Use(BaseHero Sender)
	{
		if (Sender.PayCoins(coinCost) && readyToUse)
		{
			var rndMax = spawnerEvents.Sum(x => x.randomWeight);
			var randomEventWeight = Random.Range(0, rndMax);
			foreach (var item in spawnerEvents)
			{
				randomEventWeight -= item.randomWeight;
				if (randomEventWeight <= 0)
				{
					StartSpawner(item, Sender);
					readyToUse = false;
					Invoke("CollingDown", coolDown);
					break;
				}
			}

		}
	}
	private void CollingDown()
	{
		readyToUse = true;
	}
	void StartSpawner(RandomDropUpSpawnerEvent spawnerEvent, BaseHero Sender)
	{
		List<ItemHelper> items = new List<ItemHelper>();

		for (int i = 0; i < spawnerEvent.count; i++)
		{

			var item = Instantiate(spawnerEvent.SpawnConsumable, transform.position, spawnerEvent.SpawnConsumable.transform.rotation);
			item.SetActive(false);
			items.Add(new ItemHelper() { gameObject = item, rigidbody2D = item.GetComponent<Rigidbody2D>() });

		}
		
		if (canUse&&!spawning)
		{
			spawning = true;
			canUse = false;
			StartCoroutine(Spawn(items));
		}
		//foreach (var item in items)
		//{
		//	var colliders = item.gameObject.GetComponents<Collider2D>();
		//	item.gameObject.SetActive(true);
		//	var rg = item.rigidbody2D;
		//	var rndForceX = Random.Range(xCorMinRnd, xCorMaxRnd);
		//	var rndForceY = Random.Range(yCorMinRnd, yCorMaxRnd);
		//	rg.AddForce(new Vector2(rndForceX, rndForceY), ForceMode2D.Impulse);
		//	Destroy(item.gameObject, 20);
		//}
	}
	
	IEnumerator Spawn(List<ItemHelper> items)
	{
		foreach (var item in items)
		{

			var colliders = item.gameObject.GetComponents<Collider2D>();
			item.gameObject.SetActive(true);
			var rg = item.rigidbody2D;
			var rndForceX = Random.Range(xCorMinRnd, xCorMaxRnd);
			var rndForceY = Random.Range(yCorMinRnd, yCorMaxRnd);
			rg.AddForce(new Vector2(rndForceX, rndForceY), ForceMode2D.Impulse);
			Destroy(item.gameObject, 20);
			if (singlton)
			{
				yield return new WaitForSeconds(waitBetweenSpawns);
			}
			
		}
		spawning = false;
		Unlock(coolDown);
		yield break;
	}
	
	private class ItemHelper
	{
		public GameObject gameObject;
		public Rigidbody2D rigidbody2D;
	}
}
