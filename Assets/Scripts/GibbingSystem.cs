using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GibbingSystem : MonoBehaviour
{
    public List<Sprite> GibletSprites = new List<Sprite>(4);
    private List<GameObject> m_giblets = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        // This is the fun part, create a number of sprites that this list of giblets will use from 4 different textures
        for (int i = 0; i < GibletSprites.Count; i++)
        {
            Rect tempRect; ;   // Used to check the sizes
            Sprite newSprite;
            for (int j = 0; j < 1; j++)
            {
                float randomLeft = Random.Range(0.0f, GibletSprites[i].textureRect.left);
                float randomRight = Random.Range(0.0f, GibletSprites[i].textureRect.right);
                float randomWidth = Random.Range(0.0f, GibletSprites[i].textureRect.width);
                float randomHeight = Random.Range(0.0f, GibletSprites[i].textureRect.height);
                tempRect = new Rect(randomLeft, randomRight, randomWidth, randomHeight);
                newSprite = Sprite.Create(GibletSprites[i].texture, GibletSprites[i].textureRect, Vector2.zero);
                
                // Make new game object with components required
                GameObject giblet = new GameObject();
                giblet.AddComponent<SpriteRenderer>();
                giblet.AddComponent<Rigidbody2D>();
                
                // Get the components and set their parameters
                giblet.GetComponent<SpriteRenderer>().sortingLayerName = "Play";
                giblet.GetComponent<SpriteRenderer>().sprite = newSprite;
                
                // Create a polygon collider 2D after generating sprite to make polygon collider
                // match graphics of the sprite
                giblet.AddComponent<PolygonCollider2D>();

                // Make a physics material
                PhysicsMaterial2D physMat = Resources.Load<PhysicsMaterial2D>("PhysMat/Bouncy");
                giblet.GetComponent<PolygonCollider2D>().sharedMaterial = physMat;

                m_giblets.Add(giblet);
            }
        }

        // Instantiate all the giblets
        for (int i = 0; i < m_giblets.Count; i++)
        {
            GameObject go = Instantiate(m_giblets[i]) as GameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
