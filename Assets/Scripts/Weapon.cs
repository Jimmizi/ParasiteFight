using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public string weaponName;

	public float weaponDamage;
    public float weaponSpeed;

    public Sprite WeaponTexture;
    public Sprite ProjectileTexture;
    public Sprite GrenadeTexture;

    public PhysicsMaterial2D GrenadeBouncyMat;

    public GameObject ProjectilePrefab;

    public void Shoot(float angle, Vector3 pos)
    {
        GameObject go = (GameObject)Instantiate(ProjectilePrefab, pos + new Vector3(0,1,0), Quaternion.identity);
        go.GetComponent<Projectile>().Initialise(GrenadeTexture, "GRENADE");

        go.AddComponent<PolygonCollider2D>();
        go.GetComponent<PolygonCollider2D>().sharedMaterial = GrenadeBouncyMat;

        go.GetComponent<Projectile>().Shoot(angle, weaponSpeed);
    }

    Sprite spr;

   
}
