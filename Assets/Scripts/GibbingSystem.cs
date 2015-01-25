using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//
// CLASS: Represents a leaf node for the BSP Tree
//
class Leaf
{
    public static int MIN_LEAF_SIZE = 18;
    public static int MAX_LEAF_SIZE = 40;

    //
    // Purpose: Constructor
    //
    public Leaf(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }


    //
    // Purpose: Splits the rectangle
    //
    public bool Split()
    {
        if (LeftChild != null || RightChild != null)
            return false;

        // determine direction of split
        bool splitH = Random.value > 0.5f;
        if (Width > Height && Height / Width >= 0.05f)
            splitH = false;
        else if (Height > Width && Width / Height >= 0.05f)
            splitH = true;

        int max = (splitH ? Height : Width) - MIN_LEAF_SIZE;
        if (max <= MIN_LEAF_SIZE)
            return false;

        int split = Random.Range(MIN_LEAF_SIZE, max);

        if (splitH)
        {
            LeftChild = new Leaf(X, Y, Width, split);
            RightChild = new Leaf(X, Y + split, Width, Height - split);
        }
        else
        {
            LeftChild = new Leaf(X, Y, split, Height);
            RightChild = new Leaf(X + split, Y, Width - split, Height);
        }

        Boundary = new Rect(X, Y, Width, Height);

        return true;
    }

    public int X, Y, Width, Height;
    public Rect Boundary;

    public Leaf LeftChild;
    public Leaf RightChild;
};


//
// SCRIPT: Performs the gibbing
//
public class GibbingSystem : MonoBehaviour
{
    private List<Sprite> GibletSprites = new List<Sprite>(4);
    private List<GameObject> m_giblets = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        Transform[] bodyParts = this.GetComponentsInChildren<Transform>();

        for (int i = 1; i < bodyParts.Length; i++)
        {
            if (bodyParts[i].gameObject.gameObject.GetComponent<SpriteRenderer>())
            {
                SpriteRenderer renderer = bodyParts[i].gameObject.gameObject.GetComponent<SpriteRenderer>();
                GibletSprites.Add(renderer.sprite);
                Debug.Log("Added a giblet");
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void SplitToGibs2()
    {
        // For each particular giblet texture, we partition it and store it in a list which will then
        // be instantiated to multiple game objects to represent giblets.
        for (int i = 0; i < GibletSprites.Count; i++)
        {
            Sprite currentFullGib = GibletSprites[i];

            List<Leaf> leafNodes = new List<Leaf>();

            // Using BSP Method to slice sprite
            Leaf root = new Leaf(0, 0, (int)GibletSprites[i].textureRect.width, (int)GibletSprites[i].textureRect.height);
            leafNodes.Add(root);

            bool HasSplit = true;
            while (HasSplit)
            {
                HasSplit = false;
                for (int j = 0; j < leafNodes.Count; j++)
                {
                    if (leafNodes[j].LeftChild == null && leafNodes[j].RightChild == null)
                    {
                        if (leafNodes[j].Width > Leaf.MAX_LEAF_SIZE || leafNodes[j].Height > Leaf.MAX_LEAF_SIZE || Random.value > 0.25f)
                        {
                            if (leafNodes[j].Split())
                            {
                                leafNodes.Add(leafNodes[j].LeftChild);
                                leafNodes.Add(leafNodes[j].RightChild);
                                HasSplit = true;
                            }
                        }
                    }
                }
            }

            // Remove root node to not show the full sprite picture
            leafNodes.Remove(root);

            for (int k = 0; k < leafNodes.Count; k++)
            {
                Rect tempRect;  // Used to check the sizes
                Sprite newSprite;

                tempRect = leafNodes[k].Boundary;
                newSprite = Sprite.Create(currentFullGib.texture, tempRect, Vector2.zero);

                // Make new game object with components required
                GameObject giblet = new GameObject();
                giblet.transform.position = this.transform.position;
                giblet.AddComponent<SpriteRenderer>();
                giblet.AddComponent<Rigidbody2D>();

                // Get the components and set their parameters
                giblet.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                giblet.GetComponent<SpriteRenderer>().sprite = newSprite;

                // Create a polygon collider 2D after generating sprite to make polygon collider
                // match graphics of the sprite
                giblet.AddComponent<BoxCollider2D>();

                // Make a physics material
                PhysicsMaterial2D physMat = Resources.Load<PhysicsMaterial2D>("PhysMat/Bouncy");
                giblet.GetComponent<BoxCollider2D>().sharedMaterial = physMat;
                //giblet.GetComponent<CircleCollider2D>().radius = 0.2f;

                m_giblets.Add(giblet);
            }
        }
    }


    void SplitToGibs(int i)
    {
        Rect tempRect;  // Used to check the sizes
        Sprite newSprite;
        for (int j = 0; j < 1; j++)
        {
            float randomLeft = Random.Range(0.0f, GibletSprites[i].textureRect.xMin);
            float randomRight = Random.Range(0.0f, GibletSprites[i].textureRect.xMax);
            float randomWidth = Random.Range(0.0f, GibletSprites[i].textureRect.width);
            float randomHeight = Random.Range(0.0f, GibletSprites[i].textureRect.height);

            Debug.Log("Random Left: " + randomLeft);
            Debug.Log("Random Right: " + randomRight);
            Debug.Log("Random Width: " + randomWidth);
            Debug.Log("Random Height: " + randomHeight);

            tempRect = new Rect(randomLeft, randomRight, randomWidth, randomHeight);
            newSprite = Sprite.Create(GibletSprites[i].texture, tempRect, Vector2.zero);

            // Make new game object with components required
            GameObject giblet = new GameObject();
            giblet.AddComponent<SpriteRenderer>();
            giblet.AddComponent<Rigidbody2D>();

            // Get the components and set their parameters
            giblet.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
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
}
