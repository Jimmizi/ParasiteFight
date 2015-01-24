using UnityEngine;
using System.Collections;

public class Deleter : MonoBehaviour {

    public float timeToLive;

    float timer = 0;
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        timer += Time.deltaTime;

        if (timer > timeToLive)
            Destroy(gameObject);
	}
}
