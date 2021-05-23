using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "RecourseManager", menuName = "Recourse Manager", order = 52)]
public class RecourseManager : ScriptableObject
{
    public RecourseItem[] recourses;

#if UNITY_EDITOR
	[ContextMenu("Generator")]
    public void Generator()
	{
		foreach (var item in recourses)
		{
			item.prefabPath = AssetDatabase.GetAssetPath(item.prefab);
		}
	}
#endif
}
[System.Serializable]
public class RecourseItem
{
    public GameObject prefab;
    public string prefabPath;
}