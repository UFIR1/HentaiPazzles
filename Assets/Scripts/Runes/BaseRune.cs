using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runes
{
    public abstract class BaseRune 
    {
        public RuneGrade _grade;
    }
    public enum RuneGrade
	{
        Common,
        Rare,
        Mythical
    }
}
