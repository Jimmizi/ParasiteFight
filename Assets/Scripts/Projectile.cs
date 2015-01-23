using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    enum Type { GRENADE, BULLET, ROCKET };

    Type projectileType;

    public ParticleSystem ProjectileHit;

    float yDrag = 0;

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
            //right bounce
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 1);
            if (hit.collider != null)
                this.rigidbody2D.AddForce(new Vector2(-100, 0));

            hit = Physics2D.Raycast(transform.position, -Vector2.right, 1);
            if (hit.collider != null)
                this.rigidbody2D.AddForce(new Vector2(100, 0));

            hit = Physics2D.Raycast(transform.position, -Vector2.up, 1);
            if (hit.collider != null)
            {
                this.rigidbody2D.AddForce(new Vector2(0, 100 - yDrag));

                if (yDrag < 100)
                    yDrag += 25;
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
