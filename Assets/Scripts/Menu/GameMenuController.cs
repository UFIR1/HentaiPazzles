using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour
{
	#region menu
	// Start is called before the first frame update
	GameObject MainMenu;
	public InputType LastPlayInputType = InputType.Player;
	GameObject[] MenuPanels;
	float LastTimeScale = 1;
	private void Awake()
	{
		MainMenu = transform.Find("MainMenu").gameObject;
		MenuPanels = transform.GetChildrenByTeg(Tags.Menu.ToString());
	}
	private void Start()
	{
		
	}
	
	public void SwitchMenuActive()
	{
		if (CheckActiveMenu())
		{
			MainMenu.SetActive(false);
			foreach (var item in MenuPanels)
			{
				item.SetActive(false);
			}
			HotKeysHelper.CurrentInputType = LastPlayInputType;
		}
		else
		{
			LastPlayInputType = HotKeysHelper.CurrentInputType;
			MainMenu.SetActive(true);

			HotKeysHelper.CurrentInputType = InputType.Global;
		}
	}
	public void Exit()
	{
		Application.Quit();
	}

	private bool CheckActiveMenu()
	{
		return MainMenu.activeSelf || MenuPanels.Where(x => x.activeSelf == true).Any();
	}
	#endregion
	#region player hud
	public TextMeshProUGUI WeaponStateText;
	public Image BulletImg;
	private int lastCurrentMagazineLoaded = 0;
	private int lastCurrentCount = 0;
	public void ClearBulletCounter()
	{
		WeaponStateText.enabled = false;
		BulletImg.sprite = null;
	}
	public void RepaintBulletState(BaseBullet currentBullet, int? currentMagazineLoaded = null, int? CurrentCoun = null)
	{
		if (!WeaponStateText.enabled)
		{
			WeaponStateText.enabled = !WeaponStateText.enabled;
		}
		BulletImg.sprite = currentBullet.GetComponent<SpriteRenderer>().sprite;
		if (CurrentCoun != null)
		{
			lastCurrentCount = CurrentCoun.Value;
		}
		if (currentMagazineLoaded != null)
		{
			lastCurrentMagazineLoaded = currentMagazineLoaded.Value;
		}
		WeaponStateText.text = $"{lastCurrentMagazineLoaded}/{lastCurrentCount}";
	}
	#endregion
}
