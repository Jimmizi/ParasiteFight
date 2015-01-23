using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public int playerNumber;

    public int playerHealth;
    public float playerSpeed;

    public Sprite PlayerTexture;
    public GameObject WeaponPrefab;

    private Weapon playerWeapon = new Weapon();

	
	void Start () 
    {
        playerWeapon = WeaponPrefab.GetComponent<Weapon>();
	}
	
	void Update () 
    {
        //Shoot
        if (PressedA())
        {
            float angle = Mathf.Atan2(Input.GetAxis("C1 Vertical"), 
                                      Input.GetAxis("C1 Horizontal")) * 180 / Mathf.PI;

            WeaponPrefab.GetComponent<Weapon>().Shoot(angle);
        }

        
	}



    bool PressedA()
    {
        return Input.GetButtonDown("C" + playerNumber + " A");
    }
    bool PressedB()
    {
        return Input.GetButtonDown("C" + playerNumber + " B");
    }
    bool PressedY()
    {
        return Input.GetButtonDown("C" + playerNumber + " Y");
    }
    bool PressedX()
    {
        return Input.GetButtonDown("C" + playerNumber + " X");
    }
}
