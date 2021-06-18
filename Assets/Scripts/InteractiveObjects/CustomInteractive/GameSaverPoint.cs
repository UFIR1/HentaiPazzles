using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaverPoint : BaseInteractiveObject
{
	public override void Use(BaseHero Sender)
	{
		GameController.gameSaver.SaveCurrentScene();
		var saver = GameObject.FindObjectOfType<GameMenuController>().saveMenuController;
		GameController.gameController.gameMenuController.SwitchMenuActive();
		var menuToClose= GameObject.FindGameObjectsWithTag(Tags.Menu.ToString());
		foreach (var item in menuToClose)
		{
			item.SetActive(false);
		}
		saver.gameObject.SetActive(true);
		saver.OpenWithSaves=true;
	}
}
