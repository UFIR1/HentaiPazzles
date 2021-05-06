using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public static GameController gameController;
	GameObject Canvas;
	GameMenuController gameMenuController;
	string spawnPointName;
	public GameObject Player;

	private void Awake()
	{
        DontDestroyOnLoad(gameObject);
		if (GameObject.FindGameObjectsWithTag(Tags.GameController.ToString()).Length > 1)
		{
			Destroy(gameObject);
		}
		gameController = this;
		Player = GameObject.FindGameObjectWithTag(Tags.Player.ToString());
	}
	// Start is called before the first frame update
	void Start()
    {
        HotKeysHelper.currentInputType = InputType.Player;
		Init();
	}
	private void OnLevelWasLoaded(int level)
	{
		gameController = this;
		Init();
		if (Player != null)
		{
			if (!string.IsNullOrEmpty(spawnPointName))
			{
				var spawnPoint = GameObject.Find(spawnPointName);
				Player.transform.position = spawnPoint.transform.position;
			}
		}
		else
		{
			Player = GameObject.FindGameObjectWithTag(Tags.Player.ToString());
		}

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
				gameMenuController.SwitchMenuActive();
			}
		}
	}
	public void LoadLevel(int sceneBuilNumber,string spawnPointName, LoadSceneMode loadSceneMode)
	{
		this.spawnPointName = spawnPointName;
		if (Player!=null)
		{
			GameObject.DontDestroyOnLoad(Player);
		}
		SceneManager.LoadScene(sceneBuilNumber, loadSceneMode);
		
	}

	
}

