using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class TransformSaver : MonoBehaviour, ISaveble<TransformModel>, ISaveble<ISaveModel>
{
	public string PersonalHash { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

	public bool SaveGlobalPosition=false;
	public bool SaveLocalPosition = false;
	public bool SaveRotation = false;
	public bool SaveScale= false;


	public Type getTT()
	{
		return typeof(TransformModel);
	}

	public void Load(TransformModel model)
	{
		if (SaveGlobalPosition)
		{
			transform.position = model.globalPosition ;
		}
		if (SaveLocalPosition)
		{
			transform.localPosition= model.localPosition ;
		}
		if (SaveRotation)
		{
			transform.rotation= model.rotation ;
		}
		if (SaveScale)
		{
			transform.localScale = model.localScale;
		}
	}


	public TransformModel Save()
	{
		var result = new TransformModel();
		if (SaveGlobalPosition)
		{
			result.globalPosition = transform.position;
		}
		if (SaveLocalPosition)
		{
			result.localPosition = transform.localPosition;
		}
		if (SaveRotation)
		{
			result.rotation = transform.rotation;
		}
		if (SaveScale)
		{
			result.localScale = transform.localScale;
		}
		return result;
	}
	public void Load(ISaveModel model)
	{
		Load(model as TransformModel);
	}

	ISaveModel ISaveble<ISaveModel>.Save()
	{
		return Save();
	}
}
