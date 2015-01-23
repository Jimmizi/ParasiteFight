using System;
using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Messaging;

public class PlayerControl : MonoBehaviour
{
    public float Mass = 1.0f;
    public float DragCoefficient = -10.0f;

    private const float MAX_FORCE = 10.0f;
    private const float MAX_SPEED = 10.0f;

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
        m_force = Movement();
        m_velocity = PhysicsTick(m_force);
        transform.position = transform.position + m_velocity * Time.deltaTime;
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


    Vector3 PhysicsTick(Vector3 force)
    {
        // Calculate forces
        force = Vector3.ClampMagnitude(force, MAX_FORCE);
        Vector3 drag = -DragCoefficient * m_velocity;

        // Calculate final force
        Vector3 resultantForce = force + drag;

        // Calculate acceleration
        m_acceleration = resultantForce / Mass;

        // Calculate velocity
        Vector3 newVelocity = m_velocity + m_acceleration;

        newVelocity = Vector3.ClampMagnitude(newVelocity, MAX_SPEED);

        // Reset acceleration
        m_acceleration = Vector3.zero;

        return new Vector3(newVelocity.x, newVelocity.y, 0.0f);
    }
}
