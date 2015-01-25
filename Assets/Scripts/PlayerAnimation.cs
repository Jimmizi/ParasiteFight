using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

	private Animator anim;

	// Use this for initialization
	void Start () {
	
		anim = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {
	
        //animation floats do not exist

        //if (Input.GetKey(KeyCode.S)) 
        //{
			
        //    anim.SetFloat ("Spin", 0.5f);
			
        //} 
        //else {
			
        //    anim.SetFloat("Spin", 0.0f);
			
        //}

        //if (Input.GetKeyDown (KeyCode.Space)) 
        //{

        //    anim.SetBool ("Firing", true);
		
        //} 
        //else {
		
        //    anim.SetBool("Firing", false);
		
        //}

	}
}
