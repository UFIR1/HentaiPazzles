using System.Collections.Generic;

public class SceneSaverModel : ISaveModel
{
	public override string SaveName { get => "Scene"; set { } }
	public List<ObjectSaverModel> SaveableObjects { get; set; }
}
