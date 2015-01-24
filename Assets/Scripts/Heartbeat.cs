using UnityEngine;
using System.Collections;

public class Heartbeat : MonoBehaviour {

    public Material h1, h2, h3;

    float timer = 0;
    public float changeTime = 0.3f;

	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        timer += Time.deltaTime;

        if (timer > changeTime)
        {
            if (this.GetComponent<Renderer>().material == h1)
            {
                this.GetComponent<Renderer>().material = h2;
                timer = 0;
                return;
            }
            else if (this.GetComponent<Renderer>().material == h2)
            {
                this.GetComponent<Renderer>().material = h3;
                timer = 0;
                return;
            }
            else if (this.GetComponent<Renderer>().material == h3)
            {
                this.GetComponent<Renderer>().material = h1;
                timer = 0;
                return;
            }
        }
	}
}
