using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartNewGameButton : MonoBehaviour
{
	public string sceneLoadName;
	public void Click()
	{
		SceneManager.LoadScene(sceneLoadName);
		GameController.gameSaver.DeletePreSave();
		GameController.gameSaver.CurrentSceneName = sceneLoadName;
		GameSaver.CurrentSaveName = "NewGame";
	}
}
