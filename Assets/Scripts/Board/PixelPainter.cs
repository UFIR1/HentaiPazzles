using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPainter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();

        // duplicate the original texture and assign to the material
        var texture = (Texture2D)Instantiate(rend.sprite.texture);

        for (int i = 0; i < texture.height; i++)
        {
            for (int j = 0; j < texture.width; j++)
            {
                texture.SetPixel(i, j, Color.white);
            }
        }
        Debug.Log("");
        texture.Apply(true);
        // colors used to tint the first 3 mip levels
        rend.sprite = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(texture.width, texture.height)), new Vector2(0.5f, 0.5f));
        // actually apply all SetPixels32, don't recalculate mip levels

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
