using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public int playerNumber;

    public int playerHealth;
    public float playerSpeed;
    public float jumpForce;

    public Sprite PlayerTexture;
    public GameObject WeaponPrefab;

    private Weapon playerWeapon = new Weapon();

    private float jumpLimitTimer = 0;
    private bool shot;
	
	void Start () 
    {
        playerWeapon = WeaponPrefab.GetComponent<Weapon>();
	}
	
	void Update () 
    {
        Vector3 velocity = new Vector3(0, 0, 0);
        jumpLimitTimer += Time.deltaTime;

        //vertical moving
        velocity.x = Input.GetAxis("C" + playerNumber + " Horizontal") * playerSpeed * Time.deltaTime;
        this.transform.position += new Vector3(velocity.x, 0, 0);

        //jump
        if (PressedRB() && jumpLimitTimer > 0.2f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up,1);
            if (hit.collider != null)
            {
                Debug.DrawRay(transform.position, -Vector2.up, Color.red);
                float distance = Mathf.Abs(hit.point.y - transform.position.y);

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

                WeaponPrefab.GetComponent<Weapon>().Shoot(angle, this.transform.position);

                shot = true;
            }
        }
        else
        {
            shot = false;
        }

        
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
