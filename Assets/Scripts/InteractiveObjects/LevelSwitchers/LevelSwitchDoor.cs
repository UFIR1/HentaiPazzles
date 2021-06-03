using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitchDoor : BaseInteractiveObject
{
    public string sceneName;
	public string spawnPointName;
	// Start is called before the first frame update
	public override void Use(BaseHero Sender)
	{
		GameController.gameSaver.SaveCurrentScene();
		GameController.gameController.LoadLevel(sceneName, spawnPointName);
	}
}
