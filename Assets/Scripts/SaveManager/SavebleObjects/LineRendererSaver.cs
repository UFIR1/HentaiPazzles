using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class LineRendererSaver : MonoBehaviour, ISaveble<LineRendererSaverModel>, ISaveble<ISaveModel>
{

	public Type getTT()
	{
		return typeof(LineRendererSaverModel);
	}

	public void Load(LineRendererSaverModel model)
	{
		var lineRender = transform.GetComponent<LineRenderer>();
		lineRender.loop = model.Loop;
		for (int i = 0; i < model.Positions.Count; i++)
		{
			lineRender.SetPosition(i, model.Positions[i]);
		} 


	}
	public LineRendererSaverModel Save()
	{
		var lineRender = transform.GetComponent<LineRenderer>();
		var toSave = new LineRendererSaverModel()
		{
			Loop = lineRender.loop,
			Positions = new List<Vector3>(),
		};
		for (int i = 0; i < lineRender.positionCount; i++)
		{
			toSave.Positions.Add(lineRender.GetPosition(i));
		}
		return toSave;
	}


	public void Load(ISaveModel model)
	{
		Load(model as LineRendererSaverModel);
	}

	

	ISaveModel ISaveble<ISaveModel>.Save()
	{
		return Save();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public class LineRendererSaverModel : ISaveModel
{
	public override string SaveName { get { return nameof(LineRendererSaverModel); } set { } }

	public bool Loop { get; set; }
	public List<Vector3> Positions { get; set; }
    

}
