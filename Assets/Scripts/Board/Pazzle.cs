using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pazzle : MonoBehaviour
{
	Side pazzleSide =0;

	public Side PazzleSide { get => pazzleSide; 
		set { 
			pazzleSide = value;
			transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, (transform.rotation.z+90f), transform.rotation.w);
		} 
	}

	// Start is called before the first frame update
	void Start()
	{
		var render = gameObject.GetComponent<SpriteRenderer>();
		var collader= gameObject.AddComponent<BoxCollider2D>();
		collader.size = new Vector2( render.size.x, render.size.y);
	}

	// Update is called once per frame
	void Update()
	{

	}
	public void Click()
	{
		Debug.Log("Click");
		Move();
	}

	private void Move()
	{
		PazzleSide++;
	}

	public enum Side
	{
		top = 0,
		left = 1,
		bot = 2,
		right = 3
	}

}
