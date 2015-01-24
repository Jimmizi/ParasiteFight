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

    int[] deadCollision;

    void Start()
    {
        TerrainGrid = new GameObject[yAmount];
        sprite = new Sprite[yAmount];
        texture = new Texture2D[yAmount];
        deadCollision = new int[yAmount];

        int y = 0;

        for (int i = 0; i < yAmount; i++)
        {
            texture[i] = new Texture2D(5000, 50);
            deadCollision[i] = 0;
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

        y = 17;
        int currentLayer = 17;
        int orig = currentLayer;

        //random terrain height
        for (int x = 0; x < texture[0].width; x++)
        {
            for (int i = currentLayer; i < 25; i++)
            {
                if (i == currentLayer)
                {
                    for (int y1 = y; y1 < texture[i].height; y1++)
                    {
                        texture[i].SetPixel(x, y1, new Color(0, 0, 0, 0));
                    }
                }
                else
                {
                    for (int y1 = 0; y1 < texture[i].height; y1++)
                    {
                        texture[i].SetPixel(x, y1, new Color(0, 0, 0, 0));
                    }
                }
            }

            if (Random.value < 0.1f)
            {
                if (Random.value >= 0.5f)
                {
                    y += (int)(Random.value * 15);
                }
                else y -= (int)(Random.value * 15);
            }

            if (y > 50)
            {
                currentLayer++;
                y = 0;
            }
            else if (y < 0)
            {
                currentLayer--;
                y = 49;
            }
        }

        //for (int i = 0; i < 25; i++)
        //{
        //    for (int xi = 2000; xi < 2500; xi++)
        //    {
        //        texture[15].SetPixel(xi, i, new Color(0, 0, 0, 1));
        //    }
        //}

        for (int i = 0; i < yAmount; i++)
        {
            texture[i].Apply();

            TerrainGrid[i] = (GameObject)Instantiate(GridPiece);

            Vector3 pos = camera.ScreenToWorldPoint(new Vector3(0, 39.3f * i, 10));
            Rect rec = new Rect(0, 0, texture[i].width, texture[i].height);
            Vector2 pivot = new Vector2(0.285f, 0.0f);

            TerrainGrid[i].transform.position = pos;

            sprite[i] = Sprite.Create(texture[i], rec, pivot);
            TerrainGrid[i].GetComponent<SpriteRenderer>().sprite = sprite[i];

            if (!TerrainGrid[i].gameObject.GetComponent<PolygonCollider2D>())
            {
                int pixels = 0;

                for (int x2 = 0; x2 < texture[i].width; x2++)
                {
                    for (int y2 = 0; y2 < texture[i].height; y2++)
                    {
                        if (texture[i].GetPixel(x2, y2) != new Color(0, 0, 0, 0))
                        {
                            pixels++;
                        }
                    }
                }

                if (pixels > 6)
                {
                    TerrainGrid[i].gameObject.AddComponent<PolygonCollider2D>();
                }
                else deadCollision[i] = 1;
            }
            else if (TerrainGrid[i].gameObject.GetComponent<PolygonCollider2D>())
            {
                int pixels = 0;

                for (int x2 = 0; x2 < texture[i].width; x2++)
                {
                    for (int y2 = 0; y2 < texture[i].height; y2++)
                    {
                        if (texture[i].GetPixel(x2, y2) != new Color(0, 0, 0, 0))
                        {
                            pixels++;
                        }
                    }
                }

                if (pixels > 6)
                {
                    Destroy(TerrainGrid[i].GetComponent<PolygonCollider2D>());
                    TerrainGrid[i].gameObject.AddComponent<PolygonCollider2D>();
                }
                else deadCollision[i] = 1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BloodPaint(Vector3 position)
    {
        float truePosX = (position.x - (this.transform.position.x - sizeX / 200));
        float truePosY = (position.y - (this.transform.position.y - sizeY / 200));

        int y = (int)truePosY * 100;
        int x = (int)truePosX * 100;

        List<int> closeTo = new List<int>();

        //find what is close
        for (int i = 0; i < yAmount; i++)
        {
            float diff = position.y - TerrainGrid[i].transform.position.y;
            diff = diff < 0 ? -diff : diff;

            if (diff < 2.5f)
                closeTo.Add(i);
        }

        int iter = 1;
        foreach (int i in closeTo)
        {
            //for (int y1 = 0; y1 < texture[i].height; y1++)
            //{
            //    for (int x1 = x - (int)radius * 25; x1 < x + radius * 25; x1++)
            //    {
            //        texture[i].SetPixel(x1, y1, new Color(0, 0, 0, 0));
            //    }
            //}
            int yIter = Random.Range(25, 50);
            for (int x1 = x - (int)3 * 150 + Random.Range(-175, 175); x1 < x + 3 * 50 + Random.Range(-175, 175); x1++)
            {
                for (int y1 = texture[i].height - 1; y1 > -1; y1--)
                {
                    if (y1 > 3 * 35 - (yIter * iter))
                    {
                        if (texture[i].GetPixel(x1, y1).a > 0.8f)
                        {
                            if(Random.value < 0.6f)
                                texture[i].SetPixel(x1, y1, new Color(1, 0, 0, 1 ));
                        }
                    }

                    if (Random.value < 0.1f)
                    {
                        if (Random.value < 0.5f)
                        {
                            yIter--;
                        }
                        else yIter++;
                    }
                }
            }

            texture[i].Apply();
            iter++;
        }
    }

    public void Explode(Vector3 position, float radius)
    {
        float truePosX = (position.x - (this.transform.position.x - sizeX / 200));
        float truePosY = (position.y - (this.transform.position.y - sizeY / 200));

        int y = (int)truePosY * 100;
        int x = (int)truePosX * 100;

        List<int> closeTo = new List<int>();

        //find what is close
        for (int i = 0; i < yAmount; i++)
        {
            float diff = position.y - TerrainGrid[i].transform.position.y;
            diff = diff < 0 ? -diff : diff;

            if (diff < 1.5f)
                closeTo.Add(i);
        }

        int iter = 1;
        foreach (int i in closeTo)
        {
            //for (int y1 = 0; y1 < texture[i].height; y1++)
            //{
            //    for (int x1 = x - (int)radius * 25; x1 < x + radius * 25; x1++)
            //    {
            //        texture[i].SetPixel(x1, y1, new Color(0, 0, 0, 0));
            //    }
            //}
            int yIter = Random.Range(45, 50);
            for (int x1 = x - (int)radius * 50 + Random.Range(-35, 35); x1 < x + radius * 50 + Random.Range(-35, 35); x1++)
            {
                for (int y1 = texture[i].height - 1; y1 > -1; y1--)
                {
                    if (y1 > radius * 35 - (yIter * iter))
                    {
                        texture[i].SetPixel(x1, y1, new Color(0, 0, 0, 0));
                    }

                    if (Random.value < 0.01f)
                    {
                        if (Random.value < 0.5f)
                        {
                            yIter--;
                        }
                        else yIter++;
                    }
                }
            }

            texture[i].Apply();

            if (deadCollision[i] != 1)
            {
                Destroy(TerrainGrid[i].GetComponent<PolygonCollider2D>());
                TerrainGrid[i].gameObject.AddComponent<PolygonCollider2D>();
            }

            iter++;
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


