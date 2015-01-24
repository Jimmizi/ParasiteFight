using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public string weaponName;

	public float weaponDamage;
    public float weaponSpeed;
    public float weaponRadius;

    public Sprite WeaponTexture;
    public Sprite ProjectileTexture;
    public Sprite GrenadeTexture;
    
    public GameObject ProjectilePrefab;

    public void Shoot(float angle, Vector3 pos, int wep, Vector3 additionalVel)
    {
        GameObject go = (GameObject)Instantiate(ProjectilePrefab, pos, Quaternion.identity);
        Vector2 vel = new Vector2(additionalVel.x, additionalVel.y);
        go.rigidbody2D.velocity += vel;
        go.transform.position = pos;

        if(wep == 0)
            go.GetComponent<Projectile>().Initialise(GrenadeTexture, "GRENADE", weaponDamage, weaponRadius, weaponSpeed);

        if (wep == 1)
            go.GetComponent<Projectile>().Initialise(GrenadeTexture, "BULLET", weaponDamage, weaponRadius, weaponSpeed);

        if (wep == 2)
        {
            go.GetComponent<Projectile>().Initialise(GrenadeTexture, "ROCKET", weaponDamage, weaponRadius, weaponSpeed);
            
        }

        go.AddComponent<PolygonCollider2D>();

        go.GetComponent<Projectile>().Shoot(angle, weaponSpeed);
    }


   
}
