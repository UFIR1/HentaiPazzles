using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSettings : MonoBehaviour
{
    public GameObject content;
    public GameObject keySetter;
    public float step;
    public List<GameObject> keyElement;
    // Start is called before the first frame update
    void Start()
    {
        float i = 0;
        float sumHeight = 0;
		foreach (var item in typeof( HotKeysHelper).GetProperties())
		{
            
            var newElement = Instantiate(keySetter, content.transform);
            keyElement.Add(newElement);

            var setter = newElement.GetComponent<HotKeySetter>();
            var newElementTransform = newElement.GetComponent<RectTransform>();
            setter.Property = item;
            sumHeight += newElementTransform.rect.height+step;
            i++;
        }
        sumHeight += 100;
         var contentTransform= ((RectTransform)content.transform);
        contentTransform.sizeDelta = new Vector2(0,sumHeight);
        var j = sumHeight / 2;
        i = 0.5f;
        foreach (var item in keyElement)
		{
            var newElementTransform = item.GetComponent<RectTransform>();
            var asd = newElementTransform.anchoredPosition = new Vector2(0, j-((i * newElementTransform.rect.height) + step));
            i++;
        }
    }

   
}
