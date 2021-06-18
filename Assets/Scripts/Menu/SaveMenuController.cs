using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro ;

public class SaveMenuController : MonoBehaviour
{
    public GameObject content;
    public GameObject keySetter;
    public float step;
    public List<GameObject> keyElement;
    public TMP_InputField NewSaveName;
    public Button newSaveButton;
    public TMP_InputField newSaveInputField;
	private bool openWithSaves = false;
	public bool OpenWithSaves { get => openWithSaves; set { openWithSaves = value; Repaint(); } }

	private void OnEnable()
	{
        if (GameController.gameController.Player != null)
        {
            OpenWithSaves = false;
        }
	}
	// Start is called before the first frame update
	void Start()
    {
        Repaint();
    }
	public void Repaint()
	{
		{//SetSaveButtons
            newSaveButton.interactable = OpenWithSaves;
            newSaveInputField.interactable = OpenWithSaves;
		}
		foreach (var item in keyElement)
		{
            Destroy(item);
		}
        keyElement.Clear();
        var saves = GameController.gameSaver.GetExistingSaves();
        float i = 0;
        float sumHeight = 0;
        foreach (var item in saves)
        {

            var newElement = Instantiate(keySetter, content.transform);
            keyElement.Add(newElement);

            var setter = newElement.GetComponent<SaveMenuItem>();
            var newElementTransform = newElement.GetComponent<RectTransform>();
            setter.Init(item,this,OpenWithSaves);
            sumHeight += newElementTransform.rect.height + step;
            i++;
        }
        sumHeight += 100;
        var contentTransform = ((RectTransform)content.transform);
        contentTransform.sizeDelta = new Vector2(0, sumHeight);
        var j = sumHeight / 2;
        i = 0.5f;
        foreach (var item in keyElement)
        {
            var newElementTransform = item.GetComponent<RectTransform>();
            var asd = newElementTransform.anchoredPosition = new Vector2(0, j - ((i * newElementTransform.rect.height) + step));
            i++;
        }
    }
    public void CreateNewSave()
	{
        GameController.gameSaver.SaveCurrentScene();
        var saveName = NewSaveName.text;
        if (!string.IsNullOrEmpty(saveName))
        {
            GameController.gameSaver.SaveGame(saveName);
        }
        Repaint();

    }
    public void ReSave(string saveName)
	{
        GameController.gameSaver.SaveCurrentScene();
        GameController.gameSaver.SaveGame(saveName);
        Repaint();
    }
  
    public void Load(string saveName)
	{
        GameController.gameSaver.LoadGame(saveName);
        HotKeysHelper.CurrentInputType = InputType.Player;
        Repaint();

    }
    public void Delete(string saveName)
	{
        GameController.gameSaver.DeleteSave(saveName);
        Repaint();

    }
}
