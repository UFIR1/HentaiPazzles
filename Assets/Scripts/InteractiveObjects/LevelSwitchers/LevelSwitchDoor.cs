using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitchDoor : BaseInteractiveObject
{
    public int sceneBuildNumber;
	public string spawnPointName;
	public LoadSceneMode loadSceneMode= LoadSceneMode.Single;
	// Start is called before the first frame update
	public override void Use(BaseHero Sender)
	{
		GameController.gameController.LoadLevel(sceneBuildNumber, spawnPointName, loadSceneMode);
	}
}
