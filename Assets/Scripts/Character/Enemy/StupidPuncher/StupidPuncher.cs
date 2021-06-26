using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StupidPuncher : BaseEnemy, ISaveble<StupidPuncherModel>, ISaveble<ISaveModel>
{
	public float punchForce = 5;
	public OverallSize DownForwardOverSize;
	public OverallSize DownBackOversize;
	public OverallSize ForwardDownOverSize;
	public OverallSize ForwardUpOverSize;
	public ColliderHelper2D punchTrigger;
	public float punchPreparedTime = 5;
	public bool punchStarted = false;
	public Animator anim;
	private Rigidbody2D rb;
	[SerializeField]
	private GroundEnemyMoveDirection moveDirection = GroundEnemyMoveDirection.stay;
	private GroundEnemyMoveDirection lastDirection = GroundEnemyMoveDirection.stay;

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
		punchTrigger.OnTriggerStay += OnPunchTriggerEnter;
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
				anim?.SetInteger("panchState", 1);
				punchStarted = true;
				lastDirection = MoveDirection;
				MoveDirection = GroundEnemyMoveDirection.stay;
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
		StartCoroutine(_PunchAllTargets());
	}

	

	IEnumerator _PunchAllTargets()
	{
		anim?.SetInteger("panchState", 2);
		yield return new WaitForSeconds(0.2f);
		foreach (var item in targets)
		{
			item.DealDamage(gameObject, Damage);
			item.GetComponent<Rigidbody2D>().AddForce((item.transform.position - transform.position).normalized * punchForce, ForceMode2D.Impulse);
		}
		MoveDirection = lastDirection;
		yield return new WaitForSeconds(0.2f);
		punchStarted = false;
		yield break;
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


	protected override void OnDeath()
	{
		Destroy(gameObject);
	}
	public Type getTT()
	{
		return typeof(StupidPuncherModel);
	}
	public void Load(StupidPuncherModel model)
	{
		 base.Load(model);
	}

	public StupidPuncherModel Save()
	{
		var toSave = new StupidPuncherModel();
		base.Save().PullChild(toSave);
		return toSave;
	}

	public void Load(ISaveModel model)
	{
		Load(model as StupidPuncherModel);
	}

	ISaveModel ISaveble<ISaveModel>.Save()
	{
		return Save();
	}
}
public class StupidPuncherModel: BaseEnemyModel
{
	public override string SaveName { get { return nameof(StupidPuncherModel); } set => base.SaveName = value; }
}
