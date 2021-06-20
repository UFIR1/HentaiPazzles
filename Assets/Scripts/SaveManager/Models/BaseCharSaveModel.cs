public class BaseCharSaveModel : ISaveModel
{
	private string saveName = null;
	public override string SaveName { get => saveName; set => saveName = value; }
	public int MaxHeals { get; set; }
	public int CurrentHeals { get; set; }
}
