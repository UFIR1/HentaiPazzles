using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class BaseSmartLogic: MonoBehaviour
{

    public abstract float GetWeight(BaseHero sender);
}
