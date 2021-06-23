using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderHelper2D: MonoBehaviour
{
	public OnCollision OnCollisionEnter;
	public OnCollision OnCollisionStay;
	public OnCollision OnCollisionExit;
	public OnTrigger OnTriggerEnter;
	public OnTrigger OnTriggerStay;
	public OnTrigger OnTriggerExit;


	public delegate void OnCollision( Collision2D collision);
	public delegate void OnTrigger(Collider2D Collider2D);

	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision != null && OnCollisionEnter != null)
		{
			OnCollisionEnter(collision);
		}
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision != null && OnCollisionExit != null)
		{
			OnCollisionExit(collision);
		}
	}
	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision != null && OnCollisionStay != null)
		{
			OnCollisionStay(collision);
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision != null && OnTriggerEnter != null)
		{
			OnTriggerEnter(collision);
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision != null && OnTriggerExit != null)
		{
			OnTriggerExit(collision);
		}
	}
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision != null && OnTriggerStay != null)
		{
			 OnTriggerStay(collision);
		}
	}
}
