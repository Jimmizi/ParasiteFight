using System;
using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using System.Threading;

public class PlayerControl : MonoBehaviour
{
    //public float Mass = 1.0f;
    //public float DragCoefficient = 100.0f;

    private bool m_isAllowJump = false;
    private const float MAX_FORCE = 10.0f;
    private const float MAX_SPEED = 5.0f;

    private float m_timer = 0.0f;

    private Vector3 m_force = Vector2.zero;
    private Vector3 m_acceleration = Vector2.zero;
    private Vector3 m_velocity = Vector3.zero;

    private MathHelper m_mathScript = null;

    // Use this for initialization
    void Start()
    {
        m_mathScript = GetComponent<MathHelper>();
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.D))
            rigidbody2D.AddForce(new Vector2(1.0f, 0.0f), ForceMode2D.Impulse);
        else if(Input.GetKey(KeyCode.A))
            rigidbody2D.AddForce(new Vector2(-1.0f, 0.0f), ForceMode2D.Impulse);

        // If player's velocity is not going in Y, allow 
        if (rigidbody2D.velocity.y < 6.0f)
        {
            if (m_isAllowJump)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    rigidbody2D.AddForce(new Vector2(0.0f, 50.0f));
                }
            }
        }
        
        // Do check of height
        if(rigidbody2D.velocity.y > 5.9999f)
            m_isAllowJump = false;
        if (rigidbody2D.velocity.y == 0.0f)
            m_isAllowJump = true;

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
