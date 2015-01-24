﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainTest : MonoBehaviour
{

    public GameObject GridPiece;
    public Camera camera;

    const int xAmount = 25;

    int sizeX = 5000;
    int sizeY = 1080;

    GameObject[,] TerrainGrid;
    Sprite sprite = new Sprite();
    Texture2D texture;

    void Start()
    {
        TerrainGrid = new GameObject[xAmount, 1];

        texture = new Texture2D(5000,1080);
        //sprite = new Sprite[xAmount];
        //renderer.material.mainTexture = texture;
        int y = 0;

        for (int i = 0; i < xAmount; i++)
        {
            //texture = new Texture2D(xAmount * 8, 1080);
            while (y < texture.height)
            {
                int x = 0;
                while (x < texture.width)
                {
                    texture.SetPixel(x, y, new Color(0.5f, 0.5f, 0.5f, 1));

                    ++x;
                }
                ++y;
            }
        }


        texture = new Texture2D(sizeX, sizeY);
        renderer.material.mainTexture = texture;
        y = 0;
        while (y < texture.height)
        {
            int x = 0;
            while (x < texture.width)
            {
                if (texture.GetPixel(x, y).a < 0.3f)
                {
                    texture.SetPixel(x, y, new Color(1, 1, 1, 1));

                }
                ++x;
            }
            ++y;
        }
        y = 1080  -texture.height / 2;




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
    void Update()
    {

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


