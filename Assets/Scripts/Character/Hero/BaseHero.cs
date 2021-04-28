using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseHero : BaseChar
{

	#region lockalMovemantFields

	[SerializeField]
	protected Rigidbody2D rigidbody;
	[SerializeField]
	protected BoxCollider2D collider;
	[SerializeField]
	protected HeroMoveCondition heroMoveCondition = 0;
	[SerializeField]
	[Range(0.0f, 10.0f)]
	protected float speed = 5.5f;
	[SerializeField]
	protected float jumpForse = 1f;
	[SerializeField]
	protected int jumpCount = 2;
	[SerializeField]
	protected int currentJumpCount = 2;
	protected OverallSize overallSizeLeft;
	protected OverallSize overallSizeRight;
	protected bool jumpBlock = false;
	private bool inJump = false;
	protected bool InJump
	{
		get => inJump;
		set
		{
			inJump = value;
			collider.enabled = !value;
		}
	}
	#endregion
	// Start is called before the first frame update
	void Start()
	{
		rigidbody = gameObject.GetComponent<Rigidbody2D>();
		collider = gameObject.GetComponent<BoxCollider2D>();
		overallSizeLeft = gameObject.GetComponentsInChildren<OverallSize>().Where(x => x.Name == "Left").FirstOrDefault();
		overallSizeRight = gameObject.GetComponentsInChildren<OverallSize>().Where(x => x.Name == "Right").FirstOrDefault();
		LocalStart();
	}
	abstract protected void LocalStart();

	// Update is called once per frame
	void Update()
	{
		KeyEventer();
		if (TouchGround())
		{
			//if (!jumpBlock)
			//{
				currentJumpCount = jumpCount;
			//}
		}

		LocalUpdate();
	}
	bool TouchGround()
	{
		return (
			((overallSizeRight.raycast.transform?.tag == Tags.LevelBorder.ToString() || overallSizeRight.raycast.transform?.tag == Tags.LevelPlatform.ToString())
			||
			(overallSizeLeft.raycast.transform?.tag == Tags.LevelBorder.ToString() || overallSizeLeft.raycast.transform?.tag == Tags.LevelPlatform.ToString()))
			&&
			(rigidbody.velocity.y <= 0)
			);
	}
	abstract protected void LocalUpdate();
	private void FixedUpdate()
	{
		Move();
		JumpFinish();

		LocalFixedUpdate();
	}
	void JumpFinish()
	{
		if (rigidbody.velocity.y <= 0)
		{
			if (overallSizeLeft.raycast.collider != null || overallSizeRight.raycast.collider != null)
			{
				if (!(overallSizeLeft.raycast.distance < 0.95f || overallSizeRight.raycast.distance < 0.95f))
				{
					InJump = false;
				}
			}
			else
			{
				InJump = false;
			}

		}
	}
	abstract protected void LocalFixedUpdate();
	void Move()
	{
		switch (heroMoveCondition)
		{
			case HeroMoveCondition.left:
				transform.Translate(Vector3.left * Time.deltaTime * speed);
				break;
			case HeroMoveCondition.stay:
				break;
			case HeroMoveCondition.right:
				transform.Translate(Vector3.right * Time.deltaTime * speed);
				break;
			default:
				break;
		}
	}
	void KeyEventer()
	{
		if (Input.GetKeyDown(HotKeysHelper.MoveRight))
		{
			heroMoveCondition++;
		}
		if (Input.GetKeyUp(HotKeysHelper.MoveRight))
		{
			heroMoveCondition--;
		}
		if (Input.GetKeyDown(HotKeysHelper.MoveLeft))
		{
			heroMoveCondition--;
		}
		if (Input.GetKeyUp(HotKeysHelper.MoveLeft))
		{
			heroMoveCondition++;
		}
		if (Input.GetKeyDown(HotKeysHelper.MoveUp))
		{
			Jump();
		}
		if (Input.GetKeyUp(HotKeysHelper.MoveUp))
		{
			jumpBlock = false;
		}
		if (Input.GetKeyDown(HotKeysHelper.MoveDown))
		{
			JumpOff();
		}
	}
	void JumpOff()
	{

	}
	void Jump()
	{
		if (currentJumpCount > 0)
		{
			rigidbody.velocity = Vector2.zero;
			rigidbody.AddForce(Vector2.up * jumpForse, ForceMode2D.Impulse);
			currentJumpCount--;
			jumpBlock = true;
			InJump = true;
		}

	}
}
public enum HeroMoveCondition
{
	left = -1,
	stay = 0,
	right = 1
}