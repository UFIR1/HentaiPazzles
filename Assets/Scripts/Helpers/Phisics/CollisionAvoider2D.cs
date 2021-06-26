using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoider2D : MonoBehaviour
{
    public Tags[] triggerTags;
	public bool repiting = false;
    // Start is called before the first frame update
    void Start()
	{
		IgnoreTags();

		if (repiting)
		{
			InvokeRepeating("IgnoreTags", 5, 15);
		}
	}
	private void OnLevelWasLoaded(int level)
	{
		IgnoreTags();
	}

	private void IgnoreTags()
	{
		foreach (var item in transform.GetComponents<Collider2D>())
		{
			foreach (var tag in triggerTags)
			{
				foreach (var item1 in GameObject.FindGameObjectsWithTag(tag.ToString()))
				{
					foreach (var item2 in item1.GetComponents<Collider2D>())
					{
						Physics2D.IgnoreCollision(item, item2, true);
					}
				}
			}

		}
	}
}
