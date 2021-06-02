using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HashesSaver", menuName = "HashesSaver", order = 53)]

public class HashesSaver : ScriptableObject
{
	private List<ObjIndexFinger> unicalHashController;

	public List<ObjIndexFinger> UnicalHashController
	{
		get
		{
			if (unicalHashController == null)
			{
				unicalHashController = new List<ObjIndexFinger>();
			}
			return unicalHashController;
		}
		set => unicalHashController = value;
	}
}
