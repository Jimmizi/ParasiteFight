using UnityEngine;
using System.Collections;

public class GibletSound : MonoBehaviour {



    /* NOT IN USE
     * 
     * 
     * 
     * 
     * 
     */








    public AudioClip Gib1;
    int playLimit = 3;

	// Use this for initialization
	public void Init () 
    {
        if(!gameObject.GetComponent<AudioSource>())
            gameObject.AddComponent<AudioSource>();

        gameObject.GetComponent<AudioSource>().playOnAwake = false;
        gameObject.GetComponent<AudioSource>().clip = Gib1;
        gameObject.GetComponent<AudioSource>().loop = false;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (playLimit > 0 && !gameObject.GetComponent<AudioSource>().isPlaying)
        {
            gameObject.GetComponent<AudioSource>().Play();

            playLimit--;
        }
    }
}
