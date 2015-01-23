using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public ParticleSystem ProjectileHit;

    public void Initialise(Sprite spr)
    {
        this.GetComponent<SpriteRenderer>().sprite = spr;
    }

    //Called when the bullet spawns in from the weapon class
    public void Shoot(float angle, float force)
    {
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        this.rigidbody2D.AddForce(dir * force);
        Debug.Log("ADDIING VELOCITY");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //Explode logic

        /* Spawn particle system
         * Deform terrain
         * Damage characters
         */

        Instantiate(ProjectileHit, this.transform.position,Quaternion.identity);

        Destroy(this.gameObject);
    }
}
