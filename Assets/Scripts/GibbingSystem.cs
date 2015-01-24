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
            Rect tempRect = new Rect();   // Used to check the sizes
            Sprite newSprite;
            for (int j = 0; j < 10; j++)
            {
                tempRect = new Rect(Random.Range(0.0f, GibletSprites[i].textureRect.left), Random.Range(0.0f, GibletSprites[i].textureRect.right), Random.Range(0.0f, GibletSprites[i].textureRect.width), Random.Range(0.0f, GibletSprites[i].textureRect.height));
                newSprite = Sprite.Create(GibletSprites[i].texture, tempRect, Vector2.zero);
                
                // Make new game object with components
                GameObject giblet = Resources.Load<GameObject>("Giblet");
                giblet.GetComponent<SpriteRenderer>().sprite = newSprite;
                m_giblets.Add(giblet);
            }            
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
