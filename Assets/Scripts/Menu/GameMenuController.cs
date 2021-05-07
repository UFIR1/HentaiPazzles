using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class GameMenuController : MonoBehaviour
{
	#region menu
	// Start is called before the first frame update
	GameObject MainMenu;
	InputType LastPlayInputTipe = InputType.Player;
	GameObject[] MenuPanels;
	float LastTimeScale = 1;
	private void Start()
	{
		MainMenu = transform.Find("MainMenu").gameObject;
		MenuPanels = transform.GetChildrenByTeg(Tags.Menu.ToString());
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
			Time.timeScale = LastTimeScale;
			HotKeysHelper.currentInputType = LastPlayInputTipe;
		}
		else
		{
			LastPlayInputTipe = HotKeysHelper.currentInputType;
			LastTimeScale = Time.timeScale;
			MainMenu.SetActive(true);

			Time.timeScale = 0;
			HotKeysHelper.currentInputType = InputType.Global;
		}
	}

	private bool CheckActiveMenu()
	{
		return MainMenu.activeSelf || MenuPanels.Where(x => x.activeSelf == true).Any();
	}
	#endregion
	#region player
	public TextMeshProUGUI WeaponStateText;
	private int lastCurrentMagazineLoaded = 0;
	private int lastCurrentCount = 0;
	public void RepaintBulletState(BaseBullet currentBullet, int? currentMagazineLoaded = null, int? CurrentCoun = null)
	{
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
