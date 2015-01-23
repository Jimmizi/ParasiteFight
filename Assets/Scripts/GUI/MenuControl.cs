using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour {

    public Camera cam;

    public Sprite Logo, Background;
    public Sprite PlayButton, ExitButton;

    private GameObject LogoGO, BackgroundGO, PlayBtnGO, ExitBtnGO;

    private int selection = 0;
    private bool moved = false;
	
	void Start () 
    {
        LogoGO = new GameObject();
        BackgroundGO = new GameObject();
        PlayBtnGO = new GameObject();
        ExitBtnGO = new GameObject();

        //add sprite renderers
        LogoGO.AddComponent<SpriteRenderer>();
        BackgroundGO.AddComponent<SpriteRenderer>();
        PlayBtnGO.AddComponent<SpriteRenderer>();
        ExitBtnGO.AddComponent<SpriteRenderer>();

        //set the sprites
        LogoGO.GetComponent<SpriteRenderer>().sprite = Logo;
        //BackgroundGO.GetComponent<SpriteRenderer>().sprite = Background;
        PlayBtnGO.GetComponent<SpriteRenderer>().sprite = PlayButton;
        ExitBtnGO.GetComponent<SpriteRenderer>().sprite = ExitButton;

        LogoGO.transform.position = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height * 0.8f, 10));

        PlayBtnGO.transform.position = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height * 0.5f, 10));
        ExitBtnGO.transform.position = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height * 0.3f, 10));


        PlayBtnGO.renderer.material.color = Color.red;
        ExitBtnGO.renderer.material.color = Color.white;
	}
	
	
	void Update () 
    {
        if (Input.GetButtonDown("C1 A"))
        {
            if (selection == 0)
                Application.LoadLevel("main");
            else if (selection == 1)
                Application.Quit();
        }

        if (Input.GetAxis("C1 Vertical") > 0.25f || Input.GetAxis("C1 Vertical") < -0.25f)
        {
            if (Input.GetAxis("C1 Vertical") > 0.2f)
            {
                if (selection > 0)
                    selection--;
            }
            else
            {
                if (selection < 2)
                    selection++;
            }

            if (!moved)
            {
                switch (selection)
                {
                    case 0:
                        PlayBtnGO.renderer.material.color = Color.red;
                        ExitBtnGO.renderer.material.color = Color.white;
                        break;
                    case 1:
                        PlayBtnGO.renderer.material.color = Color.white;
                        ExitBtnGO.renderer.material.color = Color.red;
                        break;
                }

                moved = true;
            }
        }
        else moved = false;
	}
}
