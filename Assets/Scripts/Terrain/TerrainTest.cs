﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainTest : MonoBehaviour
{

    public GameObject GridPiece;
    public Camera camera;

    const int yAmount = 25;

    int sizeX = 5000;
    int sizeY = 1080;

    GameObject[] TerrainGrid;
    Sprite[] sprite;
    Texture2D[] texture;

    void Start()
    {
        TerrainGrid = new GameObject[yAmount];
        sprite = new Sprite[yAmount];
        texture = new Texture2D[yAmount];
        //sprite = new Sprite[xAmount];
        //renderer.material.mainTexture = texture;
        int y = 0;

        for (int i = 0; i < yAmount; i++)
        {
            texture[i] = new Texture2D(5000, 50);
            while (y < texture[i].height)
            {
                int x = 0;
                while (x < texture[i].width)
                {
                    texture[i].SetPixel(x, y, new Color(0.5f, 0.5f, 0.5f, 1));

                    ++x;
                }
                ++y;
            }
        }


        //texture = new Texture2D(sizeX, sizeY);
        //renderer.material.mainTexture = texture[i];
        //y = 0;
        //while (y < texture[i].height)
        //{
        //    int x = 0;
        //    while (x < texture.width)
        //    {
        //        if (texture.GetPixel(x, y).a < 0.3f)
        //        {
        //            texture.SetPixel(x, y, new Color(1, 1, 1, 1));

        //        }
        //        ++x;
        //    }
        //    ++y;
        //}
        y = 1080 / 2;



        for (int i = 0; i < yAmount; i++)
        {
            for (int x = 0; x < texture[i].width; x++)
            {
                for (int y1 = y; y1 < texture[i].height; y1++)
                {
                    texture[i].SetPixel(x, y1, new Color(0, 0, 0, 0));
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

            texture[i].Apply();

            TerrainGrid[i] = (GameObject)Instantiate(GridPiece);

            Vector3 pos = camera.ScreenToWorldPoint(new Vector3(0, 39.3f*i, 10));
            Rect rec = new Rect(0, 0, texture[i].width, texture[i].height);
            Vector2 pivot = new Vector2(0.285f, 0.0f);

            TerrainGrid[i].transform.position = pos;

            sprite[i] = Sprite.Create(texture[i], rec, pivot);
            TerrainGrid[i].GetComponent<SpriteRenderer>().sprite = sprite[i];

            if (!TerrainGrid[i].gameObject.GetComponent<PolygonCollider2D>())
                TerrainGrid[i].gameObject.AddComponent<PolygonCollider2D>();
        }



        for (int x1 = 2450; x1 < 2550; x1++)
        {
            for (int y1 = 0; y1 < 50; y1++)
            {
                texture[24].SetPixel(x1, y1, new Color(0, 0, 0, 1));
            }
        }

        texture[24].Apply();


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Explode(Vector3 position, float radius)
    {
        float truePosX = (position.x - (this.transform.position.x - sizeX / 200));
        float truePosY = (position.y - (this.transform.position.y - sizeY / 200));

        int y = (int)truePosY * 100;
        int x = (int)truePosX * 100;

        Debug.Log("X: " + truePosX + " Y: " + truePosY);

        List<int> closeTo = new List<int>();

        //find what is close
        for (int i = 0; i < yAmount; i++)
        {
            float diff = position.y - TerrainGrid[i].transform.position.y;
            diff = diff < 0 ? -diff : diff;

            if (diff < 1)
                closeTo.Add(i);
        }

        foreach (int i in closeTo)
        {
            for (int x1 = x - (int)radius * 50; x1 < x + radius * 50; x1++)
            {
                for (int y1 = y; y1 > y - radius * 100; y1--)
                {

                    texture[i].SetPixel(x1, y1, new Color(0, 0, 0, 0));
                }
            }

            texture[i].Apply();

            Destroy(TerrainGrid[i].GetComponent<PolygonCollider2D>());
            TerrainGrid[i].gameObject.AddComponent<PolygonCollider2D>();
        }

        //for (int i = 0; i < yAmount; i++)
        //{
        //    for (int x1 = x; x1 < x + radius * 100; x1++)
        //    {
        //        for (int y1 = y; y1 > y - radius * 100; y1--)
        //        {
        //            texture[i].SetPixel(x1, y1, new Color(0, 0, 0, 1));
        //        }
        //    }
        //}

        

        
    }

}


