using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractive
{
	public abstract void Use(BaseHero Sender);
	public abstract void ReadyToUse();
	public abstract void NotReadyToUse();


}
