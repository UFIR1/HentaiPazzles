using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseHero : BaseChar, ISaveble<BaseHeroSaveModel>, ISaveble<ISaveModel>
{
	[SerializeField]
	protected SpriteRenderer spriteRenderer;
	#region Animation
	[SerializeField]
	protected Animator animator;
	#endregion

	#region lockalMovemantFields

	[SerializeField]
	protected Rigidbody2D rigidbody;
	[SerializeField]
	protected BoxCollider2D downCollider;
	[SerializeField]
	protected BoxCollider2D upCollider;
	[SerializeField]
	private HeroMoveCondition heroMoveCondition = 0;

	protected HeroMoveCondition HeroMoveCondition
	{
		get => heroMoveCondition;
		set
		{
			if ((int)value > 1)
			{
				heroMoveCondition = (HeroMoveCondition)1;
			}
			else
			if ((int)value < -1)
			{
				heroMoveCondition = (HeroMoveCondition)(-1);
			}
			else
			{
				heroMoveCondition = value;
			}
			if (animator != null)
			{
				if ((int)value != 0)
				{
					animator.SetBool("Run", true);
					spriteRenderer.flipX = (heroMoveCondition == HeroMoveCondition.left);
				}
				else
				{
					animator.SetBool("Run", false);
				}
				FlipWeapon(heroMoveCondition);
			}
		}
	}
	[SerializeField]
	[Range(0.0f, 10.0f)]
	protected float speed = 5.5f;
	[SerializeField]
	//[TutInput]
	protected float jumpForce = 1f;
	[SerializeField]
	//[TutInput]
	protected int jumpCount = 2;
	[SerializeField]
	protected int currentJumpCount = 2;
	protected OverallSize overallSizeULeft;
	protected OverallSize overallSizeURight;
	protected OverallSize overallSizeDLeft;
	protected OverallSize overallSizeDRight;

	protected OverallSize movementBlocUL;
	protected OverallSize movementBlocUR;
	protected OverallSize movementBlocDL;
	protected OverallSize movementBlocDR;


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
				if (overallSizeULeft.raycast.collider == null && overallSizeURight.raycast.collider == null)
				{
					upCollider.enabled = true;
				}
				if (value)
				{

					upCollider.enabled = !value;

					InvokeRepeating(nameof(FinishJump), 0.3f, 0.1f);
				}
			}
			if (animator != null)
			{
				animator.SetBool("Jump", value);
			}

		}
	}
	void FinishJump()
	{

		if (overallSizeULeft.raycast.collider == null && overallSizeURight.raycast.collider == null)
		{
			if (!InJump)
			{
				upCollider.enabled = true;
				CancelInvoke(nameof(FinishJump));
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
			InvokeRepeating(nameof(FinishJumpOff), 0.3f, 0.1f);


		}
	}


	void FinishJumpOff()
	{
		if (overallSizeDLeft.raycast.collider == null && overallSizeDRight.raycast.collider == null)
		{
			downCollider.enabled = true;
		}
		if (
				   (overallSizeULeft.raycast.collider == null
								  /* && overallSizeULeft.raycast.collider != lastDLeftCollider
								   && overallSizeURight.raycast.collider != lastDRightCollider*/)
				   &&
				   (overallSizeURight.raycast.collider == null
								  /* && overallSizeURight.raycast.collider != lastDLeftCollider
								   && overallSizeURight.raycast.collider != lastDRightCollider*/)
				   )
		{
			upCollider.enabled = true;

			CancelInvoke(nameof(FinishJumpOff));
		}

	}
	#endregion
	protected List<localInteractiveObject> interactiveObjects = new List<localInteractiveObject>();
	#region mainLogic
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

		movementBlocUL = gameObject.GetComponentsInChildren<OverallSize>().Where(x => x.Name == "ULeftMB").FirstOrDefault();
		movementBlocUR = gameObject.GetComponentsInChildren<OverallSize>().Where(x => x.Name == "URightMB").FirstOrDefault();
		movementBlocDL = gameObject.GetComponentsInChildren<OverallSize>().Where(x => x.Name == "DLeftMB").FirstOrDefault();
		movementBlocDR = gameObject.GetComponentsInChildren<OverallSize>().Where(x => x.Name == "DRightMB").FirstOrDefault();



		LocalStart();
	}

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
					InJump = false;
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



		switch (HeroMoveCondition)
		{
			case HeroMoveCondition.left:
				if (movementBlocUL.raycast.collider?.tag == Tags.LevelBorder.ToString() || movementBlocDL.raycast.collider?.tag == Tags.LevelBorder.ToString())
				{

				}
				else
				{
					transform.Translate(Vector3.left * Time.deltaTime * speed);

				}
				break;
			case HeroMoveCondition.stay:
				break;
			case HeroMoveCondition.right:
				if (movementBlocUR.raycast.collider?.tag == Tags.LevelBorder.ToString() || movementBlocDR.raycast.collider?.tag == Tags.LevelBorder.ToString())
				{

				}
				else
				{
					transform.Translate(Vector3.right * Time.deltaTime * speed);
				}
				break;
			default:
				break;
		}
	}

	void JumpOff()
	{
		CancelInvoke(nameof(FinishJumpOff));
		CancelInvoke(nameof(FinishJump));
		lastDLeftCollider = overallSizeDLeft.raycast.collider;
		lastDRightCollider = overallSizeDRight.raycast.collider;
		if (lastDLeftCollider?.tag != Tags.LevelBorder.ToString() && lastDRightCollider?.tag != Tags.LevelBorder.ToString())
		{

			InJumpOff = true;
		}

	}
	void Jump()
	{
		CancelInvoke("FinishJumpOff");
		CancelInvoke("FinishJump");
		if (currentJumpCount > 0)
		{
			rigidbody.velocity = Vector2.zero;
			rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			currentJumpCount--;
			jumpBlock = true;
			InJump = true;
		}
	}
	virtual public void OnCollisionEnter2D(Collision2D collision)
	{

		if (collision.gameObject.tag == Tags.Enemy.ToString()
			||
			collision.gameObject.tag == Tags.Consumables.ToString())
		{
			Physics2D.IgnoreCollision(collision.collider, downCollider);
			Physics2D.IgnoreCollision(collision.collider, upCollider);

		}
	}
	private void Interactive()
	{
		if (interactiveObjects.Count > 0)
		{
			for (int i = 0; i < interactiveObjects.Count;)
			{
				var item = interactiveObjects[i];
				if (item == null)
				{
					interactiveObjects.Remove(item);
				}
				else if (item.transform == null || item.interactive == null)
				{
					interactiveObjects.Remove(item);
				}
				else
				{
					i++;
				}

			}
			var minVector = new Vector2(interactiveObjects[0].transform.position.x, interactiveObjects[0].transform.position.y) - new Vector2(transform.position.x, transform.position.y);
			var closerObj = interactiveObjects[0];
			for (int i = 1; i < interactiveObjects.Count; i++)
			{
				var itemVector = (new Vector2(interactiveObjects[i].transform.position.x, interactiveObjects[i].transform.position.y) - new Vector2(transform.position.x, transform.position.y));
				if ((itemVector.x + itemVector.y) < (minVector.x + minVector.y))
				{
					minVector = itemVector;
					closerObj = interactiveObjects[i];
				}
			}
			closerObj.interactive.Use(this);
		}

	}
	virtual protected void interactiveObjEnter(Transform interactiveObjTransform, IInteractive interactiveObj)
	{
		interactiveObjects.Add(new localInteractiveObject(interactiveObjTransform, interactiveObj));
	}
	virtual protected void interactiveObjExit(Transform interactiveObjTransform, IInteractive interactiveObj)
	{
		interactiveObjects.Remove(interactiveObjects.Where(x => x.transform == interactiveObjTransform && x.interactive == interactiveObj).FirstOrDefault());
	}
	virtual public void OnTriggerEnter2D(Collider2D collision)
	{
		var interactiveObj = collision.GetComponent<IInteractive>();
		if (interactiveObj != null)
		{
			interactiveObjEnter(collision.transform, interactiveObj);
		}


	}
	virtual public void OnTriggerExit2D(Collider2D collision)
	{
		var interactiveObj = collision.GetComponent<IInteractive>();
		if (interactiveObj != null)
		{
			interactiveObjExit(collision.transform, interactiveObj);
		}

	}
	protected class localInteractiveObject
	{
		public Transform transform;
		public IInteractive interactive;
		public localInteractiveObject(Transform transform, IInteractive interactive)
		{
			this.transform = transform;
			this.interactive = interactive;
		}
	}
	#endregion

	#region keys
	void KeyEventer()
	{
		if (HotKeysHelper.PlayerKey(Input.GetKeyDown(HotKeysHelper.MoveRight)))
		{
			HeroMoveCondition++;
		}
		if (HotKeysHelper.PlayerKey(Input.GetKeyUp(HotKeysHelper.MoveRight)))
		{
			HeroMoveCondition--;
		}
		if (HotKeysHelper.PlayerKey(Input.GetKeyDown(HotKeysHelper.MoveLeft)))
		{
			HeroMoveCondition--;
		}
		if (HotKeysHelper.PlayerKey(Input.GetKeyUp(HotKeysHelper.MoveLeft)))
		{
			HeroMoveCondition++;
		}
		if (HotKeysHelper.PlayerKey(Input.GetKeyDown(HotKeysHelper.MoveUp)))
		{
			Jump();
		}
		if (HotKeysHelper.PlayerKey(Input.GetKeyUp(HotKeysHelper.MoveUp)))
		{
			jumpBlock = false;
		}
		if (HotKeysHelper.PlayerKey(Input.GetKeyDown(HotKeysHelper.MoveDown)))
		{
			JumpOff();
		}
		if (HotKeysHelper.PlayerKey(Input.GetKeyDown(HotKeysHelper.Hit)))
		{
			TriggerDown();
		}
		if (HotKeysHelper.PlayerKey(Input.GetKeyUp(HotKeysHelper.Hit)))
		{
			TriggerUp();
		}
		if (HotKeysHelper.PlayerKey(Input.GetKeyDown(HotKeysHelper.Interaction)))
		{
			Interactive();
		}
		if (HotKeysHelper.PlayerKey(Input.GetKeyDown(HotKeysHelper.Reload)))
		{
			ReloadWeapon();
		}
		if (HotKeysHelper.PlayerKey(Input.GetKeyDown(HotKeysHelper.SwitchBulletType)))
		{
			SwitchBulletType();
		}
		if (HotKeysHelper.PlayerKey(Input.GetKeyDown(HotKeysHelper.SelectFirstWeapon)))
		{
			SwitchWeapon(0);
		}
	}
	#endregion

	#region coins
	[SerializeField]
	private int coins;
	public int Coins { get => coins; protected set => coins = value; }

	public bool PayCoins(int quantity)
	{
		if (quantity <= coins)
		{
			Coins -= quantity;
			return true;
		}
		return false;
	}
	public void RaiseCoins(int quantity)
	{
		coins += OnRaiseCoins(quantity);
	}
	protected virtual int OnRaiseCoins(int quantity)
	{
		return quantity;
	}
	#endregion

	#region weaponsAndBullets
	[SerializeField]
	protected InventoryWeapon[] weapons = new InventoryWeapon[3];
	[SerializeField]
	private BaseWeapon activeWeapon;
	protected bool activeWeaponSwitchable = false;
	protected bool activeWeaponReloading = false;
	private List<StoredBullet> bullets = new List<StoredBullet>();
	public List<StoredBullet> Bullets { get => bullets; protected set => bullets = value; }
	protected BaseWeapon ActiveWeapon
	{
		get => activeWeapon;
		set
		{
			if (activeWeapon != null)
			{
				activeWeapon.gameObject.SetActive(false);
				activeWeapon.OnMagazineLoadChange -= CurrentMagazineLoadedUpdate;
				GameController.gameController.gameMenuController.ClearBulletCounter();
			}
			activeWeapon = value;
			if (activeWeapon != null)
			{
				activeWeapon.gameObject.SetActive(true);
				activeWeapon.OnMagazineLoadChange += CurrentMagazineLoadedUpdate;
				if (NextReloadBullets?.bullet == null)
				{
					NextReloadBullets = bullets.Where(x => x.bullet.ownerWeapon.GetType() == activeWeapon.GetType()).FirstOrDefault();
				}
				if (NextReloadBullets?.bullet != null)
				{
					GameController.gameController.gameMenuController.RepaintBulletState(NextReloadBullets.bullet, activeWeapon.CurrentMagazineLoaded, NextReloadBullets.CurrentCount);
				}
			}
		}
	}


	private void CurrentMagazineLoadedUpdate(int currentMagazineLoaded, BaseBullet currentBullet)
	{
		var myScore = Bullets.Where(x => x.bullet.GetType() == currentBullet.GetType()).FirstOrDefault();
		GameController.gameController.gameMenuController.RepaintBulletState(currentBullet, currentMagazineLoaded, ((myScore.bullet != null) ? myScore.CurrentCount : 0));
	}
	private void CurrentBulletsCountUpdate(int currentCount, BaseBullet currentBullet)
	{
		GameController.gameController.gameMenuController.RepaintBulletState(currentBullet, null, currentCount);
	}

	private StoredBullet nextReloadBullets = null;
	protected StoredBullet NextReloadBullets
	{
		get => nextReloadBullets;
		set
		{
			try
			{
				if (nextReloadBullets.bullet != null)
				{
					nextReloadBullets.OnCurrentCountChange -= CurrentBulletsCountUpdate;
				}
			}
			catch { }
			nextReloadBullets = value;
			try
			{
				if (nextReloadBullets.bullet != null)
				{
					nextReloadBullets.OnCurrentCountChange += CurrentBulletsCountUpdate;
					GameController.gameController.gameMenuController.RepaintBulletState(nextReloadBullets.bullet, activeWeapon.CurrentMagazineLoaded, NextReloadBullets.CurrentCount);
				}
			}
			catch { }
		}
	}

	//!смена оружия
	public void SwitchWeapon(int weaponNumber)
	{
		if (weapons[weaponNumber] != null && weapons[weaponNumber]?.unlock==true)
		{
			activeWeaponSwitchable = true;
			Invoke(nameof(activeWeaponSwitchableFinish), weapons[weaponNumber].weapon.SwitchableTime);
			if (activeWeapon != weapons[weaponNumber].weapon)
			{
				ActiveWeapon = weapons[weaponNumber].weapon;
			}
			else
			{
				ActiveWeapon = null;
			}
		}
	}
	private void activeWeaponSwitchableFinish()
	{
		activeWeaponSwitchable = false;
	}
	//!Перезарядка
	public void ReloadWeapon()
	{
		if (ActiveWeapon != null)
		{
			if (NextReloadBullets?.bullet == null)
			{
				NextReloadBullets = Bullets.Where(x => x.bullet?.ownerWeapon?.GetType() == ActiveWeapon.GetType()).FirstOrDefault();
			}
			if (NextReloadBullets?.bullet == null)
			{
				return;
			}
			if (NextReloadBullets.CurrentCount > 0)
			{
				if (ActiveWeapon.CurrentMagazineLoaded < ActiveWeapon.MagazineSize || ActiveWeapon.CurrentBullet?.GetType() != NextReloadBullets.bullet.GetType())
				{
					activeWeaponReloading = true;
					Invoke(nameof(activeWeaponReloadableFinish), ActiveWeapon.ReloadTime);
					var remains = NextReloadBullets.CurrentCount - ActiveWeapon.MagazineSize;
					var toLoad = ActiveWeapon.MagazineSize + ((remains < 0) ? remains : 0);
					BaseBullet oldBullet = null;
					var comeback = ActiveWeapon.Reload(toLoad, NextReloadBullets.bullet, out oldBullet);
					if (comeback > 0)
					{
						if (oldBullet != null)
						{
							if (NextReloadBullets.bullet.GetType() == oldBullet.GetType())
							{

								toLoad -= comeback;
							}
							else
							{
								Bullets.Where(x => x.bullet.GetType() == oldBullet.GetType()).FirstOrDefault().CurrentCount += comeback;
							}
						}
					}
					NextReloadBullets.CurrentCount -= toLoad;
				}
			}
		}
	}
	public bool PickUpBullet(StoredBullet bullet)
	{
		var insideBullet = bullets.Where(x => x.bullet.GetType() == bullet.bullet.GetType()).FirstOrDefault();
		if (insideBullet != null)
		{
			if (insideBullet.CurrentCount < insideBullet.MaxStuckSize)
			{
				insideBullet.CurrentCount += bullet.CurrentCount;
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			bullets.Add(bullet);
			if (activeWeapon?.GetType() == bullet.bullet.ownerWeapon.GetType() && nextReloadBullets == null)
			{
				NextReloadBullets = bullet;
				GameController.gameController.gameMenuController.RepaintBulletState(bullet.bullet, CurrentCoun: bullet.CurrentCount);
			}

			return true;
		}
		return false;
	}
	public bool PickUpWeapon(BaseWeapon weapon, int i)
	{
		
		return true;
	}
	private void activeWeaponReloadableFinish()
	{
		activeWeaponReloading = false;
	}

	protected virtual void TriggerDown()
	{
		ActiveWeapon?.TriggerDown(this);
	}
	protected virtual void TriggerUp()
	{
		ActiveWeapon?.TriggerUp(this);
	}
	private void FlipWeapon(HeroMoveCondition moveCondition)
	{
		if (moveCondition == HeroMoveCondition.left)
		{
			foreach (var item in weapons)
			{
				if (item.weapon != null)
				{
					item.weapon.transform.localScale = new Vector3(-Math.Abs(item.weapon.transform.localScale.x), item.weapon.transform.localScale.y);
				}
			}
		}
		if (moveCondition == HeroMoveCondition.right)
		{
			foreach (var item in weapons)
			{
				if (item.weapon != null)
				{
					item.weapon.transform.localScale = new Vector3(Math.Abs(item.weapon.transform.localScale.x), item.weapon.transform.localScale.y);
				}
			}
		}
	}
	public void SwitchBulletType()
	{
		if (activeWeapon != null)
		{
			var allBulletTypes = Bullets.Where(x => x.bullet.ownerWeapon.GetType() == activeWeapon.GetType()).ToList();
			if (allBulletTypes.Count() > 1)
			{
				for (int i = 0; i < allBulletTypes.Count(); i++)
				{
					if (allBulletTypes[i].bullet.GetType() == NextReloadBullets.bullet.GetType())
					{
						if (i == allBulletTypes.Count() - 1)
						{
							NextReloadBullets = allBulletTypes[0];
							break;
						}
						NextReloadBullets = allBulletTypes[i + 1];
						break;
					}


				}
			}
		}
	}



	#endregion

	#region save
	public string InSaverName = null;
	[SerializeField]
	private string personalHash = Guid.NewGuid().ToString();
	public string PersonalHash { get { return personalHash; } set { personalHash = value; } }
	public void Load(BaseHeroSaveModel model)
	{
		Coins = model.Coins;
		for (int i = 0; i < model.Weapons.Length; i++)//Weapons
		{
			var item = model.Weapons[i];
			weapons[i].unlock = item.unlock;
		}
		bullets = model.Bullets;
	}

	public BaseHeroSaveModel Save()
	{
		var toSave= new BaseHeroSaveModel()
		{
			Coins = Coins,
			SaveName = InSaverName
		};
		toSave.Weapons = weapons;
		toSave.Bullets = Bullets;
		return toSave;
	}
	public Type getTT()
	{
		return typeof(BaseHeroSaveModel);
	}

	void ISaveble<ISaveModel>.Load(ISaveModel model)
	{
		Load(model as BaseHeroSaveModel);
	}

	ISaveModel ISaveble<ISaveModel>.Save()
	{
		return Save();
	}
	#endregion

}



public enum HeroMoveCondition
{
	left = -1,
	stay = 0,
	right = 1
}