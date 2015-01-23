using UnityEngine;
using System.Collections;

public class tex2d : MonoBehaviour
{
    Sprite spr;
    Texture2D originalTexture;
    // Use this for initialization
    void Start()
    {

        Texture2D texture = this.GetComponent<SpriteRenderer>().sprite.texture;
        originalTexture = texture;
        renderer.material.mainTexture = texture;
        int y = 0;
        while (y < texture.height)
        {
            int x = 0;
            while (x < texture.width)
            {

                //Debug.Log(texture.GetPixel(x,y).a);

                if (texture.GetPixel(x, y).a < 0.3f)
                {
                    texture.SetPixel(x, y, new Color(1, 1, 1, 1));

                }
                ++x;
            }
            ++y;
        }
        texture.Apply();

        y = 0;
        while (y < texture.height)
        {
            int x = 0;
            while (x < texture.width)
            {

                //Debug.Log(texture.GetPixel(x,y).a);

                if (texture.GetPixel(x, y) == new Color(1, 1, 1, 1))
                {
                    texture.SetPixel(x, y, new Color(1, 1, 1, 0));

                }
                ++x;
            }
            ++y;
        }
        texture.Apply();

        Rect rec = new Rect(0, 0, texture.width, texture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);


        spr = Sprite.Create(texture, rec, pivot);
        this.GetComponent<SpriteRenderer>().sprite = spr;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnApplicationQuit()
    {
        Debug.Log("Quitting");
        Sprite spr1;
        Rect rec = new Rect(0, 0, 10, 10);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        spr1 = Sprite.Create(originalTexture, rec, pivot);
        this.GetComponent<SpriteRenderer>().sprite = spr1;
    }
}
