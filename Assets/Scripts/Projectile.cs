using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    enum Type { GRENADE, BULLET, ROCKET };

    Type projectileType;

    public ParticleSystem ProjectileHit;

    //copy the sprite
    public void Initialise(Sprite spr, string pType)
    {
        if (pType == "GRENADE")
            projectileType = Type.GRENADE;

        if (pType == "BULLET")
            projectileType = Type.BULLET;

        if (pType == "ROCKET")
            projectileType = Type.ROCKET;

        this.GetComponent<SpriteRenderer>().sprite = spr;
    }

    //Called when the bullet spawns in from the weapon class
    public void Shoot(float angle, float force)
    {
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        this.rigidbody2D.AddForce(dir * force);

        if (projectileType == Type.GRENADE)
        {
            this.rigidbody2D.AddTorque(Random.Range(0,300));
            StartCoroutine("GrenadeExplode");
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (projectileType == Type.GRENADE)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 1);
            if (hit.collider != null)
            {
                Debug.DrawRay(transform.position, Vector2.right, Color.cyan);
                //Debug.Break();
                Debug.Log(this.rigidbody2D.velocity.x);
                this.rigidbody2D.AddForce(new Vector2(-100, 0));
            }
        }

        if (projectileType == Type.BULLET)
        {
            Destroy(this.gameObject);
        }
        else if (projectileType == Type.ROCKET)
        {
            //Explode logic

            /* Spawn particle system
             * Deform terrain
             * Damage characters
             */

            Instantiate(ProjectileHit, this.transform.position, Quaternion.identity);

            Destroy(this.gameObject);
        }
    }

    IEnumerator GrenadeExplode()
    {
        float timer = 0;


        while (timer < 4)
        {
            timer += Time.deltaTime;

            yield return null;
        }

        Instantiate(ProjectileHit, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
