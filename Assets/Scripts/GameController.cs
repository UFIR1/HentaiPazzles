using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public static GameController gameController;
	public static GameSaver gameSaver;
	GameObject Canvas;
	public GameMenuController gameMenuController;
	static string spawnPointName;
	public GameObject Player;
	public RecourseManager RecourseManager;

	private void Awake()
	{
		
		gameController = this;
        DontDestroyOnLoad(gameObject);
		if (GameObject.FindGameObjectsWithTag(Tags.GameController.ToString()).Length > 1)
		{
			Destroy(gameObject);
		}
		gameController = this;
		gameSaver = gameObject.GetComponent<GameSaver>();
		gameSaver.CurrentSceneName = SceneManager.GetActiveScene().name;
		Player = GameObject.FindGameObjectWithTag(Tags.Player.ToString());
	}
	// Start is called before the first frame update
	void Start()
    {
		GameSaver.externalOnLoadFinished += SaverOnLevelLoaded;
		HotKeysHelper.CurrentInputType = InputType.Player;
		Init();
	}
	private void OnLevelWasLoaded(int level)
	{
		if (Player == null)
		{
			Player = GameObject.FindGameObjectWithTag(Tags.Player.ToString());
			DontDestroyOnLoad(Player);
		}
		gameController = this;
		gameSaver = gameObject.GetComponent<GameSaver>();
		gameSaver.CurrentSceneName = SceneManager.GetActiveScene().name;
		Init();

	}
	public void SaverOnLevelLoaded()
	{
		
		if (Player != null)
		{
			TeleportedPlayerOnSpawnPoint();
		}
		else
		{
			Player = GameObject.FindGameObjectWithTag(Tags.Player.ToString());
			DontDestroyOnLoad(Player);
			if (Player != null)
			{
				TeleportedPlayerOnSpawnPoint();
			}
		}
	}

	private void TeleportedPlayerOnSpawnPoint()
	{
		if (!string.IsNullOrEmpty(spawnPointName))
		{
			var spawnPoint = GameObject.Find(spawnPointName);
			Player.transform.position = spawnPoint.transform.position;
			spawnPointName = null;
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
	public void LoadLevel(string sceneName,string spawnPointName)
	{
		GameController.spawnPointName = spawnPointName;
		if (Player!=null)
		{
			GameObject.DontDestroyOnLoad(Player);
			Player.GetComponent<ObjectSaver>().dontDestroyMe = true;
		}
		gameSaver.LoadScene(sceneName);
		
	}

	
}

