//using UnityEditor;
//using UnityEngine;
//using UnityEditor.UIElements;
//using UnityEngine.UIElements;
//using System.Collections.Generic;

//[CustomPropertyDrawer(typeof(TutInputAttribute))]
//public class TutInputDrawer : PropertyDrawer
//{
//	public List<zzzzzs> zzzzzss = new List<zzzzzs>();
//	public override VisualElement CreatePropertyGUI(SerializedProperty property)
//	{
//		var contaner = new VisualElement();
//		foreach (var item in zzzzzss)
//		{
//			contaner.Add(new PropertyField(item.prop,item.lable.text));
//		}
//		return contaner;
//	}
//	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//	{
//		zzzzzss.Add(new zzzzzs() { pos = position, prop = property, lable = label });
		
//	}
//}
//public class zzzzzs
//{
//	public Rect pos;
//	public SerializedProperty prop;
//	public GUIContent lable;
//}
//public class TutInputAttribute : PropertyAttribute
//{
//	public TutInputAttribute()
//	{

//	}
//}