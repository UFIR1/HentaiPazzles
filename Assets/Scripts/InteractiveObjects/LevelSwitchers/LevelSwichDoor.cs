using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwichDoor : BaseInteractiveObject
{
    public int sceneBuilNumber;
	// Start is called before the first frame update
	public override void Use(BaseHero Sender)
	{
		SceneManager.LoadScene(sceneBuilNumber,LoadSceneMode.Single);
	}
}
