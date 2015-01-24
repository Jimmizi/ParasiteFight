using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.Threading;

public class TerrainTest : MonoBehaviour {

    //Thread thread;
    //Mutex mainLoop;

    Sprite sprite = new Sprite();

    int sizeX = 5000;
    int sizeY = 1080;

    Texture2D texture;// = new Texture2D(5120, 512);
    PolygonCollider2D meshCollider;

	void Start () 
    {
        texture = new Texture2D(sizeX, sizeY);
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
        y = texture.height - texture.height / 3;

        for (int x = 0; x < texture.width; x++)
        {
            for (int y1 = y; y1 < texture.height; y1++)
            {
                texture.SetPixel(x, y1, new Color(0, 0, 0, 0));
            }

            if (Random.value < 0.1f)
            {
                if (Random.value >= 0.5f)
                {
                    y += (int)(Random.value * 5);
                }
                else y -= (int)(Random.value * 5);
            }
        }

        texture.Apply();

        Rect rec = new Rect(0, 0, texture.width, texture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);


        sprite = Sprite.Create(texture, rec, pivot);
        this.GetComponent<SpriteRenderer>().sprite = sprite;
        gameObject.AddComponent<PolygonCollider2D>();
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Explode(Vector3 position, float radius)
    {
        float truePosX = position.x - (this.transform.position.x - sizeX / 200);
        float truePosY = position.y - (this.transform.position.y - sizeY / 180);

        int y = (int)truePosY * 100;
        int x = (int)truePosX * 100;

        for (int x1 = x; x1 < x + radius * 100; x1++)
        {
            for (int y1 = y; y1 > y - radius * 100; y1--)
            {
                texture.SetPixel(x1, y1, new Color(0, 0, 0, 0));
            }
        }

        texture.Apply();

        Destroy(this.GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

}


