using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveMenuItem : MonoBehaviour
{
    public Image Image;
    public TextMeshProUGUI SaveNameMash;
    public TextMeshProUGUI SaveDateMash;
    public TextMeshProUGUI UpdateDateMash;
    public SaveMenuController saveController;
    public GameSaverViewModel myModel;
    public void Init( GameSaverViewModel viewModel, SaveMenuController saveController)
	{
        this.myModel = viewModel;
        this.saveController = saveController;
        if (viewModel.Texture != null)
        {
            Image.sprite = Sprite.Create(viewModel.Texture, new Rect(0, 0, viewModel.Texture.width, viewModel.Texture.height), new Vector2(.5f, .5f));
        }
        SaveNameMash.text = viewModel.Name;
        SaveDateMash.text = viewModel.CreateTime.ToString();
        UpdateDateMash.text = viewModel?.UpdateTime.ToString();
    }
    public void ReSaveMe()
	{
        saveController.ReSave(myModel.Name);
	}
    public void LoadMe()
	{
        saveController.Load(myModel.Name);
	}
    public void DeleteMe()
	{
        saveController.Delete(myModel.Name);
	}

}
