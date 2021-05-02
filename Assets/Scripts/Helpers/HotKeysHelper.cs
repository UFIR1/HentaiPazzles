using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HotKeysHelper
{
	public static InputTipe currentInputTipe=InputTipe.Global;
	public static KeyCode MoveUp { get; set; } = KeyCode.W;
	public static KeyCode MoveLeft { get; set; } = KeyCode.A;
	public static KeyCode MoveRight { get; set; } = KeyCode.D;
	public static KeyCode MoveDown { get; set; } = KeyCode.S;
	public static KeyCode Hit { get; set; } = KeyCode.Space;
	public static KeyCode Interaction { get; set; } = KeyCode.E;

	static public bool PlayerKey(bool keyPress)
	{
		return (currentInputTipe == InputTipe.Player) && keyPress;
	}
	static public bool GlobalKey(bool keyPress)
	{
		return keyPress;
	}
	static public bool MenuKey(bool keyPress)
	{
		return (currentInputTipe == InputTipe.Menu) && keyPress;
	}

}
public enum InputTipe
{
	Global,
	Player,
	Menu
}