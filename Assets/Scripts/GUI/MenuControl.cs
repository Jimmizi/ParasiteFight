using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour {

    public Camera cam;

    public Sprite Logo, Background;
    public Sprite PlayButton, ExitButton;

    private GameObject LogoGO;

    private int selection = 0;
    private bool moved = false;
	
	void Start () 
    {
        LogoGO = new GameObject();

        //add sprite renderers
        LogoGO.AddComponent<SpriteRenderer>();
   

        //set the sprites
        LogoGO.GetComponent<SpriteRenderer>().sprite = Logo;
 

        LogoGO.transform.position = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height * 0.5f, 10));

	}
	
	
	void Update () 
    {
        if (Input.GetButtonDown("C1 A"))
        {
             Application.LoadLevel("main");
        }
	}
}
