using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformModel : ISaveModel
{
	private string saveName = "transform";
	public override string SaveName { get => saveName; set => saveName = value; }

	public Vector3 globalPosition;
	public Vector3 localPosition;
	//public MyVector4 rotation;
	public float rotation_y;
	public float rotation_x;
	public float rotation_z;
	public float rotation_w;
	public Vector3 localScale;
}
/*
[System.Serializable]
public class MyVector4
{
	public float x;
	public float y;
	public float z;
	public float w;
	public MyVector4()
	{

	}
	public MyVector4(float x, float y, float z=0, float w=0)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

}*/
