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

    // Use this for initialization
    void Start()
    {
        m_mathScript = GetComponent<MathHelper>();

        if (PlayerList == null)
            PlayerList = new List<GameObject>();

        PlayerList.Add(this.gameObject);
        playerWeapon = WeaponPrefabs[0].GetComponent<Weapon>();

        m_isAllowJump = true;

    }


    // Update is called once per frame
    void Update()
    {

        rigidbody2D.AddForce(new Vector2(Input.GetAxis("C" + playerNumber + " Horizontal"), 0.0f), ForceMode2D.Impulse);
  

        // If player's velocity is not going in Y, allow 
        if (rigidbody2D.velocity.y < 6.0f)
        {
            if (PressedRB())
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1);
                if (hit.collider != null)
                {
                    rigidbody2D.AddForce(new Vector2(0.0f, 300.0f));
                }
            }
        }

        //Shoot
        if (PressedRT())
        {
            if (!shot)
            {
                Debug.Log("Shooting");
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
