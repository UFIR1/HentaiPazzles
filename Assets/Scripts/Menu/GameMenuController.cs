using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameMenuController : MonoBehaviour
{
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
		return MainMenu.activeSelf || MenuPanels.Where(x=>x.activeSelf==true).Any();
	}
}
