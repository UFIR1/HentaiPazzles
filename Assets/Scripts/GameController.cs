using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	GameObject Canvas;
	GameMenuController gameMenuController;

	private void Awake()
	{
        DontDestroyOnLoad(gameObject);
		if (GameObject.FindGameObjectsWithTag(Tags.GameController.ToString()).Length > 1)
		{
			Destroy(gameObject);
		}
	}
	// Start is called before the first frame update
	void Start()
    {
        HotKeysHelper.currentInputTipe = InputTipe.Player;
		Init();
	}
	private void OnLevelWasLoaded(int level)
	{
		Init();

	}

	private void Init()
	{
		Canvas = GameObject.FindGameObjectWithTag(Tags.Canvas.ToString());
		if (Canvas != null)
		{
			gameMenuController = Canvas.GetComponent<GameMenuController>();
		}
	}


	// Update is called once per frame
	void Update()
    {
		
		if (HotKeysHelper.GlobalKey(Input.GetKeyDown(KeyCode.Escape)))
		{
			if (Canvas != null)
			{
				gameMenuController.SwichMenuActive();
			}
		}
	}

	
}

