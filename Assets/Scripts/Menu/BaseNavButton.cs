using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNavButton : MonoBehaviour
{
    [SerializeField]
    protected GameObject[] panelsToClose;
    [SerializeField]
    protected GameObject panelToOpen;
    public virtual void Click()
	{
		foreach (var item in panelsToClose)
		{
            item.SetActive(false);
		}
        panelToOpen.SetActive(true);
	}
}
