using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StupidPuncher : BaseEnemy
{
	public OverallSize DownForwardOverSize;
	public OverallSize DownBackOversize;
	public OverallSize ForwardDownOverSize;
	public OverallSize ForwardUpOverSize;
	public ColliderHelper2D punchTrigger;
	public float punchPreparedTime = 5;
	public bool punchStarted = false;
	private Rigidbody2D rb;
	[SerializeField]
	private GroundEnemyMoveDirection moveDirection = GroundEnemyMoveDirection.stay;

	public GroundEnemyMoveDirection MoveDirection
	{
		get => moveDirection; set
		{
			moveDirection = value;
			if (value == GroundEnemyMoveDirection.right)
			{
				transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
			}
			if (value == GroundEnemyMoveDirection.left)
			{
				transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{

	}

	protected override void LocalStart()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
		MoveDirection = (UnityEngine.Random.Range(0, 1) == 0) ? GroundEnemyMoveDirection.left : GroundEnemyMoveDirection.right;
		punchTrigger.OnTriggerEnter += OnPunchTriggerEnter;
		punchTrigger.OnTriggerExit += OnPunchTriggerExit;
	}
	public void OnPunchTriggerEnter(Collider2D collision)
	{
		var hero = collision.GetComponent<BaseHero>();
		if (hero != null)
		{
			if (!targets.Contains(hero))
			{
				targets.Add(hero);
			}
			if (!punchStarted)
			{
				punchStarted = true;
				Invoke(nameof(PunchAllTargets), punchPreparedTime);
			}
		}
	}
	public void OnPunchTriggerExit(Collider2D collision)
	{
		var hero = collision.GetComponent<BaseHero>();
		if (hero != null)
		{
			if (targets.Contains(hero))
			{
				targets.Remove(hero);
			}
		}
	}

	private List<BaseHero> targets = new List<BaseHero>();
	public void PunchAllTargets()
	{
		foreach (var item in targets)
		{
			item.DealDamage(gameObject, Damage);
		}
		punchStarted = false;
	}

	protected override void LocalUpdate()
	{

	}
	private void FixedUpdate()
	{
		if ((DownForwardOverSize.raycast.collider == null && DownBackOversize.raycast.collider != null)
			||
			(ForwardDownOverSize.raycast.collider != null || ForwardUpOverSize.raycast.collider != null)
			)
		{
			SwitchDirection();
		}
		if (DownForwardOverSize.raycast.collider != null || DownBackOversize.raycast.collider != null)
		{
			Move();
		}
	}
	public void SwitchDirection()
	{
		switch (MoveDirection)
		{
			case GroundEnemyMoveDirection.left:
				MoveDirection = GroundEnemyMoveDirection.right;
				break;
			case GroundEnemyMoveDirection.stay:
				break;
			case GroundEnemyMoveDirection.right:
				MoveDirection = GroundEnemyMoveDirection.left;
				break;
			default:
				break;
		}
	}
	void Move()
	{
		switch (moveDirection)
		{
			case GroundEnemyMoveDirection.left:
				transform.Translate(Vector3.left * Time.deltaTime * speed);
				break;
			case GroundEnemyMoveDirection.stay:
				break;
			case GroundEnemyMoveDirection.right:
				transform.Translate(Vector3.right * Time.deltaTime * speed);
				break;
			default:
				break;
		}

	}
	// Start is called before the first frame update
}
