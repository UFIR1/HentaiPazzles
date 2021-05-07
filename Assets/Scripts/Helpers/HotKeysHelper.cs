using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HotKeysHelper
{
	public static InputType currentInputType = InputType.Global;
	public static KeyCode MoveUp { get; set; } = KeyCode.W;
	public static KeyCode MoveLeft { get; set; } = KeyCode.A;
	public static KeyCode MoveRight { get; set; } = KeyCode.D;
	public static KeyCode MoveDown { get; set; } = KeyCode.S;
	public static KeyCode Hit { get; set; } = KeyCode.Space;
	public static KeyCode Interaction { get; set; } = KeyCode.E;
	public static KeyCode Reload { get; set; } = KeyCode.R;
	public static KeyCode SelectFirstWeapon { get; set; } = KeyCode.Alpha1;

	static public bool PlayerKey(bool keyPress)
	{
		return (currentInputType == InputType.Player) && keyPress;
	}
	static public bool GlobalKey(bool keyPress)
	{
		return keyPress;
	}
	static public bool MenuKey(bool keyPress)
	{
		return (currentInputType == InputType.Menu) && keyPress;
	}
	static public bool CurrentKeyCode(out KeyCode? keyCode)
	{
		keyCode = null;
		if (currentInputType == InputType.HotKeySet && Event.current.keyCode != KeyCode.Escape)
		{
			if (Event.current.isKey)
			{
				keyCode = Event.current.keyCode;
				return true;
			}
		}
		return false;
	}
}
public enum InputType
{
	Global,
	Player,
	Menu,
	HotKeySet
}