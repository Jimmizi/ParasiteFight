using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    enum Type { GRENADE, BULLET, ROCKET };

    Type projectileType;

    public ParticleSystem ProjectileHit;

    public AudioClip GrenadeExplodeSound, GrenadeFloorHit;

    GameObject Terrain;

    float yDrag = 0;
    float projectileDamage, projectileRange, projectileSpeed;

	private AudioSource source;

    float grenadeTimer, rocketTimer, gunTimer;

    //copy the sprite
    public void Initialise(Sprite spr, string pType, float damage, float radius, float speed)
    {
        source = gameObject.AddComponent<AudioSource>();

        if (pType == "GRENADE")
        {
            projectileType = Type.GRENADE;
        }

        if (pType == "BULLET")
        {
            projectileType = Type.BULLET;
        }

        if (pType == "ROCKET")
        {
            projectileType = Type.ROCKET;
        }

        Terrain = GameObject.Find("Terrain");

        projectileDamage = damage;
        projectileRange = radius;
        projectileSpeed = speed;

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

    public void Update()
    {
        
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

                source.volume = (1 - yDrag / 100) / 2;

                if(hit.collider.tag != "Player")
                source.PlayOneShot(GrenadeFloorHit);

                if (yDrag < 100)
                    yDrag += 25;
            }
        }

        if (projectileType == Type.BULLET)
        {
            if(col.collider.GetComponent<PlayerControl>())
            {
                col.collider.GetComponent<PlayerControl>().DamagePlayer((int)Random.Range(projectileDamage * 0.2f, projectileDamage * 0.4f));
            }

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
            Explode();
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
		Explode();
        Destroy(this.gameObject);
    }

    void Explode()
    {
        foreach (GameObject p in PlayerControl.PlayerList)
        {
            if (p != null)
            {
                float dist = Vector3.Distance(this.transform.position, p.transform.position);

                if (dist < projectileRange)
                {
                    //if above 25% range calculate damage based on distance else damage for the full amount
                    if (dist / projectileRange > 0.25f)
                        p.GetComponent<PlayerControl>().DamagePlayer((int)(projectileDamage * (1 - (dist / projectileRange))));
                    else
                        p.GetComponent<PlayerControl>().DamagePlayer((int)projectileDamage);

                    p.GetComponent<Rigidbody2D>().AddForce(-(p.transform.position - this.transform.position * 50));
                }
            }
        }

        Terrain.GetComponent<TerrainTest>().Explode(this.transform.position, projectileRange);
    }
}
