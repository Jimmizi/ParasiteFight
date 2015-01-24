using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour
{
    public List<GameObject> Targets = new List<GameObject>();

    Vector3 avgPosition;
    float distances;
    public Vector3 offset = new Vector3();

    void Start()
    {
        // Empty
    }



    // Update is called once per frame
    void Update()
    {
        avgPosition = Vector3.zero;
        distances = 0;

        int alive = 4;

        for(int i = 0; i < 4; i++)
        {
            if (Targets[i] != null)
            {
                avgPosition += Targets[i].transform.position;

                for (int x = 0; x < 4; x++)
                {
                    if (Targets[i] != Targets[x] && Targets[x] != null)
                        distances += Vector3.Distance(Targets[i].transform.position, Targets[x].transform.position);
                }
            }
            else alive--;
        }

        avgPosition /= alive;
        distances /= 15;

        avgPosition.z = -10;// -distances;

        this.camera.orthographicSize = 10 + distances / 2;

        this.transform.position = Vector3.Lerp(this.transform.position, avgPosition + offset, Time.deltaTime * 3);
    }
}
