using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSaverModel : ISaveModel
{
	public string saveName = "ObjectSaver";
	public override string SaveName { get => saveName; set => saveName=value; }
	public string PersonalHash { get; set; }
	public int InstanceId { get; set; }
	public string PrefabPath { get; set; }
	public bool SaveInstant { get; set; }
	public List<ISaveModel> SaveModels { get; set; }
}
