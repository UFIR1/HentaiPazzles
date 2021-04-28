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
	protected BoxCollider2D downCollider;
	[SerializeField]
	protected BoxCollider2D upCollider;
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
	protected OverallSize overallSizeULeft;
	protected OverallSize overallSizeURight;
	protected OverallSize overallSizeDLeft;
	protected OverallSize overallSizeDRight;
	protected bool jumpBlock = false;
	private bool inJump = false;
	protected bool InJump
	{
		get => inJump;
		set
		{
			inJump = value;
			downCollider.enabled = !value;
			if (overallSizeULeft.raycast.collider?.tag != Tags.LevelBorder.ToString() && overallSizeURight.raycast.collider?.tag != Tags.LevelBorder.ToString())
			{
				if (overallSizeULeft.raycast.collider == null || overallSizeULeft.raycast.collider == null)
				{
					upCollider.enabled = true;
				}
				if (value)
				{
					upCollider.enabled = !value;
					InvokeRepeating("FinishJump", 0.3f, 0.1f);
				}
			}
		}
	}
	void FinishJump()
	{
		if (overallSizeULeft.raycast.collider?.tag != Tags.LevelBorder.ToString() && overallSizeURight.raycast.collider?.tag != Tags.LevelBorder.ToString())
		{
			if (overallSizeULeft.raycast.collider == null || overallSizeULeft.raycast.collider == null)
			{
				upCollider.enabled = true;
				InvokeRepeating("FinishJump", 0.3f, 0.1f);
			}
		}
	}
	private Collider2D lastDLeftCollider;
	private Collider2D lastDRightCollider;

	private bool inJumpOff = false;
	protected bool InJumpOff
	{
		get => inJumpOff;
		set
		{
			inJumpOff = value;
			downCollider.enabled = !value;
			if (value)
			{
				upCollider.enabled = false;
			}
			InvokeRepeating("FinishJumpOff", 0.3f, 0.1f);


		}
	}
	void FinishJumpOff()
	{
		if (
				   (overallSizeULeft.raycast.collider == null
				   || overallSizeULeft.raycast.collider != lastDLeftCollider
				   || overallSizeURight.raycast.collider != lastDRightCollider)
				   &&
				   (overallSizeURight.raycast.collider == null
				   || overallSizeURight.raycast.collider != lastDLeftCollider
				   || overallSizeURight.raycast.collider != lastDRightCollider)
				   )
		{
			upCollider.enabled = true;
		}
		CancelInvoke("FinishJumpOff");
	}
	#endregion
	// Start is called before the first frame update
	void Start()
	{
		rigidbody = gameObject.GetComponent<Rigidbody2D>();
		var colliders = gameObject.GetComponents<BoxCollider2D>();
		downCollider = colliders.Where(c => c.offset.y < 0).FirstOrDefault();
		upCollider = colliders.Where(c => c.offset.y > 0).FirstOrDefault();

		overallSizeDLeft = gameObject.GetComponentsInChildren<OverallSize>().Where(x => x.Name == "DLeft").FirstOrDefault();
		overallSizeDRight = gameObject.GetComponentsInChildren<OverallSize>().Where(x => x.Name == "DRight").FirstOrDefault();
		overallSizeULeft = gameObject.GetComponentsInChildren<OverallSize>().Where(x => x.Name == "ULeft").FirstOrDefault();
		overallSizeURight = gameObject.GetComponentsInChildren<OverallSize>().Where(x => x.Name == "URight").FirstOrDefault();



		LocalStart();
	}
	abstract protected void LocalStart();

	// Update is called once per frame
	void Update()
	{
		KeyEventer();
		if (!InJump && TouchGround())
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
			((overallSizeDRight.raycast.transform?.tag == Tags.LevelBorder.ToString() || overallSizeDRight.raycast.transform?.tag == Tags.LevelPlatform.ToString())
			||
			(overallSizeDLeft.raycast.transform?.tag == Tags.LevelBorder.ToString() || overallSizeDLeft.raycast.transform?.tag == Tags.LevelPlatform.ToString()))
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
		if (InJump)
		{
			if (rigidbody.velocity.y <= 0)
			{
				if (overallSizeDLeft.raycast.collider != null || overallSizeDRight.raycast.collider != null)
				{
					if (!(overallSizeDLeft.raycast.distance < 0.95f || overallSizeDRight.raycast.distance < 0.95f))
					{
						InJump = false;
					}
				}
				else
				{
					InJump = false;
				}
			}
			else
			{
				if (overallSizeULeft.raycast.collider?.tag == Tags.LevelBorder.ToString() || overallSizeURight.raycast.collider?.tag == Tags.LevelBorder.ToString())
				{
					upCollider.enabled = true;
				}
			}
		}
		if (inJumpOff)
		{
			if (rigidbody.velocity.y <= 0)
			{
				if (overallSizeDLeft.raycast.collider != lastDLeftCollider && overallSizeDRight.raycast.collider != lastDRightCollider)
				{
					InJumpOff = false;
				}
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
		lastDLeftCollider = overallSizeDLeft.raycast.collider;
		lastDRightCollider = overallSizeDRight.raycast.collider;
		if (lastDLeftCollider?.tag != Tags.LevelBorder.ToString() && lastDRightCollider?.tag != Tags.LevelBorder.ToString())
		{

			InJumpOff = true;
		}
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