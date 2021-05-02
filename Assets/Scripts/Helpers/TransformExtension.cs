using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension
{
	public static GameObject[] GetChildrenByTeg(this Transform transform, string tag)
	{
		var result = new List<GameObject>(); 
		for (int i = 0; i < transform.childCount; i++)
		{
			var child = transform.GetChild(i);
			if (child.tag == tag)
			{
				result.Add(child.gameObject);
			}
		}
		return result.ToArray();
	}

}
