using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public string weaponName;

	public float weaponDamage;
    public float weaponSpeed;

    public Sprite WeaponTexture;
    public Sprite ProjectileTexture;

    public GameObject ProjectilePrefab;

	void Start () 
	{
        weaponSpeed = 300;
	}

	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
	}

    void Shoot()
    {
        GameObject go = (GameObject)Instantiate(ProjectilePrefab, this.transform.position, Quaternion.identity);
        go.GetComponent<Projectile>().Initialise(ProjectileTexture);
        go.AddComponent<PolygonCollider2D>();

        go.GetComponent<Projectile>().Shoot(45, weaponSpeed);
    }

}
