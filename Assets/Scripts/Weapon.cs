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

    public void Shoot(float angle, Vector3 pos, int wep)
    {
        GameObject go = (GameObject)Instantiate(ProjectilePrefab, pos, Quaternion.identity);
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
