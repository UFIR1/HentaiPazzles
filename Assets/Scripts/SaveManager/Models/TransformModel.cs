using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformModel : ISaveModel
{
	private string saveName = "transform";
	public override string SaveName { get => saveName; set => saveName = value; }

	public Vector3 globalPosition;
	public Vector3 localPosition;
	public float rotation_y;
	public float rotation_x;
	public float rotation_z;
	public float rotation_w;
	public Vector3 localScale;
}

