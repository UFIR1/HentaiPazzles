using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuController : MonoBehaviour
{
	// Start is called before the first frame update
	GameObject MainMenu;
	InputTipe LastPlayInputTipe = InputTipe.Player;
	GameObject[] MenuPanels;
	float LastTimeScale = 1;
	private void Start()
	{
		MainMenu = transform.Find("MainMenu").gameObject;
		MenuPanels = transform.GetChildrenByTeg(Tags.Menu.ToString());
	}
	public void SwitchMenuActive()
	{
		if (MainMenu.activeSelf)
		{
			MainMenu.SetActive(false);
			Time.timeScale = LastTimeScale;
			HotKeysHelper.currentInputTipe = LastPlayInputTipe;
		}
		else
		{
			LastPlayInputTipe = HotKeysHelper.currentInputTipe;
			LastTimeScale = Time.timeScale;
			foreach (var item in MenuPanels)
			{
				item.SetActive(true);
			}
			Time.timeScale = 0;
			HotKeysHelper.currentInputTipe = InputTipe.Global;
		}
	}
}
