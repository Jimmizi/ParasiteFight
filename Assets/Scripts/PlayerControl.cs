using System;
using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour
{
    //public float Mass = 1.0f;
    //public float DragCoefficient = 100.0f;

    public static List<GameObject> PlayerList;
    public List<GameObject> WeaponPrefabs;

    private bool shot = false;

    public int playerNumber;
    public int roundWins = 0;

    public int playerHealth;
    public float playerSpeed;
    public float jumpForce;

    private bool m_isAllowJump = false;
    private const float MAX_FORCE = 10.0f;
    private const float MAX_SPEED = 5.0f;

    private float m_timer = 0.0f;

    private Vector3 m_force = Vector2.zero;
    private Vector3 m_acceleration = Vector2.zero;
    private Vector3 m_velocity = Vector3.zero;

    private MathHelper m_mathScript = null;
    private Weapon playerWeapon = new Weapon();
    int weapon = 1;

    bool goingRight = true;
    private float jumpLimitTimer = 0;

    public GameObject GunSlot;
    public GameObject BulletSlot;

    float grenadeFirerate = .3f;
    float rocketFirerate = 4;
    float gunFirerate = 0.3f;

    float grenadeTimer, gunTimer, rocketTimer = 0;

    float shootAniTimer = 0;

    public GameObject Head, Body;

    // Use this for initialization
    void Start()
    {
        m_mathScript = GetComponent<MathHelper>();

        if (PlayerList == null)
            PlayerList = new List<GameObject>();

        PlayerList.Add(this.gameObject);
        playerWeapon = WeaponPrefabs[weapon].GetComponent<Weapon>();

        m_isAllowJump = true;

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        grenadeTimer += Time.deltaTime;
        rocketTimer += Time.deltaTime;
        gunTimer += Time.deltaTime;
        shootAniTimer += Time.deltaTime;

        if (playerHealth <= 0)
        {
            playerHealth = 0;
            GameObject.Find("GameplayManager").GetComponent<GameplayManager>().PlayerDeath(playerNumber);
            this.GetComponent<GibbingSystem>().SplitToGibs2();
            Destroy(gameObject);
            
            //Destroy(gameObject);
        }


        jumpLimitTimer += Time.deltaTime;
        //rigidbody2D.AddForce(new Vector2(Input.GetAxis("C" + playerNumber + " Horizontal") * 6, 0.0f));

        float velo = Input.GetAxis("C" + playerNumber + " Horizontal") * playerSpeed * Time.deltaTime;
        this.transform.position = Vector3.Lerp(this.transform.position, this.transform.position + new Vector3(velo * 10, 0, 0),
                                                Time.deltaTime * 15);

        if (Input.GetAxis("C" + playerNumber + " Horizontal") > 0.1f)
            goingRight = true;
        else if (Input.GetAxis("C" + playerNumber + " Horizontal") < -0.1f)
            goingRight = false;

        if (goingRight)
        {
            Head.transform.localScale = new Vector3(1, 1, 1);
            Body.transform.localScale = new Vector3(1, 1, 1);

        }
        else
        {
            Head.transform.localScale = new Vector3(-1, 1, 1);
            Body.transform.localScale = new Vector3(-1, 1, 1);
        }


        if (PressedA() && jumpLimitTimer > 1)
        {
            if (m_velocity.y < 0.01f && m_velocity.y > -0.01f)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1);
                if (hit.collider != null)
                {
                    Debug.DrawRay(transform.position, -Vector2.up, Color.red);
                    rigidbody2D.AddForce(new Vector2(0.0f, jumpForce));
                    jumpLimitTimer = 0;
                }
            }
        }

        if (PressedB())
        {
            if (weapon == 0)
                weapon = 1;
            else if (weapon == 1)
                weapon = 0;
            else if (weapon == 2)
                weapon = 0;

            playerWeapon = WeaponPrefabs[weapon].GetComponent<Weapon>();
            GunSlot.GetComponent<SpriteRenderer>().sprite = WeaponPrefabs[weapon].GetComponent<Weapon>().WeaponTexture;
        }

        if(Input.GetAxis("C" + playerNumber + " Vertical R") != 0 || Input.GetAxis("C" + playerNumber + " Horizontal R") != 0)
        {
            float angle = Mathf.Atan2(Input.GetAxis("C" + playerNumber + " Vertical R"),
                                          Input.GetAxis("C" + playerNumber + " Horizontal R")) * 180 / Mathf.PI;

            Quaternion rot = GunSlot.transform.rotation;

            if(goingRight)
                rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, angle);
            else
                rot.eulerAngles = -new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, -angle);
            
           GunSlot.transform.localRotation = rot;
        }

        //VIN-DEBUG:
        if (Input.GetKeyDown(KeyCode.F))
            playerHealth = 0;

        //Shoot
        if (PressedRB())
        {
            if (weapon == 0)
            {
                if (grenadeTimer < grenadeFirerate)
                {
                    return;
                }
                else grenadeTimer = 0;
            }
            else if (weapon == 1)
            {
                if (gunTimer < gunFirerate)
                {
                    return;
                }
                else gunTimer = 0;
            }
            else if (weapon == 2)
            {
                if (rocketTimer < rocketFirerate)
                {
                    return;
                }
                else rocketTimer = 0;
            }

            if (!shot)
            {
                Debug.Log("Shooting");
                float angle = Mathf.Atan2(Input.GetAxis("C" + playerNumber + " Vertical R"),
                                          Input.GetAxis("C" + playerNumber + " Horizontal R")) * 180 / Mathf.PI;

                WeaponPrefabs[weapon].GetComponent<Weapon>().Shoot(angle, BulletSlot.transform.position,weapon, this.rigidbody2D.velocity);


                shot = true;
            }
        }
        else
        {
            shot = false;
        }
        
        // Do check of height
        //if(rigidbody2D.velocity.y > 5.9999f)
        //    m_isAllowJump = false;
        //if (rigidbody2D.velocity.y < 1.0f)
        //    m_isAllowJump = true;

        Vector2 vel = rigidbody2D.velocity;
        vel.x = Mathf.Clamp(vel.x, -MAX_SPEED, MAX_SPEED);
        rigidbody2D.velocity = vel;
    }


    Vector3 Movement()
    {
        Vector3 force = Vector3.zero;

        if (Input.GetKey(KeyCode.D))
            force.x = 150.0f;
        else if (Input.GetKey(KeyCode.A))
            force.x = -150.0f;
        else
            force = Vector3.zero;

        return force;
    }

    public void DamagePlayer(int amount)
    {
        playerHealth -= amount;
        StartCoroutine("TakeDamage");
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

    IEnumerator TakeDamage()
    {
        float timer = 0;

        this.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        while (timer < 0.1f)
        {
            timer += Time.deltaTime;

            if(timer > 0.05f)
                this.GetComponentInChildren<SpriteRenderer>().color = Color.white;

            yield return null;
        }

        this.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        while (timer < 0.2f)
        {
            timer += Time.deltaTime;

            if (timer > 0.15f)
                this.GetComponentInChildren<SpriteRenderer>().color = Color.white;

            yield return null;
        }

        this.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        while (timer < 0.3f)
        {
            timer += Time.deltaTime;

            if (timer > 0.25f)
                this.GetComponentInChildren<SpriteRenderer>().color = Color.white;

            yield return null;
        }

        this.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        while (timer < 0.4f)
        {
            timer += Time.deltaTime;

            if (timer > 0.35f)
                this.GetComponentInChildren<SpriteRenderer>().color = Color.white;

            yield return null;
        }
    }

    //Vector3 PhysicsTick(Vector3 force)
    //{
    //    // Calculate forces
    //    force = Vector3.ClampMagnitude(force, MAX_FORCE);

    //    // Calculate drag
    //    Vector3 drag = -DragCoefficient * m_velocity;

    //    // Calculate gravity
    //    Vector3 gravity = Physics.gravity;

    //    // Calculate final force
    //    Vector3 resultantForce = force + drag + gravity;

    //    // Calculate acceleration
    //    m_acceleration = resultantForce / Mass;

    //    // Calculate velocity
    //    Vector3 newVelocity = m_velocity + m_acceleration;

    //    newVelocity = Vector3.ClampMagnitude(newVelocity, MAX_SPEED);

    //    // Reset acceleration
    //    m_acceleration = Vector3.zero;

    //    return new Vector3(newVelocity.x, newVelocity.y, 0.0f);
    //}
}
