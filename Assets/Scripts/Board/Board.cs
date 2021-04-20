using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private GameObject defoultPazzle;
    [SerializeField]
    private Texture2D defoultTexture;
    [SerializeField]
    private int step=100;
    // Start is called before the first frame update
    void Start()
    {
       
		for (int i = 0; i < defoultTexture.width; i+=step)
		{
			for (int j = 0; j < defoultTexture.height; j+=step)
			{
                var texture = Split(defoultTexture, new Vector2Int(i, j),new Vector2Int(step, step));
                var sp= Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(texture.width, texture.height)), new Vector2(0.5f, 0.5f));
                var pazzle= Instantiate(defoultPazzle,new Vector3(defoultPazzle.transform.localScale.x*(i/step),defoultPazzle.transform.localScale.y*(j/step)),defoultPazzle.transform.rotation,this.transform);
                var render= pazzle.GetComponent<SpriteRenderer>();
               
                render.sprite = sp;
                render.drawMode = SpriteDrawMode.Sliced;
                pazzle.transform.localPosition = new Vector3(render.size.x * (i / step), render.size.y * (j / step));

            }
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Texture2D Split(Texture2D texture, Vector2Int topLeftPixel,Vector2Int boxSize)
	{
        var result = new Texture2D(boxSize.x, boxSize.y);
		for (int i = 0; i < boxSize.x; i++)
		{
			for (int j = 0; j < boxSize.y; j++)
			{
                var pixel= texture.GetPixel(topLeftPixel.x + i, topLeftPixel.y + j);
                result.SetPixel(i, j, new Color(pixel.r, pixel.g, pixel.b, pixel.a));

            }
		}
        result.Apply(true);
        return result;
	}
}
