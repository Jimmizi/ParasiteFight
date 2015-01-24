using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public static List<GameObject> PlayerList;

    public int playerNumber;

    public int playerHealth;
    public float playerSpeed;
    public float jumpForce;

    public Sprite PlayerTexture;
    public List<GameObject> WeaponPrefabs;

    private Weapon playerWeapon = new Weapon();

    private float jumpLimitTimer = 0;
    private bool shot;
	
	void Start () 
    {
        if(PlayerList == null)
            PlayerList = new List<GameObject>();

        PlayerList.Add(this.gameObject);
        playerWeapon = WeaponPrefabs[0].GetComponent<Weapon>();
	}
	
	void FixedUpdate () 
    {
        Vector3 velocity = new Vector3(0, 0, 0);
        jumpLimitTimer += Time.deltaTime;

        if (playerHealth <= 0)
            Debug.Break();

        //vertical moving
        velocity.x = Input.GetAxis("C" + playerNumber + " Horizontal") * playerSpeed * Time.deltaTime;
        this.transform.position = Vector3.Lerp(this.transform.position,this.transform.position + new Vector3(velocity.x * 10, 0, 0),
                                                Time.deltaTime * 5);

        //jump                         
        if (PressedRB() && jumpLimitTimer > 0.2f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up,1);
            if (hit.collider != null)
            {
                this.rigidbody2D.AddForce(new Vector2(0, jumpForce));
                jumpLimitTimer = 0;
            }
        }

        //Shoot
        if (PressedRT())
        {
            if(!shot)
            {
                float angle = Mathf.Atan2(Input.GetAxis("C" + playerNumber + " Vertical R"),
                                          Input.GetAxis("C" + playerNumber + " Horizontal R")) * 180 / Mathf.PI;

                WeaponPrefabs[0].GetComponent<Weapon>().Shoot(angle, this.transform.position);

                shot = true;
            }
        }
        else
        {
            shot = false;
        }

        
	}

    public void DamagePlayer(int amount)
    {
        playerHealth -= amount;
    }

    bool PressedRB()
    {
        return Input.GetButtonDown("C" + playerNumber + " RB");
    }

    bool PressedRT()
    {
        return Input.GetAxis("C" + playerNumber + " RT") > 0.5f ? true : false;
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
