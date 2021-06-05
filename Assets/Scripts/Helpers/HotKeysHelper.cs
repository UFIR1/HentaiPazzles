using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HotKeysHelper
{
	private static InputType currentInputType = InputType.Global;
	private static float lastTimeScale = 1;
	public static KeyCode MoveUp { get; set; } = KeyCode.W;
	public static KeyCode MoveLeft { get; set; } = KeyCode.A;
	public static KeyCode MoveRight { get; set; } = KeyCode.D;
	public static KeyCode MoveDown { get; set; } = KeyCode.S;
	public static KeyCode Hit { get; set; } = KeyCode.Space;
	public static KeyCode Interaction { get; set; } = KeyCode.E;
	public static KeyCode Reload { get; set; } = KeyCode.R;
	public static KeyCode SwitchBulletType { get; set; } = KeyCode.B;
	public static KeyCode SelectFirstWeapon { get; set; } = KeyCode.Alpha1;
	public static InputType CurrentInputType
	{
		get => currentInputType; set
		{
			if (value == InputType.Global&& currentInputType!= InputType.Global)
			{
				lastTimeScale = Time.timeScale;
				Time.timeScale = 0;
				currentInputType = value;

			}
			if (value != InputType.Global)
			{
				Time.timeScale = lastTimeScale;
				currentInputType = value;
			}
		}
	}

	static public bool PlayerKey(bool keyPress)
	{
		return (CurrentInputType == InputType.Player) && keyPress;
	}
	static public bool GlobalKey(bool keyPress)
	{
		return keyPress;
	}
	static public bool MenuKey(bool keyPress)
	{
		return (CurrentInputType == InputType.Menu) && keyPress;
	}
	static public bool CurrentKeyCode(out KeyCode? keyCode)
	{
		keyCode = null;
		if (CurrentInputType == InputType.HotKeySet && Event.current.keyCode != KeyCode.Escape)
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