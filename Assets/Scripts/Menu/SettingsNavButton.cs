using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingsNavButton : BaseNavButton
{
	[SerializeField]
	private Sprite enableSprite;
	[SerializeField]
	private Animator Anim;
	[SerializeField]
	private Sprite disableSprite;

	private void Start()
	{

		if (disableSprite == null)
		{
			disableSprite = transform.GetComponent<Image>().sprite;
		}
		if (enableSprite == null)
		{
			enableSprite = transform.GetComponent<Image>().sprite;
		}
		Anim.updateMode = AnimatorUpdateMode.UnscaledTime;


	}
	private void Update()
	{
	}
	// Start is called before the first frame update
	public override void Click()
	{
		base.Click();
		StateCheck();
		foreach (var item in GameObject.FindGameObjectsWithTag(Tags.SettingsButton.ToString()).Select(x=>x.GetComponent<SettingsNavButton>()))
		{
			item.StateCheck();
		}
	}

	public void StateCheck()
	{
		if (panelToOpen.activeSelf)
		{
			Anim.SetBool("Active", true);
		}
		else
		{
			Anim.SetBool("Active", false);

		}
	}
}
