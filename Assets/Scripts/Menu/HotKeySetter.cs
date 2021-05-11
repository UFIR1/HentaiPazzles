using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using TMPro;

public class HotKeySetter : MonoBehaviour
{
	private PropertyInfo property;
	bool waitToKey = false;
	static List<HotKeySetter> hotKeys = new List<HotKeySetter>();
	public GameObject hotKeyObj;
	public GameObject hotNameObj;
	private TextMeshProUGUI hotKeyTextMesh;
	private TextMeshProUGUI hotNameTextMesh;
	private string hotKeyText;
	private string hotNameText;

	public string HotKeyText
	{
		get => hotKeyText;
		set
		{
			hotKeyText = value;
			hotKeyTextMesh.text = value??"";
		}
	}

	public string HotNameText
	{
		get => hotNameText; set
		{
			hotNameText = value;
			hotNameTextMesh.text = value;
		}
	}

	public PropertyInfo Property
	{
		get => property; 
		set
		{
			property = value;
			HotKeyText = value.GetValue(Property).ToString();
			HotNameText = value.Name;

		}
	}
	private void Awake()
	{
		hotKeyTextMesh = hotKeyObj.transform.GetComponent<TextMeshProUGUI>();
		hotNameTextMesh = hotNameObj.transform.GetComponent<TextMeshProUGUI>();
	}
	private void Start()
	{
		hotKeyTextMesh = hotKeyObj.transform.GetComponent<TextMeshProUGUI>();
		//HotKeyText = Property.GetValue(Property).ToString();
		hotNameTextMesh = hotNameObj.transform.GetComponent<TextMeshProUGUI>();
		//HotNameText = Property.Name;
		hotKeys.Add(this);
	}
	private void OnGUI()
	{
		if (waitToKey)
		{
			KeyCode? keyCode;
			if (HotKeysHelper.CurrentKeyCode(out keyCode))
			{
				if (keyCode != KeyCode.None)
				{
					Property.SetValue(Property, keyCode.Value);
					HotKeyText = keyCode.Value.ToString();
					waitToKey = false;
				}
			}
		}
	}
	public void Click()
	{
		HotKeysHelper.currentInputType = InputType.HotKeySet;
		foreach (var item in hotKeys)
		{
			item.waitToKey = false;
		}
		waitToKey = true;
	}
	~HotKeySetter()
	{
		hotKeys.Remove(this);
	}
}
